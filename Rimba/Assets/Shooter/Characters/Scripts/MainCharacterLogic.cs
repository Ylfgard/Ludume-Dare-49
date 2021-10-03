using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterLogic : BaseCharacterLogic
{
    
    private void Update()
    {
        Move();
        Aim(MouseWorld.GetPosition());
        Attack();
        
    }
    private void Move()
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
    public void Attack()
    {
        if (Input.GetMouseButtonDown(0) && characterData.Weapon != null && characterData.Weapon.WeaponObject.Ammo > characterData.Weapon.WeaponObject.AmmoPerShot && !isReloadingWeapon)
        {
            characterData.ChangeAmmo(-characterData.Weapon.WeaponObject.AmmoPerShot);
            Shoot();
            reloadingWeaponCoroutine = StartCoroutine(ReloadingWeaponsCoroutine());
        }
    }
}
