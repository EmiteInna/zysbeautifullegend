using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRussianWheel : ItemController
{
    public List<GameObject> getItems;
    public List<int> getamount;
    public List<float> possibility;
    public override void OnItemUse()
    {
        if (cooldown_now > 0) return;
        float basep = 0;
        float rd = (float)UnityEngine.Random.Range(0, 100)/100;
        Debug.Log(rd);
        for (int i = 0; i < getItems.Count; i++)
        {
            basep += possibility[i];
            if (rd < basep)
            {
                getItems[i].GetComponent<ItemController>().amount = getamount[i];
                BackpackManager.Instance.GetItem(getItems[i]);
                break;
            }

        }
        base.OnItemUse();
    }
}
