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
            Panel = new Panel();
            label3 = new Label();
            label2 = new Label();
            passtxtpanel = new TextBox();
            nicktxtpanel = new TextBox();
            SignInPanel = new Button();
            CancelPanel = new Button();
            LogInPanel = new Button();
            panel2 = new Panel();
            label4 = new Label();
            player4 = new Label();
            player3 = new Label();
            player2 = new Label();
            player1 = new Label();
            Panel.SuspendLayout();
            panel2.SuspendLayout();
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
            panel1.BackColor = Color.Red;
            panel1.Location = new Point(157, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(36, 35);
            panel1.TabIndex = 2;
            // 
            // Sign
            // 
            Sign.Location = new Point(708, 13);
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
            Nickname_player.Size = new Size(0, 25);
            Nickname_player.TabIndex = 4;
            // 
            // Consultar
            // 
            Consultar.Location = new Point(551, 199);
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
            nickConsBox.Location = new Point(395, 201);
            nickConsBox.Name = "nickConsBox";
            nickConsBox.Size = new Size(150, 31);
            nickConsBox.TabIndex = 7;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(295, 204);
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
            // Panel
            // 
            Panel.BackColor = Color.White;
            Panel.BorderStyle = BorderStyle.FixedSingle;
            Panel.Controls.Add(label3);
            Panel.Controls.Add(label2);
            Panel.Controls.Add(passtxtpanel);
            Panel.Controls.Add(nicktxtpanel);
            Panel.Controls.Add(SignInPanel);
            Panel.Controls.Add(CancelPanel);
            Panel.Controls.Add(LogInPanel);
            Panel.Location = new Point(502, 1);
            Panel.Name = "Panel";
            Panel.Size = new Size(300, 150);
            Panel.TabIndex = 10;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(22, 54);
            label3.Name = "label3";
            label3.Size = new Size(87, 25);
            label3.TabIndex = 6;
            label3.Text = "Password";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(19, 13);
            label2.Name = "label2";
            label2.Size = new Size(90, 25);
            label2.TabIndex = 5;
            label2.Text = "Nickname";
            // 
            // passtxtpanel
            // 
            passtxtpanel.Location = new Point(115, 51);
            passtxtpanel.Name = "passtxtpanel";
            passtxtpanel.Size = new Size(150, 31);
            passtxtpanel.TabIndex = 4;
            // 
            // nicktxtpanel
            // 
            nicktxtpanel.Location = new Point(115, 10);
            nicktxtpanel.Name = "nicktxtpanel";
            nicktxtpanel.Size = new Size(150, 31);
            nicktxtpanel.TabIndex = 3;
            // 
            // SignInPanel
            // 
            SignInPanel.BackColor = Color.Blue;
            SignInPanel.FlatStyle = FlatStyle.Flat;
            SignInPanel.ForeColor = Color.White;
            SignInPanel.Location = new Point(173, 99);
            SignInPanel.Name = "SignInPanel";
            SignInPanel.Size = new Size(112, 34);
            SignInPanel.TabIndex = 2;
            SignInPanel.Text = "Sign In";
            SignInPanel.UseVisualStyleBackColor = false;
            SignInPanel.Click += SignInPanel_Click;
            // 
            // CancelPanel
            // 
            CancelPanel.BackColor = Color.Red;
            CancelPanel.FlatStyle = FlatStyle.Flat;
            CancelPanel.ForeColor = Color.White;
            CancelPanel.Location = new Point(19, 99);
            CancelPanel.Name = "CancelPanel";
            CancelPanel.Size = new Size(112, 34);
            CancelPanel.TabIndex = 1;
            CancelPanel.Text = "Cancel";
            CancelPanel.UseVisualStyleBackColor = false;
            CancelPanel.Click += CancelPanel_Click;
            // 
            // LogInPanel
            // 
            LogInPanel.BackColor = Color.Blue;
            LogInPanel.FlatStyle = FlatStyle.Flat;
            LogInPanel.ForeColor = Color.White;
            LogInPanel.Location = new Point(173, 99);
            LogInPanel.Name = "LogInPanel";
            LogInPanel.Size = new Size(112, 34);
            LogInPanel.TabIndex = 0;
            LogInPanel.Text = "Log In";
            LogInPanel.UseVisualStyleBackColor = false;
            LogInPanel.Click += LogInPanel_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(label4);
            panel2.Controls.Add(player4);
            panel2.Controls.Add(player3);
            panel2.Controls.Add(player2);
            panel2.Controls.Add(player1);
            panel2.Location = new Point(12, 83);
            panel2.Name = "panel2";
            panel2.Size = new Size(194, 264);
            panel2.TabIndex = 11;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(36, 17);
            label4.Name = "label4";
            label4.Size = new Size(123, 25);
            label4.TabIndex = 4;
            label4.Text = "Online Players";
            // 
            // player4
            // 
            player4.AutoSize = true;
            player4.Location = new Point(16, 205);
            player4.Name = "player4";
            player4.Size = new Size(0, 25);
            player4.TabIndex = 3;
            // 
            // player3
            // 
            player3.AutoSize = true;
            player3.Location = new Point(16, 155);
            player3.Name = "player3";
            player3.Size = new Size(0, 25);
            player3.TabIndex = 2;
            // 
            // player2
            // 
            player2.AutoSize = true;
            player2.Location = new Point(16, 105);
            player2.Name = "player2";
            player2.Size = new Size(0, 25);
            player2.TabIndex = 1;
            // 
            // player1
            // 
            player1.AutoSize = true;
            player1.Location = new Point(16, 55);
            player1.Name = "player1";
            player1.Size = new Size(0, 25);
            player1.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel2);
            Controls.Add(Panel);
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
            Panel.ResumeLayout(false);
            Panel.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
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
        private Panel Panel;
        private Label label3;
        private Label label2;
        private TextBox passtxtpanel;
        private TextBox nicktxtpanel;
        private Button SignInPanel;
        private Button CancelPanel;
        private Button LogInPanel;
        private Panel panel2;
        private Label player4;
        private Label player3;
        private Label player2;
        private Label player1;
        private Label label4;
    }
}