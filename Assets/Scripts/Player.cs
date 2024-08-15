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
    public Joystick joystick;

    private IncreaseSize increaseSize;
    public int killCount;

    private void Start()
    {
        increaseSize = GetComponent<IncreaseSize>();
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
                weapon.Attack(other.position, transform);
                StartCoroutine(PerformAttack());
            }
        }
    }

    private Transform CheckForOtherInRange()
    {
        Collider[] others = Physics.OverlapSphere(transform.position, increaseSize.attackRange);
        foreach (Collider other in others)
        {
            if (other.CompareTag("Enemy") || other.CompareTag("Zombie"))
            {
                return other.transform;
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
        Vector2 input = joystick.InputVector;
        Vector3 movement = new Vector3(input.x, 0, input.y) * moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);

        isMoving = movement.magnitude > 0.0000000001f;

        if (isMoving)
        {
            hasAttacked = false;
            if (movement != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement.normalized);
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * moveSpeed));
            }
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

    public bool Kill()
    {
        Destroy(gameObject);
        return true;
    }

    public void IncreaseSize()
    {
        if (increaseSize != null)
        {
            increaseSize.Increase();
        }
    }
}
