from functools import wraps

from flask import Flask, request, g
import pymysql
from dotenv import load_dotenv
from os import getenv

import zdaszto


load_dotenv()
HOST =    getenv("HOST")
PORT =    int(getenv("PORT"))
DB_HOST = getenv("DB_HOST")
DB_USER = getenv("DB_USER")
DB_PASS = getenv("DB_PASS")
DB_NAME = getenv("DB_NAME")

app = Flask(__name__)

def dbconn(func):
    @wraps(func)
    def wrapper(*args, **kwargs):
        db = get_db()
        with db.cursor() as cursor:
            return func(cursor, db, *args, **kwargs)
    return wrapper

def get_db():
    if "db" not in g:
        g.db = pymysql.connect(
            host=DB_HOST,
            user=DB_USER,
            password=DB_PASS,
            database=DB_NAME,
            #cursorclass=pymysql.cursors.DictCursor,
        )
    return g.db

def auth_required(func):
    @wraps(func)
    def wrapper(cursor, db, *args, **kwargs):
        post_vars = request.form.to_dict()
        auth_resp = zdaszto.misc.user_authenticated(cursor, post_vars)
        if auth_resp is not None:
            return auth_resp
        return func(cursor, db, *args, **kwargs)
    return wrapper


@app.teardown_appcontext
def close_db(exception):
    db = g.pop("db", None)
    if db is not None:
        db.close()


@app.route("/login", methods=["POST"])
@dbconn
def login(cursor, db):
    return zdaszto.misc.login(request.form.to_dict(), cursor, db)



@app.route("/signup" , methods=["POST"])
@dbconn
def signup(cursor, db):
    return zdaszto.misc.signup(request.form.to_dict(), cursor, db)



@ app.route("/signup_form", methods=["GET"])
def signup_form():
    return zdaszto.misc.signup_form()



#@ app.route("/token_test", methods=["POST"])
#@dbconn
#@auth_required
#def token_test(cursor, db):
#    return zdaszto.misc.token_test(request.form.to_dict(), cursor, db)



@app.route("/question/image/<int:question_id>.json", methods=["POST"])
@dbconn
@auth_required
def question_image(cursor, db, question_id):
    return zdaszto.questions.question_image(cursor, question_id)



@app.route("/question/<int:question_id>.json", methods=["POST"])
@dbconn
@auth_required
def question(cursor, db, question_id):
    return zdaszto.questions.question(cursor, question_id)



@app.route("/question/random/<int:collection_id>.json", methods=["POST"])
@dbconn
@auth_required
def question_random(cursor, db, collection_id):
    return zdaszto.questions.random_question(cursor, collection_id)



@app.route("/collections.json", methods=["POST"])
@dbconn
@auth_required
def collections(cursor, db):
    return zdaszto.questions.collections(cursor)



@app.route("/collections/<int:collection_id>.json", methods=["POST"])
@dbconn
@auth_required
def collections_questions(cursor, db, collection_id):
    return zdaszto.questions.collections_questions(cursor, collection_id)



if __name__ == "__main__":
    app.run(host=HOST, port=PORT)