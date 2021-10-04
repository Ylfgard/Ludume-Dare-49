using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour, IDialogueEvent
{
    [SerializeField] private string levelName;
    [SerializeField] private bool dontStopGame;
    [SerializeField] private float levelLoadDelay;

    public void PlayEvent()
    {
        if(!dontStopGame)
        {
            GamePauser.StopGame(gameObject);
            FindObjectOfType<DialogueHandler>().PausePlayerEffect();
        } 
        StartCoroutine(DelayedLoad());
    }

    private IEnumerator DelayedLoad()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        FindObjectOfType<SceneChanger>().ChangeScene(levelName);
    }
}
