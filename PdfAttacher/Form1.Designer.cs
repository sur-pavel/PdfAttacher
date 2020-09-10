namespace PdfAttacher
{
    partial class Form1
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
            this.LoginLabel = new System.Windows.Forms.Label();
            this.LoginTextBox = new System.Windows.Forms.TextBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.DatabaseTextBox = new System.Windows.Forms.TextBox();
            this.DatabaseLabel = new System.Windows.Forms.Label();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.AttachButton = new System.Windows.Forms.Button();
            this.PathFromTextBox = new System.Windows.Forms.TextBox();
            this.PathFromLabel = new System.Windows.Forms.Label();
            this.PathToTextBox = new System.Windows.Forms.TextBox();
            this.PathToLabel = new System.Windows.Forms.Label();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.PathFromButton = new System.Windows.Forms.Button();
            this.PathToButton = new System.Windows.Forms.Button();
            this.DisconnectButton = new System.Windows.Forms.Button();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.LogTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // LoginLabel
            // 
            this.LoginLabel.AutoSize = true;
            this.LoginLabel.Location = new System.Drawing.Point(13, 22);
            this.LoginLabel.Name = "LoginLabel";
            this.LoginLabel.Size = new System.Drawing.Size(38, 13);
            this.LoginLabel.TabIndex = 0;
            this.LoginLabel.Text = "Логин";
            // 
            // LoginTextBox
            // 
            this.LoginTextBox.Location = new System.Drawing.Point(113, 22);
            this.LoginTextBox.Name = "LoginTextBox";
            this.LoginTextBox.Size = new System.Drawing.Size(100, 20);
            this.LoginTextBox.TabIndex = 1;
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Location = new System.Drawing.Point(112, 59);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(100, 20);
            this.PasswordTextBox.TabIndex = 3;
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Location = new System.Drawing.Point(12, 59);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(45, 13);
            this.PasswordLabel.TabIndex = 2;
            this.PasswordLabel.Text = "Пароль";
            // 
            // DatabaseTextBox
            // 
            this.DatabaseTextBox.Location = new System.Drawing.Point(112, 100);
            this.DatabaseTextBox.Name = "DatabaseTextBox";
            this.DatabaseTextBox.Size = new System.Drawing.Size(100, 20);
            this.DatabaseTextBox.TabIndex = 5;
            this.DatabaseTextBox.Text = "MPDA";
            // 
            // DatabaseLabel
            // 
            this.DatabaseLabel.AutoSize = true;
            this.DatabaseLabel.Location = new System.Drawing.Point(12, 100);
            this.DatabaseLabel.Name = "DatabaseLabel";
            this.DatabaseLabel.Size = new System.Drawing.Size(72, 13);
            this.DatabaseLabel.TabIndex = 4;
            this.DatabaseLabel.Text = "База данных";
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(112, 139);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(75, 23);
            this.ConnectButton.TabIndex = 6;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // AttachButton
            // 
            this.AttachButton.Location = new System.Drawing.Point(285, 258);
            this.AttachButton.Name = "AttachButton";
            this.AttachButton.Size = new System.Drawing.Size(85, 23);
            this.AttachButton.TabIndex = 7;
            this.AttachButton.Text = "Прикрепить";
            this.AttachButton.UseVisualStyleBackColor = true;
            this.AttachButton.Click += new System.EventHandler(this.AttachButton_Click);
            // 
            // PathFromTextBox
            // 
            this.PathFromTextBox.Location = new System.Drawing.Point(113, 179);
            this.PathFromTextBox.Name = "PathFromTextBox";
            this.PathFromTextBox.Size = new System.Drawing.Size(409, 20);
            this.PathFromTextBox.TabIndex = 10;
            // 
            // PathFromLabel
            // 
            this.PathFromLabel.AutoSize = true;
            this.PathFromLabel.Cursor = System.Windows.Forms.Cursors.Default;
            this.PathFromLabel.Location = new System.Drawing.Point(13, 179);
            this.PathFromLabel.Name = "PathFromLabel";
            this.PathFromLabel.Size = new System.Drawing.Size(21, 13);
            this.PathFromLabel.TabIndex = 9;
            this.PathFromLabel.Text = "Из";
            // 
            // PathToTextBox
            // 
            this.PathToTextBox.Location = new System.Drawing.Point(113, 216);
            this.PathToTextBox.Name = "PathToTextBox";
            this.PathToTextBox.Size = new System.Drawing.Size(409, 20);
            this.PathToTextBox.TabIndex = 12;
            // 
            // PathToLabel
            // 
            this.PathToLabel.AutoSize = true;
            this.PathToLabel.Location = new System.Drawing.Point(13, 216);
            this.PathToLabel.Name = "PathToLabel";
            this.PathToLabel.Size = new System.Drawing.Size(14, 13);
            this.PathToLabel.TabIndex = 11;
            this.PathToLabel.Text = "В";
            // 
            // InfoLabel
            // 
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.Location = new System.Drawing.Point(335, 144);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(0, 13);
            this.InfoLabel.TabIndex = 13;
            // 
            // PathFromButton
            // 
            this.PathFromButton.Location = new System.Drawing.Point(550, 179);
            this.PathFromButton.Name = "PathFromButton";
            this.PathFromButton.Size = new System.Drawing.Size(75, 23);
            this.PathFromButton.TabIndex = 14;
            this.PathFromButton.Text = "Открыть";
            this.PathFromButton.UseVisualStyleBackColor = true;
            this.PathFromButton.Click += new System.EventHandler(this.PathFromButton_Click);
            // 
            // PathToButton
            // 
            this.PathToButton.Location = new System.Drawing.Point(550, 216);
            this.PathToButton.Name = "PathToButton";
            this.PathToButton.Size = new System.Drawing.Size(75, 23);
            this.PathToButton.TabIndex = 15;
            this.PathToButton.Text = "Открыть";
            this.PathToButton.UseVisualStyleBackColor = true;
            this.PathToButton.Click += new System.EventHandler(this.PathToButton_Click);
            // 
            // DisconnectButton
            // 
            this.DisconnectButton.Location = new System.Drawing.Point(220, 139);
            this.DisconnectButton.Name = "DisconnectButton";
            this.DisconnectButton.Size = new System.Drawing.Size(75, 23);
            this.DisconnectButton.TabIndex = 16;
            this.DisconnectButton.Text = "Disconnect";
            this.DisconnectButton.UseVisualStyleBackColor = true;
            this.DisconnectButton.Click += new System.EventHandler(this.DisconnectButton_Click);
            // 
            // ProgressBar
            // 
            this.ProgressBar.Location = new System.Drawing.Point(50, 298);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(983, 23);
            this.ProgressBar.TabIndex = 17;
            // 
            // LogTextBox
            // 
            this.LogTextBox.Location = new System.Drawing.Point(50, 337);
            this.LogTextBox.Multiline = true;
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LogTextBox.Size = new System.Drawing.Size(983, 275);
            this.LogTextBox.TabIndex = 18;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1082, 624);
            this.Controls.Add(this.LogTextBox);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.DisconnectButton);
            this.Controls.Add(this.PathToButton);
            this.Controls.Add(this.PathFromButton);
            this.Controls.Add(this.InfoLabel);
            this.Controls.Add(this.PathToTextBox);
            this.Controls.Add(this.PathToLabel);
            this.Controls.Add(this.PathFromTextBox);
            this.Controls.Add(this.PathFromLabel);
            this.Controls.Add(this.AttachButton);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.DatabaseTextBox);
            this.Controls.Add(this.DatabaseLabel);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.LoginTextBox);
            this.Controls.Add(this.LoginLabel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LoginLabel;
        private System.Windows.Forms.TextBox LoginTextBox;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.TextBox DatabaseTextBox;
        private System.Windows.Forms.Label DatabaseLabel;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Button AttachButton;
        private System.Windows.Forms.TextBox PathFromTextBox;
        private System.Windows.Forms.Label PathFromLabel;
        private System.Windows.Forms.TextBox PathToTextBox;
        private System.Windows.Forms.Label PathToLabel;
        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.Button PathFromButton;
        private System.Windows.Forms.Button PathToButton;
        private System.Windows.Forms.Button DisconnectButton;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.TextBox LogTextBox;
    }
}

