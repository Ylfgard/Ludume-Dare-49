using System.Collections;
using UnityEngine;
using ElusiveRimba;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private TextAsset dialogueText;
    [SerializeField] private bool stopTime;
    [SerializeField] private bool playOnStart;
    private DialogueHandler dialogueHandler;

    private void Start()
    {
        dialogueHandler = FindObjectOfType<DialogueHandler>();
        if(playOnStart) TriggerDialogue();
    }

    public void TriggerDialogue()
    {
        IDialogueEvent[] dialogueEvents = gameObject.GetComponents<IDialogueEvent>();
        dialogueHandler.StartDialogue(dialogueText, dialogueEvents, stopTime);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.GetComponent<Hero>() || other.CompareTag("Player"))
        {
            TriggerDialogue();
            gameObject.GetComponent<Collider2D>().enabled = false;
        } 
    }
}
