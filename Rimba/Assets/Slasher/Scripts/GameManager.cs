using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public bool isGameActive = true;
    
    public GameObject gameOverText;
    public Text battleTimerText;
    public GameObject firstRoom;
    public GameObject mainHall;

    public string currentScene;
    public int enemyCounter = 11;

    public GameObject daVinchi;
    public GameObject enemy;
    public GameObject medicine;
    public Vector2[] spawnPos = new Vector2[]
    {
        new Vector2(-10, 9) , new Vector2(9, 9),
        new Vector2(-10, -2), new Vector2(8 ,-7)
    };

    public GameObject firstRoomEnemiesGroup;

    [SerializeField] private TextAsset firstDialogue;
    [SerializeField] private TextAsset secondDialogue;

    private DialogueHandler dialogueHandler;
    private float battleTimer = 60.0f; // в секундах
    
    void Start()
    {
        dialogueHandler = FindObjectOfType<DialogueHandler>();
        gameOverText.SetActive(false);
        
        firstRoom.SetActive(true);
        mainHall.SetActive(false);

        currentScene = "First Room";

        StartCoroutine(EnemySpawner());
        StartCoroutine(MedicineSpawner());

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
            battleTimerText.text = "";
        }
        else
        {
            BattleTimerUpdate();
        }

        if (enemyCounter == 0 && currentScene == "Throne Room")
        {
            IDialogueEvent [] diaEv = new IDialogueEvent[0];
            dialogueHandler.StartDialogue(secondDialogue, diaEv, true);
            // начать диалог
            currentScene = "Bossfight";
            Instantiate(daVinchi, new Vector3(-0.37f, 6, 0), Quaternion.identity);

            // переход на боссфайт

            FindObjectOfType<IngameMusic>().SetMusic("event:/music/DMC");
        }
    }

    public void GameOver()
    {
        isGameActive = false;
        gameOverText.SetActive(true);

        // Затычка с рестартом всего уровня...
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        IDialogueEvent [] diaEv = new IDialogueEvent[0];
        dialogueHandler.StartDialogue(firstDialogue, diaEv, true);
        FindObjectOfType<PlayerController>().GetComponent<Transform>().position = new Vector3(-1.0f, -9.0f, 0);
        mainHall.SetActive(true);
        currentScene = "Throne Room";
    }

    IEnumerator EnemySpawner()
    {
        while (currentScene == "First Room")
        {
            Instantiate(enemy, spawnPos[Random.Range(0, 4)], Quaternion.identity, firstRoomEnemiesGroup.transform);
            enemy.GetComponent<EnemyController>().enemyType = (EnemyController.EnemyType) Random.Range(0, 3);

            float randomTime = Random.Range(4.0f, 6.0f);
            yield return new WaitForSeconds(randomTime);
        }
    }

    IEnumerator MedicineSpawner()
    {
        while (currentScene == "First Room")
        {
            Instantiate(medicine, spawnPos[Random.Range(0, 4)], Quaternion.identity);
            
            float randomTime = Random.Range(10.0f, 25.0f);
            yield return new WaitForSeconds(randomTime);
        }
    }

    public void GoToCredits()
    {
        // Переход на последние титры
    }
}