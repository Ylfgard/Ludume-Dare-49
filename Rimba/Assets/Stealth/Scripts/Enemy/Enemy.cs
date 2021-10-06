using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElusiveRimba
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float speed = 100f;
        [SerializeField] private float standInPatroolTime = 3f;
        [SerializeField] private float standDistructedTime = 3f;

        [SerializeField] private float lookRange = 10f;
        [SerializeField] private float hearRange = 3f;
        [SerializeField] private float fieldOfView = 90f;

        [SerializeField] private GameObject body;
        [SerializeField] private GameObject fovPrefab;

        [Header("Enemy body sprites:")]
        [SerializeField] Sprite face;
        [SerializeField] Sprite back;
        [SerializeField] Sprite left;
        [SerializeField] Sprite right;

        [Header("")]
        [SerializeField] private GameObject hero;

        [SerializeField] private Transform[] waypoints;
        [SerializeField] private LayerMask fovLayerMask;

        private SpriteRenderer sr;
        private Mesh fovMesh;
        private MeshFilter fovMF;
        private Transform targetWaypoint;
        private Vector3 lookDirection;
        private Vector3 origin;
        private float startAngle;

        private int currWaypointNdx;
        private float changeActionTimer;
        private bool _isGameOver;
        private bool canMove;
        private bool isStanding, isDistracted;

        public bool isGameOver
        {
            get
            {
                return _isGameOver;
            }
            private set
            {
                _isGameOver = value;
                StealthStageManager.S.isGameOver = _isGameOver;
            }
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

        private void Awake()
        {
            sr = GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            StartCoroutine(StandingInPatroolCoroutine(Random.Range(0.1f, 5f)));
            targetWaypoint = waypoints[currWaypointNdx];

            fovMesh = new Mesh();
            fovMF = Instantiate(fovPrefab).GetComponent<MeshFilter>();
            fovMF.mesh = fovMesh;
        }

        private void Update()
        {
            if(!isGameOver)
            {
                AnimateBody();
                Perception();
                Patrolling();

                PosAndDirOfFieldOfView();
            }
        }

        private void LateUpdate()
        {
            DrawFieldOfView();
            fovMF.gameObject.transform.position = transform.position;
        }

        private void DrawFieldOfView()
        {
            int rayCount = 50;
            float angle = startAngle;
            float angleIncreases = fieldOfView / rayCount;

            Vector3[] verticies = new Vector3[rayCount + 2];
            Vector2[] uv = new Vector2[verticies.Length];
            int[] triangles = new int[rayCount * 3];

            verticies[0] = Vector3.zero;

            int vertexIndex = 1;
            int triangleIndex = 0;
            for(int i = 0; i <= rayCount; i++)
            {
                float angleRadian = angle * Mathf.PI / 180f;
                Vector3 v = new Vector3(Mathf.Cos(angleRadian), Mathf.Sin(angleRadian));
                Vector3 vertex;

                RaycastHit2D hit = Physics2D.Raycast(origin, v, lookRange, fovLayerMask);
                if(hit.collider == null)
                {
                    vertex = v * lookRange;
                }
                else
                {
                    vertex = ((Vector3)hit.point) - origin;
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
            fovMF.mesh.RecalculateBounds();
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
                if(angle <= fieldOfView * 0.5f)
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

        private void UpdateLookAtNextWaypointDirection()
        {
            lookDirection = (waypoints[(currWaypointNdx) % waypoints.Length].position - transform.position).normalized;
        }

        private void PosAndDirOfFieldOfView()
        {
            origin = transform.position;
            startAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            if(startAngle < 0)
                startAngle += 360;
            startAngle = startAngle + fieldOfView * 0.5f;
        }

        public void Distract(Pebble pebble)
        {
            if(isDistracted)
                return;

            isDistracted = true;
            canMove = false;

            Vector3 lookAtPebbleDir = (pebble.gameObject.transform.position - transform.position).normalized;
            lookDirection = lookAtPebbleDir;

            StopAllCoroutines();
            StartCoroutine(StandDistructedCoroutine());
        }

        private IEnumerator StandDistructedCoroutine()
        {
            yield return new WaitForSeconds(standDistructedTime);
            isDistracted = false;
            canMove = true;

            UpdateLookAtNextWaypointDirection();
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
                isStanding = true;
                //changeActionTimer = Time.time + standInPatroolTime;
                StartCoroutine(StandingInPatroolCoroutine(standInPatroolTime));
            }
        }

        private IEnumerator StandingInPatroolCoroutine(float time)
        {
            lookDirection = (waypoints[(currWaypointNdx + 1) % waypoints.Length].position - transform.position).normalized;

            yield return new WaitForSeconds(time);

            ChangeToNextTargetWaypoint();
            UpdateLookAtNextWaypointDirection();

            canMove = true;
            isStanding = false;
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



        private void AnimateBody()
        {
            Vector3 look = lookDirection;
            if(look.y > -look.x && look.y > look.x)
            {
                sr.sprite = back;
            }

            else if(look.y > -look.x && look.y < look.x)
            {
                sr.sprite = right;
            }

            else if(look.y < -look.x && look.y < look.x)
            {
                sr.sprite = face;
            }

            else if(look.y < -look.x && look.y > look.x)
            {
                sr.sprite = left;
            }
        }

        public void Died()
        {
            Destroy(gameObject);
            Destroy(fovMF.gameObject);
        }
    }
}
