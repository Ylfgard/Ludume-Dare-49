using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float speed = 6.0f;
    public static int HP = 1000;

    private Rigidbody2D playerRb;
    private Vector2 mouse;
    private new Camera camera;
    private GameManager gameManager;
    public Text HPText;

    public GameObject sword;
    public GameObject spear;
    public GameObject hammer;

    [SerializeField] private float hammerCharge = 0;
    [SerializeField] private int medicineHeal = 30;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        camera = Camera.main;
        
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    
    void Start()
    {
        HPText.text = "HP: " + HP;
    }
    
    void FixedUpdate()
    {
        if (gameManager.isGameActive)
        {
            MovePlayer();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SwordAttack();
        }
        
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        { 
            SpearAttack();
        }
        
        else if (Input.GetKey(KeyCode.Mouse2))
        {
            hammerCharge += Time.deltaTime;
        }
        
        if (Input.GetKeyUp(KeyCode.Mouse2))
        {
            if (hammerCharge >= 3.0f)
            {
                HammerAttack();
            }
            hammerCharge = 0;
        }
    }

    void MovePlayer()
    {
        // Перемещение
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        transform.Translate(Vector2.right * (inputX * speed * Time.deltaTime), Space.World);
        transform.Translate(Vector2.up * (inputY * speed * Time.deltaTime), Space.World);
        
        // Вращение
        Vector3 dir = Input.mousePosition - camera.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    // При получении урона или поднятия хилки
    public void UpdateHP(int HPDifference)
    {
        HP += HPDifference;
        HPText.text = "HP: " + HP;
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Medicine"))
        {
            UpdateHP(medicineHeal);
            Destroy(other.gameObject);
        }
    }
}