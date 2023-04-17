using System.Collections.Generic;
using UnityEngine;
using System;

public class Game
{

    [SerializeField]
    private Word wordToFind;

    [SerializeField]
    List<Char> charTry = new List<Char>();

    [SerializeField]
    private int nbCharFind = 0;

    [SerializeField]
    private bool gameWin = false;

    [SerializeField]
    private bool gameDone = false;

    [SerializeField]
    private List<Char> wordFind = new List<Char>();

    //Constructeur
    public Game(Word _wordToFind){
        wordToFind = _wordToFind;
        for (int i = 0; i < wordToFind.getWord().Length; i++)
        {
            wordFind.Add('_');
        }
    }



    //Renvoie une liste des index ou les lettres on ete trouves si null alors lettre deja essayee
    public List<int> WhereIsFind(char letter){

        if(!CanITryThis(letter)){
            return null;
        }
    
        List<int> listOfIndex = wordToFind.PositionsIfExist(letter);

        if (listOfIndex.Count == 0){
            charTry.Add(letter);
            return listOfIndex;
        }else{
            foreach (int position in listOfIndex){
                if (position == 0){
                    wordFind[position] = Char.ToUpper(letter);
                }else{
                    wordFind[position] = letter;
                }
                
            }
        }

        charTry.Add(letter);
        nbCharFind++;
        VerifAndSetIfGameWin();
        return listOfIndex;

    }

    //Verifie si la lettre a deja ete testee
    private bool CanITryThis(char letter){
        if (gameDone){
            throw new Exception("Cette partie est déjâ terminée");
        }

        foreach (char character in charTry){
            if (character == letter)
            {
                GameHolder.CharAlreadyTry();
                return false;
            }
        }
        return true;
    }


    //Gets/Sets
    private void VerifAndSetIfGameWin(){
        if (nbCharFind == wordToFind.getNbDifChar()){
            gameWin = true;
        }
    }

    public void SetgameDone(){
        gameDone = true;
    }

    public bool getWin(){
        return gameWin;
    }

    public Word getWordToFind(){
        return wordToFind;
    }

    public List<Char> getWordFind(){
        return wordFind;
    }

    public List<Char> getCharTry(){
        return charTry;
    }
    
}
