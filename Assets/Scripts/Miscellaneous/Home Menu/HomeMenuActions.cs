using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeMenuActions : MonoBehaviour
{
    public void PlayGame()
    {
        AudioManager.Instance.PlayClick();
        SceneManager.LoadScene("Game Scene");
    }
}