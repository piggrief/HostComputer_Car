namespace ChartTest
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.TestChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.TestBTN = new CCWin.SkinControl.SkinButton();
            this.TestTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.TestChart)).BeginInit();
            this.SuspendLayout();
            // 
            // TestChart
            // 
            chartArea1.Name = "ChartArea1";
            this.TestChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.TestChart.Legends.Add(legend1);
            this.TestChart.Location = new System.Drawing.Point(12, 12);
            this.TestChart.Name = "TestChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Legend = "Legend1";
            series2.Name = "Series2";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series3.Legend = "Legend1";
            series3.Name = "Series3";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series4.Legend = "Legend1";
            series4.Name = "Series4";
            this.TestChart.Series.Add(series1);
            this.TestChart.Series.Add(series2);
            this.TestChart.Series.Add(series3);
            this.TestChart.Series.Add(series4);
            this.TestChart.Size = new System.Drawing.Size(939, 527);
            this.TestChart.TabIndex = 0;
            this.TestChart.Text = "chart1";
            // 
            // TestBTN
            // 
            this.TestBTN.BackColor = System.Drawing.Color.Transparent;
            this.TestBTN.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.TestBTN.DownBack = null;
            this.TestBTN.Location = new System.Drawing.Point(695, 574);
            this.TestBTN.MouseBack = null;
            this.TestBTN.Name = "TestBTN";
            this.TestBTN.NormlBack = null;
            this.TestBTN.Size = new System.Drawing.Size(194, 68);
            this.TestBTN.TabIndex = 1;
            this.TestBTN.Text = "Test";
            this.TestBTN.UseVisualStyleBackColor = false;
            this.TestBTN.Click += new System.EventHandler(this.TestBTN_Click);
            // 
            // TestTimer
            // 
            this.TestTimer.Interval = 200;
            this.TestTimer.Tick += new System.EventHandler(this.TestTimer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 673);
            this.Controls.Add(this.TestBTN);
            this.Controls.Add(this.TestChart);
            this.Name = "Form1";
            this.Text = "Chart测试";
            ((System.ComponentModel.ISupportInitialize)(this.TestChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart TestChart;
        private CCWin.SkinControl.SkinButton TestBTN;
        private System.Windows.Forms.Timer TestTimer;
    }
}

