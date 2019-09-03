namespace HostComputer.UI
{
    partial class UARTConfig
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
            this.PortLabel = new CCWin.SkinControl.SkinLabel();
            this.PortComboBox = new CCWin.SkinControl.SkinComboBox();
            this.BaudComboBox = new CCWin.SkinControl.SkinComboBox();
            this.BaudLabel = new CCWin.SkinControl.SkinLabel();
            this.SuspendLayout();
            // 
            // PortLabel
            // 
            this.PortLabel.AutoSize = true;
            this.PortLabel.BackColor = System.Drawing.Color.Transparent;
            this.PortLabel.BorderColor = System.Drawing.Color.White;
            this.PortLabel.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.PortLabel.Location = new System.Drawing.Point(7, 45);
            this.PortLabel.Name = "PortLabel";
            this.PortLabel.Size = new System.Drawing.Size(72, 27);
            this.PortLabel.TabIndex = 0;
            this.PortLabel.Text = "端口号";
            // 
            // PortComboBox
            // 
            this.PortComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.PortComboBox.Font = new System.Drawing.Font("宋体", 12F);
            this.PortComboBox.FormattingEnabled = true;
            this.PortComboBox.Location = new System.Drawing.Point(85, 41);
            this.PortComboBox.Name = "PortComboBox";
            this.PortComboBox.Size = new System.Drawing.Size(121, 31);
            this.PortComboBox.TabIndex = 1;
            this.PortComboBox.WaterText = "";
            // 
            // BuadComboBox
            // 
            this.BaudComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.BaudComboBox.Font = new System.Drawing.Font("宋体", 12F);
            this.BaudComboBox.FormattingEnabled = true;
            this.BaudComboBox.Items.AddRange(new object[] {
            "256000",
            "128000",
            "115200",
            "57600",
            "56000",
            "38400",
            "19200",
            "14400",
            "9600",
            "4800",
            "2400"});
            this.BaudComboBox.Location = new System.Drawing.Point(85, 85);
            this.BaudComboBox.Name = "BuadComboBox";
            this.BaudComboBox.Size = new System.Drawing.Size(121, 31);
            this.BaudComboBox.TabIndex = 3;
            this.BaudComboBox.WaterText = "";
            // 
            // BaudLabel
            // 
            this.BaudLabel.AutoSize = true;
            this.BaudLabel.BackColor = System.Drawing.Color.Transparent;
            this.BaudLabel.BorderColor = System.Drawing.Color.White;
            this.BaudLabel.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.BaudLabel.Location = new System.Drawing.Point(7, 89);
            this.BaudLabel.Name = "BaudLabel";
            this.BaudLabel.Size = new System.Drawing.Size(72, 27);
            this.BaudLabel.TabIndex = 2;
            this.BaudLabel.Text = "波特率";
            this.BaudLabel.Click += new System.EventHandler(this.BaudLabel_Click);
            // 
            // UARTConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CaptionFont = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ClientSize = new System.Drawing.Size(666, 503);
            this.Controls.Add(this.BaudComboBox);
            this.Controls.Add(this.BaudLabel);
            this.Controls.Add(this.PortComboBox);
            this.Controls.Add(this.PortLabel);
            this.Name = "UARTConfig";
            this.Text = "串口配置界面";
            this.Load += new System.EventHandler(this.UARTConfig_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CCWin.SkinControl.SkinLabel PortLabel;
        private CCWin.SkinControl.SkinComboBox PortComboBox;
        private CCWin.SkinControl.SkinComboBox BaudComboBox;
        private CCWin.SkinControl.SkinLabel BaudLabel;
    }
}