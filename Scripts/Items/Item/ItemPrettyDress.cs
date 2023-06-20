using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPrettyDress : ItemAttackEquip
{
    public override void OnPlayerBeenAttack()
    {
        base.OnPlayerBeenAttack();
        Debug.Log("Im atttacked");
        GameObject pl = GameObject.FindGameObjectWithTag("Player");
        if (pl == null) return;
        pl.GetComponent<PlayerController>().Regen(3);
    }
}
