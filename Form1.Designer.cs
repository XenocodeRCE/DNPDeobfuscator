namespace DNPD
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
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.chk_antidump = new System.Windows.Forms.CheckBox();
            this.chk_antidebug = new System.Windows.Forms.CheckBox();
            this.chk_antitamp = new System.Windows.Forms.CheckBox();
            this.chk_str = new System.Windows.Forms.CheckBox();
            this.chk_Integers = new System.Windows.Forms.CheckBox();
            this.chk_prune = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 80);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.AllowDrop = true;
            this.textBox1.Location = new System.Drawing.Point(12, 40);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(410, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.TextBox1DragDrop);
            this.textBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.TextBox1DragEnter);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(171, 80);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(84, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Deobfuscate";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Your Assembly :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label2.Location = new System.Drawing.Point(12, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 4;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(347, 80);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Exit";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // chk_antidump
            // 
            this.chk_antidump.AutoSize = true;
            this.chk_antidump.Checked = true;
            this.chk_antidump.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_antidump.Location = new System.Drawing.Point(12, 109);
            this.chk_antidump.Name = "chk_antidump";
            this.chk_antidump.Size = new System.Drawing.Size(70, 17);
            this.chk_antidump.TabIndex = 6;
            this.chk_antidump.Text = "Antidump";
            this.chk_antidump.UseVisualStyleBackColor = true;
            // 
            // chk_antidebug
            // 
            this.chk_antidebug.AutoSize = true;
            this.chk_antidebug.Checked = true;
            this.chk_antidebug.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_antidebug.Location = new System.Drawing.Point(88, 109);
            this.chk_antidebug.Name = "chk_antidebug";
            this.chk_antidebug.Size = new System.Drawing.Size(74, 17);
            this.chk_antidebug.TabIndex = 7;
            this.chk_antidebug.Text = "Antidebug";
            this.chk_antidebug.UseVisualStyleBackColor = true;
            // 
            // chk_antitamp
            // 
            this.chk_antitamp.AutoSize = true;
            this.chk_antitamp.Checked = true;
            this.chk_antitamp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_antitamp.Location = new System.Drawing.Point(168, 109);
            this.chk_antitamp.Name = "chk_antitamp";
            this.chk_antitamp.Size = new System.Drawing.Size(67, 17);
            this.chk_antitamp.TabIndex = 8;
            this.chk_antitamp.Text = "Antitamp";
            this.chk_antitamp.UseVisualStyleBackColor = true;
            // 
            // chk_str
            // 
            this.chk_str.AutoSize = true;
            this.chk_str.Checked = true;
            this.chk_str.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_str.Location = new System.Drawing.Point(12, 129);
            this.chk_str.Name = "chk_str";
            this.chk_str.Size = new System.Drawing.Size(98, 17);
            this.chk_str.TabIndex = 9;
            this.chk_str.Text = "Decrypt Strings";
            this.chk_str.UseVisualStyleBackColor = true;
            // 
            // chk_Integers
            // 
            this.chk_Integers.AutoSize = true;
            this.chk_Integers.Checked = true;
            this.chk_Integers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_Integers.Location = new System.Drawing.Point(116, 129);
            this.chk_Integers.Name = "chk_Integers";
            this.chk_Integers.Size = new System.Drawing.Size(104, 17);
            this.chk_Integers.TabIndex = 10;
            this.chk_Integers.Text = "Decrypt Integers";
            this.chk_Integers.UseVisualStyleBackColor = true;
            // 
            // chk_prune
            // 
            this.chk_prune.AutoSize = true;
            this.chk_prune.Checked = true;
            this.chk_prune.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_prune.Location = new System.Drawing.Point(226, 129);
            this.chk_prune.Name = "chk_prune";
            this.chk_prune.Size = new System.Drawing.Size(101, 17);
            this.chk_prune.TabIndex = 11;
            this.chk_prune.Text = "Prune Assembly";
            this.chk_prune.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 151);
            this.Controls.Add(this.chk_prune);
            this.Controls.Add(this.chk_Integers);
            this.Controls.Add(this.chk_str);
            this.Controls.Add(this.chk_antitamp);
            this.Controls.Add(this.chk_antidebug);
            this.Controls.Add(this.chk_antidump);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DNPDeobfuscator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox chk_antidump;
        private System.Windows.Forms.CheckBox chk_antidebug;
        private System.Windows.Forms.CheckBox chk_antitamp;
        private System.Windows.Forms.CheckBox chk_str;
        private System.Windows.Forms.CheckBox chk_Integers;
        private System.Windows.Forms.CheckBox chk_prune;
    }
}

