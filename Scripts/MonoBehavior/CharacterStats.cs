using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public event Action<int, int> UpdateHealthBarOnAttack;
    public CharacterData templateData;
    public CharacterData characterData;
    public AttackData attackData;
    private void Awake()
    {
        if (templateData != null)
        {
            characterData = Instantiate(templateData);
        }
        
    }
    public int maxHealth {
        get { if (characterData != null) return characterData.maxHealth; else return 0;}
        set { characterData.maxHealth = value;} }
    public int currentHealth
    {
        get { if (characterData != null) return characterData.currentHealth; else return 0; }
        set { characterData.currentHealth = value; }
    }
    public float defense
    {
        get { if (characterData != null) return characterData.defense; else return 0; }
        set { characterData.defense = value; }
    }
    public string EntityName
    {
        get { if (characterData != null) return characterData.EntityName; else return ""; }
        set { characterData.EntityName = value; }
    }
    public float attackRange
    {
        get { if (attackData != null) return attackData.attackRange; else return 0; }
        set { attackData.attackRange = value; }
    }
    public float attackSpeed
    {
        get { if (attackData != null) return attackData.attackSpeed; else return 0; }
        set { attackData.attackSpeed = value; }
    }
    public float attackCoolDown
    {
        get { if (attackData != null) return attackData.attackCoolDown; else return 0; }
        set { attackData.attackCoolDown = value; }
    }
    public float attackDamage
    {
        get { if (attackData != null) return attackData.attackDamage; else return 0; }
        set { attackData.attackDamage = value; }
    }
    public float critChance
    {
        get { if (attackData != null) return attackData.critChance; else return 0; }
        set { attackData.critChance = value; }
    }
    public float critDamage
    {
        get { if (attackData != null) return attackData.critDamage; else return 0; }
        set { attackData.critDamage = value; }
    }


    public void TakeDamage(CharacterStats attacker,CharacterStats defender)
    {
        float damage = Mathf.Max(attacker.GetDamage() - defender.defense,0);
        currentHealth = (int)Mathf.Max(currentHealth - damage, 0);
        Debug.Log(string.Format("{0} attacked {1} with damage {2}, health have {3} left",attacker.EntityName,defender.EntityName,damage,defender.currentHealth));
        //TODO:ui   
        UpdateHealth();
    }
    public void TakeDamage(float damage, CharacterStats defender,CharacterStats origin)
    {
        float dmg = Mathf.Max(damage - defender.defense, 0);
        currentHealth = (int)Mathf.Max(currentHealth - dmg, 0);
        Debug.Log(string.Format("{0} attacked {1} with damage {2}, health have {3} left", origin.EntityName, defender.EntityName, dmg, defender.currentHealth));
        //TODO:ui   
        UpdateHealth();
    }
    public void TakeDamage(float damage)
    {
        currentHealth = (int)Mathf.Max(currentHealth - damage, 0);
        UpdateHealth();
    }
    public void UpdateHealth()
    {
           UpdateHealthBarOnAttack?.Invoke(currentHealth, maxHealth);
    }
    public float GetDamage()
    {
        float damage = attackDamage + UnityEngine.Random.Range(0, 1);
        if ((float)UnityEngine.Random.Range(0, 100)/100 < critChance)
        {
            Debug.Log("OH CRIT!");
            damage *= critDamage;
        }
        return damage;

    }
    public void Initialize()
    {
        currentHealth = maxHealth;
    }
}
