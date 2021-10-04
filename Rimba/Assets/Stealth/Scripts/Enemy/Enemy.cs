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

        [SerializeField] private GameObject body, bodyDead;
        [SerializeField] private MeshFilter fovMF;

        [Header("")]
        [SerializeField] private GameObject hero;

        [SerializeField] private Transform[] waypoints;

        private Mesh fovMesh;
        private Transform targetWaypoint;
        private Vector3 lookDirection;

        private int currWaypointNdx;
        private float changeActionTimer;
        private bool isGameOver;
        private bool canMove;

        private void Start()
        {
            body.SetActive(true);
            bodyDead.SetActive(false);

            StartCoroutine(StandingCoroutine(standStillTime));


            fovMesh = new Mesh();
            fovMF.mesh = fovMesh;

            DrawFieldOfView();
        }

        private void Update()
        {
            if(!isGameOver)
            {
                DrawFieldOfView();
                Perception();
                Patrolling();
            }
        }

        private void DrawFieldOfView()
        {
            int rayCount = 50;
            float angle = 0f;
            float angleIncreases = fieldOfView / rayCount;
            Vector3 origin = Vector3.zero;

            Vector3[] verticies = new Vector3[rayCount + 2];
            Vector2[] uv = new Vector2[verticies.Length];
            int[] triangles = new int[rayCount * 3];

            verticies[0] = origin;

            int vertexIndex = 1;
            int triangleIndex = 0;
            for(int i = 0; i <= rayCount; i++)
            {
                float angleRadian = angle * Mathf.PI / 180f;
                Vector3 v = new Vector3(Mathf.Cos(angleRadian), Mathf.Sin(angleRadian));
                Vector3 vertex;

                RaycastHit2D hit = Physics2D.Raycast(origin, origin + v, lookRange);
                if(hit.collider == null)
                {
                    vertex = origin + v * lookRange;
                }
                else
                {
                    vertex = hit.point;
                }
                verticies[vertexIndex] = vertex;

                if(i > 0)
                {
                    triangles[triangleIndex + 0] = 0;
                    triangles[triangleIndex + 1] = vertexIndex - 1;
                    triangles[triangleIndex + 2] = vertexIndex;
                    triangleIndex += 3;
                }

                angle -= angleIncreases;
                vertexIndex++;
            }

            fovMF.mesh.vertices = verticies;
            fovMF.mesh.uv = uv;
            fovMF.mesh.triangles = triangles;
        }

        private void Perception()
        {
            Vector3 vToHero = hero.transform.position - transform.position;
            
            if(vToHero.magnitude < hearRange)
            {
                StealthStageManager.S.GameOverAndRestart();
                isGameOver = true;
            }

            if(vToHero.magnitude < lookRange)
            {
                float angle = Vector3.Angle(lookDirection, vToHero);
                if(angle <= fieldOfView / 2)
                {
                    if(Physics2D.Raycast(transform.position, hero.transform.position - transform.position).collider.gameObject.CompareTag("Player"))
                    {
                        Debug.DrawRay(transform.position, hero.transform.position - transform.position, Color.yellow);

                        StealthStageManager.S.GameOverAndRestart();
                        isGameOver = true;
                    }
                }
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

            Gizmos.color = Color.white;
            for(int i = 0; i < waypoints.Length; i++)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[((i + 1 < waypoints.Length) ? i + 1 : 0)].position);
            }
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

        public void Died()
        {
            Destroy(gameObject);
        }

    }
}
