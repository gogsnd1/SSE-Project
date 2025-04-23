using UnityEngine;
using SQLite4Unity3d;
using System;
using System.IO;
using static LoginUIManager;
using System.Linq;
using System.Collections.Generic;

public class ScoreUploader : MonoBehaviour
{
    private SQLiteConnection db;
    public List<scores> uploadedScores = new List<scores>();

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        string dbPath = Path.Combine(Application.streamingAssetsPath, "gamedatabase.sqlite");
        db = new SQLiteConnection(dbPath);
        db.CreateTable<scores>();
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
            score = finalScore,
            score_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        db.Insert(newScore);
        Debug.Log($"Uploaded score: {finalScore} for user_id: {userId}");
    }

    public void OnSubmitScore()
{
    UploadFinalScore(); // Call your score-saving method
    ScoreTracking.Instance.ResetScore(); // Optional: reset after saving
}

    public class scores
    {
        [PrimaryKey, AutoIncrement]
        public int score_id { get; set; }
        public int user_id { get; set; }
        public int score { get; set; }
        public string score_date { get; set; }
    }

    public class users
    {
        [PrimaryKey, AutoIncrement]
        public int user_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }

    public string GetUsernameById(int userId)
    {
        var user = db.Table<users>().FirstOrDefault(u => u.user_id == userId);
        return user != null ? user.username : "Unknown";
    }

}
