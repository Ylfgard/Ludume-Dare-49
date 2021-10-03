using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElusiveRimba
{
    public class Pebble : MonoBehaviour
    {
        [SerializeField] private float speed = 20;
        [SerializeField] private float rotationSpeed = 100f;

        private float maxRange, range;
        private bool isResting;
        private Vector3 startPos;
        private Rigidbody2D rb;
        private Collider2D coll;

        public Vector3 endPos { get; set; }

        private void Awake()
        {
            maxRange = GameObject.FindGameObjectWithTag("Player").GetComponent<Weapon>().pebbleRange;
            rb = GetComponent<Rigidbody2D>();
            coll = GetComponent<Collider2D>();
        }

        private void Start()
        {
            startPos = transform.position;

            float r = (endPos - startPos).magnitude;
            range = r < maxRange ? r : maxRange;
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            float currPosRange = (transform.position - startPos).magnitude;
            if(currPosRange < range)
            {
                transform.Translate(Vector3.up * speed * Time.fixedDeltaTime, Space.Self);
            }
            else
            {
                // Projectile ranged stop actions
                // Fall on ground sound
                ProjectileStopped();
            }
        }

        private void ProjectileStopped()
        {
            rb.Sleep();
            coll.enabled = false;
            enabled = false;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            switch(other.gameObject.tag)
            {
                case "Enemy":
                    // When pebble hits enemy?
                    ProjectileStopped();
                    break;

                case "Obstacle":
                    // Obstacle hit sound (same as ground hit sound?)
                    ProjectileStopped();
                    break;

                default:
                    // Other collisions
                    break;
            }
        }
    }
}
