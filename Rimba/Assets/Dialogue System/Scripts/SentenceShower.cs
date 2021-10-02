using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SentenceShower : MonoBehaviour
{
    public string refSentenceText;
    public float showByLettersDelay; // Изменить на PlayerPrefs и брать из настроект
    public float delayBeforeShowNext;
    public DialogueHandler dialogueHandler;
    public string fmodSoundPath;
    private string curSentenceText;
    private int curSymbol;
    private Text text;

    private void Start()
    {
        dialogueHandler.showNextSentenceEvent.AddListener(EndWritting);
        text = gameObject.GetComponent<Text>();

        curSentenceText = ""; curSymbol = 0;
        text.text = curSentenceText;
        //if(fmodSoundPath != "") ДОБАВИТЬ ЗВУК FMOD!!!
        StartCoroutine(ShowByLetters());
    }

    private IEnumerator ShowByLetters()
    {
        yield return new WaitForSecondsRealtime(showByLettersDelay);
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
        yield return new WaitForSecondsRealtime(delayBeforeShowNext);
        dialogueHandler.NextSentence();
        EndWritting();
    }

    public void EndWritting()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
}