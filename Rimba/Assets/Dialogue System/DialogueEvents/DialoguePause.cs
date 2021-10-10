using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePause : MonoBehaviour, IDialogueEvent
{
    public void PlayEvent(float duration)
    {
        FindObjectOfType<DialogueHandler>().PausePlayerEffect();
    }
}
