using System.Collections;
using UnityEngine;
using ElusiveRimba;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private TextAsset dialogueText;
    [SerializeField] private bool stopTime;
    private DialogueHandler dialogueHandler;

    public void TriggerDialogue()
    {
        dialogueHandler = FindObjectOfType<DialogueHandler>();
        IDialogueEvent[]  dialogueEvents = gameObject.GetComponents<IDialogueEvent>();
        dialogueHandler.StartDialogue(dialogueText, dialogueEvents, stopTime);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.GetComponent<Hero>())
        {
            TriggerDialogue();
            gameObject.GetComponent<Collider2D>().enabled = false;
        } 
    }
}
