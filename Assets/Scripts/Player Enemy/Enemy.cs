using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float attackRange = 5f;
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;

    private bool isMoving;
    public bool hasAttacked;
    private Weapon weapon;
    private Rigidbody rb;
    public Animator anim;
    public IncreaseSize increaseSize;
    private NavMeshAgent agent;
    private float timer;
    private bool isDead = false;
    private Collider enemyCollider;

    public KillCount killCountUI;
    public int killCount;

    public static System.Action<Transform> OnDeath;

    private void Start()
    {
        increaseSize = GetComponent<IncreaseSize>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        weapon = GetComponentInChildren<Weapon>();
        timer = wanderTimer;

        if (killCountUI != null)
        {
            killCountUI.currentKillText.text = killCount.ToString();
        }

        hasAttacked = false;
        enemyCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (!isDead)
        {
            EnemyMovement();
            if (weapon != null && !isMoving && !hasAttacked)
            {
                Transform other = CheckForOtherInRange();
                if (other != null)
                {
                    hasAttacked = true;
                    weapon.Attack(other.position, transform);
                    StartCoroutine(PerformAttack());
                }
            }
        }
    }

    private Transform CheckForOtherInRange()
    {
        Collider[] others = Physics.OverlapSphere(transform.position, increaseSize.attackRange);
        foreach (Collider other in others)
        {
            if (other.CompareTag("Player"))// || other.CompareTag("Enemy"))
            {
                return other.transform;
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
        isMoving = agent.velocity.magnitude > 0.0000000001f;
        anim.SetFloat("speed", agent.velocity.magnitude);
        if (isMoving)
        {
            hasAttacked = false;
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

    public bool Die()
    {
        if (isDead) return false;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        anim.SetTrigger("dead");
        isDead = true;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        OnDeath?.Invoke(transform);

        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        StartCoroutine(Dead());
        return true;
    }

    private IEnumerator Dead()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    public void IncreaseSize()
    {
        if (increaseSize != null)
        {
            increaseSize.Increase();
        }
    }

    public void IncreaseKill()
    {
        killCount++;
        killCountUI.IncreaseKill(1);
    }
}
