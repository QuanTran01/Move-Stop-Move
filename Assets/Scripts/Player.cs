using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10f;
    public Animator anim;

    private bool isMoving;
    private bool hasAttacked;
    private Weapon weapon;
    private Rigidbody rb;

    private IncreaseSize increaseSize;

    private void Start()
    {
        increaseSize=GetComponent<IncreaseSize>();
        rb = GetComponent<Rigidbody>();
        weapon = GetComponentInChildren<Weapon>();
        hasAttacked = false;
    }

    private void FixedUpdate()
    {
        PlayerMovement();

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
            if (others.CompareTag("Enemy") || others.CompareTag("Zombie"))
            {
                return others.transform;
            }
        }
        return null;
    }

    private IEnumerator PerformAttack()
    {
        anim.SetTrigger("attack");

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        anim.SetTrigger("idle");
    }

    private void PlayerMovement()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);

        isMoving = movement.magnitude > 0.000000000001f;

        if (isMoving)
        {
            hasAttacked = false;
        }

        anim.SetFloat("speed", movement.magnitude);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Zombie"))
        {
            Destroy(gameObject);
        }
    }

    public void IncreaseSize()
    {
        if (increaseSize != null)
        {
            increaseSize.Increase();
        }
    }
}
