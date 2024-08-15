using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Rigidbody rb;
    private BoxCollider coll;
    public WeapPool weaponPool;

    public float attackRange = 5f;
    public float dropForce = 20f;
    public float returnDelay = 2f;

    public bool equipped;
    public IncreaseSize increase;

    public Transform owner;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<BoxCollider>();

        equipped = true;
        rb.isKinematic = true;
        coll.isTrigger = true;

        Transform weapContainer = weaponPool.weapContainer;
        transform.SetParent(weapContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Player"))
        {
            if (collision.transform != owner)
            {
                bool isKilled = false;

                if (collision.collider.CompareTag("Enemy"))
                {
                    Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                    if (enemy != null && enemy.Kill())
                    {
                        isKilled = true;
                    }
                }
                else if (collision.collider.CompareTag("Player"))
                {
                    Player player = collision.gameObject.GetComponent<Player>();
                    if (player != null && player.Kill())
                    {
                        isKilled = true;
                    }
                }

                if (isKilled)
                {
                    if (owner.CompareTag("Player"))
                    {
                        Player playerOwner = owner.GetComponent<Player>();
                        if (playerOwner != null)
                        {
                            playerOwner.killCount++;
                            KillCount.Instance.IncreaseKill(1, true);
                        }
                    }
                    else if (owner.CompareTag("Enemy"))
                    {
                        Enemy enemyOwner = owner.GetComponent<Enemy>();
                        if (enemyOwner != null)
                        {
                            enemyOwner.killCount++;
                            KillCount.Instance.IncreaseKill(1, false);
                        }
                    }
                }


                increase.Increase();
                Destroy(collision.gameObject);
            }
        }
    }

    public void Attack(Vector3 targetPosition, Transform weaponOwner)
    {
        if (!equipped) return;

        equipped = false;
        owner = weaponOwner;

        transform.SetParent(null);
        rb.isKinematic = false;
        coll.isTrigger = false;

        Vector3 direction = (targetPosition - transform.position).normalized;
        rb.AddForce(direction * dropForce, ForceMode.Impulse);

        StartCoroutine(InstantiateWeaponAfterDelay(returnDelay, targetPosition));
    }

    private IEnumerator InstantiateWeaponAfterDelay(float delay, Vector3 targetPosition)
    {
        yield return new WaitForSeconds(delay);

        GameObject newWeapon = weaponPool.GetWeapon();
        Weapon newWeaponController = newWeapon.GetComponent<Weapon>();

        newWeaponController.equipped = true;

        Rigidbody newRb = newWeapon.GetComponent<Rigidbody>();
        BoxCollider newColl = newWeapon.GetComponent<BoxCollider>();

        newRb.isKinematic = true;
        newColl.isTrigger = true;

        newWeaponController.transform.SetParent(weaponPool.weapContainer);
        newWeaponController.transform.localPosition = Vector3.zero;
        newWeaponController.transform.localRotation = Quaternion.identity;
        newWeaponController.transform.localScale = Vector3.one;
    }
}
