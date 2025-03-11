namespace ClienteProyecto
{
    partial class LogInInterf
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
            Cancelar = new Button();
            Enviar = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            nickBox = new TextBox();
            passBox = new TextBox();
            SuspendLayout();
            // 
            // Cancelar
            // 
            Cancelar.Location = new Point(12, 280);
            Cancelar.Name = "Cancelar";
            Cancelar.Size = new Size(112, 34);
            Cancelar.TabIndex = 0;
            Cancelar.Text = "Cancelar";
            Cancelar.UseVisualStyleBackColor = true;
            Cancelar.Click += Cancelar_Click;
            // 
            // Enviar
            // 
            Enviar.Location = new Point(383, 280);
            Enviar.Name = "Enviar";
            Enviar.Size = new Size(112, 34);
            Enviar.TabIndex = 1;
            Enviar.Text = "Enviar";
            Enviar.UseVisualStyleBackColor = true;
            Enviar.Click += Enviar_Click_1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(219, 35);
            label1.Name = "label1";
            label1.Size = new Size(62, 25);
            label1.TabIndex = 2;
            label1.Text = "Log In";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(92, 197);
            label2.Name = "label2";
            label2.Size = new Size(91, 25);
            label2.TabIndex = 3;
            label2.Text = "Password:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(92, 106);
            label3.Name = "label3";
            label3.Size = new Size(94, 25);
            label3.TabIndex = 4;
            label3.Text = "Nickname:";
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
            // LogInInterf
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(507, 326);
            Controls.Add(passBox);
            Controls.Add(nickBox);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(Enviar);
            Controls.Add(Cancelar);
            Name = "LogInInterf";
            Text = "LogInInterfcs";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Cancelar;
        private Button Enviar;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox nickBox;
        private TextBox passBox;
    }
}