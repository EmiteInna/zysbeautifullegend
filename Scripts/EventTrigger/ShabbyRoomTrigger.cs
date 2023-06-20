using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShabbyRoomTrigger : EvTrigger
{
    public GameObject shabbyroommaster;
    public override IEnumerator makeEffect()
    {
        GManager.Instance.playerStats.TakeDamage(80);
        GameObject pl = GameObject.FindGameObjectWithTag("Player");
        pl.GetComponent<PlayerController>().BeenAttack();
        Destroy(shabbyroommaster);
        yield return base.makeEffect();
    }
}
