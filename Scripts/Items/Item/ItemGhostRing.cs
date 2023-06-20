using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGhostRing : ItemController
{
    public override void OnPlayerAttack()
    {
        base.OnPlayerAttack();
        GameObject pl = GameObject.FindGameObjectWithTag("Player");
        if (pl == null) return;
        pl.GetComponent<PlayerController>().Regen((int)(0.5f*GManager.Instance.playerStats.attackDamage));
    }
}
