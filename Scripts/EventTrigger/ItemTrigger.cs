using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : EvTrigger
{
    public List<GameObject> items;
    public List<int> itemcounts;
    public override IEnumerator makeEffect()
    {
       for(int i=0; i < items.Count; i++)
        {
            Debug.Log("dropped");
            GameObject g = Instantiate(items[i], transform.position, Quaternion.identity);
            g.GetComponent<DroppedItemController>().count = itemcounts[i];
        }
        yield return base.makeEffect();
    }
}
