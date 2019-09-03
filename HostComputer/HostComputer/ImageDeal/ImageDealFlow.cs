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
        /// <summary>
        /// 使用传统OTSU算法寻找二值化阈值
        /// </summary>
        /// <param name="OriginalImage">原图（灰度图）</param>
        /// <param name="Rate_Reduction">降维系数</param>
        /// <returns>找到的阈值</returns>
        public UInt16 FindThreshold_OTSUNormal(Bitmap OriginalImage, int Rate_Reduction)
        {
            UInt16 Threshold = 0;

            int Image_Width = OriginalImage.Width;
            int Image_Height = OriginalImage.Height;

            const int GrayScale = 256;//单通道图像总灰度256级
            UInt64[] pixCount = new UInt64[GrayScale];//每个灰度值所占像素个数
            double pixSum= Convert.ToDouble(Image_Width) * Convert.ToDouble(Image_Height);//图像总像素点
            double[] pixPro = new double[GrayScale];//每个灰度值所占总像素比例
            
            pixCount.Initialize();
            pixPro.Initialize();
            
            //统计每个灰度级中像素的个数
            for (int i = 0; i < Image_Width; i += Rate_Reduction)
            {
                for (int j = 0; j < Image_Height; j += Rate_Reduction)
                {
                    int Pixel = OriginalImage.GetPixel(i, j).R;
                    pixCount[Pixel]++;
                }
            }

            //计算每个像素在整幅图像中的比例
            for (int i = 0; i < GrayScale; i++)
                pixPro[i] = Convert.ToDouble(pixCount[i]) / pixSum;

            double deltaMax = 0;

            //遍历所有从0到255灰度级的阈值分割条件，测试哪一个的类间方差最大
            for (int i = 0; i < GrayScale; i++)
            {
                double w0 = 0;
                double w1 = 0;
                double u0tmp = 0;
                double u1tmp = 0;
                double u0 = 0;
                double u1 = 0;
                double deltaTmp = 0;

                for (int j = 0; j < GrayScale; j++)
                {
                    if (j <= i)//背景
                    {
                        w0 += pixPro[j];
                        u0tmp += j * pixPro[j]; 
                    }
                    else//前景
                    {
                        w1 += pixPro[j];
                        u1tmp += j * pixPro[j];
                    }
                }
                u0 = u0tmp / w0;
		        u1 = u1tmp / w1;
                deltaTmp = Convert.ToDouble((w0 * w1 * (u0 - u1) * (u0 - u1))); //类间方差公式 g = w1 * w2 * (u1 - u2) ^ 2
		        if(deltaTmp > deltaMax) 
		        {
			        deltaMax = deltaTmp;
                    Threshold = Convert.ToUInt16(i);  
		        } 
            }

            return Threshold;
        }
    }
}
