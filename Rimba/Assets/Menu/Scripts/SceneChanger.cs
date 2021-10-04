using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    private string changOnScene;
    private Image image;
    private Color col = Color.black;

    private void Start() 
    {
        image = gameObject.GetComponent<Image>();
        image.enabled = false;
        col.a = 0;
        image.color =col;
    }

    public void ChangeScene(string sceneName)
    {
        changOnScene = sceneName;
        StartCoroutine(ShowDarkScreen());
        image.enabled = true;
    }

    private IEnumerator ShowDarkScreen()
    {
        yield return new WaitForEndOfFrame();
        image.color = col;
        col.a = col.a + 0.005f;
        if(col.a < 1) StartCoroutine(ShowDarkScreen());
        else LoadScene();
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(changOnScene);
    }
}
