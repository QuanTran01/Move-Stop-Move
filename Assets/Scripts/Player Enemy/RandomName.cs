using UnityEngine;
using TMPro;
public class RandomName : MonoBehaviour
{
    public string[] enemyNames;
    public TMP_Text nameText;

    private void Start()
    {
        if (enemyNames.Length > 0)
        {
            string randomName = GetRandomName();
            if (nameText != null)
            {
                nameText.text = randomName;
            }
        }
    }

    private string GetRandomName()
    {
        int randomIndex = Random.Range(0, enemyNames.Length);
        return enemyNames[randomIndex];
    }
}
