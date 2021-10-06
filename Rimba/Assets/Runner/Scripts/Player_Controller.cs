using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
        private new Camera camera;
        public float speed = 6.0f;
        public Sprite[] directions;
        private Vector2 pos; // Текущая позиция
        private SpriteRenderer spriteRenderer;
        public Vector2 mouseRel; // Координаты курсора относительно игрока

        private Rigidbody2D playerRb;
        private Vector2 mouse;

        Vector3 dir = Vector3.zero;

        public float DistanseOfJump;


         private void Awake()
        {
                spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                playerRb = GetComponent<Rigidbody2D>();
                camera = Camera.main;
        }

        void FixedUpdate()
        {
            mouse = camera.ScreenToWorldPoint(Input.mousePosition);
            MovePlayer();
            if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                        //gameObject.Transform.position += tr.right * DistanseOfJump;

                        dir = (mouse - (Vector2)transform.position).normalized;
                        transform.Translate(dir * DistanseOfJump);
                }
        }

        void MovePlayer()
        {
                // Перемещение
                float inputX = Input.GetAxis("Horizontal");
                float inputY = Input.GetAxis("Vertical");

                transform.Translate(Vector2.right * (inputX * speed * Time.fixedDeltaTime), Space.World);
                transform.Translate(Vector2.up * (inputY * speed * Time.fixedDeltaTime), Space.World);

                // Вращение спрайта за курсором
                pos = transform.position;
                mouseRel = mouse - pos;
                if (mouseRel.y > -mouseRel.x && mouseRel.y > mouseRel.x)
                {
                spriteRenderer.sprite = directions[2]; // Up
                }

                else if (mouseRel.y > -mouseRel.x && mouseRel.y < mouseRel.x)
                {
                spriteRenderer.sprite = directions[3]; // Right
                }

                else if (mouseRel.y < -mouseRel.x && mouseRel.y < mouseRel.x)
                {
                spriteRenderer.sprite = directions[0]; // Down
                }
                
                else if (mouseRel.y < -mouseRel.x && mouseRel.y > mouseRel.x)
                {
                spriteRenderer.sprite = directions[1]; // Left
                }

                // Вращение
                // Vector3 dir = Input.mousePosition - camera.WorldToScreenPoint(transform.position);
                // float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg/* - 90*/;
                // transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
}



        /*public GameObject character; // Персонаж
        public float speed, DistanseOfJump; // Скорость перемещения
        private Rigidbody2D m_rigidBody;
        Vector3 dir = Vector3.zero;

        Transform tr;

    private void Awake()
        {
                m_rigidBody = GetComponent<Rigidbody2D>();  

                tr=gameObject.GetComponent<Transform>();
    }
       
        // Update is called once per frame
        void Update () {
/*
                if(Input.GetKey(KeyCode.W))
                        character.transform.Translate(Vector2.up*speed*Time.deltaTime);
                if(Input.GetKey(KeyCode.S))
                        character.transform.Translate(Vector2.down*speed*Time.deltaTime);
                if(Input.GetKey(KeyCode.D))
                        character.transform.Translate(Vector2.right*speed*Time.deltaTime);
                if(Input.GetKey(KeyCode.A))
                        character.transform.Translate(Vector2.left*speed*Time.deltaTime);


                float moveHorizontal = Input.GetAxis("Horizontal");
                float moveVertical = Input.GetAxis("Vertical");
                Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized;
                Vector2 velocity = m_rigidBody.velocity;
                velocity = movement * speed;         
                m_rigidBody.velocity = velocity;
                
                LookAtMouse();

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                        tr.position += tr.right * DistanseOfJump;

                        /*Vector2 m_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        dir = (m_pos - (Vector2)transform.position).normalized;
                        transform.Translate(dir * speed);
                }

                
        }

        private void LookAtMouse()
    {
        var direction = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position); // Нахождение катетов для расчёта тангенса, а в последствии и градусов угла. 
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Нахождение тангенса угла и перевод его в градусы.
        tr.rotation = Quaternion.Slerp(tr.rotation, Quaternion.Euler(0, 0, angle), 1); //Поворот объекта


    }
}*/

