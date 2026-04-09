def signup_form():
    response = (
        '<style>*{background-color:#000;color:#fff}</style>'
        '<form method="POST" action="/signup">'
        'email: <input name="email">'
        'password: <input name="password">'
        'username: <input name="username">'
        '<input type="submit">'
        '</form>')
    return response