using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
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
                Time_Min = Math.Round(T_Min, 1);
                Time_Max = Math.Round(T_Max, 1);
                Data_Min = Math.Round(D_Min, 1);
                Data_Max = Math.Round(D_Max, 1);

                ShowChart.Invoke(new EventHandler(delegate
                {
                    ShowChart.ChartAreas[AreaIndex].Axes[0].Maximum = Time_Max;
                    ShowChart.ChartAreas[AreaIndex].Axes[0].Minimum = Time_Min;
                    ShowChart.ChartAreas[AreaIndex].Axes[1].Maximum = Data_Max;
                    ShowChart.ChartAreas[AreaIndex].Axes[1].Minimum = Data_Min;
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

            public Point t_min_d_min_loaction = new Point(94, 405);
            public const double t_max_location = 840;
            public const double d_max_location = 23;
            /// <summary>
            /// 鼠标点击位置转成数据坐标
            /// </summary>
            /// <param name="MousePoint">鼠标点击位置</param>
            /// <param name="If_ChangT">是否转换T坐标，否为转换D坐标</param>
            /// <returns>对应图表上的数据点</returns>
            public double MouseLocationToChartData(double MousePoint, bool If_ChangT)
            {
                double Result = 0;
                if (If_ChangT)
                {
                    double k_t = (Time_Max - Time_Min) /
                        (t_max_location - Convert.ToDouble(t_min_d_min_loaction.X));
                    double b_t = Time_Max - t_max_location * k_t;
                    Result = MousePoint * k_t + b_t;
                }
                else
                {
                    double k_d = (Data_Max - Data_Min) /
                        (d_max_location - Convert.ToDouble(t_min_d_min_loaction.Y));
                    double b_d = Data_Max - d_max_location * k_d;
                    Result = MousePoint * k_d + b_d; 
                }

                return Result;
            }
            /// <summary>
            /// 根据选择区域缩放
            /// </summary>
            /// <param name="StartLoaction">点击起始位置</param>
            /// <param name="EndLocation">结束起始位置</param>
            public void AreaScale(Point StartLoaction, Point EndLocation)
            {
                double new_t_min = 0, new_t_max = 0;
                double new_d_min = 0, new_d_max = 0;
                if (StartLoaction.X < EndLocation.X)
                {
                    new_t_min = MouseLocationToChartData(StartLoaction.X, true);
                    new_t_max = MouseLocationToChartData(EndLocation.X, true);
                }
                else
                {
                    new_t_min = MouseLocationToChartData(EndLocation.X, true);
                    new_t_max = MouseLocationToChartData(StartLoaction.X, true);
                }
                if (StartLoaction.Y < EndLocation.Y)
                {
                    new_d_min = MouseLocationToChartData(EndLocation.Y, false);
                    new_d_max = MouseLocationToChartData(StartLoaction.Y, false);
                }
                else
                {
                    new_d_min = MouseLocationToChartData(StartLoaction.Y, false);
                    new_d_max = MouseLocationToChartData(EndLocation.Y, false);
                }
                ConfigShowArea(new_t_min, new_t_max, new_d_min, new_d_max);
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
