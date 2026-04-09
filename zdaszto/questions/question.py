import json


def question(cursor, question_id):
    sql = "SELECT * FROM questions WHERE id_question=%s AND LOWER(media_path)"
    cursor.execute(sql, (question_id,))
    question = cursor.fetchall()
    response = "Invalid question ID"

    if(question != () ):
        question = question[0]
        response = json.dumps(question, default=str)
    return response