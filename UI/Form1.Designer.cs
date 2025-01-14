namespace UI
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
            this.Conectare = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Welcome = new System.Windows.Forms.Label();
            this.Inchide = new System.Windows.Forms.Button();
            this.Email = new System.Windows.Forms.TextBox();
            this.Password = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Parola = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Conectare
            // 
            this.Conectare.Location = new System.Drawing.Point(12, 201);
            this.Conectare.Name = "Conectare";
            this.Conectare.Size = new System.Drawing.Size(216, 38);
            this.Conectare.TabIndex = 0;
            this.Conectare.Text = "Conectare";
            this.Conectare.UseVisualStyleBackColor = true;
            this.Conectare.Click += new System.EventHandler(this.Conectare_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(641, 169);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 20);
            this.label1.TabIndex = 1;
            // 
            // Welcome
            // 
            this.Welcome.AutoSize = true;
            this.Welcome.Location = new System.Drawing.Point(185, 34);
            this.Welcome.Name = "Welcome";
            this.Welcome.Size = new System.Drawing.Size(157, 20);
            this.Welcome.TabIndex = 2;
            this.Welcome.Text = "Bun venit in aplicatie!";
            this.Welcome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Welcome.Click += new System.EventHandler(this.Welcome_Click);
            // 
            // Inchide
            // 
            this.Inchide.Location = new System.Drawing.Point(262, 201);
            this.Inchide.Name = "Inchide";
            this.Inchide.Size = new System.Drawing.Size(239, 38);
            this.Inchide.TabIndex = 3;
            this.Inchide.Text = "Inchide";
            this.Inchide.UseVisualStyleBackColor = true;
            this.Inchide.Click += new System.EventHandler(this.Inchide_Click);
            // 
            // Email
            // 
            this.Email.Location = new System.Drawing.Point(145, 99);
            this.Email.Name = "Email";
            this.Email.Size = new System.Drawing.Size(338, 26);
            this.Email.TabIndex = 4;
            // 
            // Password
            // 
            this.Password.Location = new System.Drawing.Point(145, 149);
            this.Password.Name = "Password";
            this.Password.Size = new System.Drawing.Size(338, 26);
            this.Password.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(53, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Email";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // Parola
            // 
            this.Parola.AutoSize = true;
            this.Parola.Location = new System.Drawing.Point(53, 155);
            this.Parola.Name = "Parola";
            this.Parola.Size = new System.Drawing.Size(54, 20);
            this.Parola.TabIndex = 6;
            this.Parola.Text = "Parola";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 299);
            this.Controls.Add(this.Parola);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Password);
            this.Controls.Add(this.Email);
            this.Controls.Add(this.Inchide);
            this.Controls.Add(this.Welcome);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Conectare);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Conectare;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Welcome;
        private System.Windows.Forms.Button Inchide;
        private System.Windows.Forms.TextBox Email;
        private System.Windows.Forms.TextBox Password;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Parola;
    }
}

