using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Data",menuName ="Character Stats/Data")]
public class CharacterData : ScriptableObject
{
    [Header("Stats")]
    public int maxHealth;
    public int currentHealth;
    public float movementSpeed;
    public float defense;
    public string EntityName;
}
