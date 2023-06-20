using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodBasketballScript : EnemyController
{
    public float LightningStrikeBaseDamage;
    public float LightningStrikeIncreasingDamage;
    public float zapDamage;
    public float baskDamage;
    public int throwNumber;
    public GameObject bask;
    public GameObject lightningbolt;
    public Cinemachine.CinemachineImpulseSource impulse;
    protected override void Start()
    {
        impulse = GetComponent<Cinemachine.CinemachineImpulseSource>(); 
    }

    public override void SkillEffect(int num)
    {
        GameObject g = GameObject.FindGameObjectWithTag("Player");
        if (num == 0)
        {
            //À×»÷£¬audioÐòºÅÎª2
            impulse.GenerateImpulse();
            AudioClip adc = GetComponent<AudioList>().clipList[2];
            SoundManager.Instance.Playsound(g.transform.position, adc, 5);
            GManager.Instance.playerStats.TakeDamage(LightningStrikeBaseDamage);
            LightningStrikeBaseDamage += LightningStrikeIncreasingDamage;
            Instantiate(lightningbolt);
        }
        else if (num == 1)
        {
            //Ë²ÒÆ£¬audioÐòºÅÎª3
            impulse.GenerateImpulse();
            AudioClip adc = GetComponent<AudioList>().clipList[3];
            SoundManager.Instance.Playsound(g.transform.position, adc, 5);
            GManager.Instance.playerStats.TakeDamage(zapDamage);
            agent.Warp(g.transform.position);
        }else if (num == 2)
        {
            StartCoroutine(Thrower());
            //Í¶ÖÀ´óÁ¿ÀºÇò
        }
        base.SkillEffect(num);
    }
    IEnumerator Thrower()
    {
        for(int i=0; i < throwNumber; i++)
        {
            yield return ThrowBasketball();
            yield return new WaitForSecondsRealtime(0.02f);
        }
    }
    IEnumerator ThrowBasketball()
    {
        if (attackTarget != null)
        {
            anim.SetTrigger("Shoot");
            transform.LookAt(attackTarget.transform);
            Vector3 dir = attackTarget.transform.position - transform.position;
            dir.y = 0;
            dir.Normalize();
            var bas = Instantiate(bask, transform.position + dir * 4+ new Vector3(0, 1, 0), Quaternion.identity);
            bas.GetComponent<AmmoController>().from = characterStats;
            bas.GetComponent<AmmoController>().damage = baskDamage;
            bas.GetComponent<AmmoController>().state = AmmoStates.HitPlayer;
            bas.GetComponent<AmmoController>().target = attackTarget;
            bas.GetComponent<AmmoController>().force = 12f;
            bas.GetComponent<AmmoController>().living_time = 3f;
        }
        yield return null;
    }
    public override void Death()
    {
        ProceedureManager.Instance.turnBGMBack();
        base.Death();
    }

}
