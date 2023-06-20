using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEatableBasketball : ItemController
{
    public override void OnItemUse()
    {
        if (cooldown_now > 0) return;
        GameObject pl = GameObject.FindGameObjectWithTag("Player");
        if (pl == null) return;
        pl.GetComponent<PlayerController>().Regen(50);
        
        base.OnItemUse();

    }
}
