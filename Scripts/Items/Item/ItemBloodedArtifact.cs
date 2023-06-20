using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBloodedArtifact : ItemController
{
    public override void OnItemUse()
    {
        if (amount >= 3)
        {
            Debug.Log("Kami laila");
            ProceedureManager.Instance.SetGodBas();
            expire();
        }
        return;
    }
    public override void OnAmountChanged()
    {
        base.OnAmountChanged();
        if (amount >= 3)
        {
            Debug.Log("Kami laila");
            ProceedureManager.Instance.SetGodBas();
            expire();
        }
    }
    
}
