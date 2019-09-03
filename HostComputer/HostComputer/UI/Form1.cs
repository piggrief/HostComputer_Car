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

using AForge;
using AForge.Imaging;

namespace HostComputer
{
    public partial class HostComputerForm : Skin_Mac
    {
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

        public ImageDealFlow IDF = new ImageDealFlow();

        private void skinTrackBar1_Scroll(object sender, EventArgs e)
        {
            BinaryThresholdShowTB.Text = BinaryThresholdTrackBar.Value.ToString();
        }

        private void HostComputerForm_Load(object sender, EventArgs e)
        {
            BinaryMethodSelectCB.SelectedIndex = 0;
        }
    }
}
