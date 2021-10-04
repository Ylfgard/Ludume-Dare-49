using UnityEngine;

namespace Rimba
{
    namespace Survival
    {
        public class Vodka : MonoBehaviour, IInteractable
        {
            public string ItemName { get { return "Vodka"; } }
            public string ItemDescription { get { return "Universal cure for all kinds of poisoning."; } }

            public void Interact(PlayerController player)
            {
                player.radiation = Mathf.Max(player.radiation - 20f, 0);
                Destroy(gameObject);
            }
        }
    }
}