using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverText;
    public Text battleTimerText;
    public bool isGameActive = true;

    private float battleTimer = 120.0f; // в секундах
    
    void Start()
    {
        gameOverText.SetActive(false);
    }
    
    void Update()
    {
        if (PlayerController.HP <= 0)
        {
            GameOver();
        }

        if (battleTimer <= 0)
        {
            battleTimerText.fontSize = 40;
            battleTimerText.text =
                "Не успел, ну а хули ты хотел, 2 минуты всего, надо было поторапливаться, а сейчас-то хули смысла горевать, давай загружайся и заново";
        }
        else
        {
            BattleTimerUpdate();
        }
    }

    public void GameOver()
    {
        isGameActive = false;
        gameOverText.SetActive(true);
    }

    void BattleTimerUpdate()
    {
        battleTimer -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(battleTimer / 60F);
        int seconds = Mathf.FloorToInt(battleTimer % 60F);
        battleTimerText.text = minutes.ToString ("00") + ":" + seconds.ToString ("00");
    }
}