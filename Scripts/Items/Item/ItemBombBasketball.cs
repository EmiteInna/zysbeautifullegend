using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBombBasketball : ItemController
{
   
    public override void OnItemUse()
    {
        if (cooldown_now > 0) return;
        GameObject pl = GameObject.FindGameObjectWithTag("Player");
        var colliders = Physics.OverlapSphere(pl.transform.position, 10);
        foreach (var tar in colliders)
        {
            if (tar.CompareTag("enemy")||tar.CompareTag("Player")||tar.CompareTag("boss")||tar.CompareTag("friend"))
            {
                tar.GetComponent<CharacterStats>().TakeDamage(100);
            }
        }
        pl.GetComponent<PlayerController>().BeenAttack();
        base.OnItemUse();
    }
    //void backup()
    //{
    //    attackData.attackDamage -= 50;
    //    PlayerUIManager.Instance.UpdateUI();
    //}
}
