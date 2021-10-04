using UnityEngine;
using UnityEngine.UI;

namespace Rimba
{
    namespace Survival
    {
        public class GameController : MonoBehaviour
        {
            [SerializeField] private PlayerController player;

            [Header("UI")]
            [SerializeField] private Image healthBar;
            [SerializeField] private Image coldBar;
            [SerializeField] private Image radiationBar;
            [SerializeField] private Image coolnessBar;

            [Header("Item info")]
            [SerializeField] private Text itemNameText;
            [SerializeField] private Text itemDescriptionText;

            [Header("Misc")]
            [SerializeField] private GameObject gameOverScreen;

            private IInteractable lastInteractable;

            void Start()
            {
                lastInteractable = null;
            }

            void Update()
            {
                healthBar.fillAmount = player.health / 100f;
                coldBar.fillAmount = player.warmth / 100f;
                radiationBar.fillAmount = player.radiation / 100f;
                coolnessBar.fillAmount = player.coolness / 100f;

                if (player.selectedInteractable != lastInteractable)
                {
                    itemNameText.text = player.selectedInteractable?.ItemName;
                    itemDescriptionText.text = player.selectedInteractable?.ItemDescription;

                    lastInteractable = player.selectedInteractable;
                }

                if (player.health <= 0)
                {
                    gameOverScreen.SetActive(true);
                    Time.timeScale = 0f;
                }
            }
        }
    }
}
