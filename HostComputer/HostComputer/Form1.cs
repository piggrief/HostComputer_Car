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

namespace HostComputer
{
    public partial class HostComputerForm : Skin_Mac
    {
        public HostComputerForm()
        {
            InitializeComponent();
        }
        private void skinButton1_Click(object sender, EventArgs e)
        {
            Bitmap OImage = new Bitmap(InitalImagePB.Image);
            Bitmap GrayImage = IDF.RGBToGray(OImage);
            DealedImage1PB.Image = GrayImage;

            Bitmap BinaryImage = IDF.GrayBinary_GlobalThreshold(GrayImage, 20);
            DealedImage2PB.Image = BinaryImage;
        }

        public ImageDealFlow IDF = new ImageDealFlow();
    }
}
