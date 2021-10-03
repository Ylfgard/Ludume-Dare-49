using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text gameOverText;
    public bool isGameActive = true;
    
    void Start()
    {
        gameOverText.enabled = false;
    }
    
    void Update()
    {
        if (PlayerController.HP <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        isGameActive = false;
        gameOverText.enabled = true;
    }
}