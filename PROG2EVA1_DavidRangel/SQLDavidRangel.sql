create table PERFILESDavidRangel (
	Rut varchar(10) not null primary key,
	Nombre varchar(30) not null,
	ApPat varchar(30) not null,
	ApMat varchar(30) not null,
	Clave varchar(13) unique not null
);

create table ACCIONESDavidRangel (
	Num int identity(1,1) not null primary key,
	Clave varchar(13) not null,
	InicioSesion Date not null,
	FinSesion Date not null,
	Accion varchar(100) not null,
	AccionF Date not null,
	foreign key (Clave) references PERFILESDavidRangel(Clave)
);

--Agregar campo NIVEL de tipo numérico de largo 1 a la tabla PERFILESnombreapellido
alter table perfilesDavidRangel add nivel int not null;

/*
Ingresar los siguientes usuarios a la tabla PERFILESnombreapellido:
 Rut:11111111-1/ Nombre: nombre/ Appat:apellido/Apmat:Perez/Clave:/Nivel:1
 Rut:22222222-2/ Nombre:Juan/ Appat:perez/Apmat:cotapos/Clave:/Nivel:2
*/
insert into PERFILESDavidRangel (rut, nombre, ApPat, ApMat, Clave, Nivel) 
values ('11111111-1', 'David','Rangel','Mendoza',',', 1);

insert into PERFILESDavidRangel (rut, nombre, ApPat, ApMat, Clave, Nivel) 
values ('22222222-2', 'Juan','Perez','Cotapos','.', 2);

-- Mostrar de la tabla PERFILESnombreapellido los registros con apellido apellido y nivel 1
select * from PERFILESDavidRangel where ApPat = 'Rangel' and nivel = 1;

--Modificar los registros asignando contenido a campo CLAVE correspondiente a cada uno.
update PERFILESDavidRangel set Clave = CONCAT(SUBSTRING(nombre,1,1), SUBSTRING(ApPat,1,1), SUBSTRING(ApMat,1,1), Rut);

--Eliminar registros con nombre Juan.
delete from PERFILESDavidRangel where nombre = 'Juan';