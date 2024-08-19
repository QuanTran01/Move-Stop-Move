using UnityEngine;

public class WeapPool : MonoBehaviour
{
    public static WeapPool playerInstance;
    public GameObject weaponUse;
    public Transform weapContainer;

    public bool isPlayerPool;

    private void Awake()
    {
        if (isPlayerPool)
        {
            if (playerInstance == null)
            {
                playerInstance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        if (weaponUse != null)
        {
            weaponUse.transform.SetParent(weapContainer);
            weaponUse.transform.localPosition = Vector3.zero;
            weaponUse.transform.localRotation = Quaternion.identity;
            weaponUse.transform.localScale = Vector3.one;
        }
    }

    public GameObject GetWeapon()
    {
        return weaponUse;
    }

    public void ReturnWeapon(GameObject weapon)
    {
        weapon.transform.SetParent(weapContainer);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        weapon.transform.localScale = Vector3.one;
    }
}