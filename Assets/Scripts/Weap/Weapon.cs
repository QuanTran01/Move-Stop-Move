using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Rigidbody rb;
    private BoxCollider coll;
    public WeapPool weaponPool;

    public float dropForce = 20f;
    public float returnDelay = 2f;

    public bool equipped;
    public IncreaseSize increase;

    public Transform owner;
    private Animator ownerAnimator;
    public float rotationSpeed = 500f;

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
                    if (enemy != null && enemy.Die())
                    {
                        isKilled = true;
                    }
                }
                else if (collision.collider.CompareTag("Player"))
                {
                    Player player = collision.gameObject.GetComponent<Player>();
                    if (player != null && player.Die())
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
                            playerOwner.killCountUI.IncreaseKill(1);
                        }
                    }
                    else if (owner.CompareTag("Enemy"))
                    {
                        Enemy enemyOwner = owner.GetComponent<Enemy>();
                        if (enemyOwner != null)
                        {
                            enemyOwner.killCount++;
                            enemyOwner.killCountUI.IncreaseKill(1);
                        }
                    }

                    ReturnToContainer();
                }
                increase.Increase();
                //Destroy(collision.gameObject);
            }
        }
    }

    private void ReturnToContainer()
    {
        rb.isKinematic = true;
        coll.isTrigger = true;

        transform.SetParent(weaponPool.weapContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        equipped = true;
    }

    public void Attack(Vector3 targetPosition, Transform weaponOwner)
    {
        if (!equipped) return;

        equipped = false;
        owner = weaponOwner;
        ownerAnimator = owner.GetComponent<Animator>();

        StartCoroutine(PerformAttackAfterAnimation(targetPosition));
    }

    private IEnumerator PerformAttackAfterAnimation(Vector3 targetPosition)
    {
        float animationDuration = GetAttackAnimationDuration();
        yield return new WaitForSeconds(animationDuration);

        if (owner == null)
        {
            Destroy(gameObject);
            yield break;
        }

        transform.SetParent(null);
        rb.isKinematic = false;
        coll.isTrigger = false;

        Physics.IgnoreCollision(coll, owner.GetComponent<Collider>(), true);

        Vector3 direction = (targetPosition - transform.position).normalized;
        rb.AddForce(direction * dropForce, ForceMode.Impulse);

        StartCoroutine(TrackWeapon());
    }



    private float GetAttackAnimationDuration()
    {
        if (ownerAnimator != null)
        {
            AnimationClip[] clips = ownerAnimator.runtimeAnimatorController.animationClips;
            foreach (var clip in clips)
            {
                if (clip.name == "Attack")
                {
                    return clip.length;
                }
            }
        }
        return 0f;
    }

    private IEnumerator TrackWeapon()
    {
        while (!equipped)
        {
            float distance = Vector3.Distance(transform.position, owner.position);
            if (distance > increase.attackRange)
            {
                ReturnToContainer();
                yield break;
            }

            Vector3 flatUp = Vector3.up; 
            transform.up = flatUp;

            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime*100000f, Space.World);

            yield return null;
        }
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
