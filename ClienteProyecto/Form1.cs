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

namespace ClienteProyecto
{
    public partial class Form1 : Form
    {
        Socket server;
        public bool sesion;
        public Form1()
        {
            InitializeComponent();
            LogOut.Visible = false;
            Log.Visible = true;
            sesion = false;
        }

        public void Conn_Click(object sender, EventArgs e)
        {
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9010);


            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                panel1.BackColor = Color.Green;
                Conn.Visible = false;
                Desconn.Visible = true;
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
            panel1.BackColor = Color.Gray;
            Desconn.Visible = false;
            Conn.Visible = true;
            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }

        private void Log_Click(object sender, EventArgs e)
        {
            LogInInterf logInInterf = new LogInInterf(this);
            logInInterf.ShowDialog();
        }

        private void Sign_Click(object sender, EventArgs e)
        {
            SignUpInterf signUpInterf = new SignUpInterf(this);
            signUpInterf.ShowDialog();
        }

        private void Consultar_Click(object sender, EventArgs e)
        {
            if (sesion == true)
            {
                string mensaje = "3/" + nickConsBox.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];

                if (mensaje == "2")
                {
                    MessageBox.Show("Nombre no econtrado en la base de datos.");
                }
                else
                {
                    nickConsBox.Text = "";
                    MessageBox.Show(mensaje);
                }
            }
            else
            {
                MessageBox.Show("Sesión no iniciada.");
            }
        }

        private void Salir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public int Registrarse(string nick, string pass)
        {
            string mensaje = "1/" + nick + "/" + pass;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            //Recibimos la respuesta del servidor
            byte[] msg2 = new byte[80];
            server.Receive(msg2);
            mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];

            if (mensaje == "1")
            {
                return 1;
            }
            else if (mensaje == "2")
            {
                return 2;
            }
            else
            {
                return 3;
            }

        }
        public int LogIn(string nick, string pass)
        {
            string mensaje = "2/" + nick + "/" + pass;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            //Recibimos la respuesta del servidor
            byte[] msg2 = new byte[80];
            server.Receive(msg2);
            mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];

            if (mensaje == "1")
            {
                Nickname_player.Text = nick;
                LogOut.Visible = true;
                Log.Visible = false;
                Sign.Visible = false;
                sesion = true;
                return 1;
            }
            else if (mensaje == "2")
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }

        private void LogOut_Click(object sender, EventArgs e)
        {
            Nickname_player.Text = null;
            LogOut.Visible = false;
            Log.Visible = true;
            Sign.Visible = true;
            sesion = false;
        }
    }
}