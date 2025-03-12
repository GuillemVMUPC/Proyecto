#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <ctype.h>
#include <mysql.h>

int main(int argc, char *argv[])
{
	
	int sock_conn, sock_listen, ret;
	struct sockaddr_in serv_adr;
	char respuesta[512];
	char peticion[512];
	// INICIALITZACIONS
	// Obrim el socket
	close(sock_conn);
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
		printf("Error creant socket");
	// Fem el bind al port
	
	
	memset(&serv_adr, 0, sizeof(serv_adr));// inicialitza a zero serv_addr
	serv_adr.sin_family = AF_INET;
	
	// asocia el socket a cualquiera de las IP de la m?quina. 
	//htonl formatea el numero que recibe al formato necesario
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	// establecemos el puerto de escucha
	serv_adr.sin_port = htons(9010);
	
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
	{
		printf ("Error al bind");
	}
	
	if (listen(sock_listen, 3) < 0)
	{
		printf("Error en el Listen");
	}
	
	int i;
	for (;;)
	{
		printf ("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf ("He recibido conexion\n");
		//sock_conn es el socket que usaremos para este cliente
		
		int terminar =0;
		// Entramos en un bucle para atender todas las peticiones de este cliente
		//hasta que se desconecte
		while (terminar ==0)
		{
			// Ahora recibimos la petici?n
			ret=read(sock_conn,peticion, sizeof(peticion));
			printf ("Recibido\n");
			printf("Bytes recibidos: %d\n", ret);
			
			// Tenemos que a?adirle la marca de fin de string 
			// para que no escriba lo que hay despues en el buffer
			peticion[ret]='\0';
			
			printf ("Peticion: %s\n",peticion);
			
			// vamos a ver que quieren
			char *p = strtok( peticion, "/");
			int codigo =  atoi (p);
			// Ya tenemos el c?digo de la petici?n	
			
			char nick[25];
			char pass[10];
			char respt[100];
			
			if (codigo ==0) //petici?n de desconexi?n
			{	
				terminar=1;
			}
			else if (codigo ==1) //piden la longitd del nombre
			{
				p = strtok( NULL, "/");
				strcpy (nick, p);
				Consulta(respt,nick, codigo);
				if(respt != NULL && respt[0] != '\0')
				{
					p = strtok( NULL, "/");
					strcpy (pass, p);
					Add(nick,pass,respt);
					strcpy(respuesta,"1");
				}
				else
				{
					strcpy(respuesta, "2");
				}
			}
			else if (codigo ==2)
			{
				p = strtok( NULL, "/");
				strcpy (nick, p);
				Consulta(respt, nick, codigo);
				if(respt == NULL || respt[0] == '\0')
				{
					strcpy(respuesta, "2");
				}
				else
				{
					p = strtok( NULL, "/");
					strcpy (pass, p);
					if(strcmp(pass,respt)==0)
					{
						strcpy(respuesta, "1");
					}
					else
					{
						strcpy(respuesta, "3");
					}
				}
			}
			else if (codigo ==3)
			{
				p = strtok( NULL, "/");
				strcpy (nick, p);
				Consulta(respt, nick, codigo);
				if(respt != NULL || respt[0] != '\0')
				{
					strcpy(respuesta,respt);
				}
				else
				{
					strcpy(respuesta, "2");
				}
			}			
			if (codigo !=0)
			{
				
				printf ("Respuesta: %s\n", respuesta);
				// Enviamos respuesta
				write (sock_conn,respuesta, strlen(respuesta));
			}
			// Se acabo el servicio para este cliente
		}
		close(sock_conn);
	}
}	
void Consulta(char* respt, char nick[25], int tipo)
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
	
	int tot;
	
	//Creamos una conexion al servidor MYSQL
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexi\ufff3n: %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//inicializar la conexin
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "Juego",0, NULL, 0);
	if (conn==NULL) 
	{
		printf ("Error al inicializar la conexion: %u %s\n",mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	strcpy(Nickname,nick);
	if(tipo==1)
	{
		strcpy (consulta,"SELECT Nickname FROM DB_players WHERE Nickname = '");
		strcat (consulta, Nickname);
		strcat (consulta,"'");
	}
	else if(tipo==2)
	{
		strcpy (consulta,"SELECT Nickname, Password FROM DB_players WHERE Nickname = '");
		strcat (consulta, Nickname);
		strcat (consulta,"'");
	}
	else if(tipo==3)
	{
		strcpy (consulta,"SELECT * FROM DB_players WHERE Nickname = '");
		strcat (consulta, Nickname);
		strcat (consulta,"'");
	}
	
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
	{
		if(tipo==1)
		{
			mysql_query(conn,"SELECT COUNT(*) AS total FROM DB_players");
			resultado = mysql_store_result (conn);
			row = mysql_fetch_row (resultado);
			tot = atoi(row[0]);
			sprintf(respt,"%d",tot);
		}
		else if(tipo==2 || tipo==3)
		{
			respt[0] = NULL;
		}
	}
	else
	{
		while (row !=NULL) 
		{
			if(tipo==1)
			{
				respt[0] = NULL;
				row = mysql_fetch_row (resultado);
			}
			else if(tipo==2)
			{
				strcpy(respt,row[1]);
				row = mysql_fetch_row (resultado);
			}
			if(tipo==3)
			{
				sprintf(respt,"%s/%s/%s/%f/%d",row[0],row[1],row[2],row[3],row[4]);
				row = mysql_fetch_row (resultado);
			}
		}
	}
	mysql_close (conn);
	return;
	exit(0);
}

int Add(char nick[25], char pass[10], char* respt)
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
	
	if (conn==NULL) 
	{
		printf ("Error al crear la conexion: %u %s\n",	mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//inicializar la conexi\uffc3\uffb3n, entrando nuestras claves de acceso y
	//el nombre de la base de datos a la que queremos acceder
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "Juego",0, NULL, 0);
	if (conn==NULL) 
	{
		printf ("Error al inicializar la conexion: %u %s\n",mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//Introduciremos en la base de datos 4 personas,
	//cuyos datos pedimos al usuario
	// Ahora construimos el string con el comando SQL
	// para insertar la persona en la base. Ese string es:
	// INSERT INTO personas VALUES ('dni', 'nombre', edad);
	strcpy(ID_player,respt);
	strcpy(Nickname,nick);
	strcpy(Password,pass);
	
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
	//concatenamos el dni
	printf("consulta = %s\n", consulta);
	// Ahora ya podemos realizar la insercion
	err = mysql_query(conn, consulta);
	if (err!=0) 
	{
		printf ("Error al introducir datos la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit (1);
		
	}
	// cerrar la conexion con el servidor MYSQL
	mysql_close (conn);
}

