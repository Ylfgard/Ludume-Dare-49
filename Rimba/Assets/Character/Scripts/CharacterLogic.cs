using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLogic : MonoBehaviour
{
    public CharacterData characterData;
    private Rigidbody2D m_rigidBody;
    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
    }
    void Move()
    {

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized;
        Vector2 velocity = m_rigidBody.velocity;
        {
            velocity = movement * characterData.Speed;                                     //Обычная скорость            
        }
        m_rigidBody.velocity = velocity;

        //if (velocity.x != 0 || velocity.y != 0)
        //{
        //    m_animator.SetBool("move", true);
        //}
        //else
        //{
        //    m_animator.SetBool("move", false);
        //}
    }
    private void Update()
    {
        Move();
    }
}
