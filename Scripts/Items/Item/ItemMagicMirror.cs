using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMagicMirror : ItemController
{
    public override void OnPlayerAttack()
    {
        base.OnPlayerAttack();
        GameObject pl = GameObject.FindGameObjectWithTag("Player");
        if (pl == null) return;
        if (GameObject.FindGameObjectWithTag("boss") != null)
        {
            BuffManager.Instance.BuffAttack(10,30);
        }
    }
}
