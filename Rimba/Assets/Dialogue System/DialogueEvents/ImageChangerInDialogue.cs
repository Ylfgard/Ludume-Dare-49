using UnityEngine.UI;
using UnityEngine;

public class ImageChangerInDialogue : MonoBehaviour, IDialogueEvent
{
    [SerializeField] private Image image;

    public void PlayEvent(float duration)
    {
        image.enabled = true;
    }
}
