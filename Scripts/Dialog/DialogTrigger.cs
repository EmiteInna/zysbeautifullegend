using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public bool death_after_trigger;
    public bool has_been_triggered;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player"&&!has_been_triggered)
        {
            has_been_triggered = true;
            DialogManager.Instance.source = gameObject;
            StartCoroutine(DialogManager.Instance.PlayDialog());
            
            if (death_after_trigger) Invoke("die", 5);
            else has_been_triggered=false;
        }
    }
    void die()
    {
        Destroy(gameObject);
    }
}
