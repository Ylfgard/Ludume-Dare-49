using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : GroundItem
{
    private Rigidbody2D rigidBody;
    public WeaponObject WeaponObject;
    public SpriteRenderer SpriteRenderer;
    public Vector3 localPosition;
    public bool isEquipped = false;
    public Transform AimGunEndPointTransform;

    void Awake() //test
    {
        rigidBody = GetComponent<Rigidbody2D>();
        SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rigidBody.bodyType = RigidbodyType2D.Kinematic;
        AimGunEndPointTransform = transform.Find("AimGunEndPoint");
        WeaponObject.Ammo = WeaponObject.MaxAmmo;
    }

    protected override void PickUp(Collector collector)
    {
        if(collector.CharacterData.Weapon == null && isEquipped == false)
        {
            m_collider.enabled = false;
            rigidBody.bodyType = RigidbodyType2D.Kinematic;
            this.transform.parent = collector.WeaponSlot.transform;
            this.transform.localPosition = localPosition;
            this.transform.rotation = new Quaternion(0, 0, 0, 0);
            transform.localScale = new Vector3(1, 1, 1);
            collector.CharacterData.Weapon = this;
            collector.CharacterData.InvokeAmmoChanged();
        }
        isEquipped = true;
    }
    public void Drop(Collector collector)
    {        
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        rigidBody.AddForceAtPosition(transform.right * 10, transform.right * 5, ForceMode2D.Impulse);
        this.transform.parent = null;
        Invoke("EnableCollider", 0.5f);
        collector.CharacterData.Weapon = null;
        isEquipped = false;
        collector.CharacterData.InvokeAmmoChanged();
    }
    void Update()
    {
        ////Debug.Log(transform.localPosition.ToString());
        //if (Input.GetKeyDown(KeyCode.O))
        //    Drop();
    }
}
