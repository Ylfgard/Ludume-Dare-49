using UnityEngine;

namespace Rimba
{
    namespace Survival
    {
        public class FirstAidKit : MonoBehaviour, IInteractable
        {
            public string ItemName { get { return "First Aid Kit"; } }
            public string ItemDescription { get { return "Invaluable set of bandaids and pills."; } }

            public void Interact(PlayerController player)
            {
                player.health = Mathf.Max(player.health + 20f, 0);
                Destroy(gameObject);
            }
        }
    }
}