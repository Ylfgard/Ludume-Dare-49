using UnityEngine;

namespace Rimba
{
    namespace Survival
    {
        public class Vodka : MonoBehaviour, IInteractable
        {
            [SerializeField] private float antidotAmount = 50f;

            public string ItemName { get { return "Водка"; } }
            public string ItemDescription { get { return "Универсальное средство от всех отравлений!"; } }

            public void Interact(PlayerController player)
            {
                player.radiation = Mathf.Max(player.radiation - antidotAmount, 0);
                Destroy(gameObject);
            }
        }
    }
}