using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Rigidbody rb;
    private BoxCollider coll;
    private WeapPool weaponPool;

    public float attackRange = 5f;
    public float dropForce = 20f;
    public float returnDelay = 2f;

    public bool equipped;

    public IncreaseSize increase;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<BoxCollider>();
        weaponPool = WeapPool.Instance;
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
        if (collision.collider.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            increase.Increase();
        }
    }

    public void Attack(Vector3 targetPosition)
    {
        if (!equipped) return;

        equipped = false;

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
