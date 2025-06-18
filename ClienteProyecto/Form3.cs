using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClienteProyecto
{
    public partial class Form3 : Form
    {
        int nForm;
        Socket server;
        int totalJugador = 0;
        int totalCrupier = 0;
        List<string> cartasJugador = new List<string>();
        List<string> cartasCrupier = new List<string>();
        public string host;
        public string guest;
        public string nickusu;
        public string turno;
        public string txtmensaje;
        public int pos;
        public bool parar;
        public Form3(int nForm, Socket server, string Host, string Guest, string nickUsu, int posGame, string turno)
        {
            InitializeComponent();
            this.server = server;
            this.host = Host;
            this.guest = Guest;
            this.nickusu = nickUsu;
            this.pos = posGame;
            this.turno = turno;
            this.nForm = nForm;
            parar = false;
            iniCrup();
            
        }

        private void PedirCarta_Click(object sender, EventArgs e)
        {
            if ((nickusu == turno) && (parar == false))
            {
                string mensaje = "12/" + pos + "/" + turno + "/" + nForm;
                byte[] msg = Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            else
            {
                txtmensaje = "No es tu turno";
                mostrarmens(txtmensaje, "Red");
            }
        }

        private void PararButton_Click(object sender, EventArgs e)
        {
            if (parar == false)
            {
                txtmensaje = $"Te plantas con {totalJugador} puntos.";
                mostrarmens(txtmensaje, "Green");
                // Aquí puedes avisar al servidor: "12/plantarse"
                string mensaje = "13/" + pos + "/" + nForm;
                byte[] msg = Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                parar = true;
            }
            else
            {
                txtmensaje = "Ya te has plantado";
                mostrarmens(txtmensaje, "Red");
            }
        }
        public void mostrarmens(string tmensaje, string color)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtlblmes.Text = tmensaje;
                txtlblmes.BackColor = Color.FromName(color);
                txtlblmes.ForeColor = Color.White;
                txtlblmes.Visible = true;

                int alpha = 255;
                Color fondoInicial = txtlblmes.BackColor;
                Color textoInicial = txtlblmes.ForeColor;

                // Espera 1 segundo antes de empezar el fade
                System.Windows.Forms.Timer delay = new System.Windows.Forms.Timer();
                delay.Interval = 1000;
                delay.Tick += (s1, e1) =>
                {
                    delay.Stop();
                    delay.Dispose();

                    System.Windows.Forms.Timer fadeTimer = new System.Windows.Forms.Timer();
                    fadeTimer.Interval = 50;
                    fadeTimer.Tick += (s2, e2) =>
                    {
                        alpha -= 15;

                        if (alpha <= 0)
                        {
                            txtlblmes.Visible = false;
                            fadeTimer.Stop();
                            fadeTimer.Dispose();
                        }
                        else
                        {
                            txtlblmes.BackColor = Color.FromArgb(alpha, fondoInicial.R, fondoInicial.G, fondoInicial.B);
                            txtlblmes.ForeColor = Color.FromArgb(alpha, textoInicial.R, textoInicial.G, textoInicial.B);
                        }
                    };
                    fadeTimer.Start();
                };
                delay.Start();
            });
        }
        public void whoturno(string newturno)
        {
            turno = newturno;
        }
        public void respRobar(int valor, string palo)
        {
            this.Invoke((MethodInvoker)delegate
            {
                cartasJugador.Add($"{valor} de {palo}");
                totalJugador += valor;

                cartaslbl.Text = string.Join(", ", cartasJugador);
                totallbl.Text = $"Total: {totalJugador}";

                if (totalJugador > 21)
                {
                    txtmensaje = "¡Has perdido! Te pasaste de 21.";
                    mostrarmens(txtmensaje, "Red");
                    string mensaje = "13/" + pos + "/" + nForm;
                    byte[] msg = Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                    parar = true;
                }
            });
        }
        public void cartCrup(int numero1, string palo1, int numero2, string palo2)
        {
            this.Invoke((MethodInvoker)delegate
            {
                int[] numeros = new int[] {numero1,numero2};
                string[] palos = new string[] { palo1, palo2 };
                for (int i = 0; i < 2; i++)
                {
                    cartasCrupier.Add($"{numeros[i]} de {palos[i]}");
                    totalCrupier += numeros[i];

                    CartasCrup.Text = string.Join(", ", cartasCrupier);
                    totalCrup.Text = $"Total: {totalCrupier}";
                }
            });
        }
        public void robarCrup(int numero, string palo)
        {
            cartasCrupier.Add($"{numero} de {palo}");
            totalCrupier += numero;

            CartasCrup.Text = string.Join(", ", cartasCrupier);
            totalCrup.Text = $"Total: {totalCrupier}";
        }
        public void iniCrup()
        {
            string mensaje = "14/" + pos + "/" + nForm;
            byte[] msg = Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }
        private void cerrarButt_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
