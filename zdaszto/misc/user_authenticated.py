def user_authenticated(cursor, request):

    #  -1 - no  token
    #   0 - bad token
    #   1 - ok  token

    status = -1
    #post_vars = parse_qs(request.split("\r\n\r\n", 1)[1])
    post_vars = request
    token = post_vars.get('token', None)
    if token:
        sql = "SELECT id_user FROM tokens WHERE token=%s AND tokens.expires_at > NOW();"
        cursor.execute(sql, (token,))
        data = cursor.fetchall()
        if data:
            status = 1
        else:
            status = 0
    else:
        status = -1

    match status:
        case -1:  # no  token
            return "No token"
        case 0:  # bad token
            return "Invalid token"
    return None