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
                    return;

                damagable.ApplyDamage(damage);
            }
        }
    }
}