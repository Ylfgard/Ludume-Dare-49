using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuFuntions : MonoBehaviour
{
    [SerializeField] private bool isStartMenu;
    [SerializeField] private GameObject menuFone;
    [SerializeField] private GameObject settingsFone;
    [SerializeField] private Slider textShowSpeedSlider;
    [SerializeField] private SceneChanger sceneChanger;

    public void LoadLevel(string levelName) 
    { 
       sceneChanger.ChangeScene(levelName);
    }

    public void LoadLastPlayedLevel() 
    { 
        string loadedLevelName;
        loadedLevelName = PlayerPrefs.GetString("lastGameScene", "MainMenu");
        sceneChanger.ChangeScene(loadedLevelName);
    }

    public void RestartLevel() 
    { 
        sceneChanger.ChangeScene(SceneManager.GetActiveScene().name); 
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
        CloseSettings();
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
        if(isStartMenu) 
        {
            OpenMenu();
        }
        else 
        {
            PlayerPrefs.SetString("lastGameScene", SceneManager.GetActiveScene().name);
            CloseMenu();
        }
    }

    private void Update() 
    {
        if(!isStartMenu && Input.GetKeyDown(KeyCode.Escape))
            if(menuFone.activeSelf) CloseMenu();
            else OpenMenu();
    }
    
    private void OnDestroy() 
    {
        CloseMenu();
    }
}
