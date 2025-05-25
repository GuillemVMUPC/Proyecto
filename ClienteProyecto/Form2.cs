using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClienteProyecto
{
    public partial class Form2 : Form
    {
        Socket server;
        List<string> jugadoresConectados = new List<string>();
        public string usu;
        public string usuario;
        public string txtmensaje;
        public bool cargado;
        public Form2(Socket server, List<string> jugadoresConectados, string usuario)
        {
            InitializeComponent();
            this.server = server;
            this.usuario = usuario;
            this.jugadoresConectados = jugadoresConectados;
            comboUsuChat.DataSource = jugadoresConectados;
            txtmensaje = "";
            cargado = false;
        }

        private void sendButt_Click(object sender, EventArgs e)
        {
            if((!string.IsNullOrEmpty(txtSend.Text)) && cargado == true)
            {
                string nom = usu + usuario;
                string mens = FormatearMensaje(txtSend.Text);
                int longi = mens.Length;
                if (longi > 80)
                {
                    txtmensaje = "Mensaje demasiado largo";
                    mostrarmens(txtmensaje, "Red");
                }
                else
                {
                    string mensaje = "10/" + usuario + "/" + usu + "/" + mens;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                    // Guardamos en el archivo
                    string nombreLimpio = SanearNombreArchivo(nom);
                    string ruta = Path.Combine(Application.StartupPath, nombreLimpio + ".txt");
                    string menstxt = "Yo: " + txtSend.Text;
                    using (StreamWriter writer = new StreamWriter(ruta, true))
                    {
                        writer.WriteLine(menstxt);
                    }


                    // Limpiamos el campo de texto
                    LoadMens(usu, usuario);
                    txtSend.Clear();
                }
            }
            else if (cargado == true)
            {
                txtmensaje = "Escribe un mensaje";
                mostrarmens(txtmensaje, "Red");
            }
            else
            {
                txtmensaje = "Seleccione un chat";
                mostrarmens(txtmensaje, "Red");
            }
        }
        private string FormatearMensaje(string mensaje)
        {
            return mensaje.Replace(" ", "_");
        }

        private void swButt_Click(object sender, EventArgs e)
        {
            usu = comboUsuChat.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(usu))
            {
                txtmensaje = "Selecciona un usuario.";
                mostrarmens(txtmensaje, "Red");
                return;
            }
            else
            {
                cargado = true;
                LoadMens(usu, usuario); //envio , enviador
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

        private void CancelPanel_Click(object sender, EventArgs e)
        {
            cargado = false;
            this.Close();
        }

        public void LoadMens(string usu, string usuario)
        {
            if (cargado)
            {
                string nom = usu + usuario;
                string nombreLimpio = SanearNombreArchivo(nom);
                string ruta = Path.Combine(Application.StartupPath, nombreLimpio + ".txt");

                if (!File.Exists(ruta))
                {
                    using (var fs = File.Create(ruta)) { }
                }
                for (int intento = 0; intento < 5; intento++)
                {
                    try
                    {
                        using (var reader = new StreamReader(ruta))
                        {
                            txtRead.Invoke((MethodInvoker)delegate
                            {
                                txtRead.Text = reader.ReadToEnd();
                            });
                        }
                        break;
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(50); // espera 50ms y reintent
                    }
                }
            }
            else
            {
                txtmensaje = "Selecciona un usuario.";
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

    }
}
