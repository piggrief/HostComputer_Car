namespace HostComputer.UI
{
    partial class ScopeConfig
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ConfigDGView = new CCWin.SkinControl.SkinDataGridView();
            this.IfShowCn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.LegendCn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LineStyleCn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.LineColorCn = new System.Windows.Forms.DataGridViewImageColumn();
            this.LineWidthCn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColorBtn = new CCWin.SkinControl.SkinButton();
            ((System.ComponentModel.ISupportInitialize)(this.ConfigDGView)).BeginInit();
            this.SuspendLayout();
            // 
            // ConfigDGView
            // 
            this.ConfigDGView.AllowUserToAddRows = false;
            this.ConfigDGView.AllowUserToDeleteRows = false;
            this.ConfigDGView.AlternatingCellBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ConfigDGView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle9;
            this.ConfigDGView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ConfigDGView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ConfigDGView.ColumnFont = null;
            this.ConfigDGView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(246)))), ((int)(((byte)(239)))));
            dataGridViewCellStyle10.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ConfigDGView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.ConfigDGView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ConfigDGView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IfShowCn,
            this.LegendCn,
            this.LineStyleCn,
            this.LineColorCn,
            this.LineWidthCn});
            this.ConfigDGView.ColumnSelectBackColor = System.Drawing.SystemColors.GrayText;
            this.ConfigDGView.ColumnSelectForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(188)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ConfigDGView.DefaultCellStyle = dataGridViewCellStyle11;
            this.ConfigDGView.EnableHeadersVisualStyles = false;
            this.ConfigDGView.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ConfigDGView.GridColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ConfigDGView.HeadFont = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ConfigDGView.HeadSelectBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.ConfigDGView.HeadSelectForeColor = System.Drawing.SystemColors.HighlightText;
            this.ConfigDGView.Location = new System.Drawing.Point(7, 44);
            this.ConfigDGView.MouseCellBackColor = System.Drawing.Color.Silver;
            this.ConfigDGView.Name = "ConfigDGView";
            this.ConfigDGView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.ConfigDGView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.GrayText;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.ConfigDGView.RowsDefaultCellStyle = dataGridViewCellStyle12;
            this.ConfigDGView.RowTemplate.Height = 23;
            this.ConfigDGView.Size = new System.Drawing.Size(711, 390);
            this.ConfigDGView.TabIndex = 0;
            this.ConfigDGView.TitleBack = null;
            this.ConfigDGView.TitleBackColorBegin = System.Drawing.Color.LightGray;
            this.ConfigDGView.TitleBackColorEnd = System.Drawing.Color.Silver;
            this.ConfigDGView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ConfigDGView_CellContentClick);
            // 
            // IfShowCn
            // 
            this.IfShowCn.HeaderText = "显示";
            this.IfShowCn.Name = "IfShowCn";
            this.IfShowCn.Width = 50;
            // 
            // LegendCn
            // 
            this.LegendCn.HeaderText = "图例";
            this.LegendCn.Name = "LegendCn";
            this.LegendCn.Width = 160;
            // 
            // LineStyleCn
            // 
            this.LineStyleCn.HeaderText = "曲线样式";
            this.LineStyleCn.Name = "LineStyleCn";
            this.LineStyleCn.Width = 90;
            // 
            // LineColorCn
            // 
            this.LineColorCn.HeaderText = "曲线颜色";
            this.LineColorCn.Name = "LineColorCn";
            this.LineColorCn.Width = 90;
            // 
            // LineWidthCn
            // 
            this.LineWidthCn.HeaderText = "曲线宽度";
            this.LineWidthCn.Name = "LineWidthCn";
            // 
            // ColorBtn
            // 
            this.ColorBtn.BackColor = System.Drawing.Color.Transparent;
            this.ColorBtn.BaseColor = System.Drawing.Color.Silver;
            this.ColorBtn.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.ColorBtn.DownBack = null;
            this.ColorBtn.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ColorBtn.Location = new System.Drawing.Point(7, 440);
            this.ColorBtn.MouseBack = null;
            this.ColorBtn.Name = "ColorBtn";
            this.ColorBtn.NormlBack = null;
            this.ColorBtn.Size = new System.Drawing.Size(660, 46);
            this.ColorBtn.TabIndex = 1;
            this.ColorBtn.Text = "手动配色好麻烦啊！自动配色！";
            this.ColorBtn.UseVisualStyleBackColor = false;
            this.ColorBtn.Click += new System.EventHandler(this.ColorBtn_Click);
            // 
            // ScopeConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(869, 526);
            this.Controls.Add(this.ColorBtn);
            this.Controls.Add(this.ConfigDGView);
            this.Name = "ScopeConfig";
            this.Text = "ScopeConfig";
            this.Load += new System.EventHandler(this.ScopeConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ConfigDGView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.SkinDataGridView ConfigDGView;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IfShowCn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LegendCn;
        private System.Windows.Forms.DataGridViewComboBoxColumn LineStyleCn;
        private System.Windows.Forms.DataGridViewImageColumn LineColorCn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LineWidthCn;
        private CCWin.SkinControl.SkinButton ColorBtn;



    }
}