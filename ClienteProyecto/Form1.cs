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
        CancellationTokenSource cts;
        public bool sesion;
        public string nickCons;
        public string nickUsu;
        public int pos;
        public List<string> jugadoresConectados = new List<string>();
        List<Form2> formularios = new List<Form2>();
        public bool cargado;

        public Form1()
        {
            InitializeComponent();
            sesion = false;
            List<string> Info = new List<string>() {"Informaci�n Usuario","Historial de Partidas"};
            consultBox.DataSource = Info;
        }
        private void AtenderServidor(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    //Recibimos mensaje del servidor
                    byte[] msg2 = new byte[80];
                    server.Receive(msg2);
                    // Dividimos el texto en fragmentos dentro de un vector para poder extraer el n�mero del c�digo.
                    string[] trozos = Encoding.ASCII.GetString(msg2).Split('/');
                    int codigo = Convert.ToInt32(trozos[0]);
                    string mensaje = trozos[1].Split('\0')[0];
                    switch (codigo)
                    {
                        case 0:
                            this.Invoke((MethodInvoker)delegate
                            {
                                cts.Cancel();           // Se�ala al hilo que debe terminar
                                server.Shutdown(SocketShutdown.Both);
                                server.Close();
                                panel1.BackColor = Color.Red;
                                Desconn.Visible = false;
                                Conn.Visible = true;
                            });
                            break;
                        case 1:  // Respuesta de la funci�n de register.
                            if (mensaje == "1")
                            {
                                // Cuando el servidor devuelve un 1 para indicar que el registro se completa sin ning�n error.
                                MessageBox.Show("Registro completado.");
                            }
                            else if (mensaje == "2")
                            {
                                // Cuando el servidor devuelve un 2 para indicar que el nickname proporcionado ya est� en uso.
                                MessageBox.Show("Nickname ya usado.");
                            }
                            else
                            {
                                // Cualquier otro error ocurrido en el servidor.
                                MessageBox.Show("Error al registrarse.");
                            }
                            break;
                        case 2:      //Respuesta funci�n Log In.
                            if (mensaje == "1")
                            {
                                // Cuando el servidor devuelve un 1 para indicar que la sesi�n se ha iniciado correctamente,
                                // se realizan algunos cambios visuales en el cliente.

                                this.Invoke((MethodInvoker)delegate
                                {
                                    nickUsu = nicktxtpanel.Text;
                                    // Vac�a el texto introducido anteriormente y oculta el panel.
                                    nicktxtpanel.Text = null;
                                    passtxtpanel.Text = null;
                                    Panel.Visible = false;
                                    LogOut.Visible = true;
                                    Log.Visible = false;
                                    Sign.Visible = false;
                                    SingOutButt.Visible = true;
                                    sesion = true;
                                    Nickname_player.Text = nickUsu;
                                });
                                MessageBox.Show("Sesi�n iniciada.");
                            }
                            else if (mensaje == "2")
                            {
                                // Cuando el servidor devuelve un 2 para indicar que el nickname no se encuentra en la base de datos.
                                MessageBox.Show("Nickname incorrecto.");
                            }
                            else
                            {
                                // Cuando el servidor devuelve un 3 para indicar que la contrase�a proporcionada para ese usuario no es correcta.
                                MessageBox.Show("Password incorrecto.");
                            }
                            break;
                        case 3:  //Respuesta funci�n Consulta
                            if (mensaje == "N")
                            {
                                // Cuando el servidor devuelve una "N" para indicar que el nickname no se ha encontrado en la base de datos.
                                MessageBox.Show("Nombre no econtrado en la base de datos.");
                            }
                            else
                            {
                                // Cuando el servidor devuelve algo distinto de una "N", proporcionando los datos del usuario solicitado.
                                string[] partes1 = trozos.Skip(1).ToArray();

                                // Mostrar el mensaje en un MessageBox
                                MessageBox.Show("Datos de " + nickCons + "\n" + "ID: " + partes1[0] + "\n" + "Nickname: " + partes1[1] + "\n" + "Total Score: " + partes1[2]);
                                this.Invoke((MethodInvoker)delegate
                                {
                                    nickCons = null;
                                });
                            }
                            break;
                        case 4:  //Notificaci�n
                            if (mensaje == "N")
                            {
                                // Cuando el servidor devuelve una "N" para indicar que no hay ning�n usuario conectado.
                                // Vac�a el texto de todos los labels.
                                int j = 4;
                                Label[] playerslab = new Label[] { player1, player2, player3, player4 };
                                this.Invoke((MethodInvoker)delegate
                                {
                                    for (int i = 0; i < j; i++)
                                    {
                                        playerslab[i].Text = null;
                                    }
                                    jugadoresConectados.Clear();
                                    comboBoxJugadores.DataSource = null;
                                    comboBoxJugadores.DataSource = jugadoresConectados;
                                });
                            }
                            else
                            {
                                // Cuando el servidor devuelve algo distinto de "N", indicando que hay usuarios conectados.
                                // Rellena el texto de todos los labels con los nicknames recibidos.
                                string[] partes2 = string.Join("/", trozos.Skip(2)).Split('\0')[0].Split('/');
                                int j = Convert.ToInt32(trozos[1]);
                                Label[] playerslab = new Label[] { player1, player2, player3, player4 };
                                jugadoresConectados.Clear();

                                this.Invoke((MethodInvoker)delegate
                                {
                                    int labelIndex = 0;
                                    for (int i = 0; i < partes2.Length; i++)
                                    {
                                        if (partes2[i] != nickUsu)
                                        {
                                            playerslab[labelIndex].Text = partes2[i];
                                            jugadoresConectados.Add(partes2[i]);
                                            labelIndex++;
                                        }
                                    }

                                    // Vac�a los labels restantes
                                    for (int k = labelIndex; k < playerslab.Length; k++)
                                    {
                                        playerslab[k].Text = null;
                                    }

                                    comboBoxJugadores.DataSource = null;
                                    comboBoxJugadores.DataSource = jugadoresConectados;
                                });

                            }
                            break;
                        case 5:
                            string[] partes = trozos.Skip(1).ToArray();
                            this.Invoke((MethodInvoker)delegate
                            {
                                panelInv.Visible = true;
                                cancelInv.Visible = false;
                                rejectButton.Visible = true;
                                acceptButton.Visible = true;
                                sendInvbut.Visible = false;
                                nameInv.Text = "Invitacion de " + partes[0];
                                pos = Convert.ToInt32(partes[1]);
                            });
                            break;
                        case 6:
                            if (mensaje == "1")
                            {
                                string mensaje1 = "8/" + pos;
                                byte[] msg = Encoding.ASCII.GetBytes(mensaje1);
                                server.Send(msg);
                            }
                            break;
                        case 7:
                            if (trozos[1] == "1")
                            {
                                MessageBox.Show("Partida Aceptada");
                                this.Invoke((MethodInvoker)delegate
                                {
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
                                });
                            }
                            else
                            {
                                MessageBox.Show("Partida Rechazada");
                            }
                            break;
                        case 8:
                            if (mensaje == "1")
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    Nickname_player.Text = null;
                                    LogOut.Visible = false;
                                    Log.Visible = true;
                                    Sign.Visible = true;
                                    SingOutButt.Visible = false;
                                    sesion = false;
                                });
                            }
                            break;
                        case 9:
                            string usu = trozos[3].Split('\0')[0];
                            string usuario = trozos[2].Split('\0')[0];
                            if (mensaje != "Error")
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    respMens1(mensaje, usu, usuario);
                                });
                            }
                            else
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    string mensaje2 = trozos[3].Split('\0')[0];
                                    respMens2(mensaje2, usu, usuario);
                                });
                            }
                            break;
                    }
                }
                catch (SocketException ex)
                {
                    Console.WriteLine("Error de socket: " + ex.Message);
                    break;
                }
            }
        }
        public void Conn_Click(object sender, EventArgs e)
        {
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse("192.168.56.101");
            IPEndPoint ipep = new IPEndPoint(direc, 9080);


            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //Intentamos conectar el socket
                server.Connect(ipep);
                // Cambia el color de un panel peque�o para indicar que hay conexi�n.
                // Oculta los botones "Sign In" y "Log In" y muestra el bot�n "Log Out".
                panel1.BackColor = Color.Green;
                Conn.Visible = false;
                Desconn.Visible = true;

                //Pongo en marcha el thread que atender� los mensajes del servidor.
                cts = new CancellationTokenSource();
                atender = new Thread(() => AtenderServidor(cts.Token));
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
            //Mensaje de desconexi�n
            string mensaje = "0/";

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        private void Log_Click(object sender, EventArgs e)
        {
            // Funci�n que se ejecuta al presionar el bot�n "Log In" y abre el panel donde se introducen los datos.

            Panel.Visible = true;
            SignInPanel.Visible = false;
            LogInPanel.Visible = true;
        }

        private void Sign_Click(object sender, EventArgs e)
        {
            // Funci�n que se ejecuta al presionar el bot�n "Sign In" y abre el panel donde se introducen los datos.
            Panel.Visible = true;
            SignInPanel.Visible = true;
            LogInPanel.Visible = false;
        }

        private void Consultar_Click(object sender, EventArgs e)
        {
            // Funci�n que se ejecuta al presionar el bot�n "Consulta".
            // Env�a una petici�n con el nombre de un usuario para obtener sus datos.
            if (string.IsNullOrEmpty(nickConsBox.Text))
            {
                // Comprueba que haya datos en los campos de texto.
                MessageBox.Show("Datos no proporcionados");
            }
            else
            {
                // Comprueba si la sesi�n est� iniciada. Es un requisito para poder consultar datos.
                if (sesion == true)
                {
                    string Infor = consultBox.SelectedItem.ToString();
                    int a = 2;
                    if(Infor == "Informaci�n Usuario")
                    {
                        a = 11;
                    }
                    else if(Infor == "Historial de Partidas")
                    {
                        a = 12;
                    }
                    string mensaje = "3/" + a + "/" + nickConsBox.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                    nickCons = nickConsBox.Text;
                    // Vac�a el texto introducido anteriormente.
                    nickConsBox.Text = null;
                }
                else
                {
                    // Muestra un mensaje en caso de que no tengas la sesi�n iniciada.
                    MessageBox.Show("Sesi�n no iniciada.");
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
            // Funci�n que se ejecuta al presionar el bot�n "Log Out".
            // Sirve para cerrar la sesi�n actual.
            string mensaje = "4/" + Nickname_player.Text;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            // Reorganiza los objetos para restaurarlos a su versi�n original.
            Nickname_player.Text = null;
            LogOut.Visible = false;
            Log.Visible = true;
            Sign.Visible = true;
            SingOutButt.Visible = false;
            sesion = false;
        }

        private void CancelPanel_Click(object sender, EventArgs e)
        {
            // Oculta el panel al presionar el bot�n de cancelar.
            Panel.Visible = false;
        }

        private void SignInPanel_Click(object sender, EventArgs e)
        {
            // Funci�n que se ejecuta al presionar el bot�n "Sign In" en el panel.
            // Env�a los datos del usuario para registrarlo en la base de datos.
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
                // Vac�a el texto introducido anteriormente y oculta el panel.
                nicktxtpanel.Text = null;
                passtxtpanel.Text = null;
                Panel.Visible = false;
            }
        }

        private void LogInPanel_Click(object sender, EventArgs e)
        {
            // Funci�n que se ejecuta al presionar el bot�n "Log In" en el panel.
            // Env�a los datos del usuario al servidor para iniciar sesi�n.
            if (string.IsNullOrEmpty(nicktxtpanel.Text) || string.IsNullOrEmpty(passtxtpanel.Text))
            {
                // Comprueba que haya datos en los campos de texto.
                MessageBox.Show("Datos no proporcionados.");
            }
            else
            {
                string mensaje = "2/" + nicktxtpanel.Text + "/" + passtxtpanel.Text;
                // Enviamos al servidor el nombre tecleado y la contrase�a
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
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
            // Esconde el panel de invitaci�n.
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

        private void SingOutButt_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("�Est�s seguro de que quieres eliminar el usuario?","Confirmaci�n",MessageBoxButtons.OKCancel,MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                // El usuario hizo clic en Aceptar
                string mensaje = "9/" + nickUsu;
                byte[] msg = Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            else
            {
                // El usuario hizo clic en Cancelar
                // No haces nada
            }
        }

        private void chatButt_Click(object sender, EventArgs e)
        {
            CancellationTokenSource cts2 = new CancellationTokenSource();
            Thread newForm = new Thread(() => NuevoForm(cts2.Token));
            newForm.Start();
        }

        public void NuevoForm(CancellationToken token)
        {
            int cont = formularios.Count;
            Form2 f = new Form2(server, jugadoresConectados, nickUsu);
            if(cont < 1)
            {
                formularios.Add(f);
            }
            f.ShowDialog();
        }
        public void respMens1(string mensaje, string usu, string usuario)
        {
            string nom = usuario + usu;
            string nombreLimpio = SanearNombreArchivo(nom);
            string ruta = Path.Combine(Application.StartupPath, nombreLimpio + ".txt");
            string menstxt = usuario + ": " + mensaje;
            using (StreamWriter writer = new StreamWriter(ruta, true))
            {
                writer.WriteLine(menstxt);
            }
            bool cargadoExitosamente = false;
            for (int intento = 0; intento < 5 && !cargadoExitosamente; intento++)
            {
                try
                {
                    formularios[0].LoadMens(usuario, usu);
                    cargadoExitosamente = true;
                }
                catch (IOException)
                {
                    Thread.Sleep(50); // esperar antes de reintentar
                }
            }

        }
        public void respMens2(string mensaje, string usu, string usuario)
        {
            string nom = usu + usuario;
            string nombreLimpio = SanearNombreArchivo(nom);
            string ruta = Path.Combine(Application.StartupPath, nombreLimpio + ".txt");

            if (!File.Exists(ruta))
                return;

            // Leer todas las l�neas del archivo
            string[] lineas = File.ReadAllLines(ruta);

            for (int i = 0; i < lineas.Length; i++)
            {
                // Si la l�nea contiene el mensaje original y a�n no est� marcada como error
                if (lineas[i].Contains(mensaje) && !lineas[i].Contains("(error)"))
                {
                    lineas[i] += " (error)";
                    break; // Solo modificamos la primera coincidencia
                }
            }

            using (var writer = new StreamWriter(ruta, false))
            {
                foreach (string linea in lineas)
                {
                    writer.WriteLine(linea);
                }
            }
            bool cargadoExitosamente = false;
            for (int intento = 0; intento < 5 && !cargadoExitosamente; intento++)
            {
                try
                {
                    formularios[0].LoadMens(usu, usuario);
                    cargadoExitosamente = true;
                }
                catch (IOException)
                {
                    Thread.Sleep(50); // esperar antes de reintentar
                }
            }

        }
        private string SanearNombreArchivo(string nombre)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                nombre = nombre.Replace(c, '_'); // Reemplaza caracteres ilegales por guiones bajos
            }
            return nombre;
        }
    }
}