using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualOscilloscope_n;

namespace ChartTest
{
    public partial class Form1 : Form
    {
        VirtualOscilloscope VO;
        public Form1()
        {
            InitializeComponent();
            VO = new VirtualOscilloscope(TestChart);
            TestTimer.Interval = 200;
        }

        private void TestBTN_Click(object sender, EventArgs e)
        {
            TestTimer.Enabled = !TestTimer.Enabled;
        }
        /// <summary>
        /// 图表控件初始化
        /// </summary>
        private void ChartInit()
        {
            TestChart.ChartAreas[0].Axes[0].IsMarksNextToAxis = true;
            TestChart.ChartAreas[0].Axes[0].Maximum = 20;
            TestChart.ChartAreas[0].Axes[0].Minimum = 0;
        }
        /// <summary>
        /// 定时器事件
        /// </summary>
        private void TestTimer_Tick(object sender, EventArgs e)
        {
            int DataBuff = VO.CreateRandomTestData(-100, -50);

            TestChart.Series[0].Points.AddXY(VO.TimeCount++, DataBuff);
            DataBuff = VO.CreateRandomTestData(70, 100);
            TestChart.Series[1].Points.AddXY(VO.TimeCount++, DataBuff);
            DataBuff = VO.CreateRandomTestData(40, 80);
            TestChart.Series[2].Points.AddXY(VO.TimeCount++, DataBuff);
            DataBuff = VO.CreateRandomTestData(-10, 50);
            TestChart.Series[3].Points.AddXY(VO.TimeCount++, DataBuff);
            //VO.ShowAreaConfigList[0].ConfigShowArea(0, 20, -100, 100);
            ulong t_Max = VO.TimeCount > 500 ? VO.TimeCount : 500;
            ulong t_Min = VO.TimeCount >= 500 ? VO.TimeCount - 500 : 0;

            VO.ShowAreaConfigList[0].ConfigShowArea(t_Min, t_Max, -100, 100);

            TestChart.ChartAreas[0].Axes[0].MajorGrid.LineColor = System.Drawing.Color.Transparent;
        }
    }
}
