using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadLevelButton : MonoBehaviour
{
    public void OnReloadButtonClick()
    {
        SceneManager.LoadScene("Level 1");
    }
}
