using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameMusic : MonoBehaviour
{
    [SerializeField] private string musicEvent;
    [SerializeField] private bool keepPlayingInNextScene;

    private FMOD.Studio.EventInstance instance;

    private void Start()
    {
        PlayMusic(musicEvent);

        SceneManager.activeSceneChanged += OnSceneChange;

        if(keepPlayingInNextScene)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
    }

    public void PlayMusic(string musicEvent)
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        //instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        instance.start();
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        if(keepPlayingInNextScene)
        {
            keepPlayingInNextScene = false;
        }
        else
        {
            DestroyMusic();
        }
    }

    private void DestroyMusic()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
        Destroy(gameObject);
    }
}
