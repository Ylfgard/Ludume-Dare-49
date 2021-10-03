using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(fileName = "New Character Data", menuName = "Character/Data")]
public class CharacterData : ScriptableObject
{    
    public float Speed;
    public float Health;
    public float Stamina;

    private float currentHealth;    
    private float currentStamina;

    public Weapon Weapon; //test

    public float CurrentHealth
    {
        get { return currentHealth; }       
    }
    public float CurrentStamina
    {
        get { return currentStamina; }
    }
    public event Action StatChanged;
    private void OnEnable()
    {
        currentHealth = Health;
        currentStamina = Stamina;
    }
    public void ChangeHealth(int volume)
    {
        currentHealth += volume;
        StatChanged?.Invoke();
    }
    public void ChangeStamina(int volume)
    {
        currentStamina += volume;
        StatChanged?.Invoke();
    }

}
