using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterLogic : MonoBehaviour
{
    public CharacterData characterData;
    protected Rigidbody2D m_rigidBody;
    private Transform aimTransform;
    //private Animator aimAnimator;
    //private Transform aimGunEndPointTransform;
    private void Awake()
    {
        aimTransform = transform.Find("Aim"); 
        m_rigidBody = GetComponent<Rigidbody2D>();
        characterData.Weapon = GetComponentInChildren<Weapon>();
    }
    protected virtual void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized;
        Vector2 velocity = m_rigidBody.velocity;
        {
            velocity = movement * characterData.Speed;
        }
        m_rigidBody.velocity = velocity;

        //if (velocity.x != 0 || velocity.y != 0)
        //{
        //    m_animator.SetBool("move", true);
        //}
        //else
        //{
        //    m_animator.SetBool("move", false);
        //}
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
    protected void Shoot()
    {

    }
}
