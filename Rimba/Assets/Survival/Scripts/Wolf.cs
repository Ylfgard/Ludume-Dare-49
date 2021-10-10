using System.Runtime.InteropServices.ComTypes;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public static class Vector3LateralHelper
{
    public static Vector3 lateral(this Vector3 direction)
    {
        return new Vector3(direction.y, -direction.x, direction.z);
    }
}

namespace Rimba
{
    namespace Survival
    {
        [RequireComponent(typeof(Rigidbody2D))]
        public class Wolf : MonoBehaviour, IDamagable
        {

            enum State
            {
                Idle,
                Roaming,
                AvoidingBonfire,
                Chasing,
                Attacking,
                Waiting
            }

            [SerializeField] private float detectionRadius = 8f;
            [SerializeField] private LayerMask detectionLayerMask;
            [SerializeField] private float maxSpeed = 4f;
            [SerializeField] private float health = 20f;

            [SerializeField] private float roamDistance = 5f;
            [SerializeField] public Collider2D roamArea;

            [SerializeField] public Bonfire bonfire;
            [SerializeField] private float bonfireDistanceOffset;

            [SerializeField] private float followDistanceMin = 2f;
            [SerializeField] private float followDistanceMax = 2f;
            [SerializeField] private float attackRange = 1f;
            [SerializeField] private float attackRate = 0.25f;
            [SerializeField] private float attackDamage = 10f;
            [SerializeField] private float attackCooldown = 2f;

            [SerializeField] private float coolnessReward = 40f;

            private Collider2D[] hits;
            private PlayerController target;
            private float speed;

            private State state;
            private float attackNext = 0;
            private Vector3 roamTarget;
            private float waitTimeout = 0;

            private new Rigidbody2D rigidbody;

            void Start()
            {
                rigidbody = GetComponent<Rigidbody2D>();
                target = null;

                state = State.Idle;

                hits = new Collider2D[5];
            }

            void Update()
            {
                if (health <= 0)
                {
                    if (target)
                    {
                        target.coolness += coolnessReward;
                    }
                    Destroy(gameObject);
                    return;
                }

            }

            void UpdateState()
            {
                switch (state)
                {
                    case State.AvoidingBonfire:
                    {
                        speed = Mathf.Lerp(speed, maxSpeed, 0.6f * Time.deltaTime);

                        Vector3 direction = (roamTarget - transform.position);
                        if (direction.sqrMagnitude < 0.2f)
                        {
                            state = (target == null) ? State.Idle : State.Waiting;
                            if (state == State.Waiting)
                            {
                                waitTimeout = Time.time + Random.Range(1f, 3f);
                            }
                        }
                        else
                        {
                            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);
                            rigidbody.MovePosition(transform.position + direction.normalized * speed * Time.deltaTime);
                        }
                        break;
                    }
                    case State.Roaming:
                    {
                        Vector3 roamDirection = (roamTarget - transform.position);
                        if (roamDirection.sqrMagnitude < 0.2f)
                        {
                            state = State.Idle;
                        }
                        else
                        {
                            if (IsCloseToBonfire())
                            {
                                roamTarget = PositionAwayFromBonfire(2f);
                                state = State.AvoidingBonfire;
                            }
                            else
                            {
                                Vector3 lateral = new Vector3(roamDirection.y, -roamDirection.x, roamDirection.z);
                                Vector3 newDirection = roamDirection + lateral * Random.value;
                                transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(roamDirection.y, roamDirection.x) * Mathf.Rad2Deg, Vector3.forward);
                                rigidbody.MovePosition(transform.position + (newDirection).normalized * maxSpeed * Time.deltaTime);
                            }
                        }

                        LookForVictim();
                        break;
                    }
                    case State.Idle:
                    {
                        if (Random.value < 0.01f)
                        {
                            if (FindNewRoamLocation())
                            {
                                state = State.Roaming;
                            }
                        }

                        LookForVictim();

                        break;
                    }
                    case State.Chasing:
                    {
                        if (IsCloseToBonfire())
                        {
                            roamTarget = PositionAwayFromBonfire(2f);
                            state = State.AvoidingBonfire;
                            break;
                        }

                        speed = Mathf.Lerp(speed, maxSpeed, 0.6f * Time.deltaTime);

                        FaceTarget();

                        float distance = Vector3.Distance(target.transform.position, transform.position);
                        if (distance > followDistanceMax)
                        {
                            MoveToTarget((followDistanceMin + followDistanceMax) * 0.5f);
                        }
                        else if (distance < followDistanceMin)
                        {
                            MoveToTarget((followDistanceMin + followDistanceMax) * 0.5f);
                        }
                        else if (Vector3.Dot(target.transform.right, transform.right) > -0.5f && Time.time > attackNext)
                        {
                            state = State.Attacking;
                        }

                        break;
                    }
                    case State.Attacking:
                    {
                        if (IsCloseToBonfire())
                        {
                            roamTarget = PositionAwayFromBonfire(2f);
                            state = State.AvoidingBonfire;
                            break;
                        }

                        speed = Mathf.Lerp(speed, maxSpeed, 0.6f * Time.deltaTime);

                        FaceTarget();

                        float distance = Vector3.Distance(target.transform.position, transform.position);
                        if (distance > attackRange)
                        {
                            MoveToTarget(1f);
                        }
                        else
                        {
                            target.ApplyDamage(attackDamage);
                            state = State.Chasing;
                            attackNext = Time.time + attackCooldown;
                        }

                        break;
                    }
                    case State.Waiting:
                    {
                        if (Time.time > waitTimeout)
                        {
                            state = (target is null) ? State.Idle : State.Chasing;
                        }
                        break;
                    }
                }
            }

            void LookForVictim()
            {
                int hitsCount = Physics2D.OverlapCircleNonAlloc(transform.position, detectionRadius, hits, detectionLayerMask);
                for (int i=0; i < hitsCount; i++)
                {
                    if (!hits[i].CompareTag("Player"))
                        continue;

                    target = hits[i].gameObject.GetComponent<PlayerController>();
                    if (target != null)
                        break;
                }

                if (target != null)
                {
                    speed = 0;
                    state = State.Chasing;
                }
            }

            bool FindNewRoamLocation()
            {
                for (int i=0; i < 5; i++)
                {
                    roamTarget = transform.position + new Vector3(Random.value, Random.value, 0f) * roamDistance;
                    if (roamArea.OverlapPoint(roamTarget))
                    {
                        return true;
                    }
                }

                return false;
            }

            void FaceTarget()
            {
                Vector3 targetPosition = target.transform.position;
                Vector3 direction = (targetPosition - transform.position).normalized;
                transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);
            }

            void MoveToTarget(float range)
            {
                Vector3 direction = (target.transform.position - transform.position).normalized;

                float distance = Vector3.Distance(target.transform.position, transform.position);
                float sign = Mathf.Sign(distance - range);
                Vector3 lateral = new Vector3(direction.y, -direction.x, direction.z);
                Vector3 newDirection = (sign * direction * speed + lateral * Random.value).normalized;
                Vector3 newPosition = transform.position + newDirection * speed * Time.deltaTime;

                rigidbody.MovePosition(newPosition);
            }

            Vector3 PositionAwayFromBonfire(float amount)
            {
                Vector3 awayDirection = (transform.position - bonfire.transform.position).normalized;
                Vector3 lateralDirection = new Vector3(awayDirection.y, -awayDirection.x, awayDirection.z);
                return transform.position + awayDirection * amount + lateralDirection * Random.Range(-2f, 2f);
            }

            bool IsCloseToBonfire()
            {
                return Vector3.Distance(transform.position, bonfire.transform.position) < bonfire.CurrentRadius + bonfireDistanceOffset;
            }

            public void ApplyDamage(float damage)
            {
                health = Mathf.Max(health - damage, 0f);
                if (state == State.Attacking)
                {
                    state = State.Chasing;
                }
            }

#if UNITY_EDITOR
            void OnDrawGizmosSelected()
            {
                Handles.color = Color.yellow;
                Handles.DrawWireDisc(transform.position, Vector3.forward, detectionRadius);

                Handles.color = Color.red;
                Handles.DrawWireDisc(transform.position, Vector3.forward, followDistanceMax);
                Handles.DrawWireDisc(transform.position, Vector3.forward, followDistanceMin);
            }
#endif
        }
    }
}
