use assemblyline;
select * from assemblyline.warehouse;
insert into assemblyline.warehouse values("1-ADC-01-2-5", "[[76.81,22.23,-117.77,83.93,21.7,35.94],[145.19,45.96,-133.76,89.47,-37.08,50.09],[146.51,-21.79,-95.0,125.15,-38.58,45.96],[0.000001],[145.19,40.96,-133.76,89.47,-37.08,50.09],[21.09,28.38,-142.82,121.46,4.39,49.92],[0.00,0.00,0.00,0.00,0.00,0.00]", "Empty","", "");
UPDATE assemblyline.warehouse
SET Status = "Empty", TypeID = "", ProductID = ""
WHERE Status = "Occupied";

SET SQL_SAFE_UPDATES=0;