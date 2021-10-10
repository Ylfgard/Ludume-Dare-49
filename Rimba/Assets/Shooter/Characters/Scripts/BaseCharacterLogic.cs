using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TerningSide { forward, back, right, left};

public class BaseCharacterLogic : MonoBehaviour
{
    public CharacterData characterData;
    private Transform aimTransform;
    protected Rigidbody2D m_rigidBody;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    //public GameObject hitPoint;


    protected bool isReloadingWeapon = false;
    protected Coroutine reloadingWeaponCoroutine;

    public Sprite LookingForward;
    public Sprite LookingBack;
    public Sprite LookingRight;
    public Sprite LookingLeft;
    protected TerningSide directionOfView = TerningSide.back;

    private void Awake()
    {
        aimTransform = transform.Find("Aim");
        m_rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();        
        animator = GetComponent<Animator>();
        characterData.Weapon = GetComponentInChildren<Weapon>();
    }
    protected void Turn(TerningSide side)
    {
        switch (side)
        {
            case TerningSide.forward:
                spriteRenderer.sprite = LookingForward;
                directionOfView = TerningSide.forward;
                break;
            case TerningSide.left:
                spriteRenderer.sprite = LookingLeft;
                directionOfView = TerningSide.left;
                break;
            case TerningSide.back:
                spriteRenderer.sprite = LookingBack;
                directionOfView = TerningSide.back;
                break;
            case TerningSide.right:
                spriteRenderer.sprite = LookingRight;
                directionOfView = TerningSide.right;
                break;
            default:
                break;
        }
    }
    protected TerningSide SetTurningSide(float angle)
    {
        if (angle < 45 && angle > -45)
            return TerningSide.right;
        else if (angle < 135 && angle > 45)
            return TerningSide.back;
        else if (angle > 135 || angle < -135)
            return TerningSide.left;
        else if (angle < -45 && angle > -135)
            return TerningSide.forward;
        else return TerningSide.forward;
    }

    protected void Aim(Vector3 target)
    {
        aimTransform.LookAt(target);
        Vector3 aimDirection = (target - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        Turn(SetTurningSide(angle));

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
        //float accuracy = 90 / characterData.Weapon.WeaponObject.Accuracy;
        //float spread = Random.Range(-accuracy, accuracy);
        //Vector3 spreadVector = Quaternion.Euler(0, 0, spread) * characterData.Weapon.AimGunEndPointTransform.right;
        RaycastHit2D raycastHit2D = Physics2D.Raycast(characterData.Weapon.AimGunEndPointTransform.position, characterData.Weapon.AimGunEndPointTransform.right);
        Debug.DrawRay(characterData.Weapon.AimGunEndPointTransform.position, characterData.Weapon.AimGunEndPointTransform.right);  //spreadVector 
        if (raycastHit2D.collider != null)
        {
            BaseCharacterLogic characterLogic = raycastHit2D.collider.gameObject.GetComponentInParent<BaseCharacterLogic>();
            if (characterLogic != null)
            {
                characterLogic.TakeDamage(characterData.Weapon.WeaponObject.Damage);
            }
        }
    }
    protected void ManageAnimation()
    {
        if (m_rigidBody.velocity.x > 0.1f || m_rigidBody.velocity.y > 0.1f)
        {
            switch (directionOfView)
            {
                case TerningSide.forward:

                    animator.SetBool("isWalkBack", false);
                    animator.SetBool("isWalkLeft", false);
                    animator.SetBool("isWalkRight", false);
                    animator.SetBool("isWalkFront", true);
                    break;
                case TerningSide.left:
                    animator.SetBool("isWalkFront", false);
                    animator.SetBool("isWalkBack", false);

                    animator.SetBool("isWalkRight", false);
                    animator.SetBool("isWalkLeft", true);
                    break;
                case TerningSide.back:
                    animator.SetBool("isWalkFront", false);

                    animator.SetBool("isWalkLeft", false);
                    animator.SetBool("isWalkRight", false);
                    animator.SetBool("isWalkBack", true);
                    break;
                case TerningSide.right:
                    animator.SetBool("isWalkFront", false);
                    animator.SetBool("isWalkBack", false);
                    animator.SetBool("isWalkLeft", false);
                    animator.SetBool("isWalkRight", true);
                    break;
                default:
                    break;
            }
        }
    }
    protected virtual void Die()
    {

    }
}
    
