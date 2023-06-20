using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItemController : MonoBehaviour
{
    public GameObject item;
    public int count;
    public void BeenPickedup()
    {
        item.GetComponent<ItemController>().amount = count;
        BackpackManager.Instance.GetItem(item);
        Destroy(gameObject);
    }

}
