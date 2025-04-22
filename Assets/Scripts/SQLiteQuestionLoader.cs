using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class SQLiteQuestionLoader : MonoBehaviour
{
    public Text questionText;
    public Button[] answerButtons;

   // private SQLiteConnection db;
    private int currentQuestionIndex = 0;
//    private List<Question> questions;

    void Start()
    {
        string dbPath = Path.Combine(Application.persistentDataPath, "gamedatabase.sqlite");

        // Copy database if it's not already in persistentDataPath
        if (!File.Exists(dbPath))
        {
            string sourcePath = Path.Combine(Application.streamingAssetsPath, "gamedatabase.sqlite");
#if UNITY_ANDROID && !UNITY_EDITOR
            WWW reader = new WWW(sourcePath);
            while (!reader.isDone) {}
            File.WriteAllBytes(dbPath, reader.bytes);
#else
            File.Copy(sourcePath, dbPath);
#endif
        }

//        db = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
//       questions = db.Table<Question>().OrderBy(q => UnityEngine.Random.value).ToList();


        //LoadQuestion(questions[currentQuestionIndex]);
    }

/*   void LoadQuestion(Question q)
    {
        questionText.text = q.question_text;

        var answers = db.Table<Answer>().Where(a => a.question_id == q.question_id).ToList();

        for (int i = 0; i < answers.Count && i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].GetComponentInChildren<Text>().text = answers[i].answer_text;
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerClicked(answers[index].is_correct));
        }
    }
*/
    void OnAnswerClicked(bool isCorrect)
    {
        Debug.Log(isCorrect ? "✅ Correct!" : "❌ Wrong.");

        currentQuestionIndex++;
        /* if (currentQuestionIndex < questions.Count)
        {
            LoadQuestion(questions[currentQuestionIndex]);
        }
        else
        {
            Debug.Log("No more questions!");
        }
        */
    }
}