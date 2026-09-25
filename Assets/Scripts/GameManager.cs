using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject gameOverText;
    public GameObject restartButton;
    public TMP_Text scoreText;

    private float timeSurvived = 0f;
    private bool isGameOver = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!isGameOver)
        {
            timeSurvived += Time.deltaTime;
            scoreText.text = "Score: " + Mathf.FloorToInt(timeSurvived);
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        gameOverText.SetActive(true);
        restartButton.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}