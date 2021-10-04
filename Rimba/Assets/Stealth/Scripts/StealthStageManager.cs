using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class StealthStageManager : MonoBehaviour
{
    public static StealthStageManager S;

    [SerializeField] private float restartDelay = 2f;

    [SerializeField] private GameObject ingameUI;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI knivesNumberText;

    private bool _isGameOver;

    public bool isGameOver
    {
        get
        {
            return _isGameOver;
        }
        set
        {
            _isGameOver = value;
        }
    }

    private void Start()
    {
        if(S == null)
        {
            S = this;
        }
    }

    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.Backspace))
    //    {
    //        RestartLevel();
    //    }
    //}

    public void GameOverAndRestart()
    {
        ingameUI.SetActive(false);
        gameOverPanel.SetActive(true);
        StartCoroutine(GameOverAndRestartCoroutine());
    }
    private IEnumerator GameOverAndRestartCoroutine()
    {
        yield return new WaitForSeconds(restartDelay);
        RestartLevel();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void KnivesUIRefresh(int knives)
    {
        knivesNumberText.text = knives.ToString();
    }
}
