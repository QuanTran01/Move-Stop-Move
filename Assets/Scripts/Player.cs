using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10f;
    public Animator anim;

    private bool isMoving;
    private Weapon weapon;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        weapon = GetComponent<Weapon>();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
        if (weapon != null && isMoving)
        {
            weapon.CheckForEnemiesInRange();
        }
    }

    private void PlayerMovement()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);

        isMoving = movement.magnitude > 0.1f;

        anim.SetFloat("speed", movement.magnitude);
    }   
}
