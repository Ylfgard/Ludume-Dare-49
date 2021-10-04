using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElusiveRimba
{

    public class Hero : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;

        [Header("Rimba body sprites:")]
        [SerializeField] Sprite face;
        [SerializeField] Sprite back;
        [SerializeField] Sprite left;
        [SerializeField] Sprite right;

        private Rigidbody2D rb;
        private SpriteRenderer sr;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponentInChildren<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            Move();
            AnimateBody();
        }

        private void Move()
        {
            Vector2 velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed * Time.fixedDeltaTime;

            rb.velocity = velocity;

        }

        private void AnimateBody()
        {
            Vector3 mouseRel = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            if(mouseRel.y > -mouseRel.x && mouseRel.y > mouseRel.x)
            {
                sr.sprite = back;
            }

            else if(mouseRel.y > -mouseRel.x && mouseRel.y < mouseRel.x)
            {
                sr.sprite = right;
            }

            else if(mouseRel.y < -mouseRel.x && mouseRel.y < mouseRel.x)
            {
                sr.sprite = face;
            }

            else if(mouseRel.y < -mouseRel.x && mouseRel.y > mouseRel.x)
            {
                sr.sprite = left;
            }
        }
    }
}
