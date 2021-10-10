using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponObject : ScriptableObject
{
    //public GameObject Visual;
    public int Damage;
    public float Accuracy;
    public float Rate;
    public int MaxAmmo;
    public int Ammo;
    public int AmmoPerShot;
    public event Action AmmoChanged;
    public void ChangeAmmo(int amount)
    {
        Ammo += amount;
    }
    
}
