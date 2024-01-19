UaClient = opcua('opc.tcp://10.4.2.26:4841');
connect(UaClient); 
R1angles = findNodeByName(UaClient.Namespace,'R1angles','-once');
R2angles = findNodeByName(UaClient.Namespace,'R2angles','-once');

AlExtendSense = findNodeByName(UaClient.Namespace,'AlExtendSense','-once');
AlRetractSense = findNodeByName(UaClient.Namespace,'AlRetractSense','-once');
PlExtendSense = findNodeByName(UaClient.Namespace,'PlExtendSense','-once');
PlRetractSense = findNodeByName(UaClient.Namespace,'PlRetractSense','-once');

PinExtSen = findNodeByName(UaClient.Namespace,'PinExtSen','-once');
PinRetSen = findNodeByName(UaClient.Namespace,'PinRetSen','-once');
PushExtSen = findNodeByName(UaClient.Namespace,'PushExtSen','-once');
PushRetSen = findNodeByName(UaClient.Namespace,'PushRetSen','-once');

TrayExtSen = findNodeByName(UaClient.Namespace,'TrayExtSen','-once');
TrayRetSen = findNodeByName(UaClient.Namespace,'TrayRetSen','-once');

while true
    R1 = readValue(UaClient,R1angles);
    R2 = readValue(UaClient,R2angles);
    R1angle1 = (R1(1)+90)*3.142857/180;
    R1angle2 = -R1(2)*3.142857/180;
    R1angle3 = R1(3)*3.142857/180;
    R1angle4 = -(R1(4)+90)*3.142857/180;
    R1angle5 = -(R1(5)-90)*3.142857/180;
    R1angle6 = -R1(6)*3.142857/180;

    R2angle1 = -(R2(1)+90)*3.142857/180;
    R2angle2 = (R2(2)-90)*3.142857/180;
    R2angle3 = (R2(3)+135)*3.142857/180;
    R2angle4 = -(R2(4)+110)*3.142857/180;
    R2angle5 = -(R2(5)+90)*3.142857/180;
    R2angle6 = -R2(6)*3.142857/180;

    set_param('CompleteAssemblyLine/Assembly_1/R1L1','Value', 'R1angle1');
    set_param('CompleteAssemblyLine/Assembly_1/R1L2','Value', 'R1angle2');
    set_param('CompleteAssemblyLine/Assembly_1/R1L3','Value', 'R1angle3');
    set_param('CompleteAssemblyLine/Assembly_1/R1L4','Value', 'R1angle4');
    set_param('CompleteAssemblyLine/Assembly_1/R1L5','Value', 'R1angle5');
    set_param('CompleteAssemblyLine/Assembly_1/R1L6','Value', 'R1angle6');

    set_param('CompleteAssemblyLine/Paletizer_1/R2L1','Value', 'R2angle1');
    set_param('CompleteAssemblyLine/Paletizer_1/R2L2','Value', 'R2angle2');
    set_param('CompleteAssemblyLine/Paletizer_1/R2L3','Value', 'R2angle3');
    set_param('CompleteAssemblyLine/Paletizer_1/R2L4','Value', 'R2angle4');
    set_param('CompleteAssemblyLine/Paletizer_1/R2L5','Value', 'R2angle5');
    set_param('CompleteAssemblyLine/Paletizer_1/R2L6','Value', 'R2angle6');

    AlEx = readValue(UaClient,AlExtendSense);
    AlRet = readValue(UaClient,AlRetractSense);
    PlEx = readValue(UaClient,PlExtendSense);
    PlRet = readValue(UaClient,PlRetractSense);
    PinEx = readValue(UaClient,PinExtSen);
    PinRet = readValue(UaClient,PinRetSen);
    PushEx = readValue(UaClient,PushExtSen);
    PushRet = readValue(UaClient,PushRetSen);
    TrayEx = readValue(UaClient,TrayExtSen);
    TrayRet = readValue(UaClient,TrayRetSen);

    if AlRet == 1
        set_param('CompleteAssemblyLine/CompIntake_Station_1_RIGID/AlPusher','Value', '0.0');
    elseif AlRet == 0
        if AlEx ==0
            set_param('CompleteAssemblyLine/CompIntake_Station_1_RIGID/AlPusher','Value', '0.03');
        end
    end

    if AlEx == 1
        set_param('CompleteAssemblyLine/CompIntake_Station_1_RIGID/AlPusher','Value', '0.06');
    elseif AlEx == 0
        if AlRet ==0
            set_param('CompleteAssemblyLine/CompIntake_Station_1_RIGID/AlPusher','Value', '0.03');
        end
    end

    if PlRet == 1
        set_param('CompleteAssemblyLine/CompIntake_Station_2_RIGID/PlPusher','Value', '0.0');
    elseif PlRet == 0
        if PlEx ==0
            set_param('CompleteAssemblyLine/CompIntake_Station_2_RIGID/PlPusher','Value', '0.03');
        end
    end

    if PlEx == 1
        set_param('CompleteAssemblyLine/CompIntake_Station_2_RIGID/PlPusher','Value', '0.06');
    elseif PlEx == 0
        if PlRet ==0
            set_param('CompleteAssemblyLine/CompIntake_Station_2_RIGID/PlPusher','Value', '0.03');
        end
    end



    if PinEx == 1
        set_param('CompleteAssemblyLine/Pinning_Station_1_RIGID/PinPinner','Value', '0.04');
    elseif PinEx == 0
        if PinRet ==0
            set_param('CompleteAssemblyLine/Pinning_Station_1_RIGID/PinPinner','Value', '0.02');
        end
    end

    if PinRet == 1
        set_param('CompleteAssemblyLine/Pinning_Station_1_RIGID/PinPinner','Value', '0.0');
    elseif PinRet == 0
        if PinEx ==0
            set_param('CompleteAssemblyLine/Pinning_Station_1_RIGID/PinPinner','Value', '0.02');
        end
    end


    if PushEx == 1
        set_param('CompleteAssemblyLine/Pinning_Station_1_RIGID/Cylinder_55mm_Assembled_1_RIGID/PinPusher','Value', '0.015');
    elseif PushEx == 0
        if PushRet ==0
            set_param('CompleteAssemblyLine/Pinning_Station_1_RIGID/Cylinder_55mm_Assembled_1_RIGID/PinPusher','Value', '0.0075');
        end
    end

    if PushRet == 1
        set_param('CompleteAssemblyLine/Pinning_Station_1_RIGID/Cylinder_55mm_Assembled_1_RIGID/PinPusher','Value', '0.0');
    elseif PushRet == 0
        if PushEx ==0
            set_param('CompleteAssemblyLine/Pinning_Station_1_RIGID/Cylinder_55mm_Assembled_1_RIGID/PinPusher','Value', '0.0075');
        end
    end

    if TrayEx == 1
        set_param('CompleteAssemblyLine/Assembly_Station_1_RIGID/Tray','Value', '0.0');
    elseif TrayEx == 0
        if TrayRet ==0
            set_param('CompleteAssemblyLine/Assembly_Station_1_RIGID/Tray','Value', '0.0575');
        end
    end

    if TrayRet == 1
        set_param('CompleteAssemblyLine/Assembly_Station_1_RIGID/Tray','Value', '0.115');
    elseif TrayRet == 0
        if TrayEx ==0
            set_param('CompleteAssemblyLine/Assembly_Station_1_RIGID/Tray','Value', '0.0575');
        end
    end

    pause(0.01);
end
