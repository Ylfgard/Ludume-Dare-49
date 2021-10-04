using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isGameActive = true;
    
    public GameObject gameOverText;
    public Text battleTimerText;

    public GameObject firstRoom;
    public GameObject mainHall;
    public GameObject mainHall2;

    private float battleTimer = 120.0f; // в секундах
    
    void Start()
    {
        gameOverText.SetActive(false);
        
        firstRoom.SetActive(true);
        mainHall.SetActive(false);
        mainHall2.SetActive(false);

        StartCoroutine(ToMainHall());
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
            battleTimerText.text = "Тут типа диалог";
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

    IEnumerator ToMainHall()
    {
        yield return new WaitForSeconds(battleTimer);
        firstRoom.SetActive(false);
        mainHall.SetActive(true);
    }
}