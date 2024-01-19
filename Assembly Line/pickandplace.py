import time
import os
import sys
import termios
import tty
import threading
import json
import serial
import serial.tools.list_ports

from pymycobot.mycobot import MyCobot
from pymycobot import PI_PORT, PI_BAUD

mc = MyCobot(PI_PORT, PI_BAUD, debug=False)
path = [[1653, 1728, 1481, 2249, 1231, 3438]
,[710, 1777, 1204, 2278, 1091, 3438]
,[710, 1450, 1120, 2193, 1023, 3438]
,[710, 2064, 1120, 2193, 1023, 3438]
,[2286, 2138, 929, 2317, 1312, 2093]
,[2957, 2104, 973, 2212, 1264, 2104]
,[2953, 2104, 794, 3448, 1022, 2077]
,[2956, 1614, 873, 3304, 1019, 1531]
,[2919, 1307, 752, 3261, 1015, 1250]
,[2927, 1885, 664, 3310, 999, 1531]]


def pickAndPlace():
    for point in path:
        mc.set_encoders(point,80)
        time.sleep(2)
        
pickAndPlace()
