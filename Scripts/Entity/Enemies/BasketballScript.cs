using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballScript : EnemyController
{
    public float meleeAttackDistance;
    public float rangeAttackDistance;
    public float rangedamage;
    public GameObject bask;
    public override void Attack()
    {
        if (attackTarget == null) return;
        
        if (Vector3.Distance(transform.position, attackTarget.transform.position) <= meleeAttackDistance)
        {
            base.Attack();
        }
        else if (Vector3.Distance(transform.position, attackTarget.transform.position) <= rangeAttackDistance)
        {
            if (attackCD > 0) return;
            anim.SetTrigger("Shoot");
            //ThrowBasketball();
            attackCD = characterStats.attackCoolDown;
        }
    }
    void ThrowBasketball()
    {
        if (attackTarget != null)
        {
            anim.SetTrigger("Shoot");
            transform.LookAt(attackTarget.transform);
            Vector3 dir = attackTarget.transform.position - transform.position;
            dir.y = 0;
            dir.Normalize();
            var bas = Instantiate(bask, transform.position+dir*3+new Vector3(0,1,0), Quaternion.identity);
            bas.GetComponent<AmmoController>().from = characterStats;
            bas.GetComponent<AmmoController>().damage = rangedamage;
            bas.GetComponent<AmmoController>().state = AmmoStates.HitPlayer;
            bas.GetComponent<AmmoController>().target = attackTarget;
            bas.GetComponent<AmmoController>().force = 6f;
            bas.GetComponent<AmmoController>().living_time = 3f;
        }
    }
}
