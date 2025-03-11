#include <mysql.h>
#include <string.h>
#include <stdlib.h>
#include <stdio.h>
int main(int argc, char **argv)
{
	//Conector para acceder al servidor de bases de datos
	MYSQL *conn;
	int err;
	//Creamos una conexion al servidor MYSQL
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//inicializar la conexion, indicando nuestras claves de acceso
	// al servidor de bases de datos (user,pass)
	conn = mysql_real_connect (conn, "localhost","root", "mysql", NULL, 0, NULL, 0);
	if (conn==NULL)
	{
		printf ("Error al inicializar la conexion: %u %s\n",mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	// ahora vamos a crear la base de datos, que se llamara personas
	// primero la borramos si es que ya existe (quizas porque hemos
	// hecho pruebas anteriormente
	mysql_query(conn, "drop database if exists Juego;");
	err=mysql_query(conn, "create database Juego;");
	if (err!=0) {
		printf ("Error al crear la base de datos %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//indicamos la base de datos con la que queremos trabajar
	err=mysql_query(conn, "use Juego;");
	if (err!=0)
	{
		printf ("Error al crear la base de datos %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	// creamos la tabla que define la entidad persona:
	// un DNI (clave principal), nombre y edad
	err=mysql_query(conn,"CREATE TABLE DB_players (ID_Player VARCHAR(10) not null primary key, Nickname VARCHAR(25), Password VARCHAR(10), Total_Score float, Last_Log int)");
		if (err!=0)
	{
			printf ("Error al definir la tabla %u %s\n",
					mysql_errno(conn), mysql_error(conn));
			exit (1);
	}
		
		// ahora tenemos la base de datos lista en el servidor de MySQL
		// cerrar la conexion con el servidor MYSQL
		mysql_close (conn);
		exit(0);
}
