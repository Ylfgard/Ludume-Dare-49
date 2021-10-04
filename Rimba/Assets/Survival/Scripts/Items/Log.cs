using UnityEngine;

namespace Rimba
{
    namespace Survival
    {
        public class Log : MonoBehaviour, IInteractable
        {
            public float fuelAmount = 30f;

            public string ItemName { get { return "Log"; } }
            public string ItemDescription { get { return "A wooden log. Can be used as bonfire fuel."; } }

            public void Interact(PlayerController player)
            {
                if (player.carryingLog)
                    return;

                player.carryingLog = true;
                player.logFuelAmount = fuelAmount;
                Destroy(gameObject);
            }
        }
    }
}