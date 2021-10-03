using System.Collections;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private TextAsset dialogueText;
    private DialogueHandler dialogueHandler;

    public void TriggerDialogue()
    {
        dialogueHandler = FindObjectOfType<DialogueHandler>();
        IDialogueEvent[]  dialogueEvents = gameObject.GetComponents<IDialogueEvent>();
        dialogueHandler.StartDialogue(dialogueText, dialogueEvents, true);
    }
}
