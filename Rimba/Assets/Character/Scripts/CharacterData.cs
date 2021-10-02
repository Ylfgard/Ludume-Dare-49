using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Character Data", menuName = "Character/Data")]
public class CharacterData : ScriptableObject
{    
    public float Speed;
    public float Health;
    public float Stamina;
}
