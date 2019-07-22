using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    PATROL,
    CHASE,
    ATTACK
}
public class EnemyController : MonoBehaviour
{
    private EnemyAnimator enemyAnimator;
    private NavMeshAgent agent;

    private EnemyState state;

    public float walkSpeed = 0.5f;
    public float runSpeed = 4f;
    public float chaseDistance = 7f;
    private float currentChaseDistance;
    public float attackDistance = 1.8f;
    public float chaseAfterAttackDistance = 2f;

    public float minPatrolRadius = 20f, maxPatrolRadius = 60f;
    public float patrolTimeLength = 15f;

    private float patrolTimer;

    public float waitBeforeAttack = 2f;
    private float attackTimer;


    private Transform target;

    public GameObject attackPoint;

    private Audio enemyAudio;

    void Awake()
    {
        enemyAnimator = GetComponent<EnemyAnimator>();
        agent = GetComponent<NavMeshAgent>();

        target = GameObject.FindWithTag("Player").transform;

        enemyAudio = GetComponentInChildren<Audio>();
    }


    // Start is called before the first frame update
    void Start()
    {
        state = EnemyState.PATROL;
        patrolTimer = patrolTimeLength;
        attackTimer = waitBeforeAttack;

        currentChaseDistance = chaseDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if(state==EnemyState.PATROL)
        {
            Patrol();
        }

        if(state==EnemyState.CHASE)
        {
            Chase();
        }

        if(state==EnemyState.ATTACK)
        {
            Attack();
        }
    }

    void Patrol()
    {
        agent.isStopped = false;
        agent.speed = walkSpeed;
        patrolTimer += Time.deltaTime;

        if(patrolTimer>patrolTimeLength)
        {
            SetNewRandomLocation();
            patrolTimer = 0f;
        }

        if(agent.velocity.sqrMagnitude>0)
        {
            enemyAnimator.Walk(true);
        }
        else
        {
            enemyAnimator.Walk(false);
        }

        if(Vector3.Distance(transform.position,target.position)<=chaseDistance)
        {
            enemyAnimator.Walk(false);
            state = EnemyState.CHASE;

            enemyAudio.PlayScream();

        }
    }

    void Chase()
    {
        agent.isStopped = false;
        agent.speed = runSpeed;

        if (agent.velocity.sqrMagnitude > 0)
        {
            enemyAnimator.Run(true);
         
        }
        else
        {
            enemyAnimator.Walk(false);
        }

        if(Vector3.Distance(transform.position,target.position)<=attackDistance)
        {
            enemyAnimator.Run(false);
            enemyAnimator.Walk(false);
            state = EnemyState.ATTACK;

            if(chaseDistance!=currentChaseDistance)
            {
                chaseDistance = currentChaseDistance;
            }
        }
        else if(Vector3.Distance(transform.position,target.position)>chaseDistance)
        {
            enemyAnimator.Run(false);
            state = EnemyState.PATROL;
            patrolTimer = patrolTimeLength;

            if (chaseDistance != currentChaseDistance)
            {
                chaseDistance = currentChaseDistance;
            }
        }
    }

    void Attack()
    {
        agent.velocity = Vector3.zero;
        agent.isStopped = true;

        attackTimer += Time.deltaTime;

        if(attackTimer>waitBeforeAttack)
        {
            enemyAnimator.Attack();
            attackTimer = 0f;
            enemyAudio.PlayAttack();
        }

        if(Vector3.Distance(transform.position,target.position)>attackDistance+chaseAfterAttackDistance)
        {
            state = EnemyState.CHASE;
        }
    }

    void SetNewRandomLocation()
    {
        float randomRadius = Random.Range(minPatrolRadius, maxPatrolRadius);
        Vector3 randomDirection = Random.insideUnitSphere * randomRadius;
        randomDirection += transform.position;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, randomRadius, -1);
        agent.SetDestination(navHit.position);
    }

    void TurnOnAttackPoint()
    {
        attackPoint.SetActive(true);
    }

    void TurnOffAttackPoint()
    {
        if (attackPoint.activeInHierarchy)
        {
            attackPoint.SetActive(true);
        }
    }

    public EnemyState enemyState
    {
        get; set;
    }
}
