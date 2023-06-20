using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBuffPotion : ItemController
{
    public float atk_amount;
    public float dfs_amount;
    public float cc_amount;
    public float cd_amount;
    public float atk_time;
    public float dfs_time;
    public float cc_time;
    public float cd_time;
    public override void OnItemUse()
    {
        if (cooldown_now > 0) return;
        BuffManager.Instance.BuffAttack(atk_amount,atk_time);
        BuffManager.Instance.BuffCritChanceDefense(cc_amount, cc_time);
        BuffManager.Instance.BuffCritDamageDefense(cd_amount, cd_time);
        BuffManager.Instance.BuffDefense(dfs_amount, dfs_time);
        //attackData.attackDamage += 50;
        //PlayerUIManager.Instance.UpdateUI();
        //Invoke("backup", 30);
        base.OnItemUse();
    }
    //void backup()
    //{
    //    attackData.attackDamage -= 50;
    //    PlayerUIManager.Instance.UpdateUI();
    //}
}
