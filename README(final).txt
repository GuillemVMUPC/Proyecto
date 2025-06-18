
Video: https://drive.google.com/drive/folders/1qrCgDBvZXbX1JCrxPUdEjMrLXzjc94eW?usp=sharing hecho por Guillermo Aliaga Fernández

Protocolo

Peticiones del cliente → servidor:

Código | Formato mensaje                       | Significado                                                  | Respuesta del servidor
-------|----------------------------------------|--------------------------------------------------------------|-------------------------
0      | 0/                                     | Solicitud de desconexión                                     | 0/OK
1      | 1/nick/pass                            | Registro de usuario                                          | 1/1 si OK, 1/2 si nick en uso
2      | 2/nick/pass                            | Login                                                        | 2/1 si OK, 2/2 no existe, 2/3 pass incorrecta, 2/4 ya conectado
3      | 3/tipo/nick                            | Consulta datos usuario (tipo=11 ó 12)                        | 3/1/... o 3/2/... o 3/N
4      | 4/nick                                 | Logout (elimina de conectados)                               | Sin respuesta
5      | 5/host/guest                           | Enviar invitación de juego                                   | 5/1/host/pos al guest, 5/2/pos al host
6      | 6/pos                                  | Aceptar invitación                                           | 6/1/ al host
7      | 7/pos                                  | Rechazar invitación                                          | 6/2/ al host
8      | 8/pos                                  | Confirmar partida                                            | 7/1/host/guest a ambos
9      | 9/nick                                 | Cierre de sesión completo                                    | 8/1/
10     | 10/dest/rem/mensaje                    | Enviar chat de rem a dest                                    | 9/mensaje/dest/rem o error
11     | 11/pos_juego                           | Crear baraja para partida                                    | 10/id_baraja/jugador_que_empieza a ambos
12     | 12/id_baraja/nick/turno_num            | Pedir carta                                                  | 11/numero/palo/turno/num y 12/otro/num a rival
13     | 13/id_baraja/turno_num/nick            | Fin de turno. Crupier juega si ambos terminaron             | 12/otro/num y 14/num/palo/num si aplica
14     | 14/id_baraja/turno_num                 | Reinicio mano crupier y reparto                              | 13/num1/palo1/num2/palo2/n a ambos


Notificaciones del servidor → clientes:

Código | Formato                              | Significado
-------|----------------------------------------|--------------------------
4      | 4/num/nick1/nick2/.../                | Lista de conectados actualizada
5      | 5/1/host/pos/                         | Invitación enviada al guest
5      | 5/2/pos/                              | Confirmación al host
6      | 6/1/                                  | Invitación aceptada
6      | 6/2/                                  | Invitación rechazada
7      | 7/1/host/guest/                       | Confirmación de inicio de partida
8      | 8/1/                                  | Cierre completo de sesión
9      | 9/texto/dest/rem/                     | Chat entre jugadores
10     | 10/id_baraja/jugador/                | Confirmación baraja y quién empieza
11     | 11/num/palo/turno/n/                 | Carta enviada a jugador
12     | 12/turno/n/                          | Cambio de turno
13     | 13/num1/palo1/num2/palo2/n/          | Reparto de cartas crupier
14     | 14/num/palo/n/                       | Carta del crupier