using UnityEngine;
using TMPro;

public class IncreaseSize : MonoBehaviour
{
    public float attackRange = 5f;
    public Transform body;
    public Transform weaponTransform;
    public CameraFollow cameraFollow;
    public SpriteRenderer attackRangeSpriteRenderer;
    public Weapon weapon;
    public TMP_Text textMeshPro;

    private const float OriginalSize = 4.5f;

    private void Start()
    {
        if (attackRangeSpriteRenderer != null)
        {
            UpdateAttackRangeSprite();
        }

        if (textMeshPro != null)
        {
            UpdateTextMeshProSize();
        }
    }

    public void Increase()
    {
        IncreaseScale(body);
        IncreaseScale(weaponTransform);
        attackRange *= 1.1f;

        if (cameraFollow != null)
        {
            cameraFollow.UpdateCameraScale(1.1f);
        }

        if (weapon != null)
        {
            weapon.dropForce *= 1.1f;
        }

        if (attackRangeSpriteRenderer != null)
        {
            UpdateAttackRangeSprite();
        }

        if (textMeshPro != null)
        {
            UpdateTextMeshProSize();
        }
    }

    private void IncreaseScale(Transform targetTransform)
    {
        if (targetTransform != null)
        {
            targetTransform.localScale *= 1.1f;
        }
    }

    private void UpdateAttackRangeSprite()
    {
        if (attackRangeSpriteRenderer != null)
        {
            float originalRadius = OriginalSize / 2;
            float scale = attackRange / originalRadius;
            attackRangeSpriteRenderer.transform.localScale = new Vector3(scale, scale, 1f);
        }
    }

    private void UpdateTextMeshProSize()
    {
        if (textMeshPro != null)
        {
            textMeshPro.fontSize *= 1.1f;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
