UaClient = opcua('opc.tcp://192.168.43.123:4841');
connect(UaClient); 
R1angles = findNodeByName(UaClient.Namespace,'R1angles','-once');
while true
    a = readValue(UaClient,R1angles);
    fprintf("%d\n", a);
    angle1 = a(1)*3.142857/180;
    angle2 = -a(2)*3.142857/180;
    angle3 = a(3)*3.142857/180;
    angle4 = -(a(4)+90)*3.142857/180;
    angle5 = -(a(5)-90)*3.142857/180;
    angle6 = a(6)*3.142857/180;
    set_param('CompleteAssemblyLine/Assembly_1/R1L1','Value', 'angle1');
    set_param('CompleteAssemblyLine/Assembly_1/R1L2','Value', 'angle2');
    set_param('CompleteAssemblyLine/Assembly_1/R1L3','Value', 'angle3');
    set_param('CompleteAssemblyLine/Assembly_1/R1L4','Value', 'angle4');
    set_param('CompleteAssemblyLine/Assembly_1/R1L5','Value', 'angle5');
    set_param('CompleteAssemblyLine/Assembly_1/R1L6','Value', 'angle6');
    pause(0.01);
end
