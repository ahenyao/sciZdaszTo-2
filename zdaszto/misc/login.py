from http.client import responses

from .generate_token import generate_token
from argon2 import PasswordHasher
from argon2.exceptions import VerifyMismatchError
ph = PasswordHasher()

def login(request, cursor, connection):
    #post_vars = parse_qs(request.split("\r\n\r\n", 1)[1])
    post_vars = request
    password = post_vars.get('password',None)
    username = post_vars.get('username',None)
    if password and username:
        sql = """SELECT id_user, username, email, password FROM users
                WHERE username=%s AND deleted_at IS NULL;"""
        cursor.execute(sql, (username,))
        user_result = cursor.fetchall()

        sql = """SELECT tokens.token, tokens.expires_at FROM users
                JOIN tokens ON users.id_user = tokens.id_user
                WHERE username=%s AND tokens.expires_at > NOW();"""

        cursor.execute(sql, (username,))
        token_result = cursor.fetchall()
        print(sql, username, user_result)
        if(user_result):
            print(user_result, token_result)

            def verify_password(hash, password):
                try:
                    return ph.verify(hash, password)
                except VerifyMismatchError:
                    return False

            if(verify_password(user_result[0][3], password)):
                if token_result:
                    response = token_result[0][0]
                else:
                    token = generate_token()
                    response = token

                    sql = """
                            INSERT INTO tokens (id_user, token, created_at, expires_at)
                            VALUES (%s, %s, NOW(), NOW() + INTERVAL 1 HOUR);"""
                    cursor.execute(sql, (user_result[0][0], token))
                    connection.commit()
            else: response = f"Invalid password"
        else:
            response = f"No user found"
    else:
        response ="Username or password missing"

    return response
