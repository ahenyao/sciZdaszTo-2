
import json

def top100(cursor, connection):

    sql = """SELECT user_ranking.id_user, users.username, score, position, total_good, total_wrong, accuracy FROM  user_ranking JOIN users ON user_ranking.id_user=users.id_user ORDER BY position LIMIT 30;"""
    cursor.execute(sql)
    connection.commit()
    columns = [col[0] for col in cursor.description]
    rows = cursor.fetchall()

    result = []
    for row in rows:
        result.append(dict(zip(columns, row)))

    return json.dumps(result)