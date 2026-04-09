codes = {
    200: "OK",
    201: "Created",
    400: "Bad Request",
    401: "Unauthorized",
    403: "Forbidden",
    404: "Not Found"
}

def http_response(data, code=None, message=None, contentType=None):

    if code is not None and message is None:
        message = codes[code]

    if code is None: code = 200
    if message is None: message = "OK"
    if contentType is None: contentType = "text/html"

    http = (
        f"HTTP/1.1 {code} {message}\r\n"
        f"Content-Type: {contentType}\r\n"
        f"Content-Length: {len(data)}\r\n"
        "Connection: close\r\n"
        "\r\n"
        f"{data}"
    )
    return http