def token_test(request, cursor, connection):
    #post_vars = parse_qs(request.split("\r\n\r\n", 1)[1])
    post_vars = request
    token = post_vars.get('token', [None])
    if token:
        sql = "SELECT id_user FROM tokens WHERE token=%s AND tokens.expires_at > NOW();"
        cursor.execute(sql, (token,))
        data = cursor.fetchall()
        if data:
            user_id = data[0][0]
            response = f":3 user {str(user_id)} {token}"
        else:
            response = data="Invalid token"
    else:
        response = "No token"
    return response