using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndManager : MonoBehaviour
{
    [Header("UI Attachments")]
    [SerializeField] private GameObject gameWinWindow;
    [SerializeField] private TextMeshProUGUI gameTimeUI;
    [SerializeField] private GameObject gamwLoseWindow;

    public static GameEndManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameWinWindow.SetActive(true);

            GameManager.Instance.ToggleMovementOvverride(false);
            GameManager.Instance.ToggleGameTimer(false);

            gameTimeUI.text = GameManager.Instance.GetGameTime();
        }
    }

    public IEnumerator TriggerLose(float delay)
    {
        GameManager.Instance.ToggleMovementOvverride(false);
        GameManager.Instance.ToggleGameTimer(false);

        yield return new WaitForSeconds(delay);

        gamwLoseWindow.SetActive(true);
    }

    #region Button Actions
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        AudioManager.Instance.PlayClick();
    }

    public void GoHome()
    {
        SceneManager.LoadScene("Home Menu");
        AudioManager.Instance.PlayClick();
    }
    #endregion
}