using System.Collections;
using UnityEngine;
using TMPro;

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
    private TextMeshProUGUI text;

    private void Start()
    {
        dialogueHandler.showNextSentenceEvent.AddListener(EndWritting);
        text = gameObject.GetComponent<TextMeshProUGUI>();
        
        showByLettersDelay = PlayerPrefs.GetFloat("TextShowSpeed");
        curSentenceText = ""; curSymbol = 0;
        text.text = curSentenceText;
        if(fmodSoundPath != "")
        {
            Debug.Log("PlayMusic: " + fmodSoundPath);
            instance = FMODUnity.RuntimeManager.CreateInstance(fmodSoundPath);
            instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(Camera.main.transform)); 
            instance.start();
            //FMODUnity.EventManager.EventFromPath(fmodSoundPath).Length/1000 + 1f
            delayBeforeShowNext = 15;
            StartCoroutine(ShowNextSentence());
        }
        StartCoroutine(ShowByLetters());
    }

    private void FixedUpdate() 
    {
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(Camera.main.transform));
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

    private void OnDestroy() 
    {
        if(fmodSoundPath != "")
        {
            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            instance.release();
        }
    }
}