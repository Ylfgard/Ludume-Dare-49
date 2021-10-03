using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float knifeRange = 6f;
    public float pebbleRange = 8f;

    [SerializeField] private float cooldown = 1f;

    [SerializeField] private GameObject knifeInHand;
    [SerializeField] private GameObject pebbleInHand;
    [SerializeField] private GameObject knifeProjectilePrefab;
    [SerializeField] private GameObject pebbleProjectilePrefab;
    [SerializeField] private LineRenderer trajectory;

    private bool canAim = true;
    private bool isKnifeAiming, isPebbleAiming;
    private bool isLMBDown, isRMBDown;
    private float lastThrowTime;

    public bool isCooldown
    {
        get
        {
            if(Time.time - lastThrowTime > cooldown)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    private void Start()
    {
        EnableTrajectory(false);
    }

    private void Update()
    {
        AimAndThrow();
        DrawTrajectory();
    }

    private void AimAndThrow()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isLMBDown = true;

            if(canAim && !isCooldown)
            {
                EnableTrajectory(true);
                knifeInHand.SetActive(true);
                isKnifeAiming = true;
                canAim = false;
            }
            else
            {
                EnableTrajectory(false);
                pebbleInHand.SetActive(false);
                isPebbleAiming = false;
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            isLMBDown = false;

            if(isKnifeAiming)
            {
                EnableTrajectory(false);
                knifeInHand.SetActive(false);
                isKnifeAiming = false;
                // Throw knife
                InitThrowing(knifeProjectilePrefab);
            }

            if(!isRMBDown)
            {
                canAim = true;
            }
        }


        if(Input.GetMouseButtonDown(1))
        {
            isRMBDown = true;

            if(canAim && !isCooldown)
            {
                EnableTrajectory(true);
                pebbleInHand.SetActive(true);
                isPebbleAiming = true;
                canAim = false;
            }
            else
            {
                EnableTrajectory(false);
                knifeInHand.SetActive(false);
                isKnifeAiming = false;
            }
        }
        if(Input.GetMouseButtonUp(1))
        {
            isRMBDown = false;

            if(isPebbleAiming)
            {
                EnableTrajectory(false);
                pebbleInHand.SetActive(false);
                isPebbleAiming = false;
                // Throw pebble
                InitThrowing(pebbleProjectilePrefab);
            }

            if(!isLMBDown)
            {
                canAim = true;
            }
        }
    }

    private void EnableTrajectory(bool visible)
    {
        trajectory.enabled = visible;
    }

    private void DrawTrajectory()
    {
        if(trajectory.enabled)
        {
            trajectory.SetPosition(0, transform.position);

            Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos = new Vector3(targetPos.x, targetPos.y, transform.position.z);

            Vector3 dist = targetPos - transform.position;
            dist = transform.position + Vector3.ClampMagnitude(dist, (isKnifeAiming ? knifeRange : pebbleRange));

            trajectory.SetPosition(1, dist);
        }
    }

    private void InitThrowing(GameObject projectilePrefab)
    {
        lastThrowTime = Time.time;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector3(mousePos.x, mousePos.y, transform.position.z);
        Vector3 direction = Vector3.Normalize(mousePos - transform.position);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));

        GameObject proj = Instantiate(projectilePrefab, transform.position, rotation);

        if(proj.TryGetComponent(out Pebble p))
        {
            p.endPos = mousePos;
        }
    }
}
