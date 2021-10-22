using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundPreloader : MonoBehaviour
{
    public static SoundPreloader sp;

    [SerializeField] private Slider loadingSlider;

    private float loadingValue;

    //private void Awake()
    //{
    //    if(sp != null)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }

    //    sp = this;
    //    DontDestroyOnLoad(gameObject);

    //    //FMODUnity.RuntimeManager.LoadBank("Master");
    //    //Debug.Log(FMODUnity.RuntimeManager.HasBankLoaded("Master"));
    //}

    private void Start()
    {
        StartCoroutine(LoadAsync());
    }

    private IEnumerator LoadAsync()
    {
        AsyncOperation aLoading = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        aLoading.allowSceneActivation = false;

        FMODUnity.RuntimeManager.LoadBank("Master", true);

        while(!aLoading.isDone)
        {
            loadingSlider.value = loadingValue + Mathf.Clamp01(aLoading.progress / 0.9f) - 0.3f;
            Debug.Log(loadingSlider.value);

            if(loadingValue == 0 && !FMODUnity.RuntimeManager.AnyBankLoading())
            {
                loadingValue = 0.3f;
                Debug.Log("Master bank loading COMPLETE");
            }

            if(loadingSlider.value >= 0.9f)
            {
                yield return new WaitForSeconds(1);
                aLoading.allowSceneActivation = true;
            }
            
            yield return null;
        }
    }
}
