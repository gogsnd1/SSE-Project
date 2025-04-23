using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class Leaderboard : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text leaderboardText;
    public ScoreUploader scoreUploader; // Reference to ScoreUploader

    void Start()
    {
        DisplayLeaderboard();
    }

    public void DisplayLeaderboard()
    {
        if (scoreUploader == null || scoreUploader.uploadedScores.Count == 0)
        {
            leaderboardText.text = "No scores available.";
            return;
        }

        var topScores = scoreUploader.uploadedScores
            .OrderByDescending(s => s.score)
            .Take(10);

        string output = "Leaderboard:\n";

        foreach (var score in topScores)
        {
            string username = scoreUploader.GetUsernameById(score.user_id);
            output += $"{username} - {score.score} points\n";
        }

        leaderboardText.text = output;
    }
}
