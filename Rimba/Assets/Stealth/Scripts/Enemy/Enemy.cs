using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElusiveRimba
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float speed = 100f;
        [SerializeField] private float standStillTime = 3f;

        [SerializeField] private float lookRange = 10f;
        [SerializeField] private float hearRange = 3f;
        [SerializeField] private float fieldOfView = 90f;

        [Header("")]
        [SerializeField] private GameObject hero;
        [SerializeField] private Transform[] waypoints;

        private Transform targetWaypoint;
        private Vector3 lookDirection;

        private int currWaypointNdx;
        private float changeActionTimer;
        private bool isGameOver;
        private bool canMove;

        private void Start()
        {
            StartCoroutine(StandingCoroutine(standStillTime));
        }

        private void Update()
        {
            if(!isGameOver)
            {
                Perception();
                Patrolling();
            }
        }

        private void Perception()
        {
            Vector3 vToHero = hero.transform.position - transform.position;
            
            if(vToHero.magnitude < lookRange)
            {
                float angle = Vector3.Angle(lookDirection, vToHero);
                if(angle <= fieldOfView / 2)
                {
                    if(Physics2D.Raycast(transform.position, hero.transform.position - transform.position).collider.gameObject.CompareTag("Player"))
                    {
                        Debug.DrawRay(transform.position, hero.transform.position - transform.position, Color.yellow);

                        //Game over
                        StealthStageManager.S.GameOverAndRestart();
                        Debug.LogWarning("RESTART LEVEL");
                        isGameOver = true;
                    }
                }
            }
            else if(vToHero.magnitude < hearRange)
            {
                StealthStageManager.S.GameOverAndRestart();
            }
        }

        private void ChangeLookDirection()
        {
            lookDirection = (waypoints[(currWaypointNdx + 1) % waypoints.Length].position - transform.position).normalized;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, lookDirection * lookRange + transform.position);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, hearRange);
        }

        private void Patrolling()
        {
            if(canMove)
            {
                Move(targetWaypoint);
            }
        }

        private void Move(Transform target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

            if((target.transform.position - transform.position).magnitude <= 0.1f)
            {
                canMove = false;
                changeActionTimer = Time.time + standStillTime;
                StartCoroutine(StandingCoroutine(standStillTime));
            }
        }

        private IEnumerator StandingCoroutine(float time)
        {
            ChangeLookDirection();

            yield return new WaitForSeconds(time);

            ChangeToNextTargetWaypoint();
            canMove = true;
        }

        private void ChangeToNextTargetWaypoint()
        {
            if(currWaypointNdx < (waypoints.Length - 1))
            {
                currWaypointNdx++;
            }
            else
            {
                currWaypointNdx = 0;
            }

            targetWaypoint = waypoints[currWaypointNdx];
        }
    }
}
