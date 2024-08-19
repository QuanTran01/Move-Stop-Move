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
    public FloatingJoystick joystick; // Changed to FloatingJoystick
    private IncreaseSize increaseSize;
    public KillCount killCountUI;
    public int killCount;
    private bool isAttacking;
    private Collider playerCollider;

    private void Start()
    {
        increaseSize = GetComponent<IncreaseSize>();
        rb = GetComponent<Rigidbody>();
        weapon = GetComponentInChildren<Weapon>();
        hasAttacked = false;
        if (killCountUI != null)
        {
            killCountUI.currentKillText.text = killCount.ToString();
        }
        playerCollider = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        PlayerMovement();

        if (isMoving && !anim.GetCurrentAnimatorStateInfo(0).IsName("run"))
        {
            anim.SetTrigger("run");
        }

        if (weapon != null && !isMoving && !hasAttacked)
        {
            Transform other = CheckForOtherInRange();
            if (other != null)
            {
                hasAttacked = true;
                isAttacking = true;
                StartCoroutine(AttackAfterAnimation(other.position, transform));
            }
        }
    }

    private IEnumerator AttackAfterAnimation(Vector3 targetPosition, Transform weaponOwner)
    {
        anim.SetTrigger("attack");

        yield return new WaitForSeconds(0.2f);

        weapon.Attack(targetPosition, weaponOwner);

        if (isMoving)
        {
            anim.SetTrigger("run");
        }
        else
        {
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
            anim.SetTrigger("idle");
        }

        isAttacking = false;
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

    private void PlayerMovement()
    {
        Vector2 input = new Vector2(joystick.Horizontal, joystick.Veritcal);
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
        if (playerCollider == null || !playerCollider.enabled)
        {
            return;
        }

        if (collision.collider.CompareTag("Zombie"))
        {
            Destroy(gameObject);
        }
    }

    public bool Die()
    {
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        anim.SetTrigger("dead");

        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }

        this.enabled = false;

        return true;
    }

    public void IncreaseKill()
    {
        killCount++;
        killCountUI.IncreaseKill(1);
    }

    public void IncreaseSize()
    {
        if (increaseSize != null)
        {
            increaseSize.Increase();
        }
    }
}
