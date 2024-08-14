using UnityEngine;

public class WeapPool : MonoBehaviour
{
    public static WeapPool Instance { get; private set; }
    public GameObject weaponUse;
    public Transform weapContainer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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
