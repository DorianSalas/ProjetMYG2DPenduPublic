using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;


public class Interface : MonoBehaviour
{
   
    
    [Header("UI mots et lettres de la partie")][Space(10)]

    [SerializeField]
    private GameObject wordActual;

    [SerializeField]
    private GameObject charTryUI;

    [SerializeField]
    private List<GameObject> winWord;

    [Space(20)]
    [Header("UI des vies")][Space(10)]

    [SerializeField]
    private int actualHandSprites = 0;

    [SerializeField]
    private GameObject handLives;

    [SerializeField]
    private List<Sprite> handSprites;

    [Space(20)]
    [SerializeField]
    private int actualNbBones = 0;

    [SerializeField]
    private int maxNbBones = 0;

    [SerializeField]
    private GameObject bonesLives;

    [SerializeField]
    private List<Sprite> bonesSprites;


    [Space(20)]
    [Header("UI des phases (biblio)")][Space(10)]

    [SerializeField]
    private int actualBibliothequeSprite = 0;

    [SerializeField]
    private GameObject bibliotheque;

    [SerializeField]
    private List<Sprite> bibliothequeSprites;

    [SerializeField]
    private GameObject rushTrophy;


    [Space(20)]
    [Header("Les Canvas")][Space(10)]

    [SerializeField]
    private GameObject waitRequest;

    //Fonctios que GameHolder apelle pour mettre a jour l affichage lors d'un evenement (victoire, defaite, telechargement...)
    public void DLEnd()
    {
        waitRequest.SetActive(false);
    }

    public void setLastPhase()
    {
        actualNbBones = maxNbBones;
        actualBibliothequeSprite++;
        bonesLives.GetComponent<Image>().sprite = bonesSprites[maxNbBones];
        bibliotheque.GetComponent<Image>().sprite = bibliothequeSprites[actualBibliothequeSprite];
        resetWinWord();

    }

    public void setPhase(){
        maxNbBones++;
        actualNbBones = maxNbBones;
        actualBibliothequeSprite++;
        bonesLives.GetComponent<Image>().sprite = bonesSprites[maxNbBones];
        bibliotheque.GetComponent<Image>().sprite = bibliothequeSprites[actualBibliothequeSprite];
        resetWinWord();

    }

    public void whriteWord(List<Char> word){
        String wordString = "";
        foreach (Char character in word)
        {
            wordString += (character+" ");
        }
        wordActual.GetComponent<TextMeshProUGUI>().text = wordString;
    }
 
    public void pvLose(){
        if (actualNbBones == 0){
            actualHandSprites = (actualHandSprites + 1) % handSprites.Count;
            handLives.GetComponent<Image>().sprite = handSprites[actualHandSprites];
        }else{
            actualNbBones--;
            bonesLives.GetComponent<Image>().sprite = bonesSprites[actualNbBones];
        }

    }

    public void gameLose(){
        Debug.Log("Domage");
        actualHandSprites = 0;
        actualNbBones = maxNbBones;
        handLives.GetComponent<Image>().sprite = handSprites[actualHandSprites];
        charTryUI.GetComponent<TextMeshProUGUI>().text = "";
        bonesLives.GetComponent<Image>().sprite = bonesSprites[maxNbBones];
    }

    public void gameWin(){
        Debug.Log("Bien joué");
        actualHandSprites = 0;
        actualNbBones = maxNbBones;
        handLives.GetComponent<Image>().sprite = handSprites[actualHandSprites];
        charTryUI.GetComponent<TextMeshProUGUI>().text = "";
        bonesLives.GetComponent<Image>().sprite = bonesSprites[maxNbBones];
    }

    public void gameWinInRush(){
        Debug.Log("Bien joué");
        charTryUI.GetComponent<TextMeshProUGUI>().text = "";
    }

    public void actuLetterTry(List<Char> _charTry){
        String chartry = "";
        foreach (Char character in _charTry)
        {
            chartry += (character + " ");
        }
        charTryUI.GetComponent<TextMeshProUGUI>().text = chartry;
    }
    public void whriteWinWordSimple(int winWordToPrint, string word){
        winWord[winWordToPrint].GetComponent<TextMeshProUGUI>().text = word;
    }

    public void whriteWinWordEndless(string word){
        // winWord[winWordToPrint].GetComponent<TextMeshProUGUI>().text = word;
        for (int i = 0; i < winWord.Count; i++){
            if (i == winWord.Count - 1){
                winWord[i].GetComponent<TextMeshProUGUI>().text = word;
            }
            else{
                winWord[i].GetComponent<TextMeshProUGUI>().text = winWord[i + 1].GetComponent<TextMeshProUGUI>().text;
            }
        }
        //Debug.Log("pas d affichage normal");
    }
    public void setTrophy(){
        rushTrophy.SetActive(true);
    }

    public void resetWinWord(){
        foreach (GameObject _winWord in winWord)
        {
            _winWord.GetComponent<TextMeshProUGUI>().text = "" ;
        }
    }

    public void restartUI() {
        actualHandSprites = 0;
        actualNbBones = 0;
        maxNbBones = 0;
        actualBibliothequeSprite = 0;
        wordActual.GetComponent<TextMeshProUGUI>().text = "";
        //Debug.Log(wordActual.GetComponent<TextMeshProUGUI>().text);
        charTryUI.GetComponent<TextMeshProUGUI>().text = "";
        bonesLives.GetComponent<Image>().sprite = bonesSprites[0];
        bibliotheque.GetComponent<Image>().sprite = bibliothequeSprites[0];
        rushTrophy.SetActive(false);
        resetWinWord();
    }

    
    void Start()
    {
        wordActual.GetComponent<TextMeshProUGUI>().text = "";
        //Debug.Log(wordActual.GetComponent<TextMeshProUGUI>().text);
        charTryUI.GetComponent<TextMeshProUGUI>().text = "";
        bonesLives.GetComponent<Image>().sprite = bonesSprites[0];
        bibliotheque.GetComponent<Image>().sprite = bibliothequeSprites[0];
        rushTrophy.SetActive(false);
        resetWinWord();
    }

}
