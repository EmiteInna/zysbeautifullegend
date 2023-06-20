using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class atkBuffer : MonoBehaviour
{
    public float amount, time;
    private void Start()
    {
        Invoke("buff", 0.1f);
    }
    void buff()
    {
        GManager.Instance.playerStats.attackDamage += amount;
        Invoke("updateUI", 0.02f);
        if (time < 0) {
            Invoke("die", 0.04f);
            return;
        }
        Invoke("returnAmount", time);
    }
    void returnAmount()
    {
        GManager.Instance.playerStats.attackDamage-= amount;
        Invoke("updateUI", 0.02f);
        Invoke("die", 0.04f);
    }
    void updateUI()
    {
        GameObject g = GameObject.FindGameObjectWithTag("Player");
        g.GetComponent<PlayerController>().UpdateUI();
    }
    void die()
    {
        Destroy(gameObject); 
    }
}
