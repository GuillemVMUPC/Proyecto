// programa en C para introducir los datos en la base de datos
//Incluir esta libreria para poder hacer las llamadas en shiva2.upc.es
//#include <my_global.h>
#include <mysql.h>
#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include "Servidor.h"

int Add()
{
	MYSQL *conn;
	int err;
	char ID_player[10];
	char Nickname [25];
	char Password[10];
	float Total_Score;
	char Total_Scores[3];
	int Last_Log;
	char Last_Logs[3];
	int i;
	char consulta [80];
	//Creamos una conexion al servidor MYSQL
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//inicializar la conexi\uffc3\uffb3n, entrando nuestras claves de acceso y
	//el nombre de la base de datos a la que queremos acceder
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "Juego",0, NULL, 0);
	if (conn==NULL) {
		printf ("Error al inicializar la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//Introduciremos en la base de datos 4 personas,
	//cuyos datos pedimos al usuario
	for (i=0; i<1; i++) {
		printf ("Escribe el ID_player, Nickname, Password, Total_Score y Last_Log del Jugador, separados por un blanco\n");
		err = scanf ("%s %s %s %f %d", ID_player, Nickname, Password, &Total_Score, &Last_Log);
		if (err!=5) {
			printf ("Error al introducir los datos \n");
			exit (1);
		}
		// Ahora construimos el string con el comando SQL
		// para insertar la persona en la base. Ese string es:
		// INSERT INTO personas VALUES ('dni', 'nombre', edad);
		strcpy (consulta, "INSERT INTO DB_players VALUES ('");
		//concatenamos el dni
		strcat (consulta, ID_player);
		strcat (consulta, "','");
		//concatenamos el nombre
		strcat (consulta, Nickname);
		strcat (consulta, "','");
		strcat (consulta, Password);
		strcat (consulta, "',");
		//convertimos la edad en un string y lo concatenamos
		sprintf(Total_Scores, "%f", Total_Score);
		strcat (consulta, Total_Scores);
		strcat (consulta, ",");
		sprintf(Last_Logs, "%d", Last_Log);
		strcat (consulta, Last_Logs);
		strcat (consulta, ");");
		printf("consulta = %s\n", consulta);
		// Ahora ya podemos realizar la insercion
		err = mysql_query(conn, consulta);
		if (err!=0) {
			printf ("Error al introducir datos la base %u %s\n",
					mysql_errno(conn), mysql_error(conn));
			exit (1);
		}
	}
	// cerrar la conexion con el servidor MYSQL
	mysql_close (conn);
	exit(0);
}
