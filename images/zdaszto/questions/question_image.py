


def question_image(cursor, question_id):
    sql = "SELECT media_path FROM questions WHERE id_question=%s"
    cursor.execute(sql, (question_id,))
    media_path = cursor.fetchall()
    if media_path:
        if media_path[0][0]:
            with open(media_path[0][0], "rb") as f:
                file_data = f.read()
            file_ext = media_path[0][0][-3:]

            file_type = "plain/text"
            if (file_ext == "mp4"): file_type = "video/mp4"
            if (file_ext == "jpg"): file_type = "image/jpg"
            if (file_ext == "png"): file_type = "image/png"
            http_headers = (
                "HTTP/1.1 200 OK\r\n"
                f"Content-Type: {file_type}\r\n"
                f"Content-Length: {len(file_data)}\r\n"
                "Connection: close\r\n"
                "\r\n"
            )
            return file_data
    return "No media"