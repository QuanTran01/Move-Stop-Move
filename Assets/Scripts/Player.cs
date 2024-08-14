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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        weapon = GetComponentInChildren<Weapon>();
        hasAttacked = false;
    }

    private void FixedUpdate()
    {
        PlayerMovement();

        if (weapon != null && !isMoving && !hasAttacked)
        {
            if (weapon.CheckForEnemiesInRange())
            {
                hasAttacked = true;
                StartCoroutine(PerformAttack());
            }
        }
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
}
