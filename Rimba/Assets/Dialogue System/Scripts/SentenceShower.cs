using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SentenceShower : MonoBehaviour
{
    public string refSentenceText;
    public float delayBeforeShowNext;
    public DialogueHandler dialogueHandler;
    public string fmodSoundPath;
    private FMOD.Studio.EventInstance instance;
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
        if(fmodSoundPath != "")
        {
            instance = FMODUnity.RuntimeManager.CreateInstance(fmodSoundPath);
            instance.start();
            delayBeforeShowNext = FMODUnity.EventManager.EventFromPath(fmodSoundPath).Length + 0.1f;
            StartCoroutine(ShowNextSentence());
        }
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
            if(fmodSoundPath == "")
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
        if(fmodSoundPath != "")
        {
            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            instance.release();
        }
        Destroy(gameObject);
    }
}