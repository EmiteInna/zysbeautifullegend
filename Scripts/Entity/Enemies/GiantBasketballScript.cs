using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GiantBasketballScript : EnemyController
{
    GameObject lastTarget;
    public float knockBackDistance=20f;
    public override void Attack()
    {
        base.Attack();
        
    }
    public override void Hit()
    {
        base.Hit();
        if (gameObject == null) return;
        if (attackTarget == null) return;
        Debug.Log("Overrided");
        Vector3 vec = attackTarget.transform.position - transform.position;
        vec.Normalize();
        attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
        attackTarget.GetComponent<NavMeshAgent>().velocity = vec * knockBackDistance;
        lastTarget = attackTarget;
        Invoke("ResetVelocity", 1);
    }
    private void ResetVelocity()
    {
        lastTarget.GetComponent<NavMeshAgent>().velocity=new Vector3(0,0,0);
    }
}
