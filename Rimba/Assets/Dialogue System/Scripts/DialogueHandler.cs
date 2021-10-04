using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class DialogueHandler : MonoBehaviour
{
    [SerializeField] private GameObject dialogueFrame;
    [SerializeField] private Transform contentTransform;
    [SerializeField] private GameObject sentencePref;
    [SerializeField] private Volume postProcessingVolume;
    [SerializeField] private GameObject playImage;
    [HideInInspector] public UnityEvent showNextSentenceEvent;
    private List<IDialogueEvent> dialogueEvents = new List<IDialogueEvent>();
    private int curSentenceIndex = 0;
    private DialogueViewer curDialogue;
    private List<GameObject> sentences = new List<GameObject>();

    private void Start() 
    {
        EndDialogue();
    }
    
    public void PausePlayerEffect()
    {
        postProcessingVolume.weight = 1;
        playImage.SetActive(true);
        GamePauser.StopGame(gameObject);
    }

    public void StartDialogue(TextAsset dialogue, IDialogueEvent[] dialEvs, bool pauseGame)
    {
        ClearSentences();
        if(pauseGame) PausePlayerEffect();
        curSentenceIndex = 0;
        curDialogue = DialogueViewer.Load(dialogue);
        foreach(IDialogueEvent ev in dialEvs)
            dialogueEvents.Add(ev);
        dialogueFrame.SetActive(true);
        NextSentence();
    }

    public void NextSentence()
    {
        if(curDialogue != null && curSentenceIndex < curDialogue.sentences.Length)
        {
            var curSentence = curDialogue.sentences[curSentenceIndex];
            showNextSentenceEvent?.Invoke();
            if(dialogueEvents.Count > 0 && curSentence.triggerEvent) // Запускает первое в очереди событие и стерает его
            {
                dialogueEvents[0].PlayEvent();
                dialogueEvents.Remove(dialogueEvents[0]);
            }
            SpawnSentence(curSentence.text, curSentence.fmodSoundPath, curSentence.duration);
            curSentenceIndex++;
        }
        else
        {
            EndDialogue();
        }
    }

    private void SpawnSentence(string sentenceText, string fmodSoundPath, float delayBeforeShowNext)
    {
        GameObject sentence = Instantiate(sentencePref, contentTransform);
        sentences.Add(sentence);

        SentenceShower sentenceShower = sentence.GetComponent<SentenceShower>();
        sentenceShower.dialogueHandler = this;
        sentenceShower.refSentenceText = sentenceText;
        sentenceShower.fmodSoundPath = fmodSoundPath;
        sentenceShower.delayBeforeShowNext = delayBeforeShowNext;
    }

    private void ClearSentences()
    {
        foreach(GameObject sentence in sentences)
            Destroy(sentence);
        sentences.Clear();
        showNextSentenceEvent?.RemoveAllListeners();
        dialogueEvents?.Clear();
    }

    public void EndDialogue()
    {
        ClearSentences();
        dialogueFrame.SetActive(false);
        curDialogue = null;
        if(playImage.activeSelf)
        {
            postProcessingVolume.weight = 0;
            playImage.SetActive(false);
        }
        GamePauser.ContinueGame(gameObject);
    }
}
