using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] public CharacterData characterData;
    [SerializeField] public Slider health;
    //[SerializeField] public Slider stamina;
    void Start()
    {
        UpdateHUD();
        characterData.StatChanged += UpdateHUD; 
    }
    public void UpdateHUD()
    {
        health.value = characterData.CurrentHealth / characterData.Health;
        //stamina.value = playerData.CurrentStamina / playerData.Stamina;
    }
}

