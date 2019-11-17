using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using CCWin;

namespace HostComputer.UI
{
    public partial class ScopeConfig : Skin_Mac
    {
        /// <summary>
        /// 曲线数量
        /// </summary>
        int SeriesCount = 1;
        System.Windows.Forms.DataVisualization.Charting.Chart ScopeChart;
        public ScopeConfig(System.Windows.Forms.DataVisualization.Charting.Chart SC)
        {
            InitializeComponent();
            SC.Invoke(new EventHandler(delegate
            {
                SeriesCount = SC.Series.Count;
                ScopeChart = SC;
            }));
        }
        void ConfigDGViewInit()
        {
 
        }

        public List<Color> SeriesColorConfig = new List<Color>();
        private void ScopeConfig_Load(object sender, EventArgs e)
        {
            LineStyleCn.Items.Add(SeriesChartType.Line.ToString());
            LineStyleCn.Items.Add(SeriesChartType.Spline.ToString());
            LineStyleCn.DefaultCellStyle.NullValue = SeriesChartType.Line.ToString();
            
            for (int i = 0; i < SeriesCount; i++)
            {
                ConfigDGView.Rows.Add();
                ConfigDGView.Rows[i].Cells[0].Value = true;
                ConfigDGView.Rows[i].Cells[1].Value = "Series" + (i + 1).ToString();

                ConfigDGView.Rows[i].Cells[4].Value = (2).ToString();
            }

            ScopeChart.Invoke(new EventHandler(delegate
            {
                for (int i = 0; i < SeriesCount; i++)
                {
                    ((DataGridViewComboBoxCell)ConfigDGView.Rows[i].Cells[2]).Style.NullValue =
                        ScopeChart.Series[i].ChartType.ToString();
                    Bitmap BI = new Bitmap(ConfigDGView.Columns[3].Width, ConfigDGView.Rows[0].Height);
                    Graphics g = Graphics.FromImage(BI);
                    g.Clear(ScopeChart.Series[i].Color);
                    ConfigDGView.Rows[i].Cells[3].Value = BI;

                    SeriesColorConfig.Add(ScopeChart.Series[i].Color);
                }
            }));
            ConfigDGView.Size = new Size(534, 300);
            ColorBtn.Size = new System.Drawing.Size(ConfigDGView.Size.Width, ColorBtn.Size.Height);
            ColorBtn.Location = new Point(0, ConfigDGView.Size.Height + ConfigDGView.Location.Y);

            this.Size = new Size(545, 340 + ColorBtn.Height);
        }

        private void ConfigDGView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 3)
                {
                    ColorDialog CD = new ColorDialog();
                    if (CD.ShowDialog() == DialogResult.OK)
                    {
                        Bitmap BI = new Bitmap(ConfigDGView.Columns[3].Width, ConfigDGView.Rows[0].Height);
                        Graphics g = Graphics.FromImage(BI);
                        g.Clear(CD.Color);
                        SeriesColorConfig[e.RowIndex] = CD.Color;
                        ConfigDGView.Rows[e.RowIndex].Cells[3].Value = BI;
                    }
                }
            }
        }

        private void ColorBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("作者还没写呢！");
        }
    }
}
