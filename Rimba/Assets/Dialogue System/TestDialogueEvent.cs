using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialogueEvent : MonoBehaviour, IDialogueEvent
{
    [SerializeField] private string massage;
    public void PlayEvent()
    {
        Debug.Log(massage);
    }
}
