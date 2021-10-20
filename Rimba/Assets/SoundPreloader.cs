using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPreloader : MonoBehaviour
{
    public static SoundPreloader sp;

    private void Awake()
    {
        if(sp != null)
        {
            Destroy(gameObject);
            return;
        }

        sp = this;
        DontDestroyOnLoad(gameObject);

        FMODUnity.RuntimeManager.LoadBank("Master");
        //Debug.Log(FMODUnity.RuntimeManager.HasBankLoaded("Master"));
    }
}
