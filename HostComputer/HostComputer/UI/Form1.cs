using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CCWin;

using ImageDeal_Pig;
using HostComputer.UI;
using UartComunication;

using AForge;
using AForge.Imaging;

namespace HostComputer
{
    public partial class HostComputerForm : Skin_Mac
    {
        public ImageDealFlow IDF = new ImageDealFlow();
        public UART UsedUart = new UART();

        public HostComputerForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 测试按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skinButton1_Click(object sender, EventArgs e)
        {
            Bitmap OImage = new Bitmap(InitalImagePB.Image);
            Bitmap GrayImage = IDF.RGBToGray(OImage);
            DealedImage1PB.Image = GrayImage;

            UInt16 Threshold = IDF.FindThreshold_OTSUNormal(GrayImage, 10);
            BinaryThresholdShowTB.Text = Threshold.ToString();
            Bitmap BinaryImage = IDF.GrayBinary_GlobalThreshold(GrayImage, Convert.ToUInt16(Threshold));
            DealedImage2PB.Image = BinaryImage;

        }

        private void skinTrackBar1_Scroll(object sender, EventArgs e)
        {
            BinaryThresholdShowTB.Text = BinaryThresholdTrackBar.Value.ToString();
        }

        private void HostComputerForm_Load(object sender, EventArgs e)
        {
            BinaryMethodSelectCB.SelectedIndex = 0;
        }
        /// <summary>
        /// 打开串口配置界面
        /// </summary>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            UARTConfig UARTConfigForm = new UARTConfig(this);
            UARTConfigForm.Show();
        }
    }
}
