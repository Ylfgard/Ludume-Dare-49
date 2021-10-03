using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElusiveRimba
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float speed = 100f;
        [SerializeField] private float standTime = 3f;
        [SerializeField] private Transform[] waypoints;

        private GameObject hero;
        private Transform targetWaypoint;

        private int currWaypointNdx;
        private float changeActionTimer;
        private bool canMove;

        //private bool canMove
        //{
        //    get
        //    {
        //        if(changeActionTimer >= Time.time)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //}

        private void Awake()
        {
            hero = GameObject.FindGameObjectWithTag("Player");
        }

        private void Start()
        {
            StartCoroutine(StandingCoroutine(standTime));
        }

        private void Update()
        {
            Patrolling();
        }

        private void Patrolling()
        {
            //if(isMoving && (targetWaypoint.transform.position - transform.position).magnitude <= 0.1f)
            //{
            //    isMoving = false;
            //    changeActionTimer = Time.time + standTime;
            //}

            if(canMove)
            {
                Move(targetWaypoint);
            }
        }

        private void Move(Transform target)
        {
            //Vector3 direction = (target.transform.position - transform.position).normalized;
            //transform.Translate(direction * speed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

            if((target.transform.position - transform.position).magnitude <= 0.1f)
            {
                //isMoving = false;
                canMove = false;
                changeActionTimer = Time.time + standTime;
                StartCoroutine(StandingCoroutine(standTime));
            }
        }

        private IEnumerator StandingCoroutine(float time)
        {
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
            Debug.Log("Waypoint: " + currWaypointNdx);
        }
    }
}
