using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using CCWin;

using ImageDeal_Pig;
using HostComputer.UI;
using UartComunication;

using AForge;
using AForge.Imaging;
using AForge.Math.Geometry;

namespace HostComputer
{
    public partial class HostComputerForm : Skin_Mac
    {
        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole(); 

        public ImageDealFlow IDF = new ImageDealFlow();
        public UART UsedUart = new UART();

        class TimerExampleState
        {
            public int counter = 0;
            public System.Threading.Timer tmr;
        }
        /// <summary>
        /// 串口接受状态枚举
        /// </summary>
        public enum UARTReceiveStatus
        {
            SerialPortClosed,
            SerialPortOpen,
            DataReceiving,
            DataReceiveFinish,
        }
        UARTReceiveStatus NowUartReceiveStatus = UARTReceiveStatus.SerialPortClosed;
        /// <summary>
        /// 串口发送状态枚举
        /// </summary>
        public enum UARTSendStatus
        {
            SerialPortClosed,
            SerialPortOpen,
            DataSending,
            DataSendFinish
        }
        UARTSendStatus NowUartSendStatus = UARTSendStatus.SerialPortClosed;
        /// <summary>是否正在进行接收标志位</summary>
        Thread getRecevice;
        string strRecieve;
        TimerExampleState s = new TimerExampleState();


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

            if (BinaryMethodSelectCB.SelectedIndex == 0)
            {
                Bitmap BinaryImage = IDF.GrayBinary_GlobalThreshold(GrayImage, 
                    Convert.ToUInt16(BinaryThresholdTrackBar.Value));
                DealedImage2PB.Image = BinaryImage;
            }
            else if (BinaryMethodSelectCB.SelectedIndex == 1)
            {
                UInt16 Threshold = IDF.FindThreshold_OTSUNormal(GrayImage, 10);
                BestThresholdText.Text = Threshold.ToString();
                Bitmap BinaryImage = IDF.GrayBinary_GlobalThreshold(GrayImage, Convert.ToUInt16(Threshold));
                DealedImage2PB.Image = BinaryImage;


            }

        }

        private void skinTrackBar1_Scroll(object sender, EventArgs e)
        {
            BinaryThresholdShowTB.Text = BinaryThresholdTrackBar.Value.ToString();
        }

        private void HostComputerForm_Load(object sender, EventArgs e)
        {
            HostComputerForm.CheckForIllegalCrossThreadCalls = false;
            BinaryMethodSelectCB.SelectedIndex = 0;
            if (Debugger.IsAttached)
                AllocConsole();
        }
        /// <summary>
        /// 打开串口配置界面
        /// </summary>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            UARTConfig UARTConfigForm = new UARTConfig(this);
            UARTConfigForm.Show();
        }

        private void SendClearBTN_Click(object sender, EventArgs e)
        {

        }

        private void SendBTN_Click(object sender, EventArgs e)
        {

        }

        private void ReceiveClearBTN_Click(object sender, EventArgs e)
        {

        }

        private void SwitchReceiveBTN_Click(object sender, EventArgs e)
        {

        }

        private void SendGB_Enter(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 用来切换串口接收或者串口不接收
        /// </summary>
        private void SwitchReceiveBTN_Click_1(object sender, EventArgs e)
        {
            if (SwitchReceiveBTN.Text == "停止接收")
            {
                NowUartReceiveStatus = UARTReceiveStatus.DataReceiveFinish;
            }
            if (NowUartReceiveStatus == UARTReceiveStatus.SerialPortClosed || NowUartSendStatus == UARTSendStatus.SerialPortClosed)
            {
                //UsedUart.SetSerialPort(PortCB.Text, int.Parse(BaudCB.Text), int.Parse(DataBitsCB.Text), int.Parse(StopBitsCB.Text));
                if (UsedUart.SwtichSP(true) == true)//打开成功
                {
                    NowUartReceiveStatus = UARTReceiveStatus.SerialPortOpen;
                    NowUartSendStatus = UARTSendStatus.SerialPortOpen;
                }
                else
                {
                    toolStripLabel1.Text = "请重新配置串口！";
                    toolStripLabel2.Text = "";
                    toolStripLabel3.Text = "";
                    toolStripLabel4.Text = "";
                    toolStripLabel5.Text = "";
                }
            }
            if (NowUartReceiveStatus == UARTReceiveStatus.SerialPortOpen)
            {
                UsedUart.sp.Encoding = Encoding.GetEncoding("GB2312");
                //使用委托以及多线程进行
                #region 打开多线程定时器
                //创建代理对象TimerCallback，该代理将被定时调用
                TimerCallback timerDelegate = new TimerCallback(TimerRecive);
                //创建一个时间间隔为1s的定时器
                System.Threading.Timer timer = new System.Threading.Timer(timerDelegate, s, 0, 100);
                s.tmr = timer;
                #endregion

                SwitchReceiveBTN.Text = "停止接收";
                NowUartReceiveStatus = UARTReceiveStatus.DataReceiving;
            }
        }
        /// <summary>
        /// 串口接收用定时器函数
        /// </summary>
        /// <param name="state"></param>
        void TimerRecive(Object state)
        {
            TimerExampleState s = (TimerExampleState)state;
            if (NowUartReceiveStatus == UARTReceiveStatus.DataReceiveFinish)
            {
                if (UsedUart.SwtichSP(false) == true)//关闭成功
                {
                    SwitchReceiveBTN.Text = "开始接收";
                    NowUartReceiveStatus = UARTReceiveStatus.SerialPortClosed;
                    NowUartSendStatus = UARTSendStatus.SerialPortClosed;
                }
                else
                {
                    MessageBox.Show("关闭串口失败！");
                }

                s.tmr.Dispose();
                s.tmr = null;
                return;
            }
            try
            {
                if (HexCB.Checked == true)
                {
                    strRecieve = "";
                    byte[] Receivebuff = new byte[500];
                    int ReceiveNum = UsedUart.sp.Read(Receivebuff, 0, UsedUart.sp.BytesToRead);

                    if (ReceiveNum > 0)
                    {
                        for (int i = 0; i < ReceiveNum; i++)
                        {
                            int hexnum1 = Receivebuff[i] / 16;
                            int hexnum2 = Receivebuff[i] % 16;

                            strRecieve += IntToHexChar(hexnum1);
                            strRecieve += IntToHexChar(hexnum2);
                            strRecieve += " ";

                            //strRecieve += Receivebuff[i];
                        }

                        Console.WriteLine(strRecieve);
                        ReceiveTB.AppendText(strRecieve);
                    }

                    //Console.WriteLine(Receivebuff.ToString());
                }
                else
                {
                    strRecieve = UsedUart.sp.ReadExisting();
                    if (strRecieve != "")
                    {
                        Console.WriteLine(strRecieve);
                        ReceiveTB.AppendText(strRecieve);

                        if (ReceiveTB.Text.Length >= 10000)
                        {
                            NowUartReceiveStatus = UARTReceiveStatus.DataReceiveFinish;
                            SwitchReceiveBTN.Text = "开始接收";
                        }
                    }
                }
            }
            catch (Exception ex) { }
            //}
        }

        private void HostComputerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Debugger.IsAttached)
                FreeConsole();
            Application.Exit();
        }
        /// <summary>
        /// int数字转换成Hex字符
        /// </summary>
        private string IntToHexChar(int HexNum)
        {
            if (HexNum >= 16 || HexNum < 0)
            { MessageBox.Show("输入的Hex数字超过了0~15"); return ""; }
            else if (HexNum < 10)
                return HexNum.ToString();
            else
            {
                switch (HexNum)
                {
                    case 10: return "A";
                    case 11: return "B";
                    case 12: return "C";
                    case 13: return "D";
                    case 14: return "E";
                    case 15: return "F";
                    default: return "";
                }
            }
        }
        /// <summary>
        /// Hex字符转Byte
        /// </summary>
        /// <param name="HexChar">Hex字符</param>
        /// <returns>Byte输出</returns>
        private byte HexCharToByte(string HexChar)
        {
            int NumBuff = 0;
            if (int.TryParse(HexChar, out NumBuff))
            {
                return Convert.ToByte(NumBuff);
            }
            else
            {
                switch (HexChar)
                {
                    case "A": return Convert.ToByte(10);
                    case "B": return Convert.ToByte(11);
                    case "C": return Convert.ToByte(12);
                    case "D": return Convert.ToByte(13);
                    case "E": return Convert.ToByte(14);
                    case "F": return Convert.ToByte(15);
                    case "a": return Convert.ToByte(10);
                    case "b": return Convert.ToByte(11);
                    case "c": return Convert.ToByte(12);
                    case "d": return Convert.ToByte(13);
                    case "e": return Convert.ToByte(14);
                    case "f": return Convert.ToByte(15);
                    default: MessageBox.Show("Hex格式不正确"); return Convert.ToByte(0);                   
                }
            }
        }
        private const string HexCheckString = "0123456789abcdefABCDEF";
        /// <summary>
        /// 用来将发送区文本框的文本通过串口发送
        /// </summary>
        private void SendBTN_Click_1(object sender, EventArgs e)
        {
            if (NowUartSendStatus == UARTSendStatus.SerialPortClosed)
            {
                //Uart1.SetSerialPort(PortCB.Text, int.Parse(BaudCB.Text), int.Parse(DataBitsCB.Text), int.Parse(StopBitsCB.Text));
                if (UsedUart.SwtichSP(true) == true)//打开成功
                {
                    NowUartSendStatus = UARTSendStatus.SerialPortOpen;
                    NowUartReceiveStatus = UARTReceiveStatus.SerialPortOpen;
                }
                else
                {
                    toolStripLabel1.Text = "找不到端口，请重新配置串口！";
                    toolStripLabel2.Text = "";
                    toolStripLabel3.Text = "";
                    toolStripLabel4.Text = "";
                    toolStripLabel5.Text = "";
                }
            }
            if (NowUartSendStatus == UARTSendStatus.SerialPortOpen)
            {
                NowUartSendStatus = UARTSendStatus.DataSending;

                string strbuff = SendTB.Text;
                if (HexSendCB.Checked == true)
                {
                    ///检测是否符合格式
                    for (int i = 0; i < strbuff.Length; i += 3)
                    {
                        try
                        {
                            if (!HexCheckString.Contains(strbuff[i]))
                                throw new Exception();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("不符合Hex的发送格式！\r\n格式为：3A 2B\r\n最后没有空格！！！");
                            return;
                        }
                        if (i + 2 < strbuff.Length && strbuff[i + 2] != ' ')
                        {
                            MessageBox.Show("不符合Hex的发送格式！\r\n格式为：3A 2B\r\n最后没有空格！！！");
                            return;
                        }

                    }

                    ///正式发送
                    int size = (strbuff.Length + 1) / 3;
                    if ((strbuff.Length + 1) % 3 != 0)
                    {
                        MessageBox.Show("不符合Hex的发送格式！\r\n格式为：3A 2B\r\n最后没有空格！！！");
                        return;
                    }
                    if (size <= 0)
                    {
                        MessageBox.Show("至少要一个16进制数！");
                        return;
                    }
                    byte[] sendbuff = new byte[size];
                    for (int i = 0; i < strbuff.Length; i += 3)
                    {
                        byte num1 = Convert.ToByte(HexCharToByte(strbuff[i].ToString()) * 16);
                        byte num2 = HexCharToByte(strbuff[i + 1].ToString());
                        sendbuff[i / 3] = Convert.ToByte(num1 + num2);
                    }

                    UsedUart.sp.Write(sendbuff, 0, size);
                }
                else
                    UsedUart.sp.Write(strbuff);

                NowUartSendStatus = UARTSendStatus.DataSendFinish;
                NowUartSendStatus = UARTSendStatus.SerialPortOpen;
            }
        }
        /// <summary>
        /// 用来清除发送区文本框内的内容
        /// </summary>
        private void SendClearBTN_Click_1(object sender, EventArgs e)
        {
            SendTB.Text = "";
        }
        /// <summary>
        /// 用来清除接收区文本框内的内容
        /// </summary>
        private void ReceiveClearBTN_Click_1(object sender, EventArgs e)
        {
            ReceiveTB.Text = "";
        }
        /// <summary>
        /// 二值化方法选择ComboBox选择值改变事件，主要用于切换空间显示
        /// </summary>
        private void BinaryMethodSelectCB_SelectedValueChanged(object sender, EventArgs e)
        {
            if (BinaryMethodSelectCB.SelectedIndex == 1)
            {
                skinLabel2.Visible = true;
                skinLabel3.Visible = true;
                skinLabel4.Visible = true;
                skinLabel5.Visible = true;
                skinLabel6.Visible = true;
                ReduceRateText.Visible = true;
                ThresholdIntervalText.Visible = true;
                BestThresholdText.Visible = true;
                BestThresholdText.Text = "未启动OTSU";
            }
            else
            {
                skinLabel2.Visible = false;
                skinLabel3.Visible = false;
                skinLabel4.Visible = false;
                skinLabel5.Visible = false;
                skinLabel6.Visible = false;
                ReduceRateText.Visible = false;
                ThresholdIntervalText.Visible = false;
                BestThresholdText.Visible = false;
            }
            if (BinaryMethodSelectCB.SelectedIndex == 0)
            {
                BinaryThresholdTrackBar.Visible = true;
                BinaryThresholdShowTB.Visible = true;
            }
            else
            {
                BinaryThresholdTrackBar.Visible = false;
                BinaryThresholdShowTB.Visible = false;
            }
        }



    }
}
