namespace ClienteProyecto
{
    partial class SignUpInterf
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Registrar = new Button();
            Cancelar = new Button();
            label1 = new Label();
            Nickname = new Label();
            label3 = new Label();
            nickBox = new TextBox();
            passBox = new TextBox();
            SuspendLayout();
            // 
            // Registrar
            // 
            Registrar.Location = new Point(383, 280);
            Registrar.Name = "Registrar";
            Registrar.Size = new Size(112, 34);
            Registrar.TabIndex = 0;
            Registrar.Text = "Registrar";
            Registrar.UseVisualStyleBackColor = true;
            Registrar.Click += Registrar_Click;
            // 
            // Cancelar
            // 
            Cancelar.Location = new Point(12, 280);
            Cancelar.Name = "Cancelar";
            Cancelar.Size = new Size(112, 34);
            Cancelar.TabIndex = 1;
            Cancelar.Text = "Cancelar";
            Cancelar.UseVisualStyleBackColor = true;
            Cancelar.Click += Cancelar_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(219, 35);
            label1.Name = "label1";
            label1.Size = new Size(75, 25);
            label1.TabIndex = 2;
            label1.Text = "Sign Up";
            // 
            // Nickname
            // 
            Nickname.AutoSize = true;
            Nickname.Location = new Point(92, 106);
            Nickname.Name = "Nickname";
            Nickname.Size = new Size(94, 25);
            Nickname.TabIndex = 3;
            Nickname.Text = "Nickname:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(92, 197);
            label3.Name = "label3";
            label3.Size = new Size(91, 25);
            label3.TabIndex = 4;
            label3.Text = "Password:";
            // 
            // nickBox
            // 
            nickBox.Location = new Point(182, 106);
            nickBox.Name = "nickBox";
            nickBox.Size = new Size(150, 31);
            nickBox.TabIndex = 5;
            // 
            // passBox
            // 
            passBox.Location = new Point(182, 194);
            passBox.Name = "passBox";
            passBox.Size = new Size(150, 31);
            passBox.TabIndex = 6;
            // 
            // SignUpInterf
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(507, 326);
            Controls.Add(passBox);
            Controls.Add(nickBox);
            Controls.Add(label3);
            Controls.Add(Nickname);
            Controls.Add(label1);
            Controls.Add(Cancelar);
            Controls.Add(Registrar);
            Name = "SignUpInterf";
            Text = "SignUpInterf";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Registrar;
        private Button Cancelar;
        private Label label1;
        private Label Nickname;
        private Label label3;
        private TextBox nickBox;
        private TextBox passBox;
    }
}