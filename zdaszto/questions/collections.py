import json


def collections(cursor):
    sql = "SELECT * from collections"
    cursor.execute(sql)
    collections = cursor.fetchall()
    response = json.dumps(collections, default=str)
    return response

def collections_questions(cursor, id_collection):
    sql = "SELECT id_question FROM collections_questions WHERE id_collection=%s"
    cursor.execute(sql, (id_collection,))
    question_raw_list = cursor.fetchall()
    question_list = [q[0] for q in question_raw_list]
    response = json.dumps(question_list, default=str)
    return response