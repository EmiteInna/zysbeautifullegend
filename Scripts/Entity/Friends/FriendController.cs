using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum FriendStates { GUARD, FOLLOW, CHASE, DEAD }
[RequireComponent(typeof(NavMeshAgent))]
public class FriendController : MonoBehaviour,IEndGameObserver
{
    public FriendStates friendStates;
    public NavMeshAgent agent;
    public Collider coll;
    public CharacterStats characterStats;
    [Header("»ù±¾ÉèÖÃ")]
    public float movespeed;
    public float detectRange;
    public float followRange;
    public float distantRange;
    public GameObject attackTarget;
    public Animator anim;
    [Header("¸úËæ")]
    public GameObject master;
    
    [Header("CD")]
    public float attackCD;
    
    [Header("×´Ì¬")]
    public bool playerAlive;
    [Header("µôÂäÎï")]
    public List<GameObject> dropping;
    public List<int> dropnumber;
    public List<float> possibility;
    private void Awake()
    {
        agentInitiate();
        friendStates = FriendStates.FOLLOW;
        characterStats = GetComponent<CharacterStats>();
        anim = GetComponent<Animator>();
        playerAlive = true;
    }
    void Start()
    {
        GManager.Instance.addObserver(this);
        characterStats.Initialize();
    }
    void FindMaster()
    {
        master = GameObject.FindGameObjectWithTag("Player");

    }
    private void FixedUpdate()
    {
        if (playerAlive == true)
        {
            if (attackCD > 0) attackCD -= Time.fixedDeltaTime;
            SwitchStates();
        }
    }
    void agentInitiate()
    {
        agent.speed = movespeed;
    }
    void SwitchStates()
    {
        
        if (friendStates == FriendStates.DEAD) return;
        if (master == null)
        {
            FindMaster();
        }
        if (characterStats.currentHealth == 0)
        {
            Death();
            return;
        }
        if (FoundTarget())
        {
            friendStates = FriendStates.CHASE;
            //  Debug.Log("OHSHIT");
        }
        switch (friendStates)
        {
            case FriendStates.GUARD:
                break;
            case FriendStates.FOLLOW:                
                if (CloseToMaster())
                {
                    agent.isStopped = true;
                }
                else
                {
                    agent.isStopped = false;
                    agent.destination = master.transform.position;
                }
                break;
            case FriendStates.CHASE:
                if (TooDistantWithMaster())
                {
                    agent.isStopped = false;
                    friendStates = FriendStates.FOLLOW;
                    break;
                }
                if (!FoundTarget())
                {
                    agent.isStopped = false;
                    friendStates = FriendStates.FOLLOW;
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
            case FriendStates.DEAD:
                break;
        }
    }
    bool FoundTarget()
    {
        var colliders = Physics.OverlapSphere(transform.position, detectRange);
        foreach (var tar in colliders)
        {
            if (tar.CompareTag("enemy")||tar.CompareTag("boss"))
            {
                transform.LookAt(tar.transform.position);
                attackTarget = tar.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }
    bool TooDistantWithMaster()
    {
        return (Vector3.Distance(transform.position, master.transform.position) >= distantRange);
    }
    bool CloseToMaster()
    {
        return (Vector3.Distance(transform.position, master.transform.position) <= followRange);
    }
    bool TargetInRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackRange;
        return false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, followRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, distantRange);
    }

    public virtual void SkillDetect()
    {

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
        if (friendStates == FriendStates.DEAD) return;
        if (attackTarget == null) return;
        AudioClip adc = GetComponent<AudioList>().clipList[0];
        SoundManager.Instance.Playsound(transform.position, adc, 1);
        var targetStats = attackTarget.GetComponent<CharacterStats>();
        targetStats.TakeDamage(characterStats, targetStats);
        if (attackTarget.tag == "Player") attackTarget.GetComponent<PlayerController>().BeenAttack();
        else if (attackTarget.tag == "enemy"|| attackTarget.tag == "boss") attackTarget.GetComponent<EnemyController>().BeenAttack();
        else if (attackTarget.tag == "friend") attackTarget.GetComponent<FriendController>().BeenAttack();
    }
    public virtual void Death()
    {
        friendStates = FriendStates.DEAD;
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
        for (int i = 0; i < dropping.Count; i++)
        {
            if (UnityEngine.Random.Range(0, 1) < possibility[i])
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

        if (friendStates == FriendStates.DEAD) return;
        anim.SetTrigger("Hurt");
        AudioClip adc = GetComponent<AudioList>().clipList[1];
        SoundManager.Instance.Playsound(transform.position, adc, 1);
    }
}
