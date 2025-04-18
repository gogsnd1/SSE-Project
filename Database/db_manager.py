import sqlite3

DB_PATH = "/Users/gilgonzalez/Documents/GitHub/SSE-Project/Database/gamedatabase.sqlite"

def get_connection():
    return sqlite3.connect(DB_PATH)