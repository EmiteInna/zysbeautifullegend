using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AmmoStates { HitPlayer,HitEnemy }
public class AmmoController : MonoBehaviour
{

    private Rigidbody rb;
    [Header("Basic Settings")]
    public float force;
    public GameObject target;
    public CharacterStats from;
    private Vector3 direction;
    public Vector3 destination;
    public float damage;
    public AmmoStates state;
    public float living_time;
    private void Start()
    {
        rb= GetComponent<Rigidbody>();
        if (target != null) FlyToTarget();
        else FlyToPosition();
        Invoke("dest", living_time);
    }
    public void FlyToPosition()
    {
        direction = (destination - transform.position).normalized;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }
    public void FlyToTarget()
    {
        if (target == null)
            target = FindObjectOfType<PlayerController>().gameObject;
        transform.LookAt(target.transform);
        direction = (target.transform.position - transform.position).normalized;
        direction.y = 0;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ammo")) return;
        if(state==AmmoStates.HitPlayer)   
            if (collision.gameObject.CompareTag("Player")|| collision.gameObject.CompareTag("friend"))
            {
                makeEffect(collision); 
            }
        else if(state==AmmoStates.HitEnemy)
             if (collision.gameObject.CompareTag("enemy")|| collision.gameObject.CompareTag("boss"))
             {
                 makeEffect(collision);
                    
             }
        Destroy(gameObject);
    }
    public virtual void makeEffect(Collision collision)
    {
        var targetStats = collision.gameObject.GetComponent<CharacterStats>();
        targetStats.TakeDamage(damage, targetStats, from);
        if (collision.gameObject.tag == "Player") collision.gameObject.GetComponent<PlayerController>().BeenAttack();
        else if (collision.gameObject.tag == "enemy") collision.gameObject.GetComponent<EnemyController>().BeenAttack();
    }
    private void dest()
    {
        Destroy(gameObject);
    }
}
