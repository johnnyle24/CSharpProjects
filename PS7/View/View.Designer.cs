namespace View
{
    partial class View
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
            this.components = new System.ComponentModel.Container();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.PlayerNameBox = new System.Windows.Forms.TextBox();
            this.PlayerNameLabel = new System.Windows.Forms.Label();
            this.ServerTextBox = new System.Windows.Forms.TextBox();
            this.ServerLabel = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Length = new System.Windows.Forms.Label();
            this.Mass = new System.Windows.Forms.Label();
            this.Food = new System.Windows.Forms.Label();
            this.FPS = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.Location = new System.Drawing.Point(329, 72);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(135, 36);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "AgCubio";
            // 
            // PlayerNameBox
            // 
            this.PlayerNameBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayerNameBox.Location = new System.Drawing.Point(238, 157);
            this.PlayerNameBox.Name = "PlayerNameBox";
            this.PlayerNameBox.Size = new System.Drawing.Size(280, 44);
            this.PlayerNameBox.TabIndex = 1;
            this.PlayerNameBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.enterPressed);
            // 
            // PlayerNameLabel
            // 
            this.PlayerNameLabel.AutoSize = true;
            this.PlayerNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayerNameLabel.Location = new System.Drawing.Point(62, 163);
            this.PlayerNameLabel.Name = "PlayerNameLabel";
            this.PlayerNameLabel.Size = new System.Drawing.Size(170, 31);
            this.PlayerNameLabel.TabIndex = 2;
            this.PlayerNameLabel.Text = "Player Name";
            // 
            // ServerTextBox
            // 
            this.ServerTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerTextBox.Location = new System.Drawing.Point(238, 207);
            this.ServerTextBox.Name = "ServerTextBox";
            this.ServerTextBox.Size = new System.Drawing.Size(280, 44);
            this.ServerTextBox.TabIndex = 3;
            this.ServerTextBox.Text = "localhost";
            this.ServerTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.enterPressed);
            // 
            // ServerLabel
            // 
            this.ServerLabel.AutoSize = true;
            this.ServerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerLabel.Location = new System.Drawing.Point(138, 213);
            this.ServerLabel.Name = "ServerLabel";
            this.ServerLabel.Size = new System.Drawing.Size(94, 31);
            this.ServerLabel.TabIndex = 4;
            this.ServerLabel.Text = "Server";
            // 
            // Length
            // 
            this.Length.AutoSize = true;
            this.Length.Location = new System.Drawing.Point(880, 72);
            this.Length.Name = "Length";
            this.Length.Size = new System.Drawing.Size(40, 13);
            this.Length.TabIndex = 5;
            this.Length.Text = "Length";
            // 
            // Mass
            // 
            this.Mass.AutoSize = true;
            this.Mass.Location = new System.Drawing.Point(880, 47);
            this.Mass.Name = "Mass";
            this.Mass.Size = new System.Drawing.Size(32, 13);
            this.Mass.TabIndex = 6;
            this.Mass.Text = "Mass";
            // 
            // Food
            // 
            this.Food.AutoSize = true;
            this.Food.Location = new System.Drawing.Point(881, 95);
            this.Food.Name = "Food";
            this.Food.Size = new System.Drawing.Size(31, 13);
            this.Food.TabIndex = 7;
            this.Food.Text = "Food";
            // 
            // FPS
            // 
            this.FPS.AutoSize = true;
            this.FPS.Location = new System.Drawing.Point(880, 20);
            this.FPS.Name = "FPS";
            this.FPS.Size = new System.Drawing.Size(27, 13);
            this.FPS.TabIndex = 8;
            this.FPS.Text = "FPS";
            // 
            // View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 962);
            this.Controls.Add(this.FPS);
            this.Controls.Add(this.Food);
            this.Controls.Add(this.Mass);
            this.Controls.Add(this.Length);
            this.Controls.Add(this.ServerLabel);
            this.Controls.Add(this.ServerTextBox);
            this.Controls.Add(this.PlayerNameLabel);
            this.Controls.Add(this.PlayerNameBox);
            this.Controls.Add(this.TitleLabel);
            this.Name = "View";
            this.Text = "Form1";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.enterPressed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.TextBox PlayerNameBox;
        private System.Windows.Forms.Label PlayerNameLabel;
        private System.Windows.Forms.TextBox ServerTextBox;
        private System.Windows.Forms.Label ServerLabel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label Length;
        private System.Windows.Forms.Label Mass;
        private System.Windows.Forms.Label Food;
        private System.Windows.Forms.Label FPS;
    }
}

