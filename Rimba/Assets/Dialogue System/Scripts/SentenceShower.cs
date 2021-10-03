using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SentenceShower : MonoBehaviour
{
    public string refSentenceText;
    public float delayBeforeShowNext;
    public DialogueHandler dialogueHandler;
    public string fmodSoundPath;
    private float showByLettersDelay;
    private string curSentenceText;
    private int curSymbol;
    private Text text;

    private void Start()
    {
        dialogueHandler.showNextSentenceEvent.AddListener(EndWritting);
        text = gameObject.GetComponent<Text>();
        
        showByLettersDelay = PlayerPrefs.GetFloat("TextShowSpeed");
        curSentenceText = ""; curSymbol = 0;
        text.text = curSentenceText;
        //if(fmodSoundPath != "") ДОБАВИТЬ ЗВУК FMOD!!!
        StartCoroutine(ShowByLetters());
    }

    private IEnumerator ShowByLetters()
    {
        yield return new WaitForSeconds(showByLettersDelay);
        if(curSymbol < refSentenceText.Length)
        {
            curSentenceText += refSentenceText[curSymbol];
            curSymbol++;
            text.text = curSentenceText;
            StartCoroutine(ShowByLetters());
        }
        else
        {
            StartCoroutine(ShowNextSentence()); 
        }
    }

    private IEnumerator ShowNextSentence()
    {
        yield return new WaitForSeconds(delayBeforeShowNext);
        dialogueHandler.NextSentence();
        EndWritting();
    }

    public void EndWritting()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
}