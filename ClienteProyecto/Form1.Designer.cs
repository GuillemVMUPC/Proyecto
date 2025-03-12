namespace ClienteProyecto
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Conn = new Button();
            Desconn = new Button();
            panel1 = new Panel();
            Sign = new Button();
            Log = new Button();
            Nickname_player = new Label();
            Consultar = new Button();
            Salir = new Button();
            nickConsBox = new TextBox();
            label1 = new Label();
            LogOut = new Button();
            SuspendLayout();
            // 
            // Conn
            // 
            Conn.Location = new Point(12, 12);
            Conn.Name = "Conn";
            Conn.Size = new Size(132, 35);
            Conn.TabIndex = 0;
            Conn.Text = "Conectar";
            Conn.UseVisualStyleBackColor = true;
            Conn.Click += Conn_Click;
            // 
            // Desconn
            // 
            Desconn.Location = new Point(12, 13);
            Desconn.Name = "Desconn";
            Desconn.Size = new Size(132, 34);
            Desconn.TabIndex = 1;
            Desconn.Text = "Desconectar";
            Desconn.UseVisualStyleBackColor = true;
            Desconn.Click += Desconn_Click;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.Control;
            panel1.Location = new Point(157, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(36, 35);
            panel1.TabIndex = 2;
            // 
            // Sign
            // 
            Sign.Location = new Point(704, 13);
            Sign.Name = "Sign";
            Sign.Size = new Size(84, 34);
            Sign.TabIndex = 0;
            Sign.Text = "Sign Up";
            Sign.UseVisualStyleBackColor = true;
            Sign.Click += Sign_Click;
            // 
            // Log
            // 
            Log.Location = new Point(610, 13);
            Log.Name = "Log";
            Log.Size = new Size(88, 34);
            Log.TabIndex = 1;
            Log.Text = "Log In";
            Log.UseVisualStyleBackColor = true;
            Log.Click += Log_Click;
            // 
            // Nickname_player
            // 
            Nickname_player.AutoSize = true;
            Nickname_player.Location = new Point(687, 416);
            Nickname_player.Name = "Nickname_player";
            Nickname_player.Size = new Size(59, 25);
            Nickname_player.TabIndex = 4;
            Nickname_player.Text = "label1";
            // 
            // Consultar
            // 
            Consultar.Location = new Point(499, 203);
            Consultar.Name = "Consultar";
            Consultar.Size = new Size(112, 34);
            Consultar.TabIndex = 5;
            Consultar.Text = "Consultar";
            Consultar.UseVisualStyleBackColor = true;
            Consultar.Click += Consultar_Click;
            // 
            // Salir
            // 
            Salir.Location = new Point(12, 404);
            Salir.Name = "Salir";
            Salir.Size = new Size(112, 34);
            Salir.TabIndex = 6;
            Salir.Text = "Salir";
            Salir.UseVisualStyleBackColor = true;
            Salir.Click += Salir_Click;
            // 
            // nickConsBox
            // 
            nickConsBox.Location = new Point(343, 206);
            nickConsBox.Name = "nickConsBox";
            nickConsBox.Size = new Size(150, 31);
            nickConsBox.TabIndex = 7;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(243, 208);
            label1.Name = "label1";
            label1.Size = new Size(94, 25);
            label1.TabIndex = 8;
            label1.Text = "Nickname:";
            // 
            // LogOut
            // 
            LogOut.Location = new Point(704, 12);
            LogOut.Name = "LogOut";
            LogOut.Size = new Size(88, 34);
            LogOut.TabIndex = 9;
            LogOut.Text = "Log Out";
            LogOut.UseVisualStyleBackColor = true;
            LogOut.Click += LogOut_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(nickConsBox);
            Controls.Add(Salir);
            Controls.Add(Consultar);
            Controls.Add(Nickname_player);
            Controls.Add(Sign);
            Controls.Add(Log);
            Controls.Add(panel1);
            Controls.Add(Conn);
            Controls.Add(Desconn);
            Controls.Add(LogOut);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Conn;
        private Button Desconn;
        private Panel panel1;
        private Button Sign;
        private Button Log;
        private Label Nick;
        private Label Nickname_player;
        private Button Consultar;
        private Button Salir;
        private TextBox nickConsBox;
        private Label label1;
        private Button LogOut;
    }
}