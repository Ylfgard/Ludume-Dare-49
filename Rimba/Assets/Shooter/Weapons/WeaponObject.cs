using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponObject : ScriptableObject
{
    //public GameObject Visual;
    public int Damage;
    public float Accuracy;
    public float Rate;
    public float Ammo;
}
