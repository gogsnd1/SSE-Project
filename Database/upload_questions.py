#!/usr/bin/env python3
import sqlite3
import re

connect= sqlite3.connect("/Users/gilgonzalez/SSE /SSE-Project/Database/gamedatabase.sqlite")
cursor= connect.cursor()

with open("/Users/gilgonzalez/SSE /SSE-Project/Database/questions.txt", "r", encoding="utf-8")as f:
    lines=[line.strip() for line in f if line.strip()]

question_id=1
answer_id=1
i=0

while i<len(lines):
    question_text= lines[i]
    #Insert questions with "EASY" and "SECURITY" placeholders
    cursor.execute('''
                   INSERT INTO questions (question_id, question_text, question_difficulty, question_category)
                   VALUES (?,?,?,?)''',
                   (question_id,question_text,"Easy","SERCURITY"))
    #Insert answers
    for j in range(1,5):
        raw= lines[i+j]
        is_correct= 1 if "✅" in raw else 0
        clean_answer=re.sub(r'✅', '', raw).strip()

        cursor.execute('''
                   INSERT INTO answers (answer_id, question_id, answer_text, is_correct)
                   VALUES (?,?,?,?)''',
                   (answer_id,question_id,clean_answer,is_correct))
        answer_id+=1
    question_id+=1
    i+=5 #next question block

connect.commit()
connect.close()

print("Upload Complete.")