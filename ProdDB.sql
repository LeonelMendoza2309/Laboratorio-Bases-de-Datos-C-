create database ProdDB;
use ProdDB;
drop table if exists `productos` ;
create table if not exists `productos`(
	`ID` int not null auto_increment,
    `Nombre` varchar(30),
    `Precio` float,
    `Cantidad` int,
     `imagen` longblob,
  PRIMARY KEY (`id`));
  
  ALTER TABLE productos
  MODIFY COLUMN fecha_creacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  MODIFY COLUMN fecha_modificacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP;
  
  select * from `productos`;
/*insercion de datos*/

insert into `productos`(`ID`, `Nombre`, `Precio`, `Cantidad`, `imagen`) values
(1, 'Teclado', 20.00, 2, null),
(2, 'Mouse', 15.70, 2, null),
(3, 'Portátil', 700.70, 2, load_file('C:\\Users\\Leonel Mendoza\\Downloads\\NT015HPR63.jpg'));

ALTER TABLE `productos`
ADD COLUMN fecha_creacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
ADD COLUMN fecha_modificacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP;

use ProdDB;
Select * from productos where nombre = '' or '1' = '1' and Precio = '' or '1' = '1';

select * from productos where id=1-sleep(8);

select * from productos where nombre = 'Mouse'; -- 'AND Precio = 20.00';