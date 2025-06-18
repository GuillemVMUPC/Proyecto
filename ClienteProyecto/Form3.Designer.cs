namespace ClienteProyecto
{
    partial class Form3
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
            PedirCarta = new Button();
            PararButton = new Button();
            txtlblmes = new Label();
            cartaslbl = new Label();
            totallbl = new Label();
            cerrarButt = new Button();
            CartasCrup = new Label();
            totalCrup = new Label();
            SuspendLayout();
            // 
            // PedirCarta
            // 
            PedirCarta.Location = new Point(55, 321);
            PedirCarta.Name = "PedirCarta";
            PedirCarta.Size = new Size(112, 34);
            PedirCarta.TabIndex = 0;
            PedirCarta.Text = "Pedir";
            PedirCarta.UseVisualStyleBackColor = true;
            PedirCarta.Click += PedirCarta_Click;
            // 
            // PararButton
            // 
            PararButton.Location = new Point(55, 372);
            PararButton.Name = "PararButton";
            PararButton.Size = new Size(112, 34);
            PararButton.TabIndex = 1;
            PararButton.Text = "Parar";
            PararButton.UseVisualStyleBackColor = true;
            PararButton.Click += PararButton_Click;
            // 
            // txtlblmes
            // 
            txtlblmes.AutoSize = true;
            txtlblmes.Location = new Point(401, 122);
            txtlblmes.Name = "txtlblmes";
            txtlblmes.Size = new Size(0, 25);
            txtlblmes.TabIndex = 2;
            txtlblmes.Visible = false;
            // 
            // cartaslbl
            // 
            cartaslbl.AutoSize = true;
            cartaslbl.Location = new Point(133, 208);
            cartaslbl.Name = "cartaslbl";
            cartaslbl.Size = new Size(61, 25);
            cartaslbl.TabIndex = 3;
            cartaslbl.Text = "Cartas";
            // 
            // totallbl
            // 
            totallbl.AutoSize = true;
            totallbl.Location = new Point(290, 252);
            totallbl.Name = "totallbl";
            totallbl.Size = new Size(49, 25);
            totallbl.TabIndex = 4;
            totallbl.Text = "Total";
            // 
            // cerrarButt
            // 
            cerrarButt.Location = new Point(654, 372);
            cerrarButt.Name = "cerrarButt";
            cerrarButt.Size = new Size(112, 34);
            cerrarButt.TabIndex = 5;
            cerrarButt.Text = "Terminar";
            cerrarButt.UseVisualStyleBackColor = true;
            cerrarButt.Click += cerrarButt_Click;
            // 
            // CartasCrup
            // 
            CartasCrup.AutoSize = true;
            CartasCrup.Location = new Point(152, 33);
            CartasCrup.Name = "CartasCrup";
            CartasCrup.Size = new Size(123, 25);
            CartasCrup.TabIndex = 6;
            CartasCrup.Text = "Cartas Crupier";
            // 
            // totalCrup
            // 
            totalCrup.AutoSize = true;
            totalCrup.Location = new Point(290, 85);
            totalCrup.Name = "totalCrup";
            totalCrup.Size = new Size(111, 25);
            totalCrup.TabIndex = 7;
            totalCrup.Text = "Total Crupier";
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(totalCrup);
            Controls.Add(CartasCrup);
            Controls.Add(cerrarButt);
            Controls.Add(totallbl);
            Controls.Add(cartaslbl);
            Controls.Add(txtlblmes);
            Controls.Add(PararButton);
            Controls.Add(PedirCarta);
            Name = "Form3";
            Text = "Form3";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button PedirCarta;
        private Button PararButton;
        private Label txtlblmes;
        private Label cartaslbl;
        private Label totallbl;
        private Button cerrarButt;
        private Label CartasCrup;
        private Label totalCrup;
    }
}