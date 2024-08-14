using UnityEngine;

public class IncreaseSize : MonoBehaviour
{
    public float attackRange = 5f;
    public Transform attackRangeIndicator;
    public Transform body;
    public Transform weaponTransform;

    public void Increase()
    {
        if (attackRangeIndicator != null)
        {
            Vector3 newScale = attackRangeIndicator.localScale * 1.1f;
            attackRangeIndicator.localScale = newScale;
        }

        if (body != null)
        {
            Vector3 newScale = body.localScale * 1.1f;
            body.localScale = newScale;
        }

        if (weaponTransform != null)
        {
            Vector3 newScale = weaponTransform.localScale * 1.1f;
            weaponTransform.localScale = newScale;
        }

        attackRange *= 1.1f;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
