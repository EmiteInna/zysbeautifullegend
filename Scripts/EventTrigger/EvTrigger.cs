using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvTrigger : MonoBehaviour
{
    public bool is_collidable;
    public bool die_after_triggered;
    public bool is_triggered;
    private void Start()
    {
        is_triggered = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && is_collidable && !is_triggered)
        {
            StartCoroutine(triggered());
        }
    }

    public IEnumerator triggered()
    {
        is_triggered=true;
        yield return makeEffect();
        if(die_after_triggered)
            yield return die();
        else
        {
            is_triggered = false;
            yield return null;
        }
    }
    public virtual IEnumerator makeEffect()
    {
        yield return null;
    }
    IEnumerator die()
    {
        Destroy(gameObject);
        yield return null;
    }
}
