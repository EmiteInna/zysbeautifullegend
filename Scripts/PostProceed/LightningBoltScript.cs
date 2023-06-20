using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
public class LightningBoltScript : MonoBehaviour
{
    public float speed;
    public Volume volume;
    private void Start()
    {
        volume = GetComponent<Volume>();
        volume.weight = 1.0f;
        StartCoroutine(Decrease());
    }
    IEnumerator Decrease()
    {
        float procedure = volume.weight;
        while(procedure > 0)
        {
            Debug.Log(procedure);
            volume.weight = procedure;
            procedure -=speed ;
            yield return new WaitForSecondsRealtime(0.02f);
        }
        Destroy(gameObject);
    }
}
