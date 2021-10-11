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
            [SerializeField] private Text survivalTimerText;

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
            [SerializeField] private MenuFuntions menuFuntions;
            [SerializeField] private DialogueTrigger startDialogue;
            [SerializeField] private GameObject gameOverScreen;
            [SerializeField] private float restartLevelDelay = 2f;
            [SerializeField] private DialogueTrigger winDialogue;

            private IInteractable lastInteractable;
            private float dawnTime;
            private float dayTime;
            private float survivalTime;

            void Start()
            {
                survivalTimerText.enabled = false;
                lastInteractable = null;
                globalLight.intensity = nightLightIntensity;
                dawnTime = Time.time + timeTillDawn;
                dayTime = Time.time + timeTillDay;
            }

            void TimerTextUpdate()
            {
                survivalTime = dayTime - Time.time;
                if(survivalTime < 0) survivalTime = 0;
                int minutes = Mathf.FloorToInt(survivalTime / 60F);
                int seconds = Mathf.FloorToInt(survivalTime % 60F);
                survivalTimerText.text = minutes.ToString ("00") + ":" + seconds.ToString ("00");
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
                    menuFuntions.RestartLevel(restartLevelDelay);
                    gameOverScreen.SetActive(true);
                    Time.timeScale = 0f;
                }

                if(survivalTimerText.enabled == false && Time.time >= timeTillDawn)
                {
                    startDialogue.TriggerDialogue();
                    TimerTextUpdate();
                    survivalTimerText.enabled = true;
                }
                else
                {
                    TimerTextUpdate();
                }

                if (Time.time >= dayTime)
                {
                    Destroy(gameObject);
                    winDialogue.TriggerDialogue();
                }
            }
        }
    }
}
