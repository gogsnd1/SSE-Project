using UnityEngine;
using TMPro;

public class ScoreTracking : MonoBehaviour
{
    public static ScoreTracking Instance { get; private set; }

    public int score = 0;

    [Header("UI Reference")]
    public TextMeshProUGUI scoreText; // Assign this in the Inspector

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateScoreText();
    }

    public void AddScore(int amount = 1)
    {
        score += amount;
        UpdateScoreText();
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
