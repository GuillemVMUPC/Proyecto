using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Eventing.Reader;
using System.Threading;
using System.Security.Cryptography.Pkcs;

namespace ClienteProyecto
{
    public partial class Form1 : Form
    {
        Socket server;
        Thread atender;
        public bool sesion;
        public string nickCons;
        public string nickUsu;
        public int pos;

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false; //Necesario para que los elementos de los formularios puedan ser
            //accedidos desde threads diferentes a los que los crearon
            sesion = false;
            List<string> jugadoresConectados = new List<string>();
        }
        private void AtenderServidor()
        {
            while (true)
            {
                //Recibimos mensaje del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                // Dividimos el texto en fragmentos dentro de un vector para poder extraer el número del código.
                string[] trozos = Encoding.ASCII.GetString(msg2).Split('/');
                int codigo = Convert.ToInt32(trozos[0]);
                switch (codigo)
                {
                    case 1:  // Respuesta de la función de register.
                        string mensaje1 = trozos[1].Split('\0')[0];
                        if (mensaje1 == "1")
                        {
                            // Cuando el servidor devuelve un 1 para indicar que el registro se completa sin ningún error.
                            MessageBox.Show("Registro completado.");
                        }
                        else if (mensaje1 == "2")
                        {
                            // Cuando el servidor devuelve un 2 para indicar que el nickname proporcionado ya está en uso.
                            MessageBox.Show("Nickname ya usado.");
                        }
                        else
                        {
                            // Cualquier otro error ocurrido en el servidor.
                            MessageBox.Show("Error al registrarse.");
                        }
                        break;
                    case 2:      //Respuesta función Log In.
                        this.Invoke((MethodInvoker)delegate
                        {
                            string mensaje2 = trozos[1].Split('\0')[0];
                            if (mensaje2 == "1")
                            {
                                // Cuando el servidor devuelve un 1 para indicar que la sesión se ha iniciado correctamente,
                                // se realizan algunos cambios visuales en el cliente.
                                LogOut.Visible = true;
                                Log.Visible = false;
                                Sign.Visible = false;
                                sesion = true;
                                Nickname_player.Text = nickUsu;
                                MessageBox.Show("Sesión iniciada.");
                            }
                            else if (mensaje2 == "2")
                            {
                                // Cuando el servidor devuelve un 2 para indicar que el nickname no se encuentra en la base de datos.
                                MessageBox.Show("Nickname incorrecto.");
                            }
                            else
                            {
                                // Cuando el servidor devuelve un 3 para indicar que la contraseña proporcionada para ese usuario no es correcta.
                                MessageBox.Show("Password incorrecto.");
                            }
                        });
                        break;
                    case 3:  //Respuesta función Consulta
                        string mensaje3 = trozos[1].Split('\0')[0];
                        if (mensaje3 == "N")
                        {
                            // Cuando el servidor devuelve una "N" para indicar que el nickname no se ha encontrado en la base de datos.
                            MessageBox.Show("Nombre no econtrado en la base de datos.");
                        }
                        else
                        {
                            // Cuando el servidor devuelve algo distinto de una "N", proporcionando los datos del usuario solicitado.
                            string[] partes = trozos.Skip(1).ToArray();

                            // Crear un mensaje combinado para mostrar en el MessageBox
                            string mensajeMostrar = string.Join(Environment.NewLine, partes);

                            // Mostrar el mensaje en un MessageBox
                            MessageBox.Show(mensajeMostrar, "Datos de" + nickCons);
                            nickCons = null;
                        }
                        break;
                    case 4:  //Notificación
                        string mensaje4 = trozos[1].Split('\0')[0];
                        if (mensaje4 == "N")
                        {
                            // Cuando el servidor devuelve una "N" para indicar que no hay ningún usuario conectado.
                            // Vacía el texto de todos los labels.
                            int j = 4;
                            Label[] playerslab = new Label[] { player1, player2, player3, player4 };
                            for (int i = 0; i < j; i++)
                            {
                                playerslab[i].Text = null;
                            }
                            List<string> jugadoresConectados = new List<string>();
                            jugadoresConectados.Clear();
                            comboBoxJugadores.DataSource = jugadoresConectados;
                        }
                        else
                        {
                            // Cuando el servidor devuelve algo distinto de "N", indicando que hay usuarios conectados.
                            // Rellena el texto de todos los labels con los nicknames recibidos.
                            string[] partes = trozos.Skip(2).ToArray();
                            int j = Convert.ToInt32(mensaje4);
                            Label[] playerslab = new Label[] { player1, player2, player3, player4 };
                            List<string> jugadoresConectados = new List<string>();
                            for (int i = 0; i < j; i++)
                            {
                                playerslab[i].Text = partes[i];
                                if (partes[i] != nickUsu)
                                {
                                    jugadoresConectados.Add(partes[i]);
                                }

                            }
                            comboBoxJugadores.DataSource = jugadoresConectados;

                        }
                        break;
                    case 5:
                        this.Invoke((MethodInvoker)delegate
                        {
                            panelInv.Visible = true;
                            cancelInv.Visible = false;
                            rejectButton.Visible = true;
                            acceptButton.Visible = true;
                            sendInvbut.Visible = false;
                            string[] partes = trozos.Skip(1).ToArray();
                            nameInv.Text = "Invitacion de " + partes[0];
                            pos = Convert.ToInt32(partes[1]);
                        });
                        break;
                    case 6:
                        string mensaje5 = trozos[1].Split('\0')[0];
                        if (mensaje5 == "1")
                        {
                            string mensaje = "8/" + pos;
                            byte[] msg = Encoding.ASCII.GetBytes(mensaje);
                            server.Send(msg);
                        }
                        break;
                    case 7:
                        this.Invoke((MethodInvoker)delegate
                        {
                            if (trozos[1] == "1")
                            {
                                MessageBox.Show("Partida Aceptada");
                                gameCancel.Visible = true;
                                startBut.Visible = true;
                                game.Visible = true;
                                player1.Visible = false;
                                player2.Visible = false;
                                player3.Visible = false;
                                player4.Visible = false;
                                label4.Visible = false;
                                Host.Text = trozos[2];
                                Guest.Text = trozos[3];
                            }
                            else
                            {
                                MessageBox.Show("Partida Rechazada");
                            }
                        });
                        break;
                }
            }
        }
        public void Conn_Click(object sender, EventArgs e)
        {
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse("192.168.56.101");
            IPEndPoint ipep = new IPEndPoint(direc, 9020);


            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //Intentamos conectar el socket
                server.Connect(ipep);
                // Cambia el color de un panel pequeño para indicar que hay conexión.
                // Oculta los botones "Sign In" y "Log In" y muestra el botón "Log Out".
                panel1.BackColor = Color.Green;
                Conn.Visible = false;
                Desconn.Visible = true;

                //Pongo en marcha el thread que atenderá los mensajes del servidor.
                ThreadStart ts = delegate { AtenderServidor(); };
                atender = new Thread(ts);
                atender.Start();
            }
            catch (SocketException ex)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
                return;
            }
        }
        private void Desconn_Click(object sender, EventArgs e)
        {
            //Mensaje de desconexión
            string mensaje = "0/";

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            this.Invoke((MethodInvoker)delegate
            {
                // Nos desconectamos
                atender.Abort();
                panel1.BackColor = Color.Red;
                Desconn.Visible = false;
                Conn.Visible = true;
                server.Shutdown(SocketShutdown.Both);
                server.Close();
            });
        }

        private void Log_Click(object sender, EventArgs e)
        {
            // Función que se ejecuta al presionar el botón "Log In" y abre el panel donde se introducen los datos.

            Panel.Visible = true;
            SignInPanel.Visible = false;
            LogInPanel.Visible = true;
        }

        private void Sign_Click(object sender, EventArgs e)
        {
            // Función que se ejecuta al presionar el botón "Sign In" y abre el panel donde se introducen los datos.
            Panel.Visible = true;
            SignInPanel.Visible = true;
            LogInPanel.Visible = false;
        }

        private void Consultar_Click(object sender, EventArgs e)
        {
            // Función que se ejecuta al presionar el botón "Consulta".
            // Envía una petición con el nombre de un usuario para obtener sus datos.
            if (string.IsNullOrEmpty(nickConsBox.Text))
            {
                // Comprueba que haya datos en los campos de texto.
                MessageBox.Show("Datos no proporcionados");
            }
            else
            {
                // Comprueba si la sesión está iniciada. Es un requisito para poder consultar datos.
                if (sesion == true)
                {
                    string mensaje = "3/" + nickConsBox.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                    nickCons = nickConsBox.Text;
                    // Vacía el texto introducido anteriormente.
                    nickConsBox.Text = null;
                }
                else
                {
                    // Muestra un mensaje en caso de que no tengas la sesión iniciada.
                    MessageBox.Show("Sesión no iniciada.");
                }
            }
        }

        private void Salir_Click(object sender, EventArgs e)
        {
            //Terminar el programa ejecutado.
            this.Close();
        }
        private void LogOut_Click(object sender, EventArgs e)
        {
            // Función que se ejecuta al presionar el botón "Log Out".
            // Sirve para cerrar la sesión actual.
            string mensaje = "4/" + Nickname_player.Text;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            // Reorganiza los objetos para restaurarlos a su versión original.
            Nickname_player.Text = null;
            LogOut.Visible = false;
            Log.Visible = true;
            Sign.Visible = true;
            sesion = false;
        }

        private void CancelPanel_Click(object sender, EventArgs e)
        {
            // Oculta el panel al presionar el botón de cancelar.
            Panel.Visible = false;
        }

        private void SignInPanel_Click(object sender, EventArgs e)
        {
            // Función que se ejecuta al presionar el botón "Sign In" en el panel.
            // Envía los datos del usuario para registrarlo en la base de datos.
            if (string.IsNullOrEmpty(nicktxtpanel.Text) || string.IsNullOrEmpty(passtxtpanel.Text))
            {
                // Comprueba que haya datos en los campos de texto.
                MessageBox.Show("Datos no proporcionados.");
            }
            else
            {
                string mensaje = "1/" + nicktxtpanel.Text + "/" + passtxtpanel.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                // Vacía el texto introducido anteriormente y oculta el panel.
                nicktxtpanel.Text = null;
                passtxtpanel.Text = null;
                Panel.Visible = false;
            }
        }

        private void LogInPanel_Click(object sender, EventArgs e)
        {
            // Función que se ejecuta al presionar el botón "Log In" en el panel.
            // Envía los datos del usuario al servidor para iniciar sesión.
            if (string.IsNullOrEmpty(nicktxtpanel.Text) || string.IsNullOrEmpty(passtxtpanel.Text))
            {
                // Comprueba que haya datos en los campos de texto.
                MessageBox.Show("Datos no proporcionados.");
            }
            else
            {
                string mensaje = "2/" + nicktxtpanel.Text + "/" + passtxtpanel.Text;
                // Enviamos al servidor el nombre tecleado y la contraseña
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                nickUsu = nicktxtpanel.Text;
                // Vacía el texto introducido anteriormente y oculta el panel.
                nicktxtpanel.Text = null;
                passtxtpanel.Text = null;
                Panel.Visible = false;
            }
        }

        private void sendInv_Click(object sender, EventArgs e)
        {
            panelInv.Visible = true;
            cancelInv.Visible = true;
            rejectButton.Visible = false;
            acceptButton.Visible = false;
            sendInvbut.Visible = true;
            nameInv.Text = null;
        }

        private void cancelInv_Click(object sender, EventArgs e)
        {
            // Esconde el panel de invitación.
            panelInv.Visible = false;
        }

        private void sendInvbut_Click(object sender, EventArgs e)
        {
            if (comboBoxJugadores.SelectedItem != null)
            {
                string jugadorSeleccionado = comboBoxJugadores.SelectedItem.ToString();

                // Aqui enviarias la invitacion al servidor
                string mensaje = "5/" + nickUsu + "/" + jugadorSeleccionado;
                byte[] msg = Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                MessageBox.Show("Invitacion enviada a " + jugadorSeleccionado);
                panelInv.Visible = false;
            }
            else
            {
                MessageBox.Show("Selecciona un jugador para invitar.");
            }
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            string mensaje = "6/" + pos;
            byte[] msg = Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            pos = 0;
            panelInv.Visible = false;
        }

        private void rejectButton_Click(object sender, EventArgs e)
        {
            string mensaje = "7/" + pos;
            byte[] msg = Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            pos = 0;
            panelInv.Visible = false;
        }

        private void gameCancel_Click(object sender, EventArgs e)
        {
            Host.Text = null;
            Guest.Text = null;
            game.Visible = false;
            gameCancel.Visible = false;
            startBut.Visible = false;
            label4.Visible = true;
            player1.Visible = true;
            player2.Visible = true;
            player3.Visible = true;
            player4.Visible = true;
        }

        private void startBut_Click(object sender, EventArgs e)
        {
            MessageBox.Show("No hay funcion");
        }
    }
}