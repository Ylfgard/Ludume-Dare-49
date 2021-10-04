using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] public CharacterData characterData;
    [SerializeField] public Slider health;
    [SerializeField] public Text AmmoText;
    //[SerializeField] public Slider stamina;
    void Start()
    {
        UpdateHUD();
        characterData.StatChanged += UpdateHUD;        
        characterData.AmmoChanged += UpdateHUD;
    }
    public virtual void UpdateHUD()
    {
        health.value = characterData.CurrentHealth / characterData.Health;
        if (characterData.Weapon == null)
        {
            AmmoText.text = 0.ToString();
        }
        else
        {
            AmmoText.text = characterData.Weapon.WeaponObject.Ammo.ToString();
        }
        //stamina.value = playerData.CurrentStamina / playerData.Stamina;
    }
}

