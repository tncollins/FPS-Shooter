using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    private EnemyAnimator enemy;
    private NavMeshAgent agent;
    private EnemyController enemyController;

    public float health = 100f;

    public bool isPlayer, isBoar, isCannibal, isDead;

    private Audio enemyAudio;

    private PlayerStats stats;
    void Awake()
    {
        if(isBoar||isCannibal)
        {
            enemy = GetComponent<EnemyAnimator>();
            enemyController = GetComponent<EnemyController>();
            agent = GetComponent<NavMeshAgent>();

            enemyAudio = GetComponentInChildren<Audio>();
        }

        if(isPlayer)
        {
            stats = GetComponent<PlayerStats>();
        }
    }

    public void ApplyDamage(float damage)
    {
        print("I hit the cannibal");
        if (isDead)
            return;
        health -= damage;

        if (isPlayer)
        {
            stats.DisplayHealthStats(health);
        }

        if(isBoar||isCannibal)
        {
            if(enemyController.enemyState==EnemyState.PATROL)
            {
                enemyController.chaseDistance = 50f;
            }
        }

        if(health<=0f)
        {
            PlayerDied();
            isDead = true;
        }
    }

    void PlayerDied()
    {
        if(isCannibal)
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<BoxCollider>().isTrigger = false;
            GetComponent<Rigidbody>().AddTorque(-transform.forward*5f);

            enemyController.enabled = false;
            agent.enabled = false;
            enemy.enabled = false;

            StartCoroutine(DeathSound());

            EnemyManager.instance.EnemyDied(true);
        }

        if(isBoar)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            enemyController.enabled = false;

            enemy.Dead();

            StartCoroutine(DeathSound());
            EnemyManager.instance.EnemyDied(false);
        }

        if(isPlayer)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for(int i=0;i<enemies.Length;i++)
            {
                enemies[i].GetComponent<EnemyController>().enabled = false;
            }

            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<WeaponManager>().GetCurrentSelectedWeapon().gameObject.SetActive(false);
            EnemyManager.instance.StopSpawning();
        }

        if(tag=="Player")
        {
            Invoke("RestartGame", 3f);
        }
        else
        {
            Invoke("TurnOffGameObject", 3f);
        }
    }

    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene");
    }

    void TurnOffGameObject()
    {
        gameObject.SetActive(false);
    }

    IEnumerator DeathSound()
    {
        yield return new WaitForSeconds(0.3f);
        enemyAudio.PlayDead();
    }
}
