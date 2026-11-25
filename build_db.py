import sqlite3
import os

db_path = "restaurant.db"
if os.path.exists(db_path):
    os.remove(db_path)

conn = sqlite3.connect(db_path)
cursor = conn.cursor()

with open("schema.sql", "r", encoding="utf-8") as f:
    cursor.executescript(f.read())

with open("seed.sql", "r", encoding="utf-8") as f:
    cursor.executescript(f.read())

conn.commit()
conn.close()
print("Базу даних restaurant.db успішно створено та наповнено даними!")
