using UnityEngine;

namespace Rimba
{
    namespace Survival
    {
        public class FirstAidKit : MonoBehaviour, IInteractable
        {
            [SerializeField] private float healAmount = 40f;

            public string ItemName { get { return "Аптечка"; } }
            public string ItemDescription { get { return "Бесценный набор бинтов и таблеток."; } }

            public void Interact(PlayerController player)
            {
                player.health = Mathf.Max(player.health + healAmount, 0);
                Destroy(gameObject);
            }
        }
    }
}