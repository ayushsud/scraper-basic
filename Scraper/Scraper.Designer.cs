namespace Scraper
{
    partial class Scraper
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
            this.choose = new System.Windows.Forms.Button();
            this.Execute = new System.Windows.Forms.Button();
            this.output = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.contact = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // choose
            // 
            this.choose.Location = new System.Drawing.Point(237, 15);
            this.choose.Name = "choose";
            this.choose.Size = new System.Drawing.Size(75, 23);
            this.choose.TabIndex = 0;
            this.choose.Text = "Choose File";
            this.choose.UseVisualStyleBackColor = true;
            this.choose.Click += new System.EventHandler(this.choose_Click);
            // 
            // Execute
            // 
            this.Execute.Location = new System.Drawing.Point(119, 135);
            this.Execute.Name = "Execute";
            this.Execute.Size = new System.Drawing.Size(75, 23);
            this.Execute.TabIndex = 1;
            this.Execute.Text = "Execute";
            this.Execute.UseVisualStyleBackColor = true;
            this.Execute.Click += new System.EventHandler(this.Execute_Click);
            // 
            // output
            // 
            this.output.Location = new System.Drawing.Point(237, 57);
            this.output.Name = "output";
            this.output.Size = new System.Drawing.Size(75, 23);
            this.output.TabIndex = 2;
            this.output.Text = "Output";
            this.output.UseVisualStyleBackColor = true;
            this.output.Click += new System.EventHandler(this.output_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(12, 59);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(182, 20);
            this.textBox2.TabIndex = 4;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 15);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(182, 20);
            this.textBox1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(94, 140);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Working! Please Wait.";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(13, 102);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(181, 20);
            this.textBox3.TabIndex = 6;
            // 
            // contact
            // 
            this.contact.Location = new System.Drawing.Point(237, 98);
            this.contact.Name = "contact";
            this.contact.Size = new System.Drawing.Size(75, 23);
            this.contact.TabIndex = 7;
            this.contact.Text = "Contact Info";
            this.contact.UseVisualStyleBackColor = true;
            this.contact.Click += new System.EventHandler(this.contact_Click);
            // 
            // Scraper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 170);
            this.Controls.Add(this.contact);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.output);
            this.Controls.Add(this.Execute);
            this.Controls.Add(this.choose);
            this.Name = "Scraper";
            this.Text = "Scraper";
            this.Load += new System.EventHandler(this.Scraper_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button choose;
        private System.Windows.Forms.Button Execute;
        private System.Windows.Forms.Button output;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button contact;
    }
}

