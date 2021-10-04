using UnityEngine;

namespace Rimba
{
    namespace Survival
    {
        public class Log : MonoBehaviour, IInteractable
        {
            public float fuelAmount = 30f;

            public string ItemName { get { return "Полено"; } }
            public string ItemDescription { get { return "Деревянно полено. Им можно что нибудь растапливать."; } }

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