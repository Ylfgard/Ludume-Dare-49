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
        if(FMODUnity.RuntimeManager.HasBankLoaded("Master"))
            PlayMusic(musicEvent);


        if(keepPlayingInNextScene)
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.activeSceneChanged += OnSceneChange;
        }
    }

    public void PlayMusic(string musicEvent)
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        //instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        instance.start();
        instance.release();
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

    private void OnDestroy()
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
    }
}
