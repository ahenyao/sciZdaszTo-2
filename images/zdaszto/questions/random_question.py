import json
import random

def random_question(cursor, collection_id):
    sql = """SELECT collections_questions.id_question FROM collections_questions 
    JOIN questions ON collections_questions.id_question=questions.id_question 
    WHERE id_collection=%s AND (questions.media_path IS NULL OR questions.media_path NOT LIKE '%%.mp4');"""
    cursor.execute(sql, (collection_id,) )
    question_raw_list = cursor.fetchall()
    if(question_raw_list != ()):
        question_list = [q[0] for q in question_raw_list]

        sql = "SELECT * FROM questions WHERE id_question=%s"
        cursor.execute(sql, (random.choice(question_list),))
        question = cursor.fetchall()[0]
        response = json.dumps(question, default=str)
        return response
    return "Invalid collection ID"