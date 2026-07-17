using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float gameTime;
    public bool gameTimeOn;

    public bool stopAllMovementsOverride = false;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void ToggleMovementOvverride(bool allowMove)
    {
        stopAllMovementsOverride = !allowMove;
    }

    private void Update()
    {
        if (gameTimeOn)
        {
            gameTime += Time.deltaTime;
        }
    }

    public void ToggleGameTimer(bool toggle)
    {
        gameTimeOn = toggle;
    }

    public string GetGameTime()
    {
        int minutes = Mathf.FloorToInt(gameTime / 60f);
        int seconds = Mathf.FloorToInt(gameTime % 60f);

        return $"{minutes:00}:{seconds:00}";
    }
}