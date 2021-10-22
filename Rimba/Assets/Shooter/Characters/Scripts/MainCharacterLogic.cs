using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterLogic : BaseCharacterLogic
{
    [SerializeField] private GameObject smokeEffect;

    private FMOD.Studio.EventInstance instance;

    // Temp for sound problem solve
    //public FMOD.Studio.EventInstance eInst
    //{
    //    get
    //    {
    //        return instance;
    //    }
    //}

    private void Update()
    {
        if(Time.timeScale == 0) return;

        Move();
        Aim(MouseWorld.GetPosition());
        Attack();
        ManageAnimation();
        
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

            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/pistol_shot", gameObject);

            Transform shot = characterData.Weapon.AimGunEndPointTransform;
            GameObject go = Instantiate(smokeEffect, shot.position, shot.rotation, shot);
            go.transform.localScale *= 0.3f;
        }
    }
}
