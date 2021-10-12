using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Rimba
{
    namespace Survival
    {
        public class Torch : MonoBehaviour
        {
            [SerializeField] private new Light2D light;
            [SerializeField] private Light2D globalLight;
            [SerializeField] private Transform torchInHand;
            [SerializeField] private GameObject bonfire;

            private float originalInnerRadius;
            private float originalOuterRadius;

            private float noise = 0;

            private Light2D bonfireLight;
            private const float torchValue = 0.5f;
            private float ndxLight;

            void Start()
            {
                if(bonfire != null)
                {
                    bonfire.TryGetComponent(out bonfireLight);

                    ndxLight = light.pointLightOuterRadius / bonfireLight.pointLightOuterRadius;
                    ndxLight = Mathf.Clamp(ndxLight, 0.2f, 0.8f);
                }

                originalInnerRadius = light.pointLightInnerRadius;
                originalOuterRadius = light.pointLightOuterRadius;

            }

            void Update()
            {
                noise = Mathf.Lerp(noise, Random.Range(-0.2f, 0.2f), 0.4f);
                light.pointLightInnerRadius = originalInnerRadius + noise;
                light.pointLightOuterRadius = originalOuterRadius + noise;

                torchInHand.localScale = Vector2.one * (torchValue + noise * torchValue);

                if(bonfire != null)
                {
                    float distToBonfire = Vector3.Magnitude(bonfire.transform.position - transform.position);
                    if(distToBonfire <= bonfireLight.pointLightOuterRadius)
                    {
                        float value = Mathf.Clamp(bonfireLight.pointLightOuterRadius - (bonfireLight.pointLightOuterRadius / light.pointLightOuterRadius), 0.1f, float.MaxValue);
                        value = distToBonfire / value * ndxLight * (1 - globalLight.intensity);
                        light.intensity = Mathf.Clamp(value, 0.1f, 1f);

                    }
                }
            }
        }
    }
}