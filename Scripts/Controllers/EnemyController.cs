using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates { GUARD, PATROL, CHASE, DEAD }
[RequireComponent(typeof(NavMeshAgent))]

public class EnemyController : MonoBehaviour,IEndGameObserver
{
    public EnemyStates enemyStates;
    public  NavMeshAgent agent;
    public  Collider coll;
    public CharacterStats characterStats;
    [Header("基本设置")]
    public float movespeed;
    public float detectRange;
    public GameObject attackTarget;
    public Animator anim;
    [Header("巡逻")]
    public Vector3 StartPoint;
    public Vector3 PatrolPoint;
    public float PatrolDistance;
    public float HoldTime;
    public float HoldTimenow;
    [Header("CD")]
    public float attackCD;
    [Header("状态")]
    public bool playerAlive;
    [Header("掉落物")]
    public List<GameObject> dropping;
    public List<int> dropnumber;
    public List<float> possibility;
    [Header("技能")]
    public int SkillAmount;
    public List<float> skillCooldown;
    public List<float> skillCooldown_now;
    protected virtual void Awake()
    {
        agentInitiate();
        enemyStates = EnemyStates.PATROL;
        StartPoint = transform.position;
        GetRandomPatrolPoint();
        HoldTimenow = HoldTime;
        characterStats = GetComponent<CharacterStats>();
        anim=GetComponent<Animator>();
        playerAlive = true;

    }
    protected virtual void Start()
    {
        GManager.Instance.addObserver(this);
        characterStats.Initialize();
    }
    protected virtual void FixedUpdate()
    {
        if (playerAlive == true)
        {
            if (attackCD > 0) attackCD -= Time.fixedDeltaTime;
            for(int i = 0; i < SkillAmount; i++)
            {
                if (skillCooldown_now[i] > 0) skillCooldown_now[i] -= Time.fixedDeltaTime;
            }
            SwitchStates();
        }
    }
    void agentInitiate()
    {
        agent.speed = movespeed;
    }
    void SwitchStates()
    {
        if (enemyStates == EnemyStates.DEAD) return;
        if (characterStats.currentHealth == 0) {
            Death();
            return;    
        }
        if (FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
          //  Debug.Log("OHSHIT");
        }
        switch (enemyStates)
        {
            case EnemyStates.GUARD:
                break;
            case EnemyStates.PATROL:
                if (Vector3.Distance(PatrolPoint, transform.position) <= agent.stoppingDistance)
                {
                    if (HoldTimenow > 0)
                    {
                        HoldTimenow -= Time.deltaTime;

                    }
                    else
                    {
                        GetRandomPatrolPoint();
                        HoldTimenow = HoldTime;
                    }
                }
                else
                {
                    agent.destination = PatrolPoint;
                }
                break;
            case EnemyStates.CHASE:
                if (!FoundPlayer())
                {
                    agent.isStopped = false;
                    if (Vector3.Distance(StartPoint, transform.position) <= agent.stoppingDistance)
                    {
                        enemyStates = EnemyStates.PATROL;
                    }
                    else
                    {
                        agent.destination = StartPoint;
                    }
                }
                else
                {
                    SkillDetect();
                    if (TargetInRange())
                    {
                        agent.isStopped = true;
                        Attack();

                    }
                    else
                    {
                        agent.isStopped = false;
                        agent.destination = attackTarget.gameObject.transform.position;
                    }
                }
                break;
            case EnemyStates.DEAD:
                break;
        }
    }
    bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, detectRange);
        foreach(var tar in colliders)
        {
            if (tar.CompareTag("Player")||tar.CompareTag("friend"))
            {
                transform.LookAt(tar.transform.position);
                attackTarget = tar.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }
    bool TargetInRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position)<=characterStats.attackRange;
        return false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,detectRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(StartPoint, PatrolDistance);
    }
    void GetRandomPatrolPoint()
    {
        float randomX = Random.Range(-PatrolDistance, PatrolDistance);
        float randomZ = Random.Range(-PatrolDistance, PatrolDistance);
        Vector3 rdPoint=new Vector3 (StartPoint.x+randomX,transform.position.y,StartPoint.z+randomZ);
        NavMeshHit hit;
        PatrolPoint=NavMesh.SamplePosition(rdPoint, out hit, PatrolDistance, 1)?hit.position:transform.position;
        PatrolPoint = rdPoint;

    }
    public virtual void SkillDetect()
    {
        for(int i = 0; i < SkillAmount; i++)
        {
            if (skillCooldown_now[i] > 0) continue;
            SkillEffect(i);
        }
    }
    public virtual void SkillEffect(int num)
    {
        skillCooldown_now[num] = skillCooldown[num];
    }
    public virtual void Attack()
    {
        if (attackCD > 0) return;
        if (attackTarget == null) return;
        transform.LookAt(attackTarget.transform);
        anim.SetTrigger("Attack");
        //Hit();        
        attackCD = characterStats.attackCoolDown;
        //attackTarget.GetComponent<CharacterStats>().currentHealth
    }
    public virtual void Hit()
    {
        
        if (gameObject == null) return;
        if (enemyStates == EnemyStates.DEAD) return;
        if (attackTarget == null) return;
        AudioClip adc = GetComponent<AudioList>().clipList[0];
        SoundManager.Instance.Playsound(transform.position, adc, 1);
        var targetStats = attackTarget.GetComponent<CharacterStats>();
        targetStats.TakeDamage(characterStats, targetStats);
        if (attackTarget.tag == "Player") attackTarget.GetComponent<PlayerController>().BeenAttack();
        else if (attackTarget.tag == "enemy" || attackTarget.tag == "boss") attackTarget.GetComponent<EnemyController>().BeenAttack();
        else if (attackTarget.tag == "friend") attackTarget.GetComponent<FriendController>().BeenAttack();
    }
    public virtual void Death()
    {
        enemyStates = EnemyStates.DEAD;
        Debug.Log("dead");
        anim.SetTrigger("Death");
        DropItem();
        GManager.Instance.removeObserver(this);
        Invoke("vanish", 1.5f);
    }
    void vanish()
    {
        Destroy(gameObject);
    }
    public void EndNotify()
    {
        agent.isStopped = true;
        playerAlive = false;
    }
    void OnEnable()
    {
        
    }
    void OnDisable()
    {
        
    }
    void DropItem()
    {
        for(int i = 0; i < dropping.Count; i++)
        {
            float rd = (float)UnityEngine.Random.Range(0, 100)/100f;
            Debug.Log(rd);
            if (rd < possibility[i])
            {
                GameObject droppin = Instantiate(dropping[i], new Vector3(i * 5, 0, 0) + transform.position, Quaternion.identity);
                droppin.GetComponent<DroppedItemController>().count = dropnumber[i];
            }
        }
        dropping.Clear();
        dropnumber.Clear();
    }
    public virtual void BeenAttack()
    {

        if (enemyStates == EnemyStates.DEAD) return;
        anim.SetTrigger("Hurt");
        AudioClip adc = GetComponent<AudioList>().clipList[1];
        SoundManager.Instance.Playsound(transform.position, adc, 1);
    }
}
