namespace R4IDPLAY_application
{
    partial class form_login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(form_login));
            this.lab_username = new System.Windows.Forms.Label();
            this.lab_password = new System.Windows.Forms.Label();
            this.txt_username = new System.Windows.Forms.TextBox();
            this.txt_password = new System.Windows.Forms.TextBox();
            this.button_login = new System.Windows.Forms.Button();
            this.lab_R4ID = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lab_username
            // 
            this.lab_username.AutoSize = true;
            this.lab_username.Location = new System.Drawing.Point(170, 94);
            this.lab_username.Name = "lab_username";
            this.lab_username.Size = new System.Drawing.Size(83, 20);
            this.lab_username.TabIndex = 0;
            this.lab_username.Text = "Username";
            // 
            // lab_password
            // 
            this.lab_password.AutoSize = true;
            this.lab_password.Location = new System.Drawing.Point(170, 142);
            this.lab_password.Name = "lab_password";
            this.lab_password.Size = new System.Drawing.Size(78, 20);
            this.lab_password.TabIndex = 1;
            this.lab_password.Text = "Password\r\n";
            this.lab_password.Click += new System.EventHandler(this.label2_Click);
            // 
            // txt_username
            // 
            this.txt_username.Location = new System.Drawing.Point(334, 94);
            this.txt_username.Name = "txt_username";
            this.txt_username.Size = new System.Drawing.Size(253, 26);
            this.txt_username.TabIndex = 2;
            // 
            // txt_password
            // 
            this.txt_password.Location = new System.Drawing.Point(334, 142);
            this.txt_password.Name = "txt_password";
            this.txt_password.Size = new System.Drawing.Size(253, 26);
            this.txt_password.TabIndex = 3;
            // 
            // button_login
            // 
            this.button_login.Location = new System.Drawing.Point(334, 194);
            this.button_login.Name = "button_login";
            this.button_login.Size = new System.Drawing.Size(134, 40);
            this.button_login.TabIndex = 4;
            this.button_login.Text = "Login";
            this.button_login.UseVisualStyleBackColor = true;
            this.button_login.Click += new System.EventHandler(this.button_login_Click);
            // 
            // lab_R4ID
            // 
            this.lab_R4ID.AutoSize = true;
            this.lab_R4ID.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_R4ID.Location = new System.Drawing.Point(223, 22);
            this.lab_R4ID.Name = "lab_R4ID";
            this.lab_R4ID.Size = new System.Drawing.Size(293, 36);
            this.lab_R4ID.TabIndex = 5;
            this.lab_R4ID.Text = "R4IDPlay Cafe Login";
            // 
            // form_login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(724, 370);
            this.Controls.Add(this.lab_R4ID);
            this.Controls.Add(this.button_login);
            this.Controls.Add(this.txt_password);
            this.Controls.Add(this.txt_username);
            this.Controls.Add(this.lab_password);
            this.Controls.Add(this.lab_username);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "form_login";
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lab_username;
        private System.Windows.Forms.Label lab_password;
        private System.Windows.Forms.TextBox txt_username;
        private System.Windows.Forms.TextBox txt_password;
        private System.Windows.Forms.Button button_login;
        private System.Windows.Forms.Label lab_R4ID;
    }
}

