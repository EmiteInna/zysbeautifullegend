using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Attack",menuName ="Attack/Attack Data")]
public class AttackData : ScriptableObject
{
    public float attackRange;
    public float attackSpeed;
    public float attackCoolDown;
    public float attackDamage;
    public float critChance;
    public float critDamage;
}
