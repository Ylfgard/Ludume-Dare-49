using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Rimba
{
    namespace Survival
    {
        [RequireComponent(typeof(Animator))]
        public class PlayerController : MonoBehaviour, IDamagable
        {
            [SerializeField] private new Camera camera;

            [Header("Movement")]
            [SerializeField] private float maxSpeed;

            [Header("Stats")]
            public float health = 100f;
            public float warmth = 100f;
            [SerializeField] private float warmthRate = 2f;
            [SerializeField] private float warmthDamageRate = 5f;
            public float radiation = 0f;
            [SerializeField] private float radiationRate = 2f;
            [SerializeField] private float radiationDamageRate = 5f;
            public float coolness = 100f;
            [SerializeField] private float coolnessRate = 2f;
            [SerializeField] private float coolnessDamageRate = 5f;

            private float lastTick;

            #region Animation
            private Animator animator;
            private bool animationRunning;
            private float animationTimeout;

            private int ANIMATOR_MOVE = Animator.StringToHash("Move");
            private int ANIMATOR_SWING = Animator.StringToHash("Swing");
            private int ANIMATOR_CARRYING_LOG = Animator.StringToHash("CarryingLog");
            #endregion

            #region Interaction
            [Header("Interaction")]
            [SerializeField] private float interactionWidth = 1f;
            [SerializeField] private LayerMask interactionLayerMask;

            [HideInInspector] public IInteractable selectedInteractable;
            private Collider2D[] hits;
            #endregion

            [HideInInspector] public bool carryingLog;
            [HideInInspector] public float logFuelAmount;

            void Start()
            {
                animator = GetComponent<Animator>();
                animationRunning = false;
                animationTimeout = -1;

                hits = new Collider2D[10];

                lastTick = Mathf.Round(Time.time);

                carryingLog = false;
            }

            void Update()
            {
                CheckAnimationTimeout();
                selectedInteractable = FindInteractable();

                HandleInput();
                UpdateStats();
            }

            void CheckAnimationTimeout()
            {
                if (animationRunning && animationTimeout < Time.time)
                {
                    animationRunning = false;
                }
            }

            void HandleInput()
            {
                if (animationRunning)
                    return;

                Vector3 lookDirection = camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg, Vector3.forward);

                Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
                if (movement.sqrMagnitude > 0.1f)
                {
                    transform.position += ((Vector3)movement) * maxSpeed * Time.deltaTime;
                    animator.SetBool(ANIMATOR_MOVE, true);
                }
                else
                {
                    animator.SetBool(ANIMATOR_MOVE, false);
                }

                if (Input.GetButtonDown("Fire1") && !carryingLog)
                {
                    animator.SetTrigger(ANIMATOR_SWING);
                }

                if (Input.GetButtonDown("Interact") && selectedInteractable != null)
                {
                    selectedInteractable.Interact(this);
                }

                animator.SetBool(ANIMATOR_CARRYING_LOG, carryingLog);
            }

            void UpdateStats()
            {
                warmth = Math.Max(warmth - warmthRate * Time.deltaTime, 0f);
                radiation = Math.Min(radiation + radiationRate * Time.deltaTime, 100f);
                coolness = Math.Max(coolness - coolnessRate * Time.deltaTime, 0f);

                float time = Mathf.Floor(Time.time);
                if (time == lastTick)
                    return;
                
                lastTick = time;
                if (warmth <= 0f)
                {
                    health = Mathf.Max(health - warmthDamageRate, 0f);
                }
                if (radiation >= 100f)
                {
                    health = Mathf.Max(health - radiationDamageRate, 0f);
                }
                if (coolness <= 0f)
                {
                    health = Mathf.Max(health - coolnessDamageRate, 0f);
                }
            }

            IInteractable FindInteractable()
            {
                int pickupCount = Physics2D.OverlapBoxNonAlloc(
                    transform.position + transform.right * 0.8f, Vector2.one * interactionWidth, 0, hits, interactionLayerMask
                );
                for (int i=0; i < pickupCount; i++)
                {
                    if (hits[i].isTrigger)
                        continue;
                    
                    IInteractable item = hits[i].GetComponent<IInteractable>();
                    if (item != null)
                        return item;
                }
                return null;
            }

            public void ApplyDamage(float damage)
            {
                health = Mathf.Max(health - damage, 0f);
            }

            /*
            public void OnAnimationStarted()
            {
                animationTimeout = Time.time + 5f;
                animationRunning = true;
            }

            public void OnAnimationFinished()
            {
                animationRunning = false;
            }
            */

#if UNITY_EDITOR
            void OnDrawGizmosSelected() {
                Handles.color = Color.blue;
                Handles.DrawWireCube(transform.position + transform.right * 0.8f, Vector2.one * interactionWidth);
            }
#endif
        }
    }
}

