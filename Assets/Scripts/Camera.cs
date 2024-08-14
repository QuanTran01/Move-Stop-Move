using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 7.8f, -14f);
    public Vector3 rotation = new Vector3(40, 0, 0);

    private void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}
