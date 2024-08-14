using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float attackRange = 5f;
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;

    private bool isMoving;
    private bool hasAttacked;
    private Weapon weapon;
    private Rigidbody rb;
    public Animator anim;
    public IncreaseSize increaseSize;

    private NavMeshAgent agent;
    private float timer;

    private void Start()
    {
        increaseSize=GetComponent<IncreaseSize>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }

    private void Update()
    {
        EnemyMovement();
        if (weapon != null && !isMoving && !hasAttacked)
        {
            Transform other = CheckForOtherInRange();
            if (other != null)
            {
                hasAttacked = true;
                weapon.Attack(other.position);
                StartCoroutine(PerformAttack());
            }
        }
    }
    private Transform CheckForOtherInRange()
    {
        Collider[] other = Physics.OverlapSphere(transform.position, increaseSize.attackRange);
        foreach (Collider others in other)
        {
            if (others.CompareTag("Player") || others.CompareTag("Zombie"))
            {
                return others.transform;
            }
        }
        return null;
    }
    private void EnemyMovement()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }
    private IEnumerator PerformAttack()
    {
        anim.SetTrigger("attack");

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        anim.SetTrigger("idle");
    }


    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, attackRange);
    }
}
