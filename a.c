#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <ctype.h>
#include <mysql.h>
#include <pthread.h>

// Estructura de un usuario conectado.
typedef struct{
	char nombre[20];
	int socket;
} Conectado;

// Estructura de la lista de usuarios conectados.
typedef struct{
	Conectado conectados[100];
	int num;
} ListaConectados;

// Estructura de un Juego en la fase de invitacion.
typedef struct{
	char Host[25];
	char Guest[25];
	int Sent;      // En espera de ser enviado -> 0 ; Enviado -> 1 ; Aceptada -> 2 ; Rechazada -> 3
}Game;

// Estructura de la lista de Juegos en la fase de invitacion
typedef struct{
	Game Games[100];
	int num;
}GameList;

pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;
int contador;

int i;
int sockets[100];

// Creación de la lista de usuarios conectados.
ListaConectados milista;

// Creacion de la lista de Juegos en fase de invitacion.
GameList mylist;

// Funcion que añade al vector de conectados los usuarios en el formato requerido para enviarlos al cliente.
void GiveConnected(char conectados[300])
{
	sprintf(conectados,"4/%d",milista.num);
	int i;
	for(i=0;i < milista.num;i++)
	{
		sprintf(conectados,"%s/%s",conectados,milista.conectados[i].nombre);
	}
}

// Funcion que elimina un usuario de la lista de conectados.
int Delete(char nombre[20])
{
	int pos = GivePos(nombre);
	if(pos == -1)
	{
		//Indica que no hay ningun usuario conectado.
		return -1;
	}
	else
	{
		int i;
		for(i=pos; i < milista.num; i++)
		{
			milista.conectados[i] = milista.conectados[i+1];
		}
		milista.num--;
		printf("Usuarios conectados despues de eliminar: %d\n",milista.num);
		// Indica que se ha eliminado el Usuario correctamente.
		return 0;
	}
}

// Funcion que devuelve la posicion de un usuario solicitado por otra funcion.
int GivePos (char nombre[20])
{
	int i = 0;
	int encontrado = 0;
	while((i < milista.num) && !encontrado)
	{
		// Comparacion del nombre dado con los nombres en el vector de la lista de conectados.
		if(strcmp(milista.conectados[i].nombre,nombre) == 0)
		{
			encontrado = 1;
		}
		//Pasa a la siguiente posicion del vector.
		if(!encontrado)
		{
			i++;
		}
		// Devuelve la posicion en el vector donde se encuentra el nombre.
		else if(encontrado)
		{
			printf("-----------------------------------\n Posicion %d \n ------------------------\n",i);
			return i;
		}
		//Si el nombre no se encuentra en la lista.
		else
		{
			return -1;
		}
	}
}

// Funcion para añadir un usuario a la lista de conectados.
int Pon (char nombre[20], int socket)
{
	// Si la lista de Conecatados esta llena devuelve un -1.
	if(milista.num == 100)
	{
		return -1;
	}
	// De lo contrario, añade el Usuario a la lista de Conectados.
	else
	{
		strcpy(milista.conectados[milista.num].nombre, nombre);
		milista.conectados[milista.num].socket = socket;
		printf("-------Funcion Pon----------\n Nombre - %s\n---------------------------\n",milista.conectados[milista.num ].nombre);
		milista.num++;
		return 0;
	}
}
//Funcion que se ejecuta con las peticiones que hace el cliente.
void *AtenderCliente (void *socket)
{
	
	int sock_conn;
	int *s;
	s= (int *) socket;
	sock_conn= *s;
	
	//int socket_conn = * (int *) socket;
	
	// Variables donde se guardan los mensajes que se mandan al cliente
	char peticion[512];
	char respuesta[512];
	char notificacion[512];
	char invitacion[512];
	char respInv[512];
	char confInv[512];
	
	int ret;
	
	int terminar =0;
		// Entramos en un bucle para atender todas las peticiones de este cliente
		//hasta que se desconecte
		while (terminar ==0)
		{
			// Ahora recibimos la petici?n
			ret=read(sock_conn,peticion, sizeof(peticion));
			
			// Tenemos que añadirle la marca de fin de string 
			// para que no escriba lo que hay despues en el buffer
			peticion[ret]='\0';
			
			printf ("-------Peticion------------\nPeticion: %s\n--------------------\n",peticion);
			
			// vamos a ver que quieren
			char *p = strtok( peticion, "/");
			int codigo =  atoi (p);
			// Ya tenemos el codigo de la peticion	
			
			// variables temporales para guardar datos
			char nick[25];
			char pass[10];
			char respt[100];
			char conectados[300];
			
			//Peticion de desconexion
			if (codigo ==0)
			{	
				terminar=1;
			}
			//Peticion de registro de usuario
			//Mensaje recibido --> 1/nickname/password
			else if (codigo ==1)
			{
				p = strtok( NULL, "/");
				strcpy (nick, p);
				//Consulta si el nickname proporcionado esta en uso.
				Consulta(respt,nick, codigo);
				//Si no esta en uso crea uno de nuevo junto a la password proporcionada.
				if(respt != NULL && respt[0] != '\0')
				{
					p = strtok( NULL, "/");
					strcpy (pass, p);
					Add(nick,pass,respt);
					strcpy(respuesta,"1/1");
					// mensaje que se devuelve --> 1/1
				}
				//Si esta en uso notifica con un 2 para informarle al usuario.
				else
				{
					strcpy(respuesta, "1/2");
					// mensaje que se devuelve --> 1/2
				}
			}
			//Peticion para iniciar sesion.
			//Mensaje recibido --> 2/nickname/password
			else if (codigo ==2)
			{
				p = strtok( NULL, "/");
				strcpy (nick, p);
				//Consulta que el nickname se encuentre en la base de datos y devuelve la contraseña
				Consulta(respt, nick, codigo);
				//Si el Nickname no se encuentra en la base
				if(respt == NULL || respt[0] == '\0')
				{
					strcpy(respuesta, "2/2");
					// Mensaje devuelto --> 2/2
				}
				//En caso de que este en la base de datos
				else
				{
					p = strtok( NULL, "/");
					strcpy (pass, p);
					//Devuelve 1 si la password esta asociada a ese nickname
					if(strcmp(pass,respt)==0)
					{
						strcpy(respuesta, "2/1");
						// Mensaje devuelto --> 2/1
						Pon(nick,sock_conn);
					}
					//Devuelve 3 si la password no es la asociada al nickname
					else
					{
						// Mensaje devuelto --> 2/3
						strcpy(respuesta, "2/3");
					}
				}
			}
			//Peticion de consulta.
			//Mensaje recibido --> 3/nickname
			else if (codigo ==3)
			{
				p = strtok( NULL, "/");
				strcpy (nick, p);
				//Recoge todos los datos del usuario
				Consulta(respt, nick, codigo);
				//Envia al cliente los datos asociados a ese nickname
				if(respt != NULL || respt[0] != '\0')
				{
					strcpy(respuesta,respt);
					//Mensaje devuelto -->
				}
				//Envia un 2 si no se ha encontrado al usuario en la base de datos
				else
				{
					strcpy(respuesta, "3/N");
					//Mensaje devuelto --> 3/N
				}
			}	
			//Peticion de log out
			//Mensaje recibido --> 4/nickname
			else if (codigo == 4)
			{
				p = strtok( NULL, "/");
				strcpy (nick, p);
				printf("-------------- Funcion Log Out ---------------\n Nickname - %s:\n--------------------------\n",nick);
				//Elimina el Usuario de la lista de Conectados
				Delete(nick);
				strcpy(respuesta, "");
			}
			//Peticion de invitacion
			//Mensaje recibido --> 5/nickname/nickUsuarioInv
			else if (codigo == 5)
			{
				p = strtok( NULL, "/");
				strcpy(nick,p);
				//Añade el nickname del Usuario que manda la invitacion a un Juego de la lista de Juegos en fase de invitacion
				strcpy(mylist.Games[mylist.num].Host,nick);
				p = strtok( NULL, "/");
				strcpy(nick,p);
				//Añade el nickname del Usuario que recibe la invitacion al mismo Juego de la lista de Juegos en fase de invitacion
				strcpy(mylist.Games[mylist.num].Guest,nick);
				//Indica en el Juego de la lista que la invitacion esta en espera de ser enviado
				mylist.Games[mylist.num].Sent = 0;
				//Aumenta el indicador de juegos en la lista de Juegos
				mylist.num++;
				strcpy(respuesta, "");
				//Empieza un bucle para encontrar la posicion del Usuario al que se le quiere invitar al juego
				for(int c = 0; c < mylist.num;c++)
				{
					//1. Comprueba si el estado de la invitacion es la de espera de ser enviado
					if(mylist.Games[c].Sent == 0)
					{
						//2. Coge la posicion del usuario que se encuentra en la posicion c del vector
						int pos = GivePos(mylist.Games[c].Guest);
						//3. Si se encuentra la posicion del usuario se manda la invitacion
						if(pos != -1)
						{
							sprintf(invitacion, "5/%s/%d",mylist.Games[c].Host,c); 
							//Mensaje enviado --> 5/nicknameHost/posicion
							printf("--------Invitacion------------\n Invitacion - %d\n-----------------------\n",invitacion[c]);
							//Se modifica el estado del Juego a enviado
							mylist.Games[mylist.num].Sent = 1;
							write(milista.conectados[pos].socket,invitacion,strlen(invitacion));
						}
					}
				}
			}
			//Peticion de respuesta de la invitacion (aceptada)
			//Mensaje recibido --> 6/posicion (en la lista de Juegos)
			if (codigo == 6)
			{
				p = strtok( NULL, "/");
				int poslis = atoi(p);
				//Modifica el estado del Juego a aceptada
				mylist.Games[poslis].Sent = 2;
				//Busca de nuevo la posicion del usuario en la lista de Conectados
				int pos = GivePos(mylist.Games[poslis].Host);
				//Si el Usuario sigue conectado y se encuentra la posicion dentro de la lista de Conectados se manda la aceptacion de la invitacion
				if (pos != -1)
				{
					sprintf(respInv, "6/%d",1);	
					//Mensaje devuelto --> 6/1
					printf("--------Respuesta Invitacion------------\n Respuesta Invitacion - %s\n------------------------\n",respInv);
					write(milista.conectados[pos].socket,respInv,strlen(respInv));
				}
				strcpy(respuesta, "");
			}
			//Peticion de respuesta de la invitacion (rechazada)
			//Mensaje recibido --> 7/posicion (en la lista de Juegos)
			if (codigo == 7)
			{
				p = strtok( NULL, "/");
				int poslis = atoi(p);
				//Modifica el estado del Juego a rechazada
				mylist.Games[poslis].Sent = 3;
				//Busca de nuevo la posicion del usuario en la lista de Conectados
				int pos = GivePos(mylist.Games[poslis].Host);
				//Si el Usuario sigue conectado y se encuentra la posicion dentro de la lista de Conectados se manda el rechazo de la invitacion
				if (pos != -1)
				{
					sprintf(respInv, ("6/%d",2));
					//Mensaje devuelto --> 6/2
					printf("--------Respuesta Invitacion------------\n Respuesta Invitacion - %s\n------------------------\n",respInv);
					write(milista.conectados[pos].socket,respInv,strlen(respInv));
				}
				strcpy(respuesta, "");
			}
			//Peticion de Confirmacion de la partida a host y guest
			//Mensaje recibido --> 8/posicion (en la lista de Juegos)
			if (codigo == 8)
			{
				p = strtok( NULL, "/");
				int g = atoi(p);
				//Busca la posicion del Host dentro de la lista de Conectados
				int pos = GivePos(mylist.Games[g].Host);
				//Busca la posicion del Guest dentro de la lista de Conectados
				int pos2 = GivePos(mylist.Games[g].Guest);
				//Si se encuentran ambas se manda el mensaje de confirmacion de Juego
				if ((pos != -1)&&(pos2 != -1))
				{
					sprintf(confInv,"7/%d/%s/%s",1,mylist.Games[g].Host,mylist.Games[g].Guest);
					//mensaje devuelto --> 7/1 (indica que la partida ha sido aceptada) /nickname (host)/nickname (guest)
					printf("--------Confirmacion Invitacion------------\n Respuesta Invitacion - %s\n------------------------\n",confInv);
					write(milista.conectados[pos].socket,confInv,strlen(confInv));
					write(milista.conectados[pos2].socket,confInv,strlen(confInv));
				}
				strcpy(respuesta, "");
			}
			//Respuesta a la peticion.
			if (codigo !=0)
			{
				printf ("--------Respuesta---------\n Respuesta - %s\n-------------------\n", respuesta);
				// Enviamos respuesta
				write (sock_conn,respuesta, strlen(respuesta));
			}
			//Notificacion.
			if ((codigo ==1)||(codigo==2)|| (codigo==3)|| (codigo==4)||(codigo==5))
			{
				pthread_mutex_lock( &mutex ); //No me interrumpas ahora
				contador = contador +1;
				pthread_mutex_unlock( &mutex); //ya puedes interrumpirme
				int j;
				GiveConnected(conectados);
				printf("------Numero de Conectados-------\n Conectados - %d\n",milista.num);
				for(int o = 0; o < milista.num;o++)
				{
					printf(" 1 - %d\n",milista.conectados[o].socket);
				}
				printf("--------------------\n Lista Conectados - %s\n---------------------\n",conectados);
				if(milista.num == 0)
				{
					strcpy(notificacion, "4/N");
				}
				else
				{
					sprintf(notificacion,("%s", conectados));
				}
				for (j=0; j< i; j++)
				{	
					printf ("---------Notificacion------------\n Notificacion - %s\n-------------------\n", notificacion);
					write (sockets[j],notificacion, strlen(notificacion));
				}
			}
			// Se acabo el servicio para este cliente
		}
		close(sock_conn);
	}
	
void Consulta(char* respt, char nick[25], int tipo)
{
	// Funcion para realizar una consulta a la base de datos SQL.	
	MYSQL *conn;
	int err;
	// Estructura especial para almacenar resultados de consultas
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	
	// Vectores temporales de la tabla de DB_Players.	
	char ID_player[10];
	char Password[10];
	
	//Vector de las consultas.
	char consulta [80];
	
	
	//Creamos una conexion al servidor MYSQL
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexi\ufff3n: %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//inicializar la conexin
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "MA_BBDDjuego",0, NULL, 0);
	if (conn==NULL) 
	{
		printf ("Error al inicializar la conexion: %u %s\n",mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	if(tipo==1)
	{
		// Cuando se ejecuta la funcion de consulta llamada desde la funcion de registro del cliente.		
		strcpy (consulta,"SELECT Nickname FROM DB_players WHERE Nickname = '");
		strcat (consulta, nick);
		strcat (consulta,"'");
	}
	else if(tipo==2)
	{
		// Cuando se ejecuta la funcion de consulta llamada desde la funcion de log in del cliente.		
		strcpy (consulta,"SELECT Nickname, Password FROM DB_players WHERE Nickname = '");
		strcat (consulta, nick);
		strcat (consulta,"'");
	}
	else if(tipo==3)
	{
		// Cuando se ejecuta la funcion de consulta llamada desde la funcion de log in del cliente.		
		strcpy (consulta,"SELECT * FROM DB_players WHERE Nickname = '");
		strcat (consulta, nick);
		strcat (consulta,"'");
	}
	
	err=mysql_query (conn, consulta);
	
	if (err!=0) 
	{
		printf ("Error al consultar datos de la base %u %s\n",mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	// Recogemos el resultado de la consulta. El resultado de la
	// consulta se devuelve en una variable de tipo puntero a
	// MYSQL_RES, tal y como hemos declarado anteriormente.
	// Se trata de una tabla virtual en memoria que es una copia
	// de la tabla real en disco.
	resultado = mysql_store_result (conn);
	
	// El resultado es una estructura matricial en memoria,
	// donde cada fila contiene los datos de una persona.
	// Ahora obtenemos la primera fila, que se almacena en una
	// variable de tipo MYSQL_ROW.
	row = mysql_fetch_row (resultado);
	
	if (row == NULL)
	{
		if(tipo==1)
		{
			// Notifica a la funcion de registro que no hay ningun usuario con el nickname dado,
			// y proporciona el numero de jugadores en la base de datos para poder asignar la ID de player.			
			mysql_query(conn,"SELECT COUNT(*) AS total FROM DB_players");
			resultado = mysql_store_result (conn);
			row = mysql_fetch_row (resultado);
			sprintf(respt,"%d",atoi(row[0]));
		}
		else if(tipo==2 || tipo==3)
		{
			// Notifica tanto a la funcion de log in como a la de consultar que el nickname dado no se encuentra en la base de datos.			
			respt[0] = NULL;
		}
	}
	else
	{
		while (row !=NULL) 
		{
			if(tipo==1)
			{
				// Notifica a la funcion de registro que el nickname dado esta siendo usado por otro player.				
				respt[0] = NULL;
			}
			else if(tipo==2)
			{
				// Notifica a la funcion de log in que el nickname dado se encuentra en la base de datos
				// y proporciona la password asociada a ese nickname.				
				strcpy(respt,row[1]);
			}
			if(tipo==3)
			{
				// Notifica a la funcion de consulta que el nickname dado se encuentra en la base de datos
				// y devuelve todos los datos de ese player.				
				sprintf(respt,"3/%s/%s/%s/%f",row[0],row[1],row[2],row[3]);
			}
			row = mysql_fetch_row (resultado);
		}
	}
	mysql_close (conn);
	return;
	exit(0);
}

int Add(char nick[25], char pass[10], char* respt)
{
	// Funcion para anadir un usuario en la base de datos de DM_players.	
	MYSQL *conn;
	int err;
	
	// Vectores temporales de la tabla de DB_Players.
	char ID_player[10];
	float Total_Score;
	char Total_Scores[3];
	
	//Vector de las consultas.
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
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "MA_BBDDjuego",0, NULL, 0);
	if (conn==NULL) 
	{
		printf ("Error al inicializar la conexion: %u %s\n",mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	strcpy(ID_player,respt);
	//copia en las variables temporales los datos proporcionados a la funcion Add()
	
	snprintf(consulta, sizeof(consulta),"INSERT INTO DB_players VALUES ('%s', '%s', '%s', %f);",ID_player, nick, pass, Total_Score);
	//Registra unn nuevo player
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
int main(int argc, char *argv[])
{
	
	int sock_conn, sock_listen, ret;
	int puerto = 9020;
	
	struct sockaddr_in serv_adr;
	
	char respuesta[512];
	char peticion[512];
	
	milista.num=0;
	mylist.num=0;
	// INICIALITZACIONS
	// Obrim el socket
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
	{
		printf("Error creant socket");
	}
	// Fem el bind al port
	
	memset(&serv_adr, 0, sizeof(serv_adr));// inicialitza a zero serv_addr
	serv_adr.sin_family = AF_INET;
	
	// asocia el socket a cualquiera de las IP de la m?quina. 
	//htonl formatea el numero que recibe al formato necesario
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	// establecemos el puerto de escucha
	serv_adr.sin_port = htons(puerto);
	
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
	{
		printf ("Error al bind");
	}
	
	if (listen(sock_listen, 3) < 0)
	{
		printf("Error en el Listen");
	}
	
	contador =0;
	i=0;
	pthread_t thread[100];
	//Creacion de bucle de threads
	for (;;)
	{
		printf ("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf ("He recibido conexion\n");
		//sock_conn es el socket que usaremos para este cliente
		sockets[i] =sock_conn;
		pthread_create (&thread[i], NULL, AtenderCliente,&sockets[i]);
		i=i+1;}
}
