using System.Collections.Generic;
using UnityEngine;
using System;

public class Rush 
{
    [SerializeField]
    private Game[] gamesInRush = new Game[20];
    
    [SerializeField]
    private int actualGame = 0;

    [SerializeField]
    private bool rushIsFinish = false;

    [SerializeField]
    private bool rushIsWin = false;

    //Construct
    public Rush(Game[] tabGame){
        if (tabGame.Length < 20)
        {
            throw new Exception("Pour creer un rush il faut passer un tableau de 20 Games");
        }
        for (int i = 0; i < tabGame.Length; i++)
        {
           gamesInRush[i] = tabGame[i];
        }
    }

    //Gets/Sets
    public void setActualGame(Game game){
        if (rushIsFinish == false)
        {
            gamesInRush[actualGame] = game;
            if (game.getWin()){
                actualGame++;
                if (actualGame == gamesInRush.Length-1){
                    setRushIsWin();
                }         
            }else{
                setRushIsLose();
            }
               
        }
    }

    public void setRushIsLose(){
        rushIsFinish = true;
        rushIsWin = false;
    }

    public void setRushIsWin(){
        rushIsFinish = true;
        rushIsWin = true;
    }

    public bool getRushIsWin(){
        if (rushIsFinish && rushIsWin)
        {
            return true;
        }
        return false;
    }

    public bool getRushIsLose(){
        if (rushIsFinish && !rushIsWin)
        {
            return true;
        }
        return false;
    }

    public List<Word> getWordsOfRush(){
        List<Word> wordList = new List<Word>();
        foreach (Game game in gamesInRush)
        {
            wordList.Add(game.getWordToFind());
        }
        return wordList;
    }

    public Game getActualGame(){
        return gamesInRush[actualGame];
    }

}
