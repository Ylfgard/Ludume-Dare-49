using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public int HP;
    public enum EnemyType { sword, spear, hammer }
    public EnemyType enemyType;
    public bool needToChase = false;

    private Transform target;
    private GameManager gameManager;
    private Text enemyHPText;
    private GameObject player;
    private bool playerIsNear;
    
    public GameObject sword;
    public GameObject spear;
    public GameObject hammer;
    
    public Sprite[] directions;
    private SpriteRenderer spriteRenderer;
    private Vector2 playerRelPos;

    private void Awake()
    {
        player = GameObject.Find("Player");
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        enemyHPText = GameObject.Find("Enemy HP Text").GetComponent<Text>();
        target = player.transform;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    
    void Start()
    {
        enemyHPText.text = "HP: " + HP;
        
        StartCoroutine(RandomAttack());
    }
    
    void Update()
    {
        if (gameManager.isGameActive && needToChase)
        {
            // Враг всегда движется в сторону игрока
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            
            // И смотрит в его сторону
            // Vector2 dir = player.transform.position;
            // float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            // transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            playerRelPos = player.transform.position - gameObject.transform.position;
            if (playerRelPos.y > -playerRelPos.x && playerRelPos.y > playerRelPos.x)
            {
                spriteRenderer.sprite = directions[2]; // Up
            }

            else if (playerRelPos.y > -playerRelPos.x && playerRelPos.y < playerRelPos.x)
            {
                spriteRenderer.sprite = directions[3]; // Right
            }

            else if (playerRelPos.y < -playerRelPos.x && playerRelPos.y < playerRelPos.x)
            {
                spriteRenderer.sprite = directions[0]; // Down
            }
        
            else if (playerRelPos.y < -playerRelPos.x && playerRelPos.y > playerRelPos.x)
            {
                spriteRenderer.sprite = directions[1]; // Left
            }
        }
    }
    
    // При получении урона или поднятия хилки меняем хп
    public void UpdateHP(int HPDifference)
    {
        HP += HPDifference;
        enemyHPText.text = "Enemy HP: " + HP;
        
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    // Если игрок в зоне видимости
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }
    
    void SwordAttack()
    {
        Instantiate(sword, gameObject.transform);
    }

    void SpearAttack()
    {
        Instantiate(spear, gameObject.transform);
    }
    
    void HammerAttack()
    {
        Instantiate(hammer, gameObject.transform);
    }

    // Нужная атака через рандомный промежуток времени
    IEnumerator RandomAttack()
    {
        while (gameManager.isGameActive)
        {
            float randomTime = 1;
            if (playerIsNear)
            {
                switch (enemyType)
                {
                    case EnemyType.sword:
                        SwordAttack();
                        randomTime = Random.Range(1, 3);
                        break;
                    
                    case EnemyType.spear:
                        SpearAttack();
                        randomTime = Random.Range(2, 4);
                        break;
                    
                    case EnemyType.hammer:
                        HammerAttack();
                        randomTime = Random.Range(4, 5);
                        break;
                }
            }
            yield return new WaitForSeconds(randomTime);
        }
    }
}