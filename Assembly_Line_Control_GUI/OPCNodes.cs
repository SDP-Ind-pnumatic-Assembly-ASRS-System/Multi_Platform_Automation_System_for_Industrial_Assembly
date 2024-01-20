using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyLineControl
{
    static class OPCNodes
    {
        public const int NameSpaceValue = 2;

        //Table 1
        public const int Dispense = 2;
        public const int AlExtendSense = 3;
        public const int AlRetractSense = 4;
        public const int PlExtendSense = 5;
        public const int PlRetractSense = 6;
        public const int C1Sen = 7;

        //Table 2
        public const int PinExtSen = 9;
        public const int PinRetSen = 10;
        public const int PushExtSen = 11;
        public const int PushRetSen = 12;
        public const int TrayExtSen = 13;
        public const int TrayRetSen = 14;
        public const int C2Sen = 15;

        //Processes
        public const int CompIntake = 17;
        public const int Pinning = 18;
        public const int Assembly = 19;
        public const int Conveyor = 20;
        public const int autoStorage = 21;
        public const int autoStoragePath = 22;

        //Grippers
        public const int Gripper1 = 24;
        public const int Gripper2 = 25;

        //Queues
        public const int ProductQueue = 27;
        public const int ProductQueue1 = 28;
        public const int ProductQueue2 = 29;
        public const int ProductQueue3 = 30;

        //Camera
        public const int Capture = 32;
        public const int CapturedValue = 33;

        //Robots
        public const int R1angles = 35;
        public const int R2angles = 36;
        public const int R3angles = 37;

        //Production
        public const int QuantityR1 = 39;
        public const int QuantityR3 = 40;
    }
}
