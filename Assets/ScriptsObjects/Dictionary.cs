using System.Collections.Generic;
using System;

public class Dictionary
{
    private List<Word> dictionary = new List<Word>();

    //Construct
    public Dictionary(List<Word> wordForStart){
        addWord(wordForStart);
    }

  /*  public Dictionary(){
    }*/

//------------- les add word ---------------------
// Ajouter un mod dans le dictionnaire de differentes manieres
    public void addWord(Word wordToAdd){
        Nullable<int> wordFind = findWord(wordToAdd);
        if (wordFind == null)
        {
            dictionary.Add(wordToAdd);
        }
    }

    public void addWord(List<Word> listWordToAdd){
        foreach (var word in listWordToAdd)
        {
            addWord(word);         
        }
    }

    public void addWord(Game gameToAdd){
        Word wordInGame = gameToAdd.getWordToFind();
        addWord(wordInGame);
    }
    
//-----------------------------------------------

    //Supprime un mot precis dans le dictionnaire
    public void deleteWord(Word wordToDelete){
        Nullable<int> index = findWord(wordToDelete);
        if (index != null)
        {
            dictionary.RemoveAt((int)index);
        }
    }
    
    //retourne index ou null (est utilise par toutes les methodes de la classe)
    public Nullable<int> findWord(Word wordToFind){
        Nullable<int> index = null;
        for (int i = 0; i < dictionary.Count; i++)
        {
            if (wordToFind.getWord() == dictionary[i].getWord()){
                index = i;
                break;
            }
        }
        return index;
    }

    //Get
    public List<Word> getDictionary(){
        return dictionary;
    }
}
