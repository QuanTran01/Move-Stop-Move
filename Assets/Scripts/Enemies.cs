using UnityEngine;
using UnityEngine.AI;

public class Enemies : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float attackRange = 5f;
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;

    private Rigidbody rb;
    public Animator anim;
    private float lastAttackTime = 0f;
    public float attackCooldown = 1f;
    private bool isInAttackRange = false;
    public Transform player;
    private NavMeshAgent agent;
    private float timer;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPos;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }

        //CheckForPlayerInRange();
    }

    //private void CheckForPlayerInRange()
    //{
    //    float distanceToPlayer = Vector3.Distance(transform.position, player.position);

    //    if (distanceToPlayer <= attackRange)
    //    {
    //        isInAttackRange = true;
    //        if (Time.time >= lastAttackTime + attackCooldown)
    //        {
    //            //ShootBullet(player.position);
    //            anim.SetBool("attack", true);
    //            lastAttackTime = Time.time;
    //        }
    //    }
    //    else
    //    {
    //        isInAttackRange = false;
    //        anim.SetBool("attack", false);
    //    }
    //}

    //private void ShootBullet(Vector3 targetPosition)
    //{
    //    GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPos.position, Quaternion.identity);
    //    Vector3 direction = (targetPosition - bulletSpawnPos.position).normalized;
    //    Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
    //    if (bulletRb != null)
    //    {
    //        bulletRb.velocity = direction * 20f;
    //    }
    //}

    //public void IncreaseAtkRange()
    //{
    //    attackRange += 1f;
    //}

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
