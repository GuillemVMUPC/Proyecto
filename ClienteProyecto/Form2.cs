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
                    string nombreLimpio = SanearNombreArchivo(usu);
                    string ruta = Path.Combine(Application.StartupPath, nombreLimpio + ".txt");
                    string menstxt = "Yo: " + txtSend.Text;
                    File.AppendAllText(ruta, menstxt + Environment.NewLine);

                    // Limpiamos el campo de texto
                    txtSend.Clear();
                    LoadMens(usu);
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
                LoadMens(usu);
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

        private void LoadMens(string usu)
        {
            if (cargado = true)
            {
                string nombreLimpio = SanearNombreArchivo(usu);
                string ruta = Path.Combine(Application.StartupPath, nombreLimpio + ".txt");

                if (File.Exists(ruta))
                {
                    txtRead.Text = File.ReadAllText(ruta);
                }

                // Si no existe, lo creamos vacío de forma segura
                if (!File.Exists(ruta))
                {
                    File.WriteAllText(ruta, string.Empty);  // Crea y escribe texto vacío
                }

                // Ahora leemos el contenido (aunque esté vacío)
                txtRead.Text = File.ReadAllText(ruta);
            }
            else
            {
                MessageBox.Show("Seleccione un usuario");
            }
        }
    }
}
