#!/usr/bin/env python3
import sqlite3
import re

# Connect to your database
connect = sqlite3.connect("/Users/gilgonzalez/SSE /SSE-Project/Database/gamedatabase.sqlite")
cursor = connect.cursor()

with open("/Users/gilgonzalez/SSE /SSE-Project/Database/questions.txt", "r", encoding="utf-8") as f:
    lines = [line.strip() for line in f if line.strip()]  # strip and remove blanks

question_id = 1
answer_id = 1
i = 0

while i < len(lines):
    question_text = lines[i]
    
    if i + 4 >= len(lines):
        print(f"❌ Skipping incomplete block at line {i}: {question_text}")
        break  # avoid out-of-range errors

    # Insert the question (with placeholder difficulty/category)
    cursor.execute('''
        INSERT INTO questions (question_id, question_text, question_difficulty, question_category)
        VALUES (?, ?, ?, ?)
    ''', (question_id, question_text, "Easy", "SECURITY"))

    # Insert the 4 answers
    for j in range(1, 5):
        raw = lines[i + j]
        is_correct = 1 if "✅" in raw else 0
        clean_answer = re.sub(r'✅', '', raw).strip()

        cursor.execute('''
            INSERT INTO answers (answer_id, question_id, answer_text, is_correct)
            VALUES (?, ?, ?, ?)
        ''', (answer_id, question_id, clean_answer, is_correct))
        answer_id += 1

    # Move to next question block
    question_id += 1
    i += 5

connect.commit()
connect.close()

print("✅ Upload Complete.")