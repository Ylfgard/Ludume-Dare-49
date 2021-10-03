using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimWeapon : MonoBehaviour
{
    private Transform aimTransform;
    private Transform aimGunEndPointTransform;
    private SpriteRenderer weaponSprite;
    //private Animator aimAnimator;
    private void Awake()
    {
        aimTransform = transform.Find("Aim");
        //aimAnimator = aimTransform.GetComponent<Animator>();
        aimGunEndPointTransform = aimTransform.Find("GunEndPointPosition");
        weaponSprite = aimTransform.GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        HandleAiming();
        HandleShooting();
        aimTransform.gameObject.SetActive(true);
    }

    private void HandleAiming()
    {
        Vector3 mousePosition = GetPosition();
        aimTransform.LookAt(mousePosition);
        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 localScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            localScale.y = -1;
            //weaponSprite.sortingLayerName = "WeaponBehind";
        }
        else
        {
            localScale.y = 1;
            //weaponSprite.sortingLayerName = "WeaponFront";
        }
            aimTransform.localScale = localScale;
    }
    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = GetPosition();
            //aimAnimator.SetTrigger("Shoot");            
        }
    }    








    //public float GetAngleFromVectorFloat(Vector3 dir)
    //{
    //    dir = dir.normalized;
    //    float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    //    if (n < 0) n += 360;

    //    return n;
    //}

    public static Vector3 GetPosition()
    {
        Vector3 vec = GetPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    public static Vector3 GetPositionWithZ()
    {
        return GetPositionWithZ(Input.mousePosition, Camera.main);
    }
    public static Vector3 GetPositionWithZ(Camera worldCamera)
    {
        return GetPositionWithZ(Input.mousePosition, worldCamera);
    }
    public static Vector3 GetPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
