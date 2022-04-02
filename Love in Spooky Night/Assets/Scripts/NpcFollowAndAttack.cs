using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcFollowAndAttack : MonoBehaviour
{
    public float attackDistance = 1f, viewDistance = 10f, cooldown = 3f, speed = 3.5f;
    public int damage = 50;

    List<GameObject> enemies;

    GameObject lockTarget;
    string searchTarget = "";

    bool canAttack = false;
    float currentCD = 0f;

    NavMeshAgent agent;

    private void Start()
    {
        enemies = new List<GameObject>();
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;

        currentCD = cooldown;
        
        if (CompareTag("enemy")) //making enemy list for enemy
        {
            searchTarget = "friend";
        }
        else  //making enemy list for friends
        {
            searchTarget = "enemy";
        }

        FillEnemyList();
    }

    private void Update()
    {
        //stop moving if dead
        if (CompareTag("dead"))
        {
            return;
        }

        //return if nothing in enemy list
        if (enemies.Count == 0)
        {
            FillEnemyList();
            agent.enabled = false;
            return;
        }

        //removing enemy if its dead with checking its tag
        if(lockTarget != null && lockTarget.CompareTag("dead"))
        {
            enemies.Remove(lockTarget);
        }
        
        //detecting target if there is none
        if(lockTarget == null)
        {
            FillEnemyList();
            agent.enabled = false;

            float shortestDistances = viewDistance;
            foreach (GameObject enemy in enemies)
            {
                float mesafe = Mathf.Abs(Vector3.Distance(transform.position, enemy.transform.position));
                if (shortestDistances > mesafe)
                {
                    shortestDistances = mesafe;
                    lockTarget = enemy;
                }
            }
            return;
        }

        //checking distance for attacking
        if(lockTarget != null)
        {
            canAttack = Mathf.Abs(Vector3.Distance(transform.position, lockTarget.transform.position)) < attackDistance;
        }

        //attacking
        if (canAttack)
        {
            transform.LookAt(lockTarget.transform);

            agent.enabled = false;
            currentCD -= Time.deltaTime;
            if(currentCD <= 0)
            {
                lockTarget.GetComponent<HealthSystem>().TakeDamage(damage);
                currentCD = cooldown;
            }
        }
        else
        {
            agent.enabled = true;
            agent.SetDestination(lockTarget.transform.position);
        }

        //resetting enemy if it went too far
        if (Mathf.Abs(Vector3.Distance(transform.position, lockTarget.transform.position)) > viewDistance) 
        {
            lockTarget = null;
        }
    }

    void FillEnemyList()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(searchTarget))
        {
            enemies.Add(enemy);
        }
        if (CompareTag("enemy"))
        {
            enemies.Add(GameObject.FindGameObjectWithTag("Player"));
        }
    }
}
