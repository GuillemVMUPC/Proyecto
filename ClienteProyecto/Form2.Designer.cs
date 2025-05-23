namespace ClienteProyecto
{
    partial class Form2
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
            txtRead = new TextBox();
            txtSend = new TextBox();
            sendButt = new Button();
            comboUsuChat = new ComboBox();
            CancelPanel = new Button();
            swButt = new Button();
            SuspendLayout();
            // 
            // txtRead
            // 
            txtRead.Location = new Point(227, 12);
            txtRead.Multiline = true;
            txtRead.Name = "txtRead";
            txtRead.ReadOnly = true;
            txtRead.ScrollBars = ScrollBars.Vertical;
            txtRead.Size = new Size(535, 349);
            txtRead.TabIndex = 0;
            // 
            // txtSend
            // 
            txtSend.Location = new Point(227, 376);
            txtSend.Name = "txtSend";
            txtSend.Size = new Size(375, 31);
            txtSend.TabIndex = 1;
            // 
            // sendButt
            // 
            sendButt.Location = new Point(621, 376);
            sendButt.Name = "sendButt";
            sendButt.Size = new Size(112, 34);
            sendButt.TabIndex = 2;
            sendButt.Text = "Send";
            sendButt.UseVisualStyleBackColor = true;
            sendButt.Click += sendButt_Click;
            // 
            // comboUsuChat
            // 
            comboUsuChat.FormattingEnabled = true;
            comboUsuChat.Location = new Point(12, 57);
            comboUsuChat.Name = "comboUsuChat";
            comboUsuChat.Size = new Size(182, 33);
            comboUsuChat.TabIndex = 4;
            // 
            // CancelPanel
            // 
            CancelPanel.BackColor = Color.Red;
            CancelPanel.FlatStyle = FlatStyle.Flat;
            CancelPanel.ForeColor = Color.White;
            CancelPanel.Location = new Point(25, 376);
            CancelPanel.Name = "CancelPanel";
            CancelPanel.Size = new Size(112, 34);
            CancelPanel.TabIndex = 5;
            CancelPanel.Text = "Cancel";
            CancelPanel.UseVisualStyleBackColor = false;
            CancelPanel.Click += CancelPanel_Click;
            // 
            // swButt
            // 
            swButt.Location = new Point(47, 96);
            swButt.Name = "swButt";
            swButt.Size = new Size(112, 34);
            swButt.TabIndex = 6;
            swButt.Text = "Switch";
            swButt.UseVisualStyleBackColor = true;
            swButt.Click += swButt_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 420);
            Controls.Add(swButt);
            Controls.Add(CancelPanel);
            Controls.Add(comboUsuChat);
            Controls.Add(sendButt);
            Controls.Add(txtSend);
            Controls.Add(txtRead);
            Name = "Form2";
            Text = "Form2";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtRead;
        private TextBox txtSend;
        private Button sendButt;
        private ComboBox comboUsuChat;
        private Button CancelPanel;
        private Button swButt;
    }
}