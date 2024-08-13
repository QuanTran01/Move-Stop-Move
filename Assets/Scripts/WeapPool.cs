using UnityEngine;

public class WeapPool : MonoBehaviour
{
    public static WeapPool Instance { get; private set; }
    public GameObject weaponInHierarchy; // Tham chiếu đến vũ khí đã có trong Hierarchy
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
        // Đặt vũ khí từ Hierarchy vào container ngay từ đầu
        if (weaponInHierarchy != null)
        {
            weaponInHierarchy.transform.SetParent(weapContainer);
            weaponInHierarchy.transform.localPosition = Vector3.zero;
            weaponInHierarchy.transform.localRotation = Quaternion.identity;
            weaponInHierarchy.transform.localScale = Vector3.one;
        }
    }

    public GameObject GetWeapon()
    {
        // Trả về vũ khí từ Hierarchy
        return weaponInHierarchy;
    }

    public void ReturnWeapon(GameObject weapon)
    {
        // Đặt lại vũ khí về container (nếu cần)
        ResetWeapon(weapon);
    }

    private void ResetWeapon(GameObject weapon)
    {
        weapon.transform.SetParent(weapContainer);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        weapon.transform.localScale = Vector3.one;
    }
}
