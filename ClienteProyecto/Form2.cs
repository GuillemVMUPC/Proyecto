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
        public bool cargado;
        public Form2(Socket server, List<string> jugadoresConectados, string usuario)
        {
            InitializeComponent();
            this.server = server;
            this.usuario = usuario;
            this.jugadoresConectados = jugadoresConectados;
            comboUsuChat.DataSource = jugadoresConectados;
            cargado = false;
            this.usuario = usuario;
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
                    MessageBox.Show("Mensaje demasiado largo");
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
                MessageBox.Show("Escribe un mensaje");
            }
            else
            {
                MessageBox.Show("Seleccione un chat");
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
                MessageBox.Show("Selecciona un usuario.");
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
                MessageBox.Show("Seleccione un usuario");
            }
        }

    }
}
