from argon2 import PasswordHasher
ph = PasswordHasher()

def signup(request, cursor, connection):
    #post_vars = parse_qs(request.split("\r\n\r\n", 1)[1])
    post_vars = request
    email = post_vars.get('email', None)
    password = post_vars.get('password', None)
    username = post_vars.get('username', None)

    if email and password and username:
        sql = "SELECT * FROM `users` WHERE `email`=%s OR `username`=%s"
        cursor.execute(sql, (email, username))
        result = cursor.fetchall()
        if (result == ()):
            sql = "INSERT INTO `users` (email, password, username) VALUES (%s, %s, %s)"
            cursor.execute(sql, (email, ph.hash(password), username))
            connection.commit()
            response = f"User created"
        else:
            response = f"User exists"
    else:
        response = f"Parameters missing"

    return response