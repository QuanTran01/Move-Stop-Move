using UnityEngine;

public interface IAttackable
{
    void Attack(Vector3 targetPosition);
    bool CheckForTargetsInRange();
}
