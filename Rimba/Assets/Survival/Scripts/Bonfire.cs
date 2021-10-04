using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Rimba
{
    namespace Survival
    {
        [RequireComponent(typeof(CircleCollider2D))]
        public class Bonfire : MonoBehaviour, IInteractable
        {
            [SerializeField] private new Light2D light;
            [SerializeField] private GameObject shape;
            [SerializeField] private float flameTwinklingMagnitude = 0.2f;

            [SerializeField] private float radius = 1f;
            [SerializeField] private float initialTime = 30f;
            [SerializeField] private float maxTime = 30f;

            [Space]
            [SerializeField] private float warmRadius = 1f;
            [SerializeField] private float warmRate = 2f;

            private float burnoutTime;
            private bool burnedOut;

            private float originalInnerRadius;
            private float originalOuterRadius;
            private float noise = 0;

            private new CircleCollider2D collider;
            private PlayerController player;

            void Start() {
                originalInnerRadius = radius * 0.5f;
                originalOuterRadius = radius;
                
                burnedOut = false;
                burnoutTime = Time.time + initialTime;

                collider = GetComponent<CircleCollider2D>();
                collider.radius = warmRadius;

                player = null;
            }

            void Update()
            {
                if (burnedOut)
                    return;

                float intensity = (Time.time > burnoutTime) ? 0f : (burnoutTime - Time.time) / maxTime;
                if (intensity <= 0) {
                    burnedOut = true;
                    return;
                }

                if (player != null)
                {
                    player.warmth += warmRate * Time.deltaTime;
                }

                noise = Mathf.Lerp(noise, Random.Range(-1f, 1f) * flameTwinklingMagnitude, 0.4f);
                light.pointLightInnerRadius = originalInnerRadius * intensity + noise;
                light.pointLightOuterRadius = originalOuterRadius * intensity + noise;
                shape.transform.localScale = Vector3.one * intensity;
                collider.radius = warmRadius * intensity;
            }

            public void AddTime(float time) {
            }

            public void OnTriggerEnter2D(Collider2D collider)
            {
                if (!collider.CompareTag("Player"))
                    return;

                player = collider.GetComponent<PlayerController>();
            }

            void OnDrawGizmosSelected() {
                Handles.color = Color.yellow;
                Handles.DrawWireDisc(transform.position, Vector3.forward, radius);

                Handles.color = Color.red;
                Handles.DrawWireDisc(transform.position, Vector3.forward, warmRadius);
            }

            #region IInteractable
            public string ItemName { get { return "Bonfire"; } }
            public string ItemDescription { get { return "A good thing to stay warm and keep predators away in a middle of a night."; } }

            public void Interact(PlayerController player)
            {
                if (!player.carryingLog)
                    return;

                burnoutTime = Mathf.Min(burnoutTime + 10f, Time.time + maxTime);
                player.carryingLog = false;
            }
            #endregion
        }
    }
}