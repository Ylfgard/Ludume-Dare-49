using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DaVinchiMechController : MonoBehaviour
{
    public float speed;
    public int HP;
    [SerializeField] private TextAsset finalDialogue;
    private Transform target;
    private GameManager gameManager;
    private GameObject player;
    public GameObject mechAttack;

    public Sprite[] directions;
    private SpriteRenderer spriteRenderer;
    private Vector2 playerRelPos;

    private void Awake()
    {
        player = GameObject.Find("Player");
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        target = player.transform;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    
    void Start()
    {
        StartCoroutine(RandomAttack());
    }

    private void OnEnable() 
    {
        StartCoroutine(RandomAttack());
    }
    
    void Update()
    {
        if (gameManager.isGameActive)
        {
            // Враг всегда движется в сторону игрока
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

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

        if (HP <= 0)
        {
            GameObject.Find("EndDialogue").GetComponent<DialogueTrigger>().TriggerDialogue();
            Destroy(gameObject);
            gameManager.GoToCredits();
        }
    }
    
    void MechAttack()
    {
        Instantiate(mechAttack, gameObject.transform);
    }

    // Атака через рандомный промежуток времени
    IEnumerator RandomAttack()
    {
        while (gameManager.isGameActive)
        {
            float randomTime = 1;
            MechAttack();
            randomTime = Random.Range(1, 3);
            yield return new WaitForSeconds(randomTime);
        }
    }
}
