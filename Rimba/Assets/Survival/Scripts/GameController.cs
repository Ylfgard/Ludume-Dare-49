using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
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

            [Header("Dawn")]
            [SerializeField] private float timeTillDawn;
            [SerializeField] private float timeTillDay;
            [SerializeField] private Light2D globalLight;
            [SerializeField] private float nightLightIntensity;
            [SerializeField] private float dayLightIntensity;

            [Header("Misc")]
            [SerializeField] private GameObject gameOverScreen;
            [SerializeField] private GameObject winScreen;

            private IInteractable lastInteractable;
            private float dawnTime;
            private float dayTime;

            void Start()
            {
                lastInteractable = null;
                globalLight.intensity = nightLightIntensity;
                dawnTime = Time.time + timeTillDawn;
                dayTime = Time.time + timeTillDay;
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

                globalLight.intensity = (Time.time < dawnTime) ? nightLightIntensity :
                    Mathf.Lerp(nightLightIntensity, dayLightIntensity, Mathf.Min((Time.time - dawnTime) / (dayTime - dawnTime), 1f));

                if (player.health <= 0)
                {
                    gameOverScreen.SetActive(true);
                    Time.timeScale = 0f;
                }

                if (Time.time >= dayTime)
                {
                    winScreen.SetActive(true);
                    Time.timeScale = 0f;
                }
            }
        }
    }
}
