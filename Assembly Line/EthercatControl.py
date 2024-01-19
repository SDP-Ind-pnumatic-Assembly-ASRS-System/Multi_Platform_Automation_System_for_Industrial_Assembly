import pyads
import time

AMSNETID = "10.1.2.54.1.1"

plc = pyads.Connection(AMSNETID, pyads.PORT_TC3PLC1)
plc.open()
print(f"Connected?: {plc.is_open}") #debugging statement, optional
print(f"Local Address? : {plc.get_local_address()}") #debugging statement, optional
print(plc.ams_net_port)

# plc.write_by_name("GVL.C2Fwd", 0)
# plc.write_by_name("GVL.PinExtend", 0)
# plc.write_by_name("GVL.PushExtend", 0)
# plc.write_by_name("GVL.TrayExtend", 0)
# plc.write_by_name("GVL.TrayRetract", 0)
# plc.write_by_name("GVL.ComExtend", 0)
# plc.write_by_name("GVL.ComRetract", 1)
plc.write_by_name("GVL.DoorExtend", 0)
plc.write_by_name("GVL.DoorRetract", 0)
# plc.write_by_name("GVL.PushExtend", 0)
# plc.write_by_name("GVL.PushExtend", 0)

# plc.write_by_name("GVL.DoorRetract", 0)

def startpartIntake():
    plc.write_by_name("GVL.AlExtend", 1)
    time.sleep(0.5)
    plc.write_by_name("GVL.AlExtend", 0)
    plc.write_by_name("GVL.AlRetract", 1)
    time.sleep(0.5)
    plc.write_by_name("GVL.AlRetract", 0)
    plc.write_by_name("GVL.C1Fwd", 1)
    time.sleep(4)
    plc.write_by_name("GVL.C1Fwd", 0)
    time.sleep(0.5)
    plc.write_by_name("GVL.PlExtend", 1)
    time.sleep(0.5)
    plc.write_by_name("GVL.PlExtend", 0)
    plc.write_by_name("GVL.PlRetract", 1)
    time.sleep(0.5)
    plc.write_by_name("GVL.PlRetract", 0)
    while(plc.read_by_name("GVL.C1Sen") == False):
        plc.write_by_name("GVL.C1Fwd", 1)
    time.sleep(0.25)
    plc.write_by_name("GVL.C1Fwd", 0)
    plc.close()

def startPinning():
    time.sleep(3)
    plc.write_by_name("GVL.PushExtend", 1)
    plc.write_by_name("GVL.PinExtend", 1)
    time.sleep(2)
    plc.write_by_name("GVL.PushExtend", 0)
    time.sleep(1)
    plc.write_by_name("GVL.PinExtend", 0)

def startAssembly():
    plc.write_by_name("GVL.TrayExtend", 1)
    plc.write_by_name("GVL.TrayRetract", 0)
    plc.write_by_name("GVL.ComExtend", 0)
    plc.write_by_name("GVL.ComRetract", 1)
    plc.write_by_name("GVL.DoorExtend", 0)
    plc.write_by_name("GVL.DoorRetract", 1)
    time.sleep(5)
    plc.write_by_name("GVL.TrayExtend", 0)
    plc.write_by_name("GVL.ComRetract", 0)
    plc.write_by_name("GVL.DoorRetract", 0)
    time.sleep(2)
    if(plc.read_by_name("GVL.DoorRetSen") == True and plc.read_by_name("GVL.TrayExtSen") == True):
        time.sleep(5)    
        plc.write_by_name("GVL.TrayRetract", 1)
        time.sleep(1)
        plc.write_by_name("GVL.DoorExtend", 1)
        plc.write_by_name("GVL.ComRetract", 0)
        plc.write_by_name("GVL.ComExtend", 1)
        time.sleep(2)
        plc.write_by_name("GVL.ComExtend", 0)
        plc.write_by_name("GVL.ComRetract", 1)
        plc.write_by_name("GVL.DoorExtend", 0)
        time.sleep(2)
        plc.write_by_name("GVL.DoorRetract", 1)
        time.sleep(1)
        plc.write_by_name("GVL.TrayRetract", 0)
        plc.write_by_name("GVL.TrayExtend", 1)

def startConveyor2():
    time.delay(2)
    if(plc.read_by_name("C2Sen") == True):
        plc.write_by_name("GVL.C2Fwd", 1)

def door():
    plc.write_by_name("GVL.DoorRetract", 1)
    time.sleep(10)
    plc.write_by_name("GVL.TrayRetract", 1)
    time.sleep(2)
    i = 0
    for i in range(5):
        plc.write_by_name("GVL.DoorExtend", 1)
        time.sleep(2)
        plc.write_by_name("GVL.DoorRetract", 1)
        time.sleep(2)

# plc.write_by_name("GVL.C2Fwd", 0)
# plc.write_by_name("GVL.C2Rev", 1)

# plc.write_by_name("GVL.TrayRetract", 1)
#door()
# startpartIntake()
# startPinning()
# startAssembly()


# bOut1 = plc.read_by_name("GVL1.bOut2")
# print(f"bOut1 State {bOut1}")
# plc.close()

# AlExtend AT %Q*: BOOL;
# 	AlRetract AT %Q*: BOOL;
#     PlExtend AT %Q*: BOOL;
# 	PlRetract AT %Q*: BOOL;
# 	C1Fwd AT %Q*: BOOL;
# 	C1Rev AT %Q*: BOOL;
# 	C1Sen AT %I*: BOOL;