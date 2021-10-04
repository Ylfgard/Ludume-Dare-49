using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour, IDialogueEvent
{
    [SerializeField] private string levelName;
    [SerializeField] private float levelLoadDelay;

    public void PlayEvent()
    {
        StartCoroutine(DelayedLoad());
    }

    private IEnumerator DelayedLoad()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        SceneManager.LoadScene(levelName);
    }
}
