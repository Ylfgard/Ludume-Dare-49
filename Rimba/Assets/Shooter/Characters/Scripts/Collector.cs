using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public GameObject WeaponSlot;
    public CharacterData CharacterData;
    private void Start()
    {
        CharacterData.Weapon = GetComponentInChildren<Weapon>();
        if (CharacterData.Weapon != null) CharacterData.Weapon.DisableCollider();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            if (CharacterData.Weapon != null)
                CharacterData.Weapon.Drop(this);
        }
    }
}
