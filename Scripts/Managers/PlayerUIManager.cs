using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance;
    public Text currentHealth;
    public Text maxHealth;
    public Text atk;
    public Text defense;
    public Text critChance;
    public Text critDamage;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void UpdateUI()
    {
        currentHealth.text = GManager.Instance.playerStats.currentHealth.ToString();
        maxHealth.text = GManager.Instance.playerStats.maxHealth.ToString();
        atk.text = GManager.Instance.playerStats.attackDamage.ToString();
        defense.text = GManager.Instance.playerStats.defense.ToString();
        critChance.text = GManager.Instance.playerStats.critChance.ToString();
        critDamage.text = GManager.Instance.playerStats.critDamage.ToString();
    }
}
