using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Word
{
    [SerializeField]
    private String word;

    //pas necessaire dans l'absolu mais j'en ai besoin pour la difficulte du mot donc je prefere le stocker une bonne fois pour toute plutot que répéter des recherches
    [SerializeField]
    private int nbDifChar;

    //Constructeur
    public Word(String _word){
        if(_word.Length == 0){
            throw new Exception("Tu essaye de créér un mot vide...");
        }else{
            word = _word.ToLower();
            BuildNbDifChar();
        }
    }

    //Parcourt le string mot pour compter combien de characteres differents il comporte
    private void BuildNbDifChar(){

        String tempoExistChar = "";

        foreach (Char letter in word)
        {
            if(!tempoExistChar.Contains(letter.ToString())){
                tempoExistChar += letter.ToString();
            }
        }
        nbDifChar = tempoExistChar.Length;
    }

    //Renvoie une liste des indexes ou se trouvent la lettre recherchee (permet de savoir quelles lettres afficher a chaque fois que l'utilisateur teste une lettre)
    public List<int> PositionsIfExist(Char charSearch){

        List<int> listOfPositions = new List<int>();
        
        for (int i = 0; i < word.Length; i++)
        {
            if (word.Substring(i, 1) == charSearch.ToString())
            {
                listOfPositions.Add(i);
            }
        }
        
        return listOfPositions;
    }

    //Accesseur basique pour que game puisse verifier si c'est gagné ou pas
    public int getNbDifChar(){
        return nbDifChar;
    }

    public String getWord(){
        return word;
    }
    
}
