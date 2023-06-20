using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAttackEquip : ItemController
{
    public float atk_amount;
    public float dfs_amount;
    public float cc_amount;
    public float cd_amount;
    public override void OnPickedUp()
    {
        base.OnPickedUp();
        BuffManager.Instance.BuffDefense(dfs_amount, -1);
        BuffManager.Instance.BuffCritChanceDefense(cc_amount, -1);
        BuffManager.Instance.BuffCritDamageDefense(cd_amount, -1);
        BuffManager.Instance.BuffAttack(atk_amount, -1);
    }
    public override void OnThrowed()
    {
        base.OnThrowed();
        BuffManager.Instance.BuffAttack(-atk_amount, -1);
        BuffManager.Instance.BuffDefense(-dfs_amount, -1);
        BuffManager.Instance.BuffCritChanceDefense(-cc_amount, -1);
        BuffManager.Instance.BuffCritDamageDefense(-cd_amount, -1);
    }
}
