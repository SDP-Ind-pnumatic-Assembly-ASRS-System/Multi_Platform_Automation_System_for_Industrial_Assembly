% Simscape(TM) Multibody(TM) version: 7.7

% This is a model data file derived from a Simscape Multibody Import XML file using the smimport function.
% The data in this file sets the block parameter values in an imported Simscape Multibody model.
% For more information on this file, see the smimport function help page in the Simscape Multibody documentation.
% You can modify numerical values, but avoid any other changes to this file.
% Do not add code to this file. Do not edit the physical units shown in comments.

%%%VariableName:smiData


%============= RigidTransform =============%

%Initialize the RigidTransform structure array by filling in null values.
smiData.RigidTransform(5).translation = [0.0 0.0 0.0];
smiData.RigidTransform(5).angle = 0.0;
smiData.RigidTransform(5).axis = [0.0 0.0 0.0];
smiData.RigidTransform(5).ID = "";

%Translation Method - Cartesian
%Rotation Method - Arbitrary Axis
smiData.RigidTransform(1).translation = [17.5 31.607864376269042 -17.5];  % mm
smiData.RigidTransform(1).angle = 2.0943951023931953;  % rad
smiData.RigidTransform(1).axis = [-0.57735026918962584 -0.57735026918962584 -0.57735026918962584];
smiData.RigidTransform(1).ID = "B[6cm Cylinder-1:-:Cylinder Rod 50mm-1]";

%Translation Method - Cartesian
%Rotation Method - Arbitrary Axis
smiData.RigidTransform(2).translation = [17.5 -26.392135623730958 -17.5];  % mm
smiData.RigidTransform(2).angle = 2.0943951023931953;  % rad
smiData.RigidTransform(2).axis = [-0.57735026918962584 -0.57735026918962584 -0.57735026918962584];
smiData.RigidTransform(2).ID = "F[6cm Cylinder-1:-:Cylinder Rod 50mm-1]";

%Translation Method - Cartesian
%Rotation Method - Arbitrary Axis
smiData.RigidTransform(3).translation = [0 8.0000000000000036 0];  % mm
smiData.RigidTransform(3).angle = 2.0943951023931953;  % rad
smiData.RigidTransform(3).axis = [-0.57735026918962584 -0.57735026918962584 -0.57735026918962584];
smiData.RigidTransform(3).ID = "B[Cylinder Rod 50mm-1:-:Cylinder Rod Cap-1]";

%Translation Method - Cartesian
%Rotation Method - Arbitrary Axis
smiData.RigidTransform(4).translation = [35 -3.5527136788005009e-15 3.0000000000000639];  % mm
smiData.RigidTransform(4).angle = 3.1415926535897931;  % rad
smiData.RigidTransform(4).axis = [0.70710678118654768 -0.70710678118654746 1.1102230246251565e-16];
smiData.RigidTransform(4).ID = "F[Cylinder Rod 50mm-1:-:Cylinder Rod Cap-1]";

%Translation Method - Cartesian
%Rotation Method - Arbitrary Axis
smiData.RigidTransform(5).translation = [-49.133741636255756 -30.882497769770453 35.471635514413727];  % mm
smiData.RigidTransform(5).angle = 0;  % rad
smiData.RigidTransform(5).axis = [0 0 0];
smiData.RigidTransform(5).ID = "RootGround[6cm Cylinder-1]";


%============= Solid =============%
%Center of Mass (CoM) %Moments of Inertia (MoI) %Product of Inertia (PoI)

%Initialize the Solid structure array by filling in null values.
smiData.Solid(3).mass = 0.0;
smiData.Solid(3).CoM = [0.0 0.0 0.0];
smiData.Solid(3).MoI = [0.0 0.0 0.0];
smiData.Solid(3).PoI = [0.0 0.0 0.0];
smiData.Solid(3).color = [0.0 0.0 0.0];
smiData.Solid(3).opacity = 0.0;
smiData.Solid(3).ID = "";

%Inertia Type - Custom
%Visual Properties - Simple
smiData.Solid(1).mass = 0.013682221962492852;  % kg
smiData.Solid(1).CoM = [17.499999999999996 -7.7488325802631612 -17.499999999999996];  % mm
smiData.Solid(1).MoI = [4.6580195026766944 2.0121252526082363 4.6580195026766953];  % kg*mm^2
smiData.Solid(1).PoI = [0 0.25183016528652863 0];  % kg*mm^2
smiData.Solid(1).color = [0.792156862745098 0.81960784313725488 0.93333333333333335];
smiData.Solid(1).opacity = 1;
smiData.Solid(1).ID = "Cylinder Rod 50mm*:*Default";

%Inertia Type - Custom
%Visual Properties - Simple
smiData.Solid(2).mass = 0.0033429175404620266;  % kg
smiData.Solid(2).CoM = [17.499741904799006 15.769109129751859 1.7095801102513546];  % mm
smiData.Solid(2).MoI = [0.2700573707741914 0.32784088016717844 0.58529553898436826];  % kg*mm^2
smiData.Solid(2).PoI = [-0.0022780191065211361 -3.2872709774120438e-06 0.0047997816400400448];  % kg*mm^2
smiData.Solid(2).color = [0 0 0];
smiData.Solid(2).opacity = 1;
smiData.Solid(2).ID = "Cylinder Rod Cap*:*Default";

%Inertia Type - Custom
%Visual Properties - Simple
smiData.Solid(3).mass = 0.045543036671226625;  % kg
smiData.Solid(3).CoM = [17.496334243107825 28.612784129538884 -17.496278012151578];  % mm
smiData.Solid(3).MoI = [17.369640706901841 9.7739239588067583 17.062224614080247];  % kg*mm^2
smiData.Solid(3).PoI = [0.0046049099710860283 -0.10076212504623133 -0.0046153679592775745];  % kg*mm^2
smiData.Solid(3).color = [0.792156862745098 0.81960784313725488 0.93333333333333335];
smiData.Solid(3).opacity = 1;
smiData.Solid(3).ID = "6cm Cylinder*:*Default";

