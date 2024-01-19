from opcua import Server
from random import randint
import socket

server =  Server()
hostname = socket.gethostname()
IPAddr = socket.gethostbyname(hostname)

print("Your Computer Name is:" + hostname)
print("Your Computer IP Address is:" + IPAddr)

IP_ADDRESS =  "10.4.2.26"
PORT = "4841"

url = f"opc.tcp://{IP_ADDRESS}:{PORT}"
server.set_endpoint(url)

AddressSpace_Name = "Assembly_Line"
addspace = server.register_namespace(AddressSpace_Name)
node = server.get_objects_node()

Param1 = node.add_object(addspace, "Table 1")
Dispense = Param1.add_variable(addspace, "Dispense", 0)
AlExtendSense = Param1.add_variable(addspace,"AlExtendSense", 0)
AlRetractSense = Param1.add_variable(addspace,"AlRetractSense", 0)
PlExtendSense = Param1.add_variable(addspace,"PlExtendSense", 0)
PlRetractSense = Param1.add_variable(addspace,"PlRetractSense", 0)
C1Sen = Param1.add_variable(addspace,"C1Sen", 0)
Dispense.set_writable()
AlExtendSense.set_writable()
AlRetractSense.set_writable()
PlExtendSense.set_writable()
PlRetractSense.set_writable()
C1Sen.set_writable()

Param2 = node.add_object(addspace, "Table 2")
PinExtSen = Param2.add_variable(addspace,"PinExtSen", 0)
PinRetSen = Param2.add_variable(addspace,"PinRetSen", 0)
PushExtSen = Param2.add_variable(addspace,"PushExtSen", 0)
PushRetSen = Param2.add_variable(addspace,"PushRetSen", 0)
TrayExtSen = Param2.add_variable(addspace,"TrayExtSen", 0)
TrayRetSen = Param2.add_variable(addspace,"TrayRetSen", 0)
C2Sen = Param2.add_variable(addspace,"C2Sen", 0)
PinExtSen.set_writable()
PinRetSen.set_writable()
PushExtSen.set_writable()
PushRetSen.set_writable()
TrayExtSen.set_writable()
TrayRetSen.set_writable()
C2Sen.set_writable()

Param3 = node.add_object(addspace, "Processes")
compIntake = Param3.add_variable(addspace,"compIntake", 0)
pinning = Param3.add_variable(addspace,"pinning", 0)
assembly = Param3.add_variable(addspace,"assembly", 0)
conveyor = Param3.add_variable(addspace,"conveyor", 0)
autoStorage = Param3.add_variable(addspace,"autoStorage", 0)
autoStoragePath = Param3.add_variable(addspace,"autoStoragePath", [[0.0,0.0,0.0,0.0,0.0,0.0],[0.0,0.0,0.0,0.0,0.0,0.0]])
compIntake.set_writable()
pinning.set_writable()
assembly.set_writable()
conveyor.set_writable()
autoStorage.set_writable()
autoStoragePath.set_writable()

Param4 = node.add_object(addspace, "Grippers")
Gripper1 = Param4.add_variable(addspace,"Gripper1", 0)
Gripper2 = Param4.add_variable(addspace,"Gripper2", 0)
Gripper1.set_writable()
Gripper2.set_writable()

Param5 = node.add_object(addspace, "Queues")
ProductQueue = Param5.add_variable(addspace, "ProductQueue", ["0"])
ProductQueue1 = Param5.add_variable(addspace, "ProductQueue1", ["0"])
ProductQueue2 = Param5.add_variable(addspace, "ProductQueue2", ["0"])
ProductQueue3 = Param5.add_variable(addspace, "ProductQueue3", ["0"])
ProductQueue.set_writable()
ProductQueue1.set_writable()
ProductQueue2.set_writable()
ProductQueue3.set_writable()

Param6 = node.add_object(addspace, "Camera")
Capture = Param6.add_variable(addspace, "Capture", 0)
CapturedValue = Param6.add_variable(addspace, "CapturedValue", "none")
CapturedValue.set_writable()
Capture.set_writable()

Param7 = node.add_object(addspace, "Robots")
R1angles = Param7.add_variable(addspace,"R1angles", [0.0,0.0,0.0,0.0,0.0,0.0])
R2angles = Param7.add_variable(addspace,"R2angles", [0.0,0.0,0.0,0.0,0.0,0.0])
R3angles = Param7.add_variable(addspace,"R3angles", [0.0,0.0,0.0,0.0,0.0,0.0])
R1angles.set_writable()
R2angles.set_writable() 
R3angles.set_writable()

Param8 = node.add_object(addspace, "Production")
QuantityR1 = Param8.add_variable(addspace,"QuantityR1", 0)
QuantityR1.set_writable()
QuantityR3 = Param8.add_variable(addspace,"QuantityR3", 0)
QuantityR3.set_writable()

server.start()
print("Server started at {}".format(url))