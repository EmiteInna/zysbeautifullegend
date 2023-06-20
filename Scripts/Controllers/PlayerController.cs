using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PlayerController : MonoBehaviour
{
    
    public  NavMeshAgent agent;
    public  GameObject attackTarget;
    public  GameObject pickTarget;
    public  CharacterStats characterStats;
    public Animator anim;
    private Cinemachine.CinemachineImpulseSource impulse;
    private float lastAttackTime;
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        characterStats = GetComponent<CharacterStats>();
        anim=GetComponent<Animator>();
        impulse=GetComponent<Cinemachine.CinemachineImpulseSource>(); 
    }
    private void Start()
    {
    //    characterStats.Initialize();
        if(GManager.Instance == null)
        {
            Debug.Log("it is null ok?");
        }
       
    }
    private void OnEnable()
    {
        if(MouseManager.Instance == null)
        {
            Debug.Log("cant find mousemanager instance");
        } 
        GManager.Instance.RegisterPlayer(characterStats);
        
        MouseManager.Instance.OnMouseClicked += EventMoveToTarget;
        MouseManager.Instance.OnEnemyClicked += EventMoveToAttackTarget;
        MouseManager.Instance.OnItemClicked += EventMoveToPickItem;
        Invoke("BindStats", 0.02f);
        Invoke("UpdateUI", 0.4f);
        //   Debug.Log("Registered");
    }
    private void OnDisable()
    {
        
        MouseManager.Instance.OnMouseClicked -= EventMoveToTarget; ;
        MouseManager.Instance.OnEnemyClicked -= EventMoveToAttackTarget;
        MouseManager.Instance.OnItemClicked -= EventMoveToPickItem;
       // Debug.Log("UnRegistered");
    }
    private void FixedUpdate()
    {
        SwitchAnimation();
        if (characterStats.currentHealth < 0) {
        //    Death();
        //TODO:Only for test;
            return;
        }
            
        if(lastAttackTime>0)lastAttackTime -= Time.fixedDeltaTime;
    }
    public void EventMoveToTarget(Vector3 target)
    {
  //      Debug.Log("Now moving");
        StopAllCoroutines();
       // transform.LookAt(target);
        AudioClip adc = GetComponent<AudioList>().clipList[3];
        SoundManager.Instance.Playsound(transform.position, adc, 1);
        agent.destination = target;
        agent.isStopped = false;
    }
    public void EventMoveToAttackTarget(GameObject target)
    {
        if (target != null)
        {

            attackTarget = target;
            StartCoroutine(MoveToAttackTarget());
        }
    }
    public void EventMoveToPickItem(GameObject target)
    {
        StopAllCoroutines();
        if(target != null)
        {
            pickTarget = target;
            StartCoroutine(MoveToPickTarget());

        }

    }
    void BindStats()
    {
        characterStats.UpdateHealth();
      //  yield return null;
    }
    IEnumerator MoveToAttackTarget()
    {
        while (attackTarget != null) 
       {
            agent.isStopped = false;
            transform.LookAt(attackTarget.transform);
            while (attackTarget != null&&Vector3.Distance(attackTarget.transform.position, transform.position) > characterStats.attackRange)
            {
                agent.destination = attackTarget.transform.position;
                yield return null;
            }
            agent.isStopped = true;
            if (lastAttackTime <= 0)
            {
                //  Debug.Log("Attaced MotherFucker");
                //anim.SetTrigger("Attack");
                Attack();
                lastAttackTime = characterStats.attackCoolDown;
            }
            yield return null;
        }
    }
    IEnumerator MoveToPickTarget()
    {
        while (pickTarget != null)
        {
            agent.isStopped = false;
            transform.LookAt(pickTarget.transform);
            
            while (Vector3.Distance(pickTarget.transform.position, transform.position) > characterStats.attackRange)
            {
                agent.destination = pickTarget.transform.position;
                yield return null;
            }
            agent.isStopped = true;
            AudioClip adc = GetComponent<AudioList>().clipList[2];
            SoundManager.Instance.Playsound(transform.position, adc, 1);
            pickTarget.GetComponent<DroppedItemController>().BeenPickedup();
            yield return null;
        }
    }
    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position,5);
    //}
    void Attack()
    {
        if (attackTarget == null) return;
        anim.SetTrigger("Attack");
       // Hit();
    }
    void Hit()
    {
        if (attackTarget == null) return;
        if (attackTarget.GetComponent<EnemyController>().enemyStates == EnemyStates.DEAD) return;
        foreach(var item in BackpackManager.Instance.items)
        {
            if (item.GetComponent<ItemController>().is_null) continue;
            item.GetComponent<ItemController>().OnPlayerAttack();
        }
        AudioClip adc = GetComponent<AudioList>().clipList[0];
        SoundManager.Instance.Playsound(transform.position, adc, 1);
        var targetStats = attackTarget.GetComponent<CharacterStats>();
        targetStats.TakeDamage(characterStats, targetStats);
        if (attackTarget.tag == "Player") attackTarget.GetComponent<PlayerController>().BeenAttack();
        else if (attackTarget.tag == "enemy" || attackTarget.tag == "boss") attackTarget.GetComponent<EnemyController>().BeenAttack();
        else if (attackTarget.tag == "friend") attackTarget.GetComponent<FriendController>().BeenAttack();

    }
    void Death()
    {
        anim.SetTrigger("Death");
        Invoke("Vanish", 2f);
    }
    void Vanish()
    {
        Destroy(gameObject);
        GManager.Instance.NotifyObservers();
    }
    public void BeenAttack()
    {
        impulse.GenerateImpulse();
        AudioClip adc = GetComponent<AudioList>().clipList[1];
        SoundManager.Instance.Playsound(transform.position, adc, 1);
        anim.SetTrigger("Hurt"); 
        foreach (var item in BackpackManager.Instance.items)
        {
            if (item.GetComponent<ItemController>().is_null) continue;
            item.GetComponent<ItemController>().OnPlayerBeenAttack();
        }
        Invoke("UpdateUI", 0.4f);
       
    }
    public void Regen(int amount)
    {
        characterStats.currentHealth = Mathf.Min(characterStats.maxHealth, characterStats.currentHealth + amount);
        characterStats.UpdateHealth();
        Invoke("UpdateUI", 0.4f);
    }

    
    public void UpdateUI()
    {
        PlayerUIManager.Instance.UpdateUI();
    }
    void SwitchAnimation()
    {
        anim.SetFloat("Speed",agent.velocity.sqrMagnitude);
    }
}
