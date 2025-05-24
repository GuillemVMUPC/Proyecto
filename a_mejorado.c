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

// Estructura que representa a un usuario conectado al servidor
typedef struct{
	char nombre[20];       // Nombre del usuario
	int socket;            // Socket asociado al usuario
} Conectado;

// Estructura que representa la lista de usuarios actualmente conectados
typedef struct{
	Conectado conectados[100]; // Array de usuarios conectados
	int num;                   // Numero total de usuarios conectados
} ListaConectados;

// Estructura que representa una solicitud de juego en fase de invitacion
typedef struct{
	char Host[25];   // Nombre del usuario que envia la invitacion
	char Guest[25];  // Nombre del usuario que recibe la invitacion
	int Sent;        // Estado de la invitacion: 0 -> pendiente de envio, 1 -> enviada, 2 -> aceptada, 3 -> rechazada
} Game;

// Estructura que representa la lista de juegos en fase de invitacion
typedef struct{
	Game Games[100]; // Array de solicitudes de juegos
	int num;         // Numero total de juegos en lista
} GameList;

// Mutex para controlar el acceso concurrente a recursos compartidos
pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;

// Variables globales
int contador;               
int i;                      
int sockets[100];           // Array de sockets para clientes
ListaConectados milista;    // Lista de usuarios conectados
GameList mylist;            // Lista de juegos en fase de invitacion

// Funcion que construye un mensaje con la lista de usuarios conectados
void GiveConnected(char conectados[300]) {
	sprintf(conectados, "4/%d", milista.num);
	int i;
	for (i = 0; i < milista.num; i++) {
		strcat(conectados, "/");
		strcat(conectados, milista.conectados[i].nombre);
	}
}

// Funcion que elimina un usuario de la lista de conectados
int Delete(char nombre[20]) {
	int pos = GivePos(nombre);
	if (pos == -1) 
	{
		// No se encontro el usuario en la lista
		return -1;
	} 
	else 
	{
		int i;
		// Desplaza los usuarios una posicion hacia atras para eliminar al usuario
		for (i = pos; i < milista.num - 1; i++) 
		{
			milista.conectados[i] = milista.conectados[i + 1];
		}
		milista.num--;
		printf("Usuarios conectados despues de eliminar: %d\n", milista.num);
		return 0; // Usuario eliminado correctamente
	}
}

// Funcion que devuelve la posicion de un usuario en la lista
int GivePos(char nombre[20]) {
	int i = 0;
	int encontrado = 0;
	while ((i < milista.num) && !encontrado) 
	{
		// Compara el nombre actual con el nombre buscado
		if (strcmp(milista.conectados[i].nombre, nombre) == 0) 
		{
			encontrado = 1;
		}
		if (!encontrado) 
		{
			i++;
		} 
		else 
		{
			// Usuario encontrado, retorna su posicion
			printf("-----------------------------------\n Posicion %d \n ------------------------\n", i);
			return i;
		}
	}
	// Si no se encontro el usuario, retorna -1
	return -1;
}

// Funcion que agrega un nuevo usuario a la lista de conectados
int Pon(char nombre[20], int socket) {
	if (milista.num == 100) 
	{
		// La lista de conectados esta llena
		return -1;
	} 
	else 
	{
		// Se agrega el nuevo usuario con su nombre y socket correspondiente
		strcpy(milista.conectados[milista.num].nombre, nombre);
		milista.conectados[milista.num].socket = socket;
		printf("-------Funcion Pon----------\n Nombre - %s\n---------------------------\n", milista.conectados[milista.num].nombre);
		milista.num++;
		return 0;
	}
}

// Funcion principal que atiende a cada cliente de forma concurrente
// Gestiona las peticiones que realiza el cliente y responde segun el tipo de solicitud
void *AtenderCliente(void *socket) {
	int sock_conn;
	int *s;
	s = (int *) socket;
	sock_conn = *s;
	
	char peticion[512];         // Mensaje recibido del cliente
	char respuesta[512];        // Mensaje de respuesta al cliente
	char invitacion[512];       // Invitacion a jugar
	char respInv[512];          // Respuesta a la invitacion
	char confInv[512];          // Confirmacion de partida
	char chats[512];
	int ret;
	int terminar = 0;           // Flag para controlar la desconexion del cliente
	
	// Bucle principal que atiende continuamente al cliente hasta que este se desconecte
	while (terminar == 0) {
		ret = read(sock_conn, peticion, sizeof(peticion));
		peticion[ret] = '\0';  // Fin de cadena para evitar basura en el buffer
		printf("-------Peticion------------\nPeticion: %s\n--------------------\n", peticion);
		
		char *p = strtok(peticion, "/");
		int codigo = atoi(p); // Codigo de operacion
		
		// Variables auxiliares
		char nick[25];
		char nick2[25];
		char pass[10];
		char respt[100];
		char conectados[300];
		char mensaje[90];
		
		// Procesamiento segun el codigo recibido
		if (codigo == 0) 
		{
			// Codigo 0: solicitud de desconexion
			strcpy(respuesta, "0/OK");
			write(sock_conn, respuesta, strlen(respuesta)); // Enviar confirmación
			terminar = 1;
		} 
		else if (codigo == 1) 
		{
			// Codigo 1: registro de usuario (1/nickname/password)
			p = strtok(NULL, "/");
			strcpy(nick, p);
			Consulta(respt, nick, codigo);
			if (respt != NULL && respt[0] != '\0') 
			{
				p = strtok(NULL, "/");
				strcpy(pass, p);
				Add(nick, pass, respt);
				strcpy(respuesta, "1/1"); // Registro exitoso
			} 
			else 
			{
				strcpy(respuesta, "1/2"); // Nickname en uso
			}
			printf("--------Respuesta---------\n Respuesta - %s\n-------------------\n", respuesta);
			write(sock_conn, respuesta, strlen(respuesta));
		} 
		else if (codigo == 2) 
		{
			// Codigo 2: inicio de sesion (2/nickname/password)
			p = strtok(NULL, "/");
			strcpy(nick, p);
			Consulta(respt, nick, codigo);
			if (respt == NULL || respt[0] == '\0') 
			{
				strcpy(respuesta, "2/2"); // Usuario no encontrado
			} 
			else 
			{
				p = strtok(NULL, "/");
				strcpy(pass, p);
				if (strcmp(pass, respt) == 0) 
				{
					strcpy(respuesta, "2/1"); // Login correcto
					Pon(nick, sock_conn);
				} 
				else 
				{
					strcpy(respuesta, "2/3"); // Contrasena incorrecta
				}
			}
			printf("--------Respuesta---------\n Respuesta - %s\n-------------------\n", respuesta);
			write(sock_conn, respuesta, strlen(respuesta));
		} 
		else if (codigo == 3) 
		{
			// Codigo 3: consulta de datos del usuario (3/tipo de consulta/nickname)
			int cod;
			p = strtok(NULL, "/");
			cod = atoi(p);
			p = strtok(NULL, "/");
			strcpy(nick, p);
			Consulta(respt, nick, cod);
			if (respt != NULL && respt[0] != '\0') 
			{
				// Usuario encontrado, se devuelven los datos
				strcpy(respuesta, respt);
			} 
			else 
			{
				// Usuario no encontrado
				strcpy(respuesta, "3/N");
			}
			printf("--------Respuesta---------\n Respuesta - %s\n-------------------\n", respuesta);
			write(sock_conn, respuesta, strlen(respuesta));
		}
		else if (codigo == 4) 
		{
			// Codigo 4: cierre de sesion o log out (4/nickname)
			p = strtok(NULL, "/");
			strcpy(nick, p);
			printf("-------------- Funcion Log Out ---------------\n Nickname - %s:\n--------------------------\n", nick);
			Delete(nick); // Se elimina al usuario de la lista de conectados
		}
		else if (codigo == 5) 
		{
			// Codigo 5: envio de invitacion para jugar (5/nickname_host/nickname_guest)
			p = strtok(NULL, "/");
			strcpy(nick, p);
			strcpy(mylist.Games[mylist.num].Host, nick); // Guardamos el host
			p = strtok(NULL, "/");
			strcpy(nick, p);
			strcpy(mylist.Games[mylist.num].Guest, nick); // Guardamos el guest
			mylist.Games[mylist.num].Sent = 0; // Estado inicial de la invitacion
			mylist.num++; // Incrementamos el numero de juegos en lista
			
			// Enviamos la invitacion si el usuario invitado esta conectado
			for (int c = 0; c < mylist.num; c++) 
			{
				if (mylist.Games[c].Sent == 0) 
				{
					int pos = GivePos(mylist.Games[c].Guest);
					if (pos != -1) 
					{
						sprintf(invitacion, "5/%s/%d", mylist.Games[c].Host, c); // Formato: 5/nickname_host/posicion
						printf("--------Invitacion------------\n Invitacion - %d\n-----------------------\n", invitacion[c]);
						mylist.Games[c].Sent = 1; // Marcamos como enviada
						write(milista.conectados[pos].socket, invitacion, strlen(invitacion));
					}
				}
			}
		} 
		else if (codigo == 6) 
		{
			// Codigo 6: respuesta positiva a la invitacion (6/posicion_en_lista)
			p = strtok(NULL, "/");
			int poslis = atoi(p);
			mylist.Games[poslis].Sent = 2; // Marcamos como aceptada
			int pos = GivePos(mylist.Games[poslis].Host);
			if (pos != -1) 
			{
				sprintf(respInv, "6/1"); // Invitacion aceptada
				printf("--------Respuesta Invitacion------------\n Respuesta Invitacion - %s\n------------------------\n", respInv);
				write(milista.conectados[pos].socket, respInv, strlen(respInv));
			}
		} 
		else if (codigo == 7) 
		{
			// Codigo 7: respuesta negativa a la invitacion (7/posicion_en_lista)
			p = strtok(NULL, "/");
			int poslis = atoi(p);
			mylist.Games[poslis].Sent = 3; // Marcamos como rechazada
			int pos = GivePos(mylist.Games[poslis].Host);
			if (pos != -1) 
			{
				sprintf(respInv, "6/2"); // Invitacion rechazada
				printf("--------Respuesta Invitacion------------\n Respuesta Invitacion - %s\n------------------------\n", respInv);
				write(milista.conectados[pos].socket, respInv, strlen(respInv));
			}
		} 
		else if (codigo == 8) 
		{
			// Codigo 8: confirmacion de partida (8/posicion_en_lista)
			p = strtok(NULL, "/");
			int g = atoi(p);
			int pos = GivePos(mylist.Games[g].Host);
			int pos2 = GivePos(mylist.Games[g].Guest);
			if (pos != -1 && pos2 != -1) 
			{
				sprintf(confInv, "7/1/%s/%s", mylist.Games[g].Host, mylist.Games[g].Guest);
				// Formato: 7/1/host/guest - confirmacion de inicio de partida
				printf("--------Confirmacion Invitacion------------\n Respuesta Invitacion - %s\n------------------------\n", confInv);
				write(milista.conectados[pos].socket, confInv, strlen(confInv));
				write(milista.conectados[pos2].socket, confInv, strlen(confInv));
			}
		}
		else if (codigo == 9)
		{
			p = strtok(NULL, "/");
			strcpy(nick, p);
			printf("-------------- Funcion Sign Out ---------------\n Nickname - %s:\n--------------------------\n", nick);
			Delete(nick);
			strcpy(respuesta, "8/1");
			printf("--------Respuesta---------\n Respuesta - %s\n-------------------\n", respuesta);
			write(sock_conn, respuesta, strlen(respuesta));			
		}
		else if (codigo == 10)
		{
			p = strtok(NULL, "/");
			strcpy(nick2, p);
			p = strtok(NULL, "/");
			strcpy(nick, p);
			int pos = GivePos(nick);
			if (pos != -1)
			{
				p = strtok(NULL, "/");
				strcpy(mensaje,p);
				sprintf(chats, "9/%s/%s/%s", mensaje, nick2, nick);
				printf("-------------- Funcion Chat ---------------\n Mensaje - %s:\n--------------------------\n", mensaje);
				write(milista.conectados[pos].socket, chats, strlen(chats));
			}
			else
			{
				sprintf(chats, "9/%s/%s/%s/%s", "Error", nick2, nick, mensaje);
				printf("-------------- Funcion Chat Error ---------------\n Mensaje - %s:\n--------------------------\n", mensaje);
				write(sock_conn, chats, strlen(chats));
			}
		}
		// Envio de notificaciones a todos los usuarios conectados
		if (codigo != 0) 
		{
			char notificacion[512];     // Mensaje de notificacion general
			pthread_mutex_lock(&mutex); // Proteccion de la variable compartida
			contador = contador + 1;
			pthread_mutex_unlock(&mutex);
			
			GiveConnected(conectados); // Genera la lista actual de usuarios conectados
			printf("------Numero de Conectados-------\n Conectados - %d\n", milista.num);
			for (int o = 0; o < milista.num; o++) 
			{
				printf(" 1 - %d\n", milista.conectados[o].socket);
			}
			printf("--------------------\n Lista Conectados - %s\n---------------------\n", conectados);
			
			if (milista.num == 0) 
			{
				strcpy(notificacion, "4/N"); // Notificacion vacia
			} 
			else 
			{
				sprintf(notificacion, "%s", conectados); // Notificacion con lista de usuarios
			}
			
			for (int j = 0; j < i; j++) 
			{
				printf("---------Notificacion------------\n Notificacion - %s\n-------------------\n", notificacion);
				write(sockets[j], notificacion, strlen(notificacion));
			}
			conectados[0] = '\0';			
		}
	} // Fin del while
	
	close(sock_conn); // Cierra la conexion con el cliente
}
// Funcion que realiza una consulta a la base de datos segun el tipo solicitado
// tipo 1: verificar si nickname esta disponible
// tipo 2: verificar credenciales de inicio de sesion
// tipo 3: obtener datos completos del usuario
void Consulta(char* respt, char nick[25], int tipo) {
	MYSQL *conn;
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char ID_player[10];
	char Password[10];
	char consulta[80];
	
	// Inicializa la conexion a MySQL
	conn = mysql_init(NULL);
	if (conn == NULL) 
	{
		printf("Error al crear la conexion: %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit(1);
	}
	
	conn = mysql_real_connect(conn, "localhost", "root", "mysql", "MA_BBDDjuego", 0, NULL, 0);
	if (conn == NULL) 
	{
		printf("Error al inicializar la conexion: %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit(1);
	}
	
	// Construye la consulta segun el tipo
	if (tipo == 1) 
	{
		strcpy(consulta, "SELECT Nickname FROM DB_players WHERE Nickname = '");
		strcat(consulta, nick);
		strcat(consulta, "'");
	} 
	else if (tipo == 2) 
	{
		strcpy(consulta, "SELECT Nickname, Password FROM DB_players WHERE Nickname = '");
		strcat(consulta, nick);
		strcat(consulta, "'");
	} 
	else if (tipo == 11) 
	{
		strcpy(consulta, "SELECT ID_PLAYER, NICKNAME, Total_Score FROM DB_players WHERE Nickname = '");
		strcat(consulta, nick);
		strcat(consulta, "'");
	}
	//Consulta tipo 12
	err = mysql_query(conn, consulta);
	if (err != 0) 
	{
		printf("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit(1);
	}
	
	resultado = mysql_store_result(conn);
	row = mysql_fetch_row(resultado);
	
	if (row == NULL) 
	{
		// No se encontro el usuario en la base de datos
		if (tipo == 1) 
		{
			mysql_query(conn, "SELECT COUNT(*) AS total FROM DB_players");
			resultado = mysql_store_result(conn);
			row = mysql_fetch_row(resultado);
			sprintf(respt, "%d", atoi(row[0])); // Devuelve el nuevo ID posible
		} 
		else 
		{
			respt[0] = '\0'; // Indica que no se encontro el usuario
		}
	} 
	else 
	{
		// Usuario encontrado, se extraen los datos
		while (row != NULL) 
		{
			if (tipo == 1) 
			{
				respt[0] = '\0'; // Nickname ya en uso
			} 
			else if (tipo == 2) 
			{
				strcpy(respt, row[1]); // Devuelve la contrasena
			} 
			else if (tipo == 11) 
			{
				sprintf(respt, "3/%s/%s/%s", row[0], row[1], row[2]); // Devuelve datos completos
			}
			row = mysql_fetch_row(resultado);
		}
	}
	mysql_close(conn);
}

// Funcion que agrega un nuevo usuario a la base de datos
int Add(char nick[25], char pass[10], char* respt) {
	MYSQL *conn;
	int err;
	char ID_player[10];
	float Total_Score = 0.0;
	char consulta[200];
	
	conn = mysql_init(NULL);
	if (conn == NULL) 
	{
		printf("Error al crear la conexion: %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit(1);
	}
	
	conn = mysql_real_connect(conn, "localhost", "root", "mysql", "MA_BBDDjuego", 0, NULL, 0);
	if (conn == NULL) 
	{
		printf("Error al inicializar la conexion: %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit(1);
	}
	
	strcpy(ID_player, respt);
	snprintf(consulta, sizeof(consulta), "INSERT INTO DB_players VALUES ('%s', '%s', '%s', %f);", ID_player, nick, pass, Total_Score);
	printf("consulta = %s\n", consulta);
	
	err = mysql_query(conn, consulta);
	if (err != 0) 
	{
		printf("Error al introducir datos en la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit(1);
	}
	
	mysql_close(conn);
	return 0;
}
// Funcion principal del servidor
int main(int argc, char *argv[]) {
	int sock_conn, sock_listen, ret;
	int puerto = 9080; // Puerto del servidor
	struct sockaddr_in serv_adr;
	char respuesta[512];
	char peticion[512];
	
	milista.num = 0; // Inicializa la lista de conectados
	mylist.num = 0;  // Inicializa la lista de juegos
	
	// Inicializaciones de red
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0) 
	{
		printf("Error creando socket\n");
	}
	
	// Configuracion de la direccion del servidor
	memset(&serv_adr, 0, sizeof(serv_adr));
	serv_adr.sin_family = AF_INET;
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY); // Acepta conexiones desde cualquier interfaz
	serv_adr.sin_port = htons(puerto); // Puerto definido anteriormente
	
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0) 
	{
		printf("Error en bind\n");
	}
	
	if (listen(sock_listen, 3) < 0) 
	{
		printf("Error en listen\n");
	}
	
	contador = 0;
	i = 0;
	pthread_t thread[100]; // Array de hilos para atender hasta 100 clientes
	
	// Bucle principal que acepta conexiones entrantes
	for (;;) 
	{
		printf("Escuchando\n");
		sock_conn = accept(sock_listen, NULL, NULL); // Acepta conexion de cliente
		printf("Conexion recibida\n");
		sockets[i] = sock_conn; // Guarda el socket del cliente
		pthread_create(&thread[i], NULL, AtenderCliente, &sockets[i]); // Lanza hilo para atenderlo
		i++;
	}
}
