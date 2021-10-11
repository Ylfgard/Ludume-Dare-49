using UnityEngine;

namespace Rimba
{
    namespace Survival
    {
        public class Slash : MonoBehaviour
        {
            [SerializeField] private float damage;

            void OnTriggerEnter2D(Collider2D other) {
                IDamagable damagable = other.GetComponent<IDamagable>();
                if (damagable is null)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/whoosh_rock_throw", gameObject);
                    return;
                }

                damagable.ApplyDamage(damage);
            }
        }
    }
}