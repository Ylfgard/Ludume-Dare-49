using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] public CharacterData playerData;
    [SerializeField] public Slider health;
    [SerializeField] public Slider stamina;
    void Start()
    {
        UpdateHUD();
        playerData.StatChanged += UpdateHUD;        
    }
    public void UpdateHUD()
    {
        health.value = playerData.CurrentHealth / playerData.Health;
        stamina.value = playerData.CurrentStamina / playerData.Stamina;
    }
}

