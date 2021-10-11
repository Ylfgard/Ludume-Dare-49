using System.Collections;
using UnityEngine;
using TMPro;
using System;
using System.Runtime.InteropServices;

public class SentenceShower : MonoBehaviour
{
    [Multiline(10)] public string refSentenceText;
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

        #region Experimenting
        text.fontSize = 40f;
        text.alignment = TextAlignmentOptions.TopLeft;
        text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Application.isEditor ? Camera.main.scaledPixelWidth * 0.75f : Screen.width * 0.75f);
        //text.rectTransform.sizeDelta = Vector2.one * 0.9f;
        text.rectTransform.ForceUpdateRectTransforms();
        #endregion

        if(fmodSoundPath != "")
        {
            Debug.Log("PlayMusic: " + fmodSoundPath);
            instance = FMODUnity.RuntimeManager.CreateInstance(fmodSoundPath);
            instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(Camera.main.transform)); 
            instance.start();
            FMOD.Studio.EventDescription discription;
            instance.getDescription(out discription);
            int lenght;
            discription.getLength(out lenght);
            delayBeforeShowNext = lenght/1000 + 0.7f;
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