//Incluir esta libreri\uffc2\uffada para poder hacer las llamadas en shiva2.upc.es
//#include <my_global.h>
#include <mysql.h>
#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include "ConsultaDBPlayers.h"

char* Consulta(char nick[25])
{
	MYSQL *conn;
	int err;
	// Estructura especial para almacenar resultados de consultas
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	
	int Last_Log;
	float Total_Score;
	char ID_player[10];
	char Nickname[25];
	char Password[10];
	
	char consulta [80];
	char respuesta [1];
	
	//Creamos una conexion al servidor MYSQL
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexi\ufff3n: %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//inicializar la conexin
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "Juego",0, NULL, 0);
	if (conn==NULL) {
		printf ("Error al inicializar la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	strcpy(Nickname,nick);
	strcpy (consulta,"SELECT * FROM DB_players WHERE Nickname = '");
	strcat (consulta, Nickname);
	strcat (consulta,"'");
	err=mysql_query (conn, consulta);
	
	if (err!=0) 
	{
		printf ("Error al consultar datos de la base %u %s\n",mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//recogemos el resultado de la consulta. El resultado de la
	//consulta se devuelve en una variable del tipo puntero a
	//MYSQL_RES tal y como hemos declarado anteriormente.
	//Se trata de una tabla virtual en memoria que es la copia
	//de la tabla real en disco.
	resultado = mysql_store_result (conn);
	// El resultado es una estructura matricial en memoria
	// en la que cada fila contiene los datos de una persona.
	// Ahora obtenemos la primera fila que se almacena en una
	// variable de tipo MYSQL_ROW
	row = mysql_fetch_row (resultado);
	
	if (row == NULL)
		return respuesta;
	else
	{
		while (row !=NULL) 
		{
			// la columna 0 contiene el nombre del jugador
			sprintf(respuesta[0],"%s", row[0]);
			sprintf(respuesta[1],"%s", row[1]);
			sprintf(respuesta[2],"%s", row[2]);
			sprintf(respuesta[3],"%f", row[3]);
			sprintf(respuesta[4],"%d", row[4]);
		}
	}
	return respuesta;
	mysql_close (conn);
	exit(0);
}
