-- SQLite
CREATE TABLE users (
    user_id INTEGER PRIMARY KEY ,
    user_username TEXT UNIQUE NOT NULL,
    user_password TEXT NOT NULL CHECK (LENGTH(user_password) >= 10),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);


CREATE TABLE questions (
    question_id INTEGER PRIMARY KEY ,
    question_text TEXT NOT NULL,
    question_difficulty TEXT NOT NULL CHECK (question_difficulty IN ('Easy', 'Medium', 'Hard')),
    question_category TEXT NOT NULL
);

CREATE TABLE answers (
    answer_id INTEGER PRIMARY KEY ,
    question_id INTEGER NOT NULL,
    answer_text TEXT NOT NULL,
    is_correct BOOLEAN NOT NULL CHECK (is_correct IN (0, 1)),
    FOREIGN KEY (question_id) REFERENCES questions(question_id) ON DELETE CASCADE
);
CREATE TABLE scores (
    score_id INTEGER PRIMARY KEY ,
    user_id INTEGER,
    score INTEGER NOT NULL,
    completed_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE
);

CREATE TABLE user_answers (
    response_id INTEGER PRIMARY KEY ,
    score_id INTEGER NOT NULL,              -- Ties to the game session
    question_id INTEGER NOT NULL,           -- Which question was answered
    selected_answer_id INTEGER NOT NULL,    -- What the user picked
    is_correct BOOLEAN NOT NULL,            -- Snapshot of whether it was correct
    FOREIGN KEY (score_id) REFERENCES scores(score_id) ON DELETE CASCADE,
    FOREIGN KEY (question_id) REFERENCES questions(question_id),
    FOREIGN KEY (selected_answer_id) REFERENCES answers(answer_id)
);
 SELECT * FROM users, questions, answers, scores, user_answers;

 SELECT * FROM users;