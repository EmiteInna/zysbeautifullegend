using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTicket : ItemController
{
    public List<GameObject> getItems;
    public List<int> getamount;
    public List<float> possibility;
    public override void OnItemUse()
    {
        if (cooldown_now > 0) return;
        for(int i = 0; i < getItems.Count; i++)
        {
            if (UnityEngine.Random.Range(0, 1) < possibility[i])
            {
                GameObject g=Instantiate(getItems[i], BackpackManager.Instance.Backpackmenu.transform);
                g.GetComponent<ItemController>().amount = getamount[i];
                BackpackManager.Instance.GetItem(g);
            }
            
        }
        base.OnItemUse();
    }
}
