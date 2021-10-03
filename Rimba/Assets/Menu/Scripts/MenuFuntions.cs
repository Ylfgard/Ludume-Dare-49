using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuFuntions : MonoBehaviour
{
    [SerializeField] private GameObject menuFone;
    [SerializeField] private GameObject settingsFone;
    [SerializeField] private Slider textShowSpeedSlider;

    public void LoadLevel(string levelName) 
    { 
        SceneManager.LoadScene(levelName); 
    }

    public void RestartLevel() 
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void OpenSettings()
    {
        settingsFone.SetActive(true);
    }
    
    public void ChangeTextShowSpeed()
    {
        float textShowSpeed = textShowSpeedSlider.value;
        PlayerPrefs.SetFloat("TextShowSpeed", textShowSpeed);
    }
    
    public void CloseSettings()
    {
        settingsFone.SetActive(false);
    }
    
    public void OpenMenu()
    {
        GamePauser.StopGame(gameObject);
        menuFone.SetActive(true);
    }

    public void CloseMenu()
    {
        GamePauser.ContinueGame(gameObject);
        CloseSettings();
        menuFone.SetActive(false);
    }

    public void Exit() 
    { 
        Application.Quit(); 
    }

    

    private void Start() 
    {
        CloseMenu();
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            if(menuFone.activeSelf) CloseMenu();
            else OpenMenu();
    }
    
    private void OnDestroy() 
    {
        CloseMenu();
    }
}
