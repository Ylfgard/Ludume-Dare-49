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
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider dialogsVolumeSlider;
    [SerializeField] private SceneChanger sceneChanger;

    private FMOD.Studio.Bus musicBus;
    private FMOD.Studio.Bus dialogsBus;

    public void SetVolume()
    {
        float musicVolume = musicVolumeSlider.value;
        float dialogsVolume = dialogsVolumeSlider.value;
        musicBus.setVolume(musicVolume);
        dialogsBus.setVolume(dialogsVolume);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("dialogsVolume", dialogsVolume);
        PlayerPrefs.Save();
    }

    public void GetVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat("musicVolume");
        float dialogsVolume = PlayerPrefs.GetFloat("dialogsVolume");
        musicVolumeSlider.value = musicVolume;
        dialogsVolumeSlider.value = dialogsVolume;
    }

    public void LoadLevel(string levelName) 
    { 
       sceneChanger.ChangeScene(levelName);
    }

    public void LoadLastPlayedLevel() 
    { 
        string loadedLevelName;
        loadedLevelName = PlayerPrefs.GetString("lastGameScene", "StartComics");
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

    private void Awake()
    {
        musicBus = FMODUnity.RuntimeManager.GetBus("bus:/music");
        dialogsBus = FMODUnity.RuntimeManager.GetBus("bus:/dialogs");
    }

    private void Start() 
    {
        GetVolume();

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
