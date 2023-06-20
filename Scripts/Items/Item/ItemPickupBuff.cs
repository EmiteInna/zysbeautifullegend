using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupBuff : ItemController
{
    public float atk_amount;
    public float dfs_amount;
    public float cc_amount;
    public float cd_amount;
    public int hp_amount;
    public override void OnItemUse()
    {
        if (cooldown_now > 0) return;
        BuffManager.Instance.BuffDefense(dfs_amount, -1);
        BuffManager.Instance.BuffCritChanceDefense(cc_amount, -1);
        BuffManager.Instance.BuffCritDamageDefense(cd_amount, -1);
        BuffManager.Instance.BuffAttack(atk_amount, -1);
        GManager.Instance.playerStats.maxHealth += hp_amount;
        GManager.Instance.playerStats.currentHealth += hp_amount;
        base.OnItemUse();      
    }
}
