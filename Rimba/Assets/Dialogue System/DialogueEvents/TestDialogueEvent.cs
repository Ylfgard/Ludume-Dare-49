using UnityEngine;

public class TestDialogueEvent : MonoBehaviour, IDialogueEvent
{
    [SerializeField] private string massage;
    public void PlayEvent()
    {
        Debug.Log(massage);
    }
}
