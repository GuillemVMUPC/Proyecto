using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace ClienteProyecto
{
    public partial class LogInInterf : Form
    {
        private Form1 form1;
        public LogInInterf()
        {
            InitializeComponent();
        }

        private void Enviar_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nickBox.Text) || string.IsNullOrEmpty(passBox.Text))
            {
                MessageBox.Show("Datos no proporcionados.");
            }
            else
            {
                try
                {
                    int compl = form1.LogIn(nickBox.Text, passBox.Text);
                    if (compl == 1)
                    {
                        MessageBox.Show("Sesión iniciada.");
                        this.Close();
                    }
                    else if (compl == 2)
                    {
                        MessageBox.Show("Nickname incorrecto.");
                    }
                    else if (compl == 3)
                    {
                        MessageBox.Show("Password incorrecto.");
                    }
                    else
                    {
                        MessageBox.Show("Error al iniciar sesión.");
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show("Error al iniciar sesión.");
                    return;
                }
            }
        }

        private void Cancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
