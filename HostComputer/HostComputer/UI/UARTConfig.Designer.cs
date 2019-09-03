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
            this.components = new System.ComponentModel.Container();
            this.PortLabel = new CCWin.SkinControl.SkinLabel();
            this.PortComboBox = new CCWin.SkinControl.SkinComboBox();
            this.BaudComboBox = new CCWin.SkinControl.SkinComboBox();
            this.BaudLabel = new CCWin.SkinControl.SkinLabel();
            this.DataBitComboBox = new CCWin.SkinControl.SkinComboBox();
            this.skinLabel1 = new CCWin.SkinControl.SkinLabel();
            this.StopBitComboBox = new CCWin.SkinControl.SkinComboBox();
            this.skinLabel2 = new CCWin.SkinControl.SkinLabel();
            this.CheckBitComboBox = new CCWin.SkinControl.SkinComboBox();
            this.skinLabel3 = new CCWin.SkinControl.SkinLabel();
            this.ConfigOKBTN = new CCWin.SkinControl.SkinButton();
            this.ConfigDefaultBTN = new CCWin.SkinControl.SkinButton();
            this.ConfigCancelBTN = new CCWin.SkinControl.SkinButton();
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
            this.PortComboBox.Click += new System.EventHandler(this.PortComboBox_Click);
            // 
            // BaudComboBox
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
            this.BaudComboBox.Name = "BaudComboBox";
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
            // DataBitComboBox
            // 
            this.DataBitComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.DataBitComboBox.Font = new System.Drawing.Font("宋体", 12F);
            this.DataBitComboBox.FormattingEnabled = true;
            this.DataBitComboBox.Items.AddRange(new object[] {
            "8",
            "7",
            "6",
            "5"});
            this.DataBitComboBox.Location = new System.Drawing.Point(312, 85);
            this.DataBitComboBox.Name = "DataBitComboBox";
            this.DataBitComboBox.Size = new System.Drawing.Size(121, 31);
            this.DataBitComboBox.TabIndex = 5;
            this.DataBitComboBox.WaterText = "";
            // 
            // skinLabel1
            // 
            this.skinLabel1.AutoSize = true;
            this.skinLabel1.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel1.BorderColor = System.Drawing.Color.White;
            this.skinLabel1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.skinLabel1.Location = new System.Drawing.Point(234, 89);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new System.Drawing.Size(72, 27);
            this.skinLabel1.TabIndex = 4;
            this.skinLabel1.Text = "数据位";
            // 
            // StopBitComboBox
            // 
            this.StopBitComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.StopBitComboBox.Font = new System.Drawing.Font("宋体", 12F);
            this.StopBitComboBox.FormattingEnabled = true;
            this.StopBitComboBox.Items.AddRange(new object[] {
            "1"});
            this.StopBitComboBox.Location = new System.Drawing.Point(85, 128);
            this.StopBitComboBox.Name = "StopBitComboBox";
            this.StopBitComboBox.Size = new System.Drawing.Size(121, 31);
            this.StopBitComboBox.TabIndex = 7;
            this.StopBitComboBox.WaterText = "";
            // 
            // skinLabel2
            // 
            this.skinLabel2.AutoSize = true;
            this.skinLabel2.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel2.BorderColor = System.Drawing.Color.White;
            this.skinLabel2.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.skinLabel2.Location = new System.Drawing.Point(7, 132);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new System.Drawing.Size(72, 27);
            this.skinLabel2.TabIndex = 6;
            this.skinLabel2.Text = "停止位";
            // 
            // CheckBitComboBox
            // 
            this.CheckBitComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.CheckBitComboBox.Font = new System.Drawing.Font("宋体", 12F);
            this.CheckBitComboBox.FormattingEnabled = true;
            this.CheckBitComboBox.Items.AddRange(new object[] {
            "无"});
            this.CheckBitComboBox.Location = new System.Drawing.Point(312, 128);
            this.CheckBitComboBox.Name = "CheckBitComboBox";
            this.CheckBitComboBox.Size = new System.Drawing.Size(121, 31);
            this.CheckBitComboBox.TabIndex = 9;
            this.CheckBitComboBox.WaterText = "";
            // 
            // skinLabel3
            // 
            this.skinLabel3.AutoSize = true;
            this.skinLabel3.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel3.BorderColor = System.Drawing.Color.White;
            this.skinLabel3.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.skinLabel3.Location = new System.Drawing.Point(234, 132);
            this.skinLabel3.Name = "skinLabel3";
            this.skinLabel3.Size = new System.Drawing.Size(72, 27);
            this.skinLabel3.TabIndex = 8;
            this.skinLabel3.Text = "校验位";
            // 
            // ConfigOKBTN
            // 
            this.ConfigOKBTN.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ConfigOKBTN.BackColor = System.Drawing.Color.Transparent;
            this.ConfigOKBTN.BaseColor = System.Drawing.Color.AliceBlue;
            this.ConfigOKBTN.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.ConfigOKBTN.DownBack = null;
            this.ConfigOKBTN.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.ConfigOKBTN.Location = new System.Drawing.Point(165, 181);
            this.ConfigOKBTN.MouseBack = null;
            this.ConfigOKBTN.Name = "ConfigOKBTN";
            this.ConfigOKBTN.NormlBack = null;
            this.ConfigOKBTN.Size = new System.Drawing.Size(118, 36);
            this.ConfigOKBTN.TabIndex = 10;
            this.ConfigOKBTN.Text = "配置完成";
            this.ConfigOKBTN.UseVisualStyleBackColor = false;
            this.ConfigOKBTN.Click += new System.EventHandler(this.ConfigOKBTN_Click);
            // 
            // ConfigDefaultBTN
            // 
            this.ConfigDefaultBTN.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ConfigDefaultBTN.BackColor = System.Drawing.Color.Transparent;
            this.ConfigDefaultBTN.BaseColor = System.Drawing.Color.AliceBlue;
            this.ConfigDefaultBTN.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.ConfigDefaultBTN.DownBack = null;
            this.ConfigDefaultBTN.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.ConfigDefaultBTN.Location = new System.Drawing.Point(12, 181);
            this.ConfigDefaultBTN.MouseBack = null;
            this.ConfigDefaultBTN.Name = "ConfigDefaultBTN";
            this.ConfigDefaultBTN.NormlBack = null;
            this.ConfigDefaultBTN.Size = new System.Drawing.Size(118, 36);
            this.ConfigDefaultBTN.TabIndex = 11;
            this.ConfigDefaultBTN.Text = "恢复默认";
            this.ConfigDefaultBTN.UseVisualStyleBackColor = false;
            this.ConfigDefaultBTN.Click += new System.EventHandler(this.ConfigDefaultBTN_Click);
            // 
            // ConfigCancelBTN
            // 
            this.ConfigCancelBTN.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ConfigCancelBTN.BackColor = System.Drawing.Color.Transparent;
            this.ConfigCancelBTN.BaseColor = System.Drawing.Color.AliceBlue;
            this.ConfigCancelBTN.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.ConfigCancelBTN.DownBack = null;
            this.ConfigCancelBTN.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.ConfigCancelBTN.Location = new System.Drawing.Point(317, 181);
            this.ConfigCancelBTN.MouseBack = null;
            this.ConfigCancelBTN.Name = "ConfigCancelBTN";
            this.ConfigCancelBTN.NormlBack = null;
            this.ConfigCancelBTN.Size = new System.Drawing.Size(118, 36);
            this.ConfigCancelBTN.TabIndex = 12;
            this.ConfigCancelBTN.Text = "取消配置";
            this.ConfigCancelBTN.UseVisualStyleBackColor = false;
            this.ConfigCancelBTN.Click += new System.EventHandler(this.ConfigCancelBTN_Click);
            // 
            // UARTConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CaptionFont = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ClientSize = new System.Drawing.Size(442, 237);
            this.Controls.Add(this.ConfigCancelBTN);
            this.Controls.Add(this.ConfigDefaultBTN);
            this.Controls.Add(this.ConfigOKBTN);
            this.Controls.Add(this.CheckBitComboBox);
            this.Controls.Add(this.skinLabel3);
            this.Controls.Add(this.StopBitComboBox);
            this.Controls.Add(this.skinLabel2);
            this.Controls.Add(this.DataBitComboBox);
            this.Controls.Add(this.skinLabel1);
            this.Controls.Add(this.BaudComboBox);
            this.Controls.Add(this.BaudLabel);
            this.Controls.Add(this.PortComboBox);
            this.Controls.Add(this.PortLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
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
        private CCWin.SkinControl.SkinComboBox DataBitComboBox;
        private CCWin.SkinControl.SkinLabel skinLabel1;
        private CCWin.SkinControl.SkinComboBox StopBitComboBox;
        private CCWin.SkinControl.SkinLabel skinLabel2;
        private CCWin.SkinControl.SkinComboBox CheckBitComboBox;
        private CCWin.SkinControl.SkinLabel skinLabel3;
        private CCWin.SkinControl.SkinButton ConfigOKBTN;
        private CCWin.SkinControl.SkinButton ConfigDefaultBTN;
        private CCWin.SkinControl.SkinButton ConfigCancelBTN;
    }
}