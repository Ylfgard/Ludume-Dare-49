using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
        public GameObject character; // Персонаж
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
*/

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
                        transform.Translate(dir * speed);*/
                }

                
        }

        private void LookAtMouse()
    {
        var direction = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position); // Нахождение катетов для расчёта тангенса, а в последствии и градусов угла. 
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Нахождение тангенса угла и перевод его в градусы.
        tr.rotation = Quaternion.Slerp(tr.rotation, Quaternion.Euler(0, 0, angle), 1); //Поворот объекта


    }

}

