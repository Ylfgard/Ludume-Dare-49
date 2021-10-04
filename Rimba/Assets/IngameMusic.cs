using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameMusic : MonoBehaviour
{
    [SerializeField] private string musicEvent;

    FMOD.Studio.EventInstance instance;

    private void Start()
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        instance.start();
    }

    private void OnDestroy()
    {
        instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
