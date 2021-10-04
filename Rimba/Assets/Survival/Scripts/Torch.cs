using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Rimba
{
    namespace Survival
    {
        public class Torch : MonoBehaviour
        {
            [SerializeField] private new Light2D light;

            private float originalInnerRadius;
            private float originalOuterRadius;

            private float noise = 0;
            void Start()
            {
                originalInnerRadius = light.pointLightInnerRadius;
                originalOuterRadius = light.pointLightOuterRadius;
            }

            void Update()
            {
                noise = Mathf.Lerp(noise, Random.Range(-0.2f, 0.2f), 0.4f);
                light.pointLightInnerRadius = originalInnerRadius + noise;
                light.pointLightOuterRadius = originalOuterRadius + noise;
            }
        }
    }
}