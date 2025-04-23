using UnityEngine;
using TMPro;
using System.Collections.Generic;
using SQLite4Unity3d;
using System.IO;
using System.Linq;

public class Leaderboard : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text leaderboardText;

    private SQLiteConnection db;

    void Start()
    {
        string dbPath = Path.Combine(Application.streamingAssetsPath, "gamedatabase.sqlite");
        db = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadOnly);

        DisplayLeaderboard();
    }

    public void DisplayLeaderboard()
    {
        string output = "Leaderboard:\n";

        var topScores = db.Query<ScoreEntry>(
            @"SELECT users.user_username AS username, scores.score_value AS score 
              FROM scores 
              JOIN users ON scores.user_id = users.user_id 
              ORDER BY score DESC 
              LIMIT 10");

        if (topScores.Count == 0)
        {
            leaderboardText.text = "No scores available.";
            return;
        }

        foreach (var entry in topScores)
        {
            output += $"{entry.username} - {entry.score} points\n";
        }

        leaderboardText.text = output;
    }

    // DTO for result mapping
    private class ScoreEntry
    {
        public string username { get; set; }
        public int score { get; set; }
    }
}

