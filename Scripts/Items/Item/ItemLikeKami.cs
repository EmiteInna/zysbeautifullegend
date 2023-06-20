using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLikeKami : ItemController
{
    public override void OnItemUse()
    {
        if (cooldown_now > 0) return;
        BuffManager.Instance.BuffAttack(0.5f*GManager.Instance.playerStats.attackDamage, 30);
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
