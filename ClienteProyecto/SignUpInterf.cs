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
    public partial class SignUpInterf : Form
    {
        private Form1 form1;
        public SignUpInterf(Form1 mainform)
        {
            InitializeComponent();
            form1 = mainform;
        }

        private void Registrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nickBox.Text) || string.IsNullOrEmpty(passBox.Text))
            {
                MessageBox.Show("Datos no proporcionados.");
            }
            else
            {
                try
                {
                    int compl = form1.Registrarse(nickBox.Text, passBox.Text);
                     
                    if(compl == 1)
                    {
                        MessageBox.Show("Registro completado.");
                        this.Close();
                    }
                    else if(compl == 2)
                    {
                        MessageBox.Show("Nickname ya usado.");
                    }
                    else 
                    {
                        MessageBox.Show("Error al registrarse.");
                    }
                }
                catch 
                {
                    MessageBox.Show("Error al registrarse.");
                }
            }
        }

        private void Cancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
