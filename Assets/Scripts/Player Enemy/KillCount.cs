using UnityEngine;
using TMPro;

public class KillCount : MonoBehaviour
{
    public TMP_Text currentKillText;
    private int currentKill;
    private const float SizeIncreaseFactor = 1.1f;

    private void Start()
    {
        currentKillText.text = currentKill.ToString();
        UpdateTextSize();
    }
    private void LateUpdate()
    {
        gameObject.transform.rotation = Quaternion.Euler(40, 0, 0);
    }
    public void IncreaseKill(int v)
    {
        currentKill += v;
        currentKillText.text = currentKill.ToString();
        UpdateTextSize();
    }

    private void UpdateTextSize()
    {
        if (currentKillText != null)
        {
            currentKillText.fontSize *= SizeIncreaseFactor;
        }
    }
}
