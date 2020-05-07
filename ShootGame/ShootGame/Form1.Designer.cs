namespace ShootGame
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Button2 = new System.Windows.Forms.Button();
            this.Score = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.TextBox4 = new System.Windows.Forms.TextBox();
            this.ListBox1 = new System.Windows.Forms.ListBox();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.Label4 = new System.Windows.Forms.Label();
            this.Button1 = new System.Windows.Forms.Button();
            this.TextBox3 = new System.Windows.Forms.TextBox();
            this.TextBox2 = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.TextBox1 = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Timer2 = new System.Windows.Forms.Timer(this.components);
            this.Panel1 = new System.Windows.Forms.Panel();
            this.Q = new System.Windows.Forms.PictureBox();
            this.P = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Q)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Button2
            // 
            this.Button2.Location = new System.Drawing.Point(152, 380);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(23, 23);
            this.Button2.TabIndex = 157;
            this.Button2.UseVisualStyleBackColor = true;
            // 
            // Score
            // 
            this.Score.AutoSize = true;
            this.Score.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Score.Location = new System.Drawing.Point(84, 380);
            this.Score.Name = "Score";
            this.Score.Size = new System.Drawing.Size(11, 12);
            this.Score.TabIndex = 156;
            this.Score.Text = "0";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Label5.Location = new System.Drawing.Point(36, 326);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(53, 12);
            this.Label5.TabIndex = 155;
            this.Label5.Text = "系統訊息";
            // 
            // TextBox4
            // 
            this.TextBox4.Enabled = false;
            this.TextBox4.Location = new System.Drawing.Point(38, 341);
            this.TextBox4.Name = "TextBox4";
            this.TextBox4.Size = new System.Drawing.Size(116, 22);
            this.TextBox4.TabIndex = 154;
            // 
            // ListBox1
            // 
            this.ListBox1.FormattingEnabled = true;
            this.ListBox1.ItemHeight = 12;
            this.ListBox1.Location = new System.Drawing.Point(43, 212);
            this.ListBox1.Name = "ListBox1";
            this.ListBox1.Size = new System.Drawing.Size(116, 100);
            this.ListBox1.TabIndex = 153;
            this.ListBox1.SelectedIndexChanged += new System.EventHandler(this.ListBox1_SelectedIndexChanged);
            // 
            // Timer1
            // 
            this.Timer1.Enabled = true;
            this.Timer1.Interval = 25;
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Label4.Location = new System.Drawing.Point(39, 197);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(65, 12);
            this.Label4.TabIndex = 152;
            this.Label4.Text = "線上使用者";
            // 
            // Button1
            // 
            this.Button1.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Button1.Location = new System.Drawing.Point(38, 153);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(121, 31);
            this.Button1.TabIndex = 151;
            this.Button1.Text = "登入伺服器";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // TextBox3
            // 
            this.TextBox3.Location = new System.Drawing.Point(43, 125);
            this.TextBox3.Name = "TextBox3";
            this.TextBox3.Size = new System.Drawing.Size(115, 22);
            this.TextBox3.TabIndex = 150;
            // 
            // TextBox2
            // 
            this.TextBox2.Location = new System.Drawing.Point(43, 75);
            this.TextBox2.Name = "TextBox2";
            this.TextBox2.Size = new System.Drawing.Size(116, 22);
            this.TextBox2.TabIndex = 148;
            this.TextBox2.Text = "2013";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Label2.Location = new System.Drawing.Point(41, 60);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(72, 12);
            this.Label2.TabIndex = 147;
            this.Label2.Text = "伺服器Port：";
            // 
            // TextBox1
            // 
            this.TextBox1.Location = new System.Drawing.Point(43, 24);
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.Size = new System.Drawing.Size(116, 22);
            this.TextBox1.TabIndex = 146;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Label1.Location = new System.Drawing.Point(41, 9);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(63, 12);
            this.Label1.TabIndex = 145;
            this.Label1.Text = "伺服器IP：";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Label3.Location = new System.Drawing.Point(41, 110);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(65, 12);
            this.Label3.TabIndex = 149;
            this.Label3.Text = "玩家名稱：";
            // 
            // Timer2
            // 
            this.Timer2.Enabled = true;
            this.Timer2.Interval = 25;
            this.Timer2.Tick += new System.EventHandler(this.Timer2_Tick);
            // 
            // Panel1
            // 
            this.Panel1.BackColor = System.Drawing.Color.White;
            this.Panel1.Controls.Add(this.Q);
            this.Panel1.Controls.Add(this.P);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.Panel1.Enabled = false;
            this.Panel1.Location = new System.Drawing.Point(0, 0);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(400, 413);
            this.Panel1.TabIndex = 144;
            // 
            // Q
            // 
            this.Q.BackColor = System.Drawing.Color.Transparent;
            this.Q.Image = global::ShootGame.Properties.Resources.ShooterX;
            this.Q.Location = new System.Drawing.Point(180, 0);
            this.Q.Name = "Q";
            this.Q.Size = new System.Drawing.Size(43, 50);
            this.Q.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.Q.TabIndex = 2;
            this.Q.TabStop = false;
            this.Q.Tag = "Q";
            // 
            // P
            // 
            this.P.BackColor = System.Drawing.Color.Transparent;
            this.P.Image = global::ShootGame.Properties.Resources.Shooter;
            this.P.Location = new System.Drawing.Point(180, 362);
            this.P.Name = "P";
            this.P.Size = new System.Drawing.Size(43, 50);
            this.P.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.P.TabIndex = 1;
            this.P.TabStop = false;
            this.P.Tag = "P";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.Label1);
            this.panel2.Controls.Add(this.Button2);
            this.panel2.Controls.Add(this.Label3);
            this.panel2.Controls.Add(this.Score);
            this.panel2.Controls.Add(this.TextBox1);
            this.panel2.Controls.Add(this.Label5);
            this.panel2.Controls.Add(this.Label2);
            this.panel2.Controls.Add(this.TextBox4);
            this.panel2.Controls.Add(this.TextBox2);
            this.panel2.Controls.Add(this.ListBox1);
            this.panel2.Controls.Add(this.TextBox3);
            this.panel2.Controls.Add(this.Label4);
            this.panel2.Controls.Add(this.Button1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(400, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(189, 413);
            this.panel2.TabIndex = 158;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(36, 380);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 158;
            this.label6.Text = "得分";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 413);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.Panel1);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "線上射擊遊戲";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Q)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button Button2;
        internal System.Windows.Forms.Label Score;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.TextBox TextBox4;
        internal System.Windows.Forms.ListBox ListBox1;
        internal System.Windows.Forms.Timer Timer1;
        internal System.Windows.Forms.PictureBox Q;
        internal System.Windows.Forms.PictureBox P;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Button Button1;
        internal System.Windows.Forms.TextBox TextBox3;
        internal System.Windows.Forms.TextBox TextBox2;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox TextBox1;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Timer Timer2;
        internal System.Windows.Forms.Panel Panel1;
        private System.Windows.Forms.Panel panel2;
        internal System.Windows.Forms.Label label6;
    }
}
