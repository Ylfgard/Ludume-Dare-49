using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterLogic : BaseCharacterLogic
{
    private FMOD.Studio.EventInstance instance;

    // Temp for sound problem solve
    public FMOD.Studio.EventInstance eInst
    {
        get
        {
            return instance;
        }
    }

    private void Update()
    {
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

            // Temp shooting sound, need to fix pistol sound event
            PlayShootSound();
        }
    }
    #region Tempotaty shooting sound
    private void PlayShootSound()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance("event:/pistol_shot");
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
        instance.start();
        instance.release();
        StartCoroutine(StopShootSoundCoroutine());
    }
    private IEnumerator StopShootSoundCoroutine()
    {
        yield return new WaitForSeconds(characterData.Weapon.WeaponObject.Rate);
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void OnDestroy()
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    #endregion
}
