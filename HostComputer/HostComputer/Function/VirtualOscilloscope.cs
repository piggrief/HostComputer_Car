using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualOscilloscope_n
{
    public class VirtualOscilloscope
    {
        /// <summary>
        /// 当前示波器运行时刻(以接受一个数据包的时间长度为单位时间)
        /// </summary>
        public UInt64 TimeCount = 0;
        /// <summary>
        /// Chart控件
        /// </summary>
        System.Windows.Forms.DataVisualization.Charting.Chart ShowChart;
        /// <summary>
        /// 显示区域配置
        /// </summary>
        public class ShowAreaConfig
        {
            System.Windows.Forms.DataVisualization.Charting.Chart ShowChart;
            int AreaIndex = 0;
            double Time_Max = 1;
            double Time_Min = 0;
            double Data_Max = 1;
            double Data_Min = 0;

            public ShowAreaConfig(System.Windows.Forms.DataVisualization.Charting.Chart ChartSet, int AreaIndexSet)
            {
                ShowChart = ChartSet;
                AreaIndex = AreaIndexSet;
            }

            public void ConfigShowArea(double T_Min, double T_Max, double D_Min, double D_Max)
            {
                Time_Min = T_Min;
                Time_Max = T_Max;
                Data_Min = D_Min;
                Data_Max = D_Max;

                ShowChart.Invoke(new EventHandler(delegate
                {
                    ShowChart.ChartAreas[AreaIndex].Axes[0].Maximum = T_Max;
                    ShowChart.ChartAreas[AreaIndex].Axes[0].Minimum = T_Min;
                    ShowChart.ChartAreas[AreaIndex].Axes[1].Maximum = D_Max;
                    ShowChart.ChartAreas[AreaIndex].Axes[1].Minimum = D_Min;
                }));
            }
        }
        public List<ShowAreaConfig> ShowAreaConfigList = new List<ShowAreaConfig>();
        public VirtualOscilloscope(System.Windows.Forms.DataVisualization.Charting.Chart ChartSet)
        {
            ShowChart = ChartSet;
            ShowAreaConfigList.Add(new ShowAreaConfig(ShowChart, 0));
        }
        /// <summary>
        /// 生成随机测试数据
        /// </summary>
        /// <param name="Min">最小值</param>
        /// <param name="Max">最大值</param>
        /// <returns>随机数据</returns>
        public int CreateRandomTestData(int Min, int Max)
        {
            Random Rd = new Random();
            int TestData = Rd.Next(Min, Max);

            return TestData;
        }
    }
}
