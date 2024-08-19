using UnityEngine;

public class Name : MonoBehaviour
{
    
    private void LateUpdate()
    {
        gameObject.transform.rotation=Quaternion.Euler(40,0,0);
    }
}