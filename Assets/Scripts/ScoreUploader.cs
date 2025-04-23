using UnityEngine;
using SQLite4Unity3d;
using System;
using System.IO;

public class ScoreUploader : MonoBehaviour
{
    private SQLiteConnection db;

    void Start()
    {
        string dbPath = Path.Combine(Application.streamingAssetsPath, "gamedatabase.sqlite");
        db = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        db.CreateTable<scores>(); // Make sure the table exists
    }

    public void UploadFinalScore()
    {
        int userId = LoginUIManager.LoggedInUserId;
        int finalScore = ScoreTracking.Instance.GetScore();

        if (userId == -1)
        {
            Debug.LogError("User not logged in. Cannot upload score.");
            return;
        }

        scores newScore = new scores
        {
            user_id = userId,
            score_value = finalScore,
            score_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        db.Insert(newScore);
        Debug.Log($"Uploaded score: {finalScore} for user_id: {userId}");
    }

    public class scores
    {
        [PrimaryKey, AutoIncrement]
        public int score_id { get; set; }
        public int user_id { get; set; }
        public int score_value { get; set; }
        public string score_date { get; set; }
    }
}
