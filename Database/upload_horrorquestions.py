import sqlite3
import re
import json

connect=sqlite3.connect("/Users/gilgonzalez/Documents/GitHub/SSE-Project/Database/gamedatabase.sqlite")
cursor= connect.cursor()

with open("/Users/gilgonzalez/Documents/GitHub/SSE-Project/Database/horror_questions.txt", "r", encoding="utf-8") as f:
    lines= [line.strip() for line in f if line.strip()]

i=0
while i< len(lines):
    line=lines[i]

    if line.endswith("?") or "(Open Input)" in line:
        prompt=line
        trigger=""
        response_type="auto"
        options=[]

        if i+1<len(lines) and lines[i+1].startswith("Trigger:"):
            trigger=lines[i+1].replace("Trigger:","").strip()
            i+=1
        if i+1< len(lines) and re.match(r"[A-D]\.", lines[i+1]):
            response_type="multiple"
            j=i+1
            while j<len(lines) and re.match(r"[A-D]\.", lines[j]):
                opt=re.sub(r"[A-D]\.\s*","",lines[j])
                options.append(opt)
                j+=1
            i= j-1
        elif"(Open Input)" in prompt:
            response_type="open"
        else:
            response_type="auto"
        
        cursor.execute('''
        INSERT INTO horror_events(prompt, trigger, response_type, options)
        VALUES (?,?,?,?)''', (prompt,trigger,response_type,json.dumps(options)if options else None))
    i+=1
connect.commit()
connect.close()
print("Horror events uploaded.")