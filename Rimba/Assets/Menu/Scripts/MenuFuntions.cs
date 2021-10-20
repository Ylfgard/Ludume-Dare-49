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
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        float dialogsVolume = PlayerPrefs.GetFloat("dialogsVolume", 0.7f);
        musicVolumeSlider.value = musicVolume;
        dialogsVolumeSlider.value = dialogsVolume;
    }

    public void LoadLevel(string levelName) 
    {
        if(isStartMenu == false)
            return;

        sceneChanger.ChangeScene(levelName);
    }

    public void LoadLastPlayedLevel() 
    {
        if(isStartMenu == false)
        {
            CloseMenu();
            return;
        }

        string loadedLevelName;
        loadedLevelName = PlayerPrefs.GetString("lastGameScene", "");
        if(loadedLevelName != "")
            sceneChanger.ChangeScene(loadedLevelName);
    }

    public void RestartLevel() 
    { 
        sceneChanger.ChangeScene(SceneManager.GetActiveScene().name); 
    }
    public void RestartLevel(float delay) 
    {
        StartCoroutine(RestartLevelCoroutine(delay));
    }
    private IEnumerator RestartLevelCoroutine(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        RestartLevel();
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
        if(!isStartMenu)
            GamePauser.StopGame(gameObject);
        CloseSettings();
        menuFone.SetActive(true);
    }

    public void CloseMenu()
    {
        if(!isStartMenu)
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
        if(FMODUnity.RuntimeManager.HasBankLoaded("Master"))
        {
            musicBus = FMODUnity.RuntimeManager.GetBus("bus:/music");
            dialogsBus = FMODUnity.RuntimeManager.GetBus("bus:/dialogs");
        }
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
