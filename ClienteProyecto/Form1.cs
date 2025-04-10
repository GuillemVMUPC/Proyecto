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

namespace ClienteProyecto
{
    public partial class Form1 : Form
    {
        Socket server;
        Thread atender;
        public bool sesion;

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false; //Necesario para que los elementos de los formularios puedan ser
            //accedidos desde threads diferentes a los que los crearon
            LogOut.Visible = false;
            Log.Visible = true;
            sesion = false; //temporalmente cambiado a true por fallos en el log in
            Panel.Visible = false;
        }
        private void AtenderServidor()
        {
            while (true)
            {
                //Recibimos mensaje del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                string[] trozos = Encoding.ASCII.GetString(msg2).Split('/');
                int codigo = Convert.ToInt32(trozos[0]);
                string mensaje = trozos[1].Split('\0')[0];

                switch (codigo)
                {
                    case 1:  // respuesta register
                        if (mensaje == "1")
                        {
                            nicktxtpanel.Text = "";
                            passtxtpanel.Text = "";
                            MessageBox.Show("Registro completado.");
                        }
                        else if (mensaje == "2")
                        {
                            MessageBox.Show("Nickname ya usado.");
                        }
                        else
                        {
                            MessageBox.Show("Error al registrarse.");
                        }
                        break;
                    case 2:      //respuesta Log In
                        this.Invoke((MethodInvoker)delegate
                        {
                            if (mensaje == "1")
                            {
                                LogOut.Visible = true;
                                Log.Visible = false;
                                Sign.Visible = false;
                                sesion = true;
                                Nickname_player.Text = nicktxtpanel.Text;
                                nicktxtpanel.Text = "";
                                passtxtpanel.Text = "";
                                MessageBox.Show("Sesión iniciada.");
                            }
                            else if (mensaje == "2")
                            {
                                MessageBox.Show("Nickname incorrecto.");
                            }
                            else
                            {
                                MessageBox.Show("Password incorrecto.");
                            }
                        });                        
                        break;
                    case 3:       //Recibimos Consulta
                        if (mensaje == "N")
                        {
                            MessageBox.Show("Nombre no econtrado en la base de datos.");
                        }
                        else
                        {
                            string[] partes = trozos.Skip(1).ToArray();

                            // Crear un mensaje combinado para mostrar en el MessageBox
                            string mensajeMostrar = string.Join(Environment.NewLine, partes);

                            // Mostrar el mensaje en un MessageBox
                            MessageBox.Show(mensajeMostrar, "Datos de" + nickConsBox.Text);
                            nickConsBox.Text = "";
                        }
                        break;
                    case 4:
                        if (mensaje == "N")
                        {
                            MessageBox.Show("Nombre no econtrado en la base de datos.");
                        }
                        else
                        {
                            string[] partes = trozos.Skip(2).ToArray();
                            int j = Convert.ToInt32(mensaje);
                            Label[] playerslab = new Label[] { player1, player2, player3, player4 };
                            for (int i = 0; i < j;j++)
                            {
                                playerslab[i].Text = partes[i];
                            }

                        }
                        break;
                }
            }
        }
        public void Conn_Click(object sender, EventArgs e)
        {
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse("192.168.56.101");
            IPEndPoint ipep = new IPEndPoint(direc, 9050);


            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                panel1.BackColor = Color.Green;
                Conn.Visible = false;
                Desconn.Visible = true;

                //pongo en marcha el thread que atenderá los mensajes del servidor
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

            // Nos desconectamos
            atender.Abort();
            panel1.BackColor = Color.Red;
            Desconn.Visible = false;
            Conn.Visible = true;
            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }

        private void Log_Click(object sender, EventArgs e)
        {
            Panel.Visible = true;
            SignInPanel.Visible = false;
            LogInPanel.Visible = true;
        }

        private void Sign_Click(object sender, EventArgs e)
        {
            Panel.Visible = true;
            SignInPanel.Visible = true;
            LogInPanel.Visible = false;
        }

        private void Consultar_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(nickConsBox.Text))
            {
                MessageBox.Show("Datos no proporcionados");
            }
            else
            {
                if (sesion == true)
                {
                    string mensaje = "3/" + nickConsBox.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                    nickConsBox.Text = "";
                }
                else
                {
                    MessageBox.Show("Sesión no iniciada.");
                }
            }
        }

        private void Salir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void LogOut_Click(object sender, EventArgs e)
        {
            string mensaje = "4/" + Nickname_player.Text;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            Nickname_player.Text = null;
            LogOut.Visible = false;
            Log.Visible = true;
            Sign.Visible = true;
            sesion = false;
        }

        private void CancelPanel_Click(object sender, EventArgs e)
        {
            Panel.Visible = false;
        }

        private void SignInPanel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nicktxtpanel.Text) || string.IsNullOrEmpty(passtxtpanel.Text))
            {
                MessageBox.Show("Datos no proporcionados.");
            }
            else
            {
                string mensaje = "1/" + nicktxtpanel.Text + "/" + passtxtpanel.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                Panel.Visible = false;
            }
        }

        private void LogInPanel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nicktxtpanel.Text) || string.IsNullOrEmpty(passtxtpanel.Text))
            {
                MessageBox.Show("Datos no proporcionados.");
            }
            else
            {
                string mensaje = "2/" + nicktxtpanel.Text + "/" + passtxtpanel.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                Panel.Visible = false;
            }
        }
    }
}