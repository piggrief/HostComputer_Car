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
            public double Time_Max = 1;
            public double Time_Min = 0;
            public double Data_Max = 1;
            public double Data_Min = 0;

            public ShowAreaConfig(System.Windows.Forms.DataVisualization.Charting.Chart ChartSet, int AreaIndexSet)
            {
                ShowChart = ChartSet;
                AreaIndex = AreaIndexSet;
            }
            /// <summary>
            /// 配置显示区域范围
            /// </summary>
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
            /// <summary>
            /// 显示区域平移
            /// </summary>
            /// <param name="x_diff"></param>
            /// <param name="y_diff"></param>
            /// <param name="ChartSize"></param>
            public void PanChange(int x_diff, int y_diff, System.Drawing.Size ChartSize)
            {
                double x_diff_changrate = (Time_Max - Time_Min) / Convert.ToDouble(ChartSize.Width);
                double y_diff_changrate = (Data_Max - Data_Min) / Convert.ToDouble(ChartSize.Height);

                Int32 x_ChangedValue = Convert.ToInt32(x_diff * x_diff_changrate);
                Int32 y_ChangedValue = Convert.ToInt32(y_diff * y_diff_changrate);

                double t_Min = x_ChangedValue + Time_Min;
                double t_Max = x_ChangedValue + Time_Max;
                double d_Min = -y_ChangedValue + Data_Min;
                double d_Max = -y_ChangedValue + Data_Max;

                ConfigShowArea(t_Min, t_Max, d_Min, d_Max);
            }
            /// <summary>
            /// 显示区域缩放
            /// </summary>
            /// <param name="Scale_Rate">缩放比例</param>
            public void ScaleChange(double Scale_Rate)
            {
                double t_Min = Time_Min * Scale_Rate;
                double t_Max = Time_Max * Scale_Rate;
                double d_Min = Data_Min * Scale_Rate;
                double d_Max = Data_Max * Scale_Rate;

                ConfigShowArea(t_Min, t_Max, d_Min, d_Max);
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
