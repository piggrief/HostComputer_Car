using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace ImageDeal_Pig
{
    public class ImageDealFlow
    {
        Bitmap OriginalImage = new Bitmap(2, 2);
        Bitmap Image_1 = new Bitmap(2, 2);
        Bitmap Image_2 = new Bitmap(2, 2);
        /// <summary>
        /// RGB图转灰度图
        /// </summary>
        /// <param name="OriginalImage">原图</param>
        /// <returns>灰度图</returns>
        public Bitmap RGBToGray(Bitmap OriginalImage)
        {
            int Image_Width = OriginalImage.Width;
            int Image_Height = OriginalImage.Height;

            Bitmap ChangedImage = new Bitmap(Image_Width, Image_Height);

            for (int i = 0; i < Image_Width; i++)
            {
                for (int j = 0; j < Image_Height; j++)
                {
                    Color Pixel = OriginalImage.GetPixel(i, j);
                    int gray = (int)(Pixel.R * 0.3 + Pixel.G * 0.59 + Pixel.B * 0.11);
                    Color GrayPixel = Color.FromArgb(gray, gray, gray);
                    ChangedImage.SetPixel(i, j, GrayPixel);
                }
            }

            return ChangedImage;
        }
        /// <summary>
        /// 全局阈值灰度二值化
        /// </summary>
        /// <param name="OriginalImage">原图（必须是灰度图）</param>
        /// <param name="Threshold">分割阈值</param>
        /// <returns>处理后的图像</returns>
        public Bitmap GrayBinary_GlobalThreshold(Bitmap OriginalImage, UInt16 Threshold)
        {
            int Image_Width = OriginalImage.Width;
            int Image_Height = OriginalImage.Height;

            Bitmap DealedImage = new Bitmap(Image_Width, Image_Height);

            for (int i = 0; i < Image_Width; i++)
            {
                for (int j = 0; j < Image_Height ; j++)
                {
                    Color Pixel = OriginalImage.GetPixel(i, j);
                    int gray = Pixel.R;
                    if (gray > Threshold)
                        gray = 255;
                    else
                        gray = 0;
                    Color GrayPixel = Color.FromArgb(gray, gray, gray);
                    DealedImage.SetPixel(i, j, GrayPixel);                  
                }
            }

            return DealedImage;
        }
    }
}
