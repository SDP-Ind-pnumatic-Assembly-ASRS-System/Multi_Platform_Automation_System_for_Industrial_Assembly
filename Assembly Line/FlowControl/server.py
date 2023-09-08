from opcua import Server
from random import randint
import datetime
import time
import socket

server =  Server()
# hostname = socket.gethostname()
# IPAddr = socket.gethostbyname(hostname)

# print("Your Computer Name is:" + hostname)
# print("Your Computer IP Address is:" + IPAddr)

IP_ADDRESS =  "192.168.39.96"
PORT = "4841"

url = f"opc.tcp://{IP_ADDRESS}:{PORT}"
server.set_endpoint(url)

AddressSpace_Name = "Assembly_Line"
addspace = server.register_namespace(AddressSpace_Name)

node = server.get_objects_node()
Param1 = node.add_object(addspace, "Table 1")

AlExtend = Param1.add_variable(addspace,"AlExtend", 0)
AlRetract = Param1.add_variable(addspace,"AlRetract", 0)
PlExtend = Param1.add_variable(addspace,"PlExtend", 0)
PlRetract = Param1.add_variable(addspace,"PlRetract", 0)
AlExtendSense = Param1.add_variable(addspace,"AlExtendSense", 0)
AlRetractSense = Param1.add_variable(addspace,"AlRetractSense", 0)
PlExtendSense = Param1.add_variable(addspace,"PlExtendSense", 0)
PlRetractSense = Param1.add_variable(addspace,"PlRetractSense", 0)
C1Fwd = Param1.add_variable(addspace,"C1Fwd", 0)
C1Rev = Param1.add_variable(addspace,"C1Rev", 0)
C1Sen = Param1.add_variable(addspace,"C1Sen", 0)

AlExtend.set_writable()
AlRetract.set_writable()
PlExtend.set_writable()
PlRetract.set_writable()
AlExtendSense.set_writable()
AlRetractSense.set_writable()
PlExtendSense.set_writable()
PlRetractSense.set_writable()
C1Fwd.set_writable()
C1Rev.set_writable()
C1Sen.set_writable()

Param2 = node.add_object(addspace, "Table 2")
PinExtend = Param2.add_variable(addspace,"PinExtend", 0)
PinRetract = Param2.add_variable(addspace,"PinRetract", 0)
PinExtSen = Param2.add_variable(addspace,"PinExtSen", 0)
PinRetSen = Param2.add_variable(addspace,"PinRetSen", 0)
PushExtend = Param2.add_variable(addspace,"PushExtend", 0)
PushRetract = Param2.add_variable(addspace,"PushRetract", 0)
PushExtSen = Param2.add_variable(addspace,"PushExtSen", 0)
PushRetSen = Param2.add_variable(addspace,"PushRetSen", 0)

PinExtend.set_writable()
PinRetract.set_writable()
PinExtSen.set_writable()
PinRetSen.set_writable()
PushExtend.set_writable()
PushRetract.set_writable()
PushExtSen.set_writable()
PushRetSen.set_writable()

ComExtend = Param2.add_variable(addspace,"ComExtend", 0)
ComRetract = Param2.add_variable(addspace,"ComRetract", 0)
TrayExtend = Param2.add_variable(addspace,"TrayExtend", 0)
TrayRetract = Param2.add_variable(addspace,"TrayRetract", 0)
TrayExtSen = Param2.add_variable(addspace,"TrayExtSen", 0)
TrayRetSen = Param2.add_variable(addspace,"TrayRetSen", 0)
DoorExtend = Param2.add_variable(addspace,"DoorExtend", 0)
DoorRetract = Param2.add_variable(addspace,"DoorRetract", 0)
DoorExtSen = Param2.add_variable(addspace,"DoorExtSen", 0)
DoorRetSen = Param2.add_variable(addspace,"DoorRetSen", 0)

ComExtend.set_writable()
ComRetract.set_writable()
TrayExtend.set_writable()
TrayRetract.set_writable()
TrayExtSen.set_writable()
TrayRetSen.set_writable()
DoorExtend.set_writable()
DoorRetract.set_writable()
DoorExtSen.set_writable()
DoorRetSen.set_writable()

C2Fwd = Param2.add_variable(addspace,"C2Fwd", 0)
C2Rev = Param2.add_variable(addspace,"C2Rev", 0)
C2Sen = Param2.add_variable(addspace,"C2Sen", 0)

C2Fwd.set_writable()
C2Rev.set_writable()
C2Sen.set_writable()

Param3 = node.add_object(addspace, "Processes")
comIntake = Param3.add_variable(addspace,"comIntake", 0)
pinning = Param3.add_variable(addspace,"pinning", 0)
assembly = Param3.add_variable(addspace,"assembly", 0)
productType = Param3.add_variable(addspace,"productType", 0)

comIntake.set_writable()
pinning.set_writable()
assembly.set_writable()
productType.set_writable()

server.start()
print("Server started at {}".format(url))

# while True:
#     time.sleep(2)
#     Temperature = randint(10,50)
#     Temp.set_value(Temperature)
#     print(Temperature)