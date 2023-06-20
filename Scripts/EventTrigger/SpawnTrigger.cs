using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : EvTrigger
{
    public List<GameObject> entities;
    public List<Vector3> offsets;
    public override IEnumerator makeEffect()
    {
        for (int i = 0; i < entities.Count; i++)
        {
            GameObject g = Instantiate(entities[i], transform.position + offsets[i], Quaternion.identity);
            
        }
        yield return base.makeEffect();
    }
}
