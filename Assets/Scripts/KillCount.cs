using UnityEngine;
using TMPro;

public class KillCount : MonoBehaviour
{
    public static KillCount Instance;
    public TMP_Text playerKillText;
    public TMP_Text enemyKillText;
    public int currentPlayerKills;
    public int currentEnemyKills;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerKillText.text = currentPlayerKills.ToString();
        enemyKillText.text = currentEnemyKills.ToString();
    }

    public void IncreaseKill(int v, bool isPlayer)
    {
        if (isPlayer)
        {
            currentPlayerKills += v;
            playerKillText.text = currentPlayerKills.ToString();
        }
        else
        {
            currentEnemyKills += v;
            enemyKillText.text = currentEnemyKills.ToString();
        }
    }
}
