using UnityEngine;
using UnityEngine.UI;

public class OffScreenIndicator : MonoBehaviour
{
    public Transform target;
    public Image indicatorImage;
    public Camera mainCamera;
    public float edgeBuffer = 10f;

    void Update()
    {
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(target.position);

        if (screenPosition.z > 0 && screenPosition.x > 0 && screenPosition.x < Screen.width && screenPosition.y > 0 && screenPosition.y < Screen.height)
        {
            indicatorImage.gameObject.SetActive(false);
        }
        else
        {
            indicatorImage.gameObject.SetActive(true);

            screenPosition.x = Mathf.Clamp(screenPosition.x, edgeBuffer, Screen.width - edgeBuffer);
            screenPosition.y = Mathf.Clamp(screenPosition.y, edgeBuffer, Screen.height - edgeBuffer);

            indicatorImage.transform.position = screenPosition;

            Vector3 indicatorDirection = (target.position - mainCamera.transform.position).normalized;
            float angle = Mathf.Atan2(indicatorDirection.y, indicatorDirection.x) * Mathf.Rad2Deg;
            indicatorImage.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
