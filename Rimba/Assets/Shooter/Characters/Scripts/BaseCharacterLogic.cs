using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterLogic : MonoBehaviour
{
    public CharacterData characterData;
    private Transform aimTransform;
    protected Rigidbody2D m_rigidBody;
    public GameObject hitPoint;
    
    //private Animator aimAnimator;
    
    protected bool isReloadingWeapon = false;
    protected Coroutine reloadingWeaponCoroutine;
    private void Awake()
    {
        aimTransform = transform.Find("Aim"); 
        m_rigidBody = GetComponent<Rigidbody2D>();
        characterData.Weapon = GetComponentInChildren<Weapon>();
    }
    
    protected void Aim(Vector3 target)
    {
        aimTransform.LookAt(target);
        Vector3 aimDirection = (target - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 localScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            localScale.y = -1;
        }
        else
        {
            localScale.y = 1;
        }
        aimTransform.localScale = localScale;

        //if(weaponSprite != null)
        //{
        //    if (Vector3.Dot(Vector3.up, aimTransform.right) < 0)
        //        weaponSprite.sortingLayerName = "WeaponFront";
        //    else
        //        weaponSprite.sortingLayerName = "WeaponBehind";
        //}
        if (characterData.Weapon != null)
        {
            if (Vector3.Dot(Vector3.up, aimTransform.right) < 0)
                characterData.Weapon.SpriteRenderer.sortingLayerName = "WeaponFront";
            else
                characterData.Weapon.SpriteRenderer.sortingLayerName = "WeaponBehind";
        }
    }
    protected IEnumerator ReloadingWeaponsCoroutine()
    {
        isReloadingWeapon = true;
        yield return new WaitForSeconds(characterData.Weapon.WeaponObject.Rate);
        isReloadingWeapon = false;
        yield return null;
    }
    protected void TakeDamage(int damage)
    {
        characterData.ChangeHealth(-damage);
        if (characterData.CurrentHealth < 1) Die();
    }
    protected void Shoot()
    {
        float accuracy = 90/characterData.Weapon.WeaponObject.Accuracy;
        float spread = Random.Range(-accuracy, accuracy);
        Vector3 spreadVector = Quaternion.Euler(0, 0, spread) * characterData.Weapon.AimGunEndPointTransform.right;
        RaycastHit2D raycastHit2D = Physics2D.Raycast(characterData.Weapon.AimGunEndPointTransform.position, spreadVector);
        Debug.DrawRay(characterData.Weapon.AimGunEndPointTransform.position, spreadVector);
        if (raycastHit2D.collider != null)
        {
            BaseCharacterLogic characterLogic = raycastHit2D.collider.gameObject.GetComponent<BaseCharacterLogic>();
            if (characterLogic != null)
            {                
                characterLogic.TakeDamage(characterData.Weapon.WeaponObject.Damage);
            }
        }
    }
    protected virtual void Die()
    {

    }
}
