using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
//http://rali.iro.umontreal.ca/LVF_DEM/DEM.jsonl
public class GameHolder : MonoBehaviour
{

    /* MultiJoueur ou tableau des scores------------------
    [SerializeField]
    private List<Player> allPlayers = new List<Player>();
    ------------------------------------------------------*/

    [SerializeField]
    private GameObject myInterface;
    [SerializeField]
    private bool rushAlreadyWin = false;
    [SerializeField]
    public static int nbWinsForPhase = 5;
    [SerializeField]
    private int actualPhase = 0;
    [SerializeField]
    public static Dictionary dictionaryTotal = new Dictionary(new List<Word>(){
                                                            new Word("bonjour"),
                                                            new Word("bienvenue"),
                                                            new Word("pendu"),
                                                            new Word("jouer"),
                                                            new Word("super"),
                                                            new Word("autre"),
                                                            new Word("facile"),
                                                            new Word("juste"),
                                                            new Word("libre"),
                                                            new Word("propre"),
                                                            new Word("rouge"),
                                                            new Word("tranquille"),
                                                            new Word("bonne"),
                                                            new Word("doux"),
                                                            new Word("heureux"),
                                                            new Word("beau"),
                                                            new Word("content"),
                                                            new Word("entier"),
                                                            new Word("humain"),
                                                            new Word("lundi"),
                                                            new Word("mardi"),
                                                            new Word("mercredi"),
                                                            new Word("jeudi"),
                                                            new Word("vendredi"),
                                                            new Word("essayer")
                                                        });

    [SerializeField]
    public static Dictionary dictionary = new Dictionary(dictionaryTotal.getDictionary());

    [SerializeField]
    private string[] gamephases = new string[] {"tuto", "debutant", "facile", "moyen", "difficile", "expert", "marathon"};

    [SerializeField]
    private Player player;//new Player("Tartampion")

    [SerializeField]
    private string urlDictionnary = "http://rali.iro.umontreal.ca/LVF_DEM/DEM.jsonl";

    [SerializeField]
    private GameObject AudioManager;

    //S'occupe de passer une partie avec le marteau
    public void passGame(){
        if (player.isInRush()){
            myInterface.GetComponent<Interface>().resetWinWord();
        }
        player.NextGame();
        myInterface.GetComponent<Interface>().gameLose();
        myInterface.GetComponent<Interface>().whriteWord(player.getGameActu().getWordFind());
        Debug.Log(player.getGameActu().getWordToFind().getNbDifChar() + " lettres differentes dans " + player.getGameActu().getWordToFind().getWord());
        
    }

    //Remets tout les parametres d'origine pour relancer une partie
    public void restartGame(){

        dictionary = new Dictionary(dictionaryTotal.getDictionary());
        player = new Player("Tartampion");
        rushAlreadyWin = false;
        nbWinsForPhase = 5;
        actualPhase = 0;
        player.NextGame();
        myInterface.GetComponent<Interface>().restartUI();
        myInterface.GetComponent<Interface>().whriteWord(player.getGameActu().getWordFind());
        Debug.Log(player.getGameActu().getWordToFind().getNbDifChar() + " lettres differentes dans " + player.getGameActu().getWordToFind().getWord());
    }

    //Declare de debut d'un "rush"
    public void GameHInitRush(){
        player.initRush();
        myInterface.GetComponent<Interface>().whriteWord(player.getGameActu().getWordFind());
        Debug.Log(player.getGameActu().getWordToFind().getNbDifChar() + " lettres differentes dans " + player.getGameActu().getWordToFind().getWord());
    }

    //Est la pour d'evantuelles evolutions du fonctionnement lorsqu'on essaye une lettre deja essayee
    public static void CharAlreadyTry(){
        Debug.Log("Tu essaye une lettre qui a deja été testée...");
    }

    //Si le dictionnaire est fini le signale et le re rempli 
    public static void youFindAll(){
        
        Debug.Log("Bravo Vous avez fini le dictionnaire------------------------");
        dictionary = new Dictionary(dictionaryTotal.getDictionary());
    }

    //Verifie la phase a laquelle on est et fais les opperations pour passer a la prochaine si besoin
    public void NextPhase()
    {
        Debug.Log(gamephases.Length);
        if (actualPhase == gamephases.Length - 2) {
            if (player.getRushRun()[player.getRushRun().Count - 1].getRushIsWin())
            {
                myInterface.GetComponent<Interface>().setLastPhase();
                AudioManager.GetComponent<Audio>().PlaySoundNextPhase();
                actualPhase++;
                rushAlreadyWin = true;
            }
        } else if (actualPhase == gamephases.Length - 1) {
        } else {
            player.AddOneHpMax();
            myInterface.GetComponent<Interface>().setPhase();
            actualPhase++;
            AudioManager.GetComponent<Audio>().PlaySoundNextPhase();
        }

    }

    //Effectue une web request gerant les erreurs reseaux et si tout se passe bien se sert du json recu pour remplir le dictionnaire
    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                    player = new Player("Tartampion");
                    myInterface.GetComponent<Interface>().DLEnd();
                    myInterface.GetComponent<Interface>().whriteWord(player.getGameActu().getWordFind());
                    Debug.Log(player.getGameActu().getWordToFind().getNbDifChar() + " lettres differentes dans " + player.getGameActu().getWordToFind().getWord());
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log(pages[page] + ": HTTP Error: " + webRequest.error);
                    player = new Player("Tartampion");
                    myInterface.GetComponent<Interface>().DLEnd();
                    myInterface.GetComponent<Interface>().whriteWord(player.getGameActu().getWordFind());
                    Debug.Log(player.getGameActu().getWordToFind().getNbDifChar() + " lettres differentes dans " + player.getGameActu().getWordToFind().getWord());
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    string[] eachJson = webRequest.downloadHandler.text.Split("\n");
                    List<Word> listDictionary = new List<Word>();
                    for (int i = 0; i < eachJson.Length - 1; i++)
                    {
                        JsonObject tempJsonObject = JsonUtility.FromJson<JsonObject>(eachJson[i]);
                        //tri du JSON, retire les doublons, les accents, les mots composes et les mots trop courts
                        if (Regex.IsMatch(tempJsonObject.M, @"^[a-z]{3,}$") && (listDictionary.Count == 0 || tempJsonObject.M != listDictionary[listDictionary.Count - 1].getWord()))
                        {
                            listDictionary.Add(new Word(tempJsonObject.M));
                        }
                    }
                    dictionaryTotal = new Dictionary(listDictionary);
                    dictionary  = new Dictionary(dictionaryTotal.getDictionary());
                    player = new Player("Tartampion");
                    
                    myInterface.GetComponent<Interface>().DLEnd();
                    myInterface.GetComponent<Interface>().whriteWord(player.getGameActu().getWordFind());
                    Debug.Log(player.getGameActu().getWordToFind().getNbDifChar() + " lettres differentes dans " + player.getGameActu().getWordToFind().getWord());
                    break;
            }
        }
    }

    //Lance la machine
    void Start()
    {
        StartCoroutine(GetRequest(urlDictionnary));
    }


    //Detecte les touches a chaque frame et fais les appels au programme et a l'ihm en consequences
    void Update()
    {
        
        foreach(KeyCode kCode in Enum.GetValues(typeof(KeyCode)))
        {
            //Si une lettre est pressee
            if(Input.GetKeyDown(kCode) && kCode.ToString().Length == 1 && char.IsLetter((char)kCode) ){
                List<int> letterFinds = null;
                letterFinds = player.TryLetter((char)kCode);
                //Verifie si la lettre est dans le mot, si on perd ou gagne...
                if (letterFinds == null){

                }else if (letterFinds.Count == 0){                
                    myInterface.GetComponent<Interface>().pvLose();
                    myInterface.GetComponent<Interface>().actuLetterTry(player.getGameActu().getCharTry());
                    if (player.getHp() == 0){
                        myInterface.GetComponent<Interface>().gameLose();
                        if (player.isInRush()){
                            myInterface.GetComponent<Interface>().resetWinWord();
                        }
                        AudioManager.GetComponent<Audio>().PlaySoundLose();
                        player.NextGame();
                        Debug.Log(player.getGameActu().getWordToFind().getNbDifChar() + " lettres differentes dans " + player.getGameActu().getWordToFind().getWord());
                    }
                    
                }else if (letterFinds.Count != 0){
                    myInterface.GetComponent<Interface>().actuLetterTry(player.getGameActu().getCharTry());
                    if (player.getGameActu().getWin()){

                        if (!player.isInRush() && actualPhase <= gamephases.Length - 2){

                            if (player.getNbWins() % GameHolder.nbWinsForPhase == 0){

                                if(actualPhase == gamephases.Length - 3){

                                    NextPhase();
                                    myInterface.GetComponent<Interface>().setTrophy();

                                }else if (actualPhase < gamephases.Length - 3){
                                    NextPhase();
                                }
                            }else if(actualPhase == gamephases.Length - 2){
                                myInterface.GetComponent<Interface>().whriteWinWordEndless(player.getGameActu().getWordToFind().getWord());
                                AudioManager.GetComponent<Audio>().PlaySoundWin();
                            }
                            else{
                                myInterface.GetComponent<Interface>().whriteWinWordSimple(player.getNbWins() % 5 - 1, player.getGameActu().getWordToFind().getWord());
                                AudioManager.GetComponent<Audio>().PlaySoundWin();
                            }
                        }else if (player.isInRush()){
                            myInterface.GetComponent<Interface>().whriteWinWordEndless(player.getGameActu().getWordToFind().getWord());
                            
                        }
                        
                        player.NextGame();
                        if (player.isInRush()){
                            myInterface.GetComponent<Interface>().gameWinInRush();
                        }else{
                            myInterface.GetComponent<Interface>().gameWin();
                            
                        }
                        
                        Debug.Log(player.getGameActu().getWordToFind().getNbDifChar() + " lettres differentes dans " + player.getGameActu().getWordToFind().getWord());
                        myInterface.GetComponent<Interface>().actuLetterTry(player.getGameActu().getCharTry());
                        if (!rushAlreadyWin && player.getRushRun().Count!=0 && player.getRushRun()[player.getRushRun().Count - 1].getRushIsWin()){
                            NextPhase();
                            player.rushIsWin();

                        }
                    }
                }

                myInterface.GetComponent<Interface>().whriteWord(player.getGameActu().getWordFind());

            }
        }
    }




}
