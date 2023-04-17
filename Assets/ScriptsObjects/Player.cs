using System.Collections.Generic;
using UnityEngine;
using System;


public class Player 
{
    
    //-----------Systeme pour les rushs------------

    [SerializeField]
    private bool inRush = false;

    [SerializeField]
    private List<Rush> rushsRun = new List<Rush>();

    //---------------------------------------------
    
    [SerializeField]
    public List<Game> listGames = new List<Game>();
    
    [SerializeField]
    public Game gameActu;
    
    [SerializeField]
    private int hpMax = 6;

    [SerializeField]
    private int hp;

    [SerializeField]
    private int nbWins = 0;

    //ne seras utile que lors du multijoueur
    [SerializeField]
    private string nickname;


    //Constructeur (lance automatiquement la 1ere partie avec le dictionnaire)
    public Player(string _nickname){
        if (GameHolder.dictionary.getDictionary().Count == 0)
        {
            throw new Exception("Le dictionnaire ne doit jamais être vide lors de la création d'un joueur");
        }
        nickname = _nickname;
        hp = hpMax;
        var random = new System.Random();
        //cree et ajoute une partie avec un mot aleatoire du dictionnaire accessible en static
        gameActu = new Game(GameHolder.dictionary.getDictionary()[random.Next(0,GameHolder.dictionary.getDictionary().Count)]);
    }
    //Constructeur (la 1ere partie est donnee en paramettre)
    public Player(string _nickname, Game firstGame){
        nickname = _nickname;
        hp = hpMax;
        gameActu = firstGame;
    }

    //Si les conditions sont bonnes initialise l'etat "rush" en cours
    public void initRush(){
        if (inRush)
        {
            Debug.Log("Un Rush est deja en cours");
        }
        else if (GameHolder.dictionary.getDictionary().Count < 20)
        {
            Debug.Log("Veuillez terminer le dictionnaire en cours avant de lancer un rush (mois de 20 mots restants)");
        }
        else
        {
            Game[] tabInitRushs = new Game[20];
            Dictionary tempDictionary = new Dictionary(GameHolder.dictionary.getDictionary());
            for (int i = 0; i < 20; i++)
            {
                tabInitRushs[i] = createNewGame(tempDictionary);
                tempDictionary.deleteWord(tabInitRushs[i].getWordToFind());
            }

            rushsRun.Add(new Rush(tabInitRushs));
            gameActu = rushsRun[rushsRun.Count - 1].getActualGame();
            inRush = true;
        }
    }

    //Gere la victoire d'un rush
    public void rushIsWin(){
        if (rushsRun[rushsRun.Count-1].getRushIsWin())
        {
            List<Word> wordsInRush = rushsRun[rushsRun.Count-1].getWordsOfRush();
            foreach (Word word in wordsInRush)
            {
                GameHolder.dictionary.deleteWord(word);
            }
            inRush = false;
        }
    }

    //Lance une nouvelle partie en enregistrant et respectant les conditions de la précedente (rush en cours, derniere partie gagnee/perdue...)
    public void NextGame(){
        if (inRush){
            if (gameActu.getWin()){
                gameActu.SetgameDone();
                rushsRun[rushsRun.Count-1].setActualGame(gameActu);
                if (rushsRun[rushsRun.Count-1].getRushIsWin()){
                    //rushIsWin();
                    if (GameHolder.dictionary.getDictionary().Count == 0 || GameHolder.dictionary.getDictionary() == null){
                        GameHolder.youFindAll();
                    }
                    Debug.Log("Rush Gagné !!!!");
                    gameActu = createNewGame(GameHolder.dictionary);
                }else{
                    gameActu = rushsRun[rushsRun.Count-1].getActualGame();
                }
            }else{
                gameActu.SetgameDone();
                rushsRun[rushsRun.Count-1].setActualGame(gameActu);
                inRush = false;
                hp = hpMax;
                gameActu = createNewGame(GameHolder.dictionary);
            }
        }else{
            if (gameActu.getWin()){
                //nbWins++;
                gameActu.SetgameDone();
                listGames.Add(gameActu);
                GameHolder.dictionary.deleteWord(gameActu.getWordToFind());
                /*if (nbWins%GameHolder.nbWinsForPhase == 0)
                {
                    GameHolder.NextPhase();
                }*/
                if (GameHolder.dictionary.getDictionary().Count == 0 || GameHolder.dictionary.getDictionary() == null){
                    GameHolder.youFindAll();
                }
                gameActu = createNewGame(GameHolder.dictionary);
                hp = hpMax;
            }else{
                gameActu.SetgameDone();
                listGames.Add(gameActu);
                gameActu = createNewGame(GameHolder.dictionary);
                hp = hpMax;
            }
        }
    }

    //Cree une partie avec un mot aleatoire restant, si le dictionnaire est termine alors le re rempli 
    private Game createNewGame(Dictionary dico){
        var random = new System.Random();
        List<Word> passDictionary = dico.getDictionary();
        if (passDictionary.Count == 0)
        {
            GameHolder.youFindAll();
        }
        return new Game(passDictionary[random.Next(0,passDictionary.Count-1)]);
    }



    //Test si une lettre existe dans le mot a trouver. si la lettre a deja ete cherchee retourne null sinon retourne les occurences trouvees
    public List<int> TryLetter(char _letter){
        char letter = Char.ToLower(_letter);
        List<int> letterFinds = gameActu.WhereIsFind(letter);
        if (letterFinds == null){
            return null;
        }

        if (letterFinds.Count == 0){
            hp --;
        }

        if (gameActu.getWin() && !inRush){
            nbWins++;
        }

        return letterFinds;

    }

    //Get/Set...
    public void AddOneHpMax()
    {
        hpMax++;
    }

    public Game getGameActu(){
        return gameActu;
    }

    public int getHp(){
        return hp;
    }

    public int getNbWins(){
        return nbWins;
    }

    public bool isInRush(){
        return inRush;
    }

    public List<Rush> getRushRun(){
        return rushsRun;
    }

}
