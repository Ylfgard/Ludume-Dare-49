using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour, IDialogueEvent
{
    [SerializeField] private string levelName;
    [SerializeField] private bool dontStopGame;
    [SerializeField] private float levelLoadDelay;

    public void PlayEvent(float duration)
    {
        if(!dontStopGame)
        {
            GamePauser.StopGame(gameObject);
            FindObjectOfType<DialogueHandler>().PausePlayerEffect();
        } 
        levelLoadDelay = duration;
        StartCoroutine(DelayedLoad(levelLoadDelay));

        // Temp shooter pistol sound problem solving
        //MainCharacterLogic logic = FindObjectOfType<MainCharacterLogic>();
        //if(logic != null && logic.TryGetComponent(out MainCharacterLogic mcl))
        //{
        //    mcl.eInst.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        //}
    }

    private IEnumerator DelayedLoad(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        FindObjectOfType<SceneChanger>().ChangeScene(levelName);
    }
}
