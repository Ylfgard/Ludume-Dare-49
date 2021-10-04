using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Rimba
{
    namespace Survival
    {
        [RequireComponent(typeof(Rigidbody2D))]
        public class Wolf : MonoBehaviour, IDamagable
        {
            [SerializeField] private float detectionRadius = 8f;
            [SerializeField] private LayerMask detectionLayerMask;
            [SerializeField] private float maxSpeed = 4f;
            [SerializeField] private float health = 20f;

            [SerializeField] private float roamDistance = 5f;
            [SerializeField] public Collider2D roamArea;

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

            private bool attacking;
            private float attackNext = 0;

            private bool roaming;
            private Vector3 roamTarget;

            private new Rigidbody2D rigidbody;

            void Start()
            {
                rigidbody = GetComponent<Rigidbody2D>();
                target = null;

                roaming = false;
                attacking = false;

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

                if (target is null)
                {
                    Roam();
                    LookForVictim();
                }
                else
                {
                    speed = Mathf.Lerp(speed, maxSpeed, 0.6f * Time.deltaTime);

                    Vector3 targetPosition = target.transform.position;
                    Vector3 direction = (targetPosition - transform.position).normalized;
                    transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);

                    float distance = Vector3.Distance(target.transform.position, transform.position);
                    if (attacking)
                    {
                        if (distance > attackRange)
                        {
                            MoveToTarget(1f);
                        }
                        else
                        {
                            target.ApplyDamage(attackDamage);
                            attacking = false;
                            attackNext = Time.time + attackCooldown;
                        }
                    }
                    // TODO: implement better logic to keep distance
                    else if (distance > followDistanceMax)
                    {
                        MoveToTarget((followDistanceMin + followDistanceMax) * 0.5f);
                    }
                    else if (distance < followDistanceMin)
                    {
                        MoveToTarget((followDistanceMin + followDistanceMax) * 0.5f);
                    }
                    else if (Vector3.Dot(target.transform.right, transform.right) > -0.5f && Time.time > attackNext)
                    {
                        attacking = true;
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
                    roaming = false;
                }
            }

            void Roam()
            {
                if (roaming)
                {
                    Vector3 roamDirection = (roamTarget - transform.position);
                    if (roamDirection.sqrMagnitude < 0.2f)
                    {
                        roaming = false;
                    }
                    else
                    {
                        Vector3 lateral = new Vector3(roamDirection.y, -roamDirection.x, roamDirection.z);
                        Vector3 newDirection = roamDirection + lateral * Random.value;
                        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(roamDirection.y, roamDirection.x) * Mathf.Rad2Deg, Vector3.forward);
                        rigidbody.MovePosition(transform.position + (newDirection).normalized * maxSpeed * Time.deltaTime);
                    }
                }
                else if (Random.value < 0.01f)
                {
                    for (int i=0; i < 5; i++)
                    {
                        roamTarget = transform.position + new Vector3(Random.value, Random.value, 0f) * roamDistance;
                        if (roamArea.OverlapPoint(roamTarget))
                        {
                            roaming = true;
                            break;
                        }
                    }                    
                }

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

            public void ApplyDamage(float damage)
            {
                health = Mathf.Max(health - damage, 0f);
                attacking = false;
            }

            void OnDrawGizmosSelected() {
                // Handles.color = Color.yellow;
                // Handles.DrawWireDisc(transform.position, Vector3.forward, detectionRadius);
                //
                // Handles.color = Color.red;
                // Handles.DrawWireDisc(transform.position, Vector3.forward, followDistanceMax);
                // Handles.DrawWireDisc(transform.position, Vector3.forward, followDistanceMin);
            }
        }
    }
}
