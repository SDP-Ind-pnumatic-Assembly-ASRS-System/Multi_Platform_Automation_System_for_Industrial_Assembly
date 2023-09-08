from opcua import Client

url = "opc.tcp://buntykrgdg:49320"

try:
    client = Client(url)
    client.set_user("Administrator")
    client.set_password("ashutoshrevankar")
    client.activate_session
    # client.set_security_string("Basic256Sha256,SignAndEncrypt,certificate-example.der,private-key-example.pem")
    # #client.session_timeout(60000)  # Set the session timeout to match the server's limit
    # client.activate_session("Administrator", "ashutoshrevankar")
    client.connect()
    

    # Now you can interact with the server using the client object

except Exception as err:
    print(err)
