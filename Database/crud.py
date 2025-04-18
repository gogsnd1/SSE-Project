from db_manager import get_connection
import random

# Insert new user (with already hashed password)
def insertUser(username, password_hash):
    conn = get_connection()
    cursor = conn.cursor()
    try:
        cursor.execute('''
            INSERT INTO users (user_username, user_password)
            VALUES (?, ?)
        ''', (username, password_hash))
        conn.commit()
        return True
    except Exception as e:
        print(f"[insertUser] Error: {e}")
        return False
    finally:
        conn.close()


# Check login credentials
def checkLogin(username, password_hash):
    conn = get_connection()
    cursor = conn.cursor()
    cursor.execute('''
        SELECT * FROM users
        WHERE user_username = ? AND user_password = ?
    ''', (username, password_hash))
    result = cursor.fetchone()
    conn.close()
    return result is not None


# Fetch questions from the questions table
def fetchQuestions(category=None, difficulty=None):
    conn = get_connection()
    cursor = conn.cursor()

    query = "SELECT * FROM questions WHERE 1=1"
    params = []

    if category:
        query += " AND question_category = ?"
        params.append(category)
    if difficulty:
        query += " AND question_difficulty = ?"
        params.append(difficulty)

    cursor.execute(query, tuple(params))
    results = cursor.fetchall()
    conn.close()
    return results


# Save a user's score after a game session
def saveScore(user_id, score):
    conn = get_connection()
    cursor = conn.cursor()
    cursor.execute('''
        INSERT INTO scores (user_id, score, completed_at)
        VALUES (?, ?, datetime('now'))
    ''', (user_id, score))
    conn.commit()
    conn.close()


# Get the user's ID by username
def getUserId(username):
    conn = get_connection()
    cursor = conn.cursor()
    cursor.execute('''
        SELECT user_id FROM users WHERE user_username = ?
    ''', (username,))
    result = cursor.fetchone()
    conn.close()
    return result[0] if result else None


# NEW: Fetch and combine both questions and horror events
def fetchCombinedQuestionsAndEvents(category=None, difficulty=None):
    conn = get_connection()
    cursor = conn.cursor()

    # Fetch standard questions
    q_query = "SELECT * FROM questions WHERE 1=1"
    q_params = []
    if category:
        q_query += " AND question_category = ?"
        q_params.append(category)
    if difficulty:
        q_query += " AND question_difficulty = ?"
        q_params.append(difficulty)

    cursor.execute(q_query, tuple(q_params))
    questions = cursor.fetchall()

    # Fetch horror events
    cursor.execute("SELECT * FROM horror_events")
    events = cursor.fetchall()

    conn.close()

    # Tag each with a type label
    formatted_questions = [{"type": "question", "data": q} for q in questions]
    formatted_events = [{"type": "event", "data": e} for e in events]

    # Combine and shuffle
    merged = formatted_questions + formatted_events
    random.shuffle(merged)

    return merged