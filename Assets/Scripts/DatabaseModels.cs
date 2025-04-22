using SQLite4Unity3d;

public class Question
{
    [PrimaryKey, AutoIncrement]
    public int question_id { get; set; }
    public string question_text { get; set; }
    public string question_difficulty { get; set; }
    public string question_category { get; set; }
}

public class Answer
{
    [PrimaryKey, AutoIncrement]
    public int answer_id { get; set; }
    public int question_id { get; set; }
    public string answer_text { get; set; }
    public bool is_correct { get; set; }
}
