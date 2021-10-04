using UnityEngine;

namespace Rimba
{
    namespace Survival
    {
        public class Tree : MonoBehaviour, IDamagable
        {
            [SerializeField] private float health;
            [SerializeField] private GameObject trunk;
            [SerializeField] private GameObject logPrefab;

            private bool active;

            void Start()
            {
                active = true;
            }

            public void ApplyDamage(float amount)
            {
                if (!active)
                    return;
                
                health -= amount;
                if (health <= 0f)
                {
                    active = false;
                    Destroy(trunk);

                    Vector2 direction = (new Vector2(Random.value, Random.value)).normalized;

                    GameObject log = Instantiate(logPrefab, transform.position + (Vector3)direction, Quaternion.identity);
                    Rigidbody2D logRigidbody = log.GetComponent<Rigidbody2D>();
                    if (logRigidbody != null)
                    {
                        logRigidbody.velocity = direction;
                    }
                }
            }
        }
    }
}
