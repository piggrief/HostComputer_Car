using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using CCWin;

using ImageDeal_Pig;
using HostComputer.UI;
using UartComunication;
using PigCommunication;
using VirtualOscilloscope_n;

//using AForge;
//using AForge.Imaging;
//using AForge.Math.Geometry;

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

        Communication UsedUARTCommunication = new Communication();//通信协议对象

        Thread UartDataDecoding;//串口数据解算线程
        /// <summary>
        /// 图像刷新线程
        /// </summary>
        Thread ImageRefresh;
        /// <summary>
        /// 图像处理线程
        /// </summary>
        Thread ImageDealThread;
        /// <summary>
        /// 虚拟示波器刷新线程
        /// </summary>
        Thread ScopeRenewThread;
        /// <summary>
        /// InitalImagePB的互斥锁对象
        /// </summary>
        public static readonly object Lock_InitalImagePB = new object();
        /// <summary>
        /// DealedImage1PB的互斥锁对象
        /// </summary>
        public static readonly object Lock_DealedImage1PB = new object();
        /// <summary>
        /// DealedImage2PB的互斥锁对象
        /// </summary>
        public static readonly object Lock_DealedImage2PB = new object();
        /// <summary>
        /// 图像处理标志，用于控制时序
        /// </summary>
        bool Flag_ImageDeal = false;
        /// <summary>
        /// 虚拟示波器对象
        /// </summary>
        VirtualOscilloscope VO;

        public HostComputerForm()
        {
            InitializeComponent();
        }
        public void ImageDeal()
        {
            while (true)
            {
                if (Flag_ImageDeal)
                {
                    Bitmap OImage;

                    Monitor.Enter(Lock_InitalImagePB);
                    OImage = new Bitmap(InitalImagePB.Image);
                    Monitor.Exit(Lock_InitalImagePB);

                    Bitmap GrayImage = IDF.RGBToGray(OImage);

                    Monitor.Enter(Lock_DealedImage1PB);
                    DealedImage1PB.Image = GrayImage;
                    DealedImage1PB.Refresh();
                    Monitor.Exit(Lock_DealedImage1PB);

                    if (BinaryMethodSelectCB.SelectedIndex == 0)
                    {
                        Bitmap BinaryImage = IDF.GrayBinary_GlobalThreshold(GrayImage,
                            Convert.ToUInt16(BinaryThresholdTrackBar.Value));
                        
                        Monitor.Enter(Lock_DealedImage2PB);
                        DealedImage2PB.Image = BinaryImage;
                        Monitor.Exit(Lock_DealedImage2PB);
                    }
                    else if (BinaryMethodSelectCB.SelectedIndex == 1)
                    {
                        UInt16 Threshold = IDF.FindThreshold_OTSUNormal(GrayImage, 10);
                        BestThresholdText.Text = Threshold.ToString();
                        Bitmap BinaryImage = IDF.GrayBinary_GlobalThreshold(GrayImage, Convert.ToUInt16(Threshold));
                        
                        Monitor.Enter(Lock_DealedImage2PB);
                        DealedImage2PB.Image = BinaryImage;
                        Monitor.Exit(Lock_DealedImage2PB);
                    }
                    Flag_ImageDeal = false;
                }                
            }
        }
        /// <summary>
        /// 测试按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (ImageDealThread == null || ImageDealThread.ThreadState == System.Threading.ThreadState.Unstarted)
            {
                ImageDealThread = new Thread(ImageDeal);
                ImageDealThread.Start();
                UsedUARTCommunication.DataBagReadFinish = false;
            }
            else
            {
                ImageDealThread.Resume();
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

            skinTabControl1.SelectedIndex = 0;

            VO = new VirtualOscilloscope(ScopeChart);

            if (Debugger.IsAttached)
                AllocConsole();
            UartDataDecoding = new Thread(UsedUARTCommunication.DataDecoding);
            UartDataDecoding.Start();

            ImageRefresh = new Thread(ImageRenew);
            ImageDealThread = new Thread(ImageDeal);
            ScopeRenewThread = new Thread(ScopeDataRenew);
            Random rd = new Random(0);
            #region 生成Chart测试数据
            for (int i = 0; i < 100; i++)
            {
                ScopeChart.Series[0].Points.AddXY(i, rd.Next(100));                
            }
            rd = new Random(5);
            for (int i = 100; i < 1000; i++)
            {
                ScopeChart.Series[0].Points.AddXY(i, rd.Next(1000));
            }
            rd = new Random(500);
            for (int i = 1000; i < 1100; i++)
            {
                ScopeChart.Series[0].Points.AddXY(i, -rd.Next(100));
            }
            # endregion
            ScopeChartInit();
        }
        /// <summary>
        /// 虚拟示波器图表初始化
        /// </summary>
        private void ScopeChartInit()
        {
            double t_Min = 0;
            double t_Max = 500;
            double d_Min = -500;
            double d_Max = 500;

            VO.ShowAreaConfigList[0].ConfigShowArea(t_Min, t_Max, d_Min, d_Max);

            ScopeChart.ChartAreas[0].Axes[0].MajorGrid.LineColor = Color.Transparent;
            ScopeChart.ChartAreas[0].Axes[1].MajorGrid.LineColor = Color.Transparent;
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
        /// 串口接收事件
        /// </summary>
        private void UART_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            //Console.WriteLine("********进入串口线程********");
            if (!UsedUart.sp.IsOpen)
                return;
            byte[] ReceivedBuff = new byte[UsedUart.sp.BytesToRead];
            UsedUart.sp.Read(ReceivedBuff, 0, ReceivedBuff.Length);
            bool Select_UARTTabPage = false;
            bool HexCBChecked = false;
            skinTabControl1.Invoke(new EventHandler(delegate
            {
                Select_UARTTabPage = (skinTabControl1.SelectedTab == UART_TabPage);                
            }));
            HexCB.Invoke(new EventHandler(delegate
            {
                HexCBChecked = HexCB.Checked;
            }));
            
            if (Select_UARTTabPage)
            {
                if (!HexCBChecked)
                {
                    string str1 = Encoding.UTF8.GetString(ReceivedBuff);
                    str1 = str1.Replace("\\r", "\r");
                    str1 = str1.Replace("\\n", "\n");
                    ReceiveTB.AppendText(str1);
                }
                else
                {
                    string str_byte = Communication.ByteArrToStr(ReceivedBuff);
                    ReceiveTB.AppendText(str_byte);
                }
            }
            try
            {
                UsedUARTCommunication.RWLock_ReceivedBuff.EnterWriteLock();
                
                for (int i = 0; i < ReceivedBuff.Length; i++)
                    UsedUARTCommunication.ReceivedBuff.Add(ReceivedBuff[i]);
            }
            finally
            {
                UsedUARTCommunication.RWLock_ReceivedBuff.ExitWriteLock();
            }

            if (UsedUARTCommunication.NowDecodingStatus == DecodingStatus.Decoded)
                UsedUARTCommunication.NowDecodingStatus = DecodingStatus.Check_BagBeginning;
            if (HexCB.Checked)
            {
                //Console.WriteLine("串口线程接受的数据包：");
                //lock (this)
                //{
                    //UsedUARTCommunication.PrintByteStrWithByteArr(UsedUARTCommunication.ReceivedBuff);
                //}
            }
            else
                ;// Console.WriteLine(System.Text.Encoding.Default.GetString(ReceivedBuff));
            
            //Console.WriteLine("********退出串口线程********");
        }
        /// <summary>
        /// 用来切换串口接收或者串口不接收
        /// </summary>
        private void SwitchReceiveBTN_Click_1(object sender, EventArgs e)
        {
            if (SwitchReceiveBTN.Text == "停止接收")
            {
                NowUartReceiveStatus = UARTReceiveStatus.DataReceiveFinish;
                if (UsedUart.SwtichSP(false) == true)//关闭成功
                {
                    SwitchReceiveBTN.Text = "开始接收";
                    NowUartReceiveStatus = UARTReceiveStatus.SerialPortClosed;
                    NowUartSendStatus = UARTSendStatus.SerialPortClosed;

                    SaveDataBTN.Enabled = true;
                }
                else
                {
                    MessageBox.Show("关闭串口失败！");
                }
                return;
            }
            if (NowUartReceiveStatus == UARTReceiveStatus.SerialPortClosed || NowUartSendStatus == UARTSendStatus.SerialPortClosed)
            {
                //UsedUart.SetSerialPort(PortCB.Text, int.Parse(BaudCB.Text), int.Parse(DataBitsCB.Text), int.Parse(StopBitsCB.Text));
                if (UsedUart.SwtichSP(true) == true)//打开成功
                {
                    UsedUart.sp.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(UART_DataReceived);

                    NowUartReceiveStatus = UARTReceiveStatus.SerialPortOpen;
                    NowUartSendStatus = UARTSendStatus.SerialPortOpen;

                    SaveDataBTN.Enabled = false;
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
                //#region 打开多线程定时器
                ////创建代理对象TimerCallback，该代理将被定时调用
                //TimerCallback timerDelegate = new TimerCallback(TimerRecive);
                ////创建一个时间间隔为1s的定时器
                //System.Threading.Timer timer = new System.Threading.Timer(timerDelegate, s, 0, 10);
                //s.tmr = timer;
                //#endregion

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
                    byte[] Receivebuff = new byte[50000];
                    int ReceiveNum = UsedUart.sp.Read(Receivebuff, 0, UsedUart.sp.BytesToRead);

                    if (ReceiveNum > 0)
                    {
                        for (int i = 0; i < ReceiveNum; i++)
                        {
                            int hexnum1 = Receivebuff[i] / 16;
                            int hexnum2 = Receivebuff[i] % 16;

                            strRecieve += Communication.IntToHexChar(hexnum1);
                            strRecieve += Communication.IntToHexChar(hexnum2);
                            strRecieve += " ";

                            //strRecieve += Receivebuff[i];
                        }

                        //Console.WriteLine(strRecieve);
                        ReceiveTB.AppendText(strRecieve);
                    }

                    //Console.WriteLine(Receivebuff.ToString());
                }
                else
                {
                    strRecieve = UsedUart.sp.ReadExisting();
                    if (strRecieve != "")
                    {
                        //Console.WriteLine(strRecieve);
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
            UartDataDecoding.Abort();
            UartDataDecoding.Join();
            Application.Exit();
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
                        byte num1 = Convert.ToByte(Communication.HexCharToByte(strbuff[i].ToString()) * 16);
                        byte num2 = Communication.HexCharToByte(strbuff[i + 1].ToString());
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
        /// <summary>
        /// 用于打开文件并保存数据
        /// </summary>
        private void SaveDataBTN_Click(object sender, EventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog();//创建打开文件对话框对象
            //SFD.Title = "请选择文件夹";
            SFD.Filter = "文本文件(*.txt)|*.txt";
            SFD.RestoreDirectory = true;

            string FilePath = "";
            string fileNameExt = "";
            if (SFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FilePath = SFD.FileName;//路径+文件名
                fileNameExt = FilePath.Substring(FilePath.LastIndexOf("\\") + 1);//文件名不含路径

                FileStream FS = new FileStream(FilePath, FileMode.Append, FileAccess.Write);
                byte[] data = System.Text.Encoding.Default.GetBytes(ReceiveTB.Text);

                FS.Write(data, 0, data.Length);
                FS.Flush();
                FS.Close();
                MessageBox.Show("保存成功！");
            }
        }

        private void TestBTN2_Click(object sender, EventArgs e)
        {
            
            //Console.WriteLine(Array.IndexOf(DataBag, 0x55).ToString());
        }

        private void skinTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (skinTabControl1.SelectedTab == ImagePreDealTP)
            {
                if (ImageRefresh.ThreadState == System.Threading.ThreadState.Unstarted)
                {
                    ImageRefresh = new Thread(ImageRenew);
                    ImageRefresh.Start();
                }
                else
                    ImageRefresh.Resume();

                if (ScopeRenewThread != null && ScopeRenewThread.ThreadState != System.Threading.ThreadState.Unstarted)
                    ScopeRenewThread.Suspend();
            }
            else if (skinTabControl1.SelectedTab == ScopeTabPage)
            {
                if (ScopeRenewThread.ThreadState == System.Threading.ThreadState.Unstarted)
                {
                    ScopeRenewThread = new Thread(ScopeDataRenew);
                    ScopeRenewThread.Start();
                }
                else
                    ScopeRenewThread.Resume();
                if (ImageRefresh != null && ImageRefresh.ThreadState != System.Threading.ThreadState.Unstarted)
                    ImageRefresh.Suspend();
                if (ImageDealThread != null && ImageDealThread.ThreadState != System.Threading.ThreadState.Unstarted)
                    ImageDealThread.Suspend();
            }
            else
            {
                if (ImageRefresh != null && ImageRefresh.ThreadState != System.Threading.ThreadState.Unstarted)
                    ImageRefresh.Suspend();
                if (ImageDealThread != null && ImageDealThread.ThreadState != System.Threading.ThreadState.Unstarted)
                    ImageDealThread.Suspend();
                if (ScopeRenewThread != null && ScopeRenewThread.ThreadState != System.Threading.ThreadState.Unstarted)
                    ScopeRenewThread.Suspend();
            }
        }
        /// <summary>
        /// 图像控件更新
        /// </summary>
        public void ImageRenew()
        {
            Console.WriteLine("ImageRenew线程开启");
            while (true)
            {
                if (UsedUARTCommunication.NowDecodingFunction != FunctionType.CameraSend)
                    continue;
                bool IfRefresh = false;

                try
                {
                    UsedUARTCommunication.RWLock_DataBag.EnterReadLock();
                    if (UsedUARTCommunication.DataBag.Count == UsedUARTCommunication.DataBagLength && 
                        UsedUARTCommunication.DataBag.Count > 0)
                    {
                        int Width = UsedUARTCommunication.ParaList[1];
                        int Height = UsedUARTCommunication.ParaList[0];
                        Bitmap BTBuff = new Bitmap(Width, Height);

                        for (int i = 0; i < Height; i++)//Height
                        {
                            for (int j = 0; j < Width; j++)//Width
                            {
                                int Pixel = Convert.ToInt32(
                                    UsedUARTCommunication.DataBag[i * Width + j]);
                                Color GrayPixel = Color.FromArgb(Pixel, Pixel, Pixel);
                                BTBuff.SetPixel(i, j, GrayPixel);
                            }
                        }
                        Monitor.Enter(Lock_InitalImagePB);
                        InitalImagePB.Image = BTBuff;                            
                        Monitor.Exit(Lock_InitalImagePB);
                        UsedUARTCommunication.DataBag.Clear();
                        UsedUARTCommunication.DataBagReadFinish = false;
                        Flag_ImageDeal = true;
                        IfRefresh = true;
                    }
                }
                finally
                {
                    UsedUARTCommunication.RWLock_DataBag.ExitReadLock();
                }
                if (IfRefresh)
                {
                    Monitor.Enter(Lock_InitalImagePB);
                    InitalImagePB.Refresh();
                    Monitor.Exit(Lock_InitalImagePB);
                }
            }
        }
        /// <summary>
        /// 虚拟示波器Chart数据更新线程函数
        /// </summary>
        public void ScopeDataRenew()
        {
            Console.WriteLine("ScopeDataRenew线程开启");
            while (true)
            {
                if (UsedUARTCommunication.NowDecodingFunction != FunctionType.Oscilloscope)
                    continue;
                try
                {
                    UsedUARTCommunication.RWLock_DataBag.EnterReadLock();
                    if (UsedUARTCommunication.DataBag.Count == UsedUARTCommunication.DataBagLength && 
                        UsedUARTCommunication.DataBag.Count > 0)
                    {
                        int DataChannelNum = UsedUARTCommunication.ParaList[0];
                        DataType_t DataType = (DataType_t)(UsedUARTCommunication.ParaList[1]);

                        if (!UsedUARTCommunication.DataTypeSizeDic.ContainsKey(DataType))
                            throw(new Exception());

                        int DataSize = UsedUARTCommunication.DataTypeSizeDic[DataType];
                        
                        for (int i = 0, index = 0; i < DataChannelNum; i++, index += DataSize)
			            {
                            double DataBuff = 0;
                            switch (DataType)
                            {
                                case DataType_t.float_dt:
                                    DataBuff = BitConverter.ToSingle(
                                        UsedUARTCommunication.DataBag.ToArray(), index);
                                    break;
                                default:
                                    break;
                            }
                            ScopeChart.Series[i].Points.AddXY(VO.TimeCount, DataBuff);
                            ulong t_Max = VO.TimeCount > 15 ? VO.TimeCount : 15;
                            ulong t_Min = VO.TimeCount >= 15 ? VO.TimeCount - 15 : 0;
                            VO.ShowAreaConfigList[0].ConfigShowArea(t_Min, t_Max, 0, 100);
			            }
                        VO.TimeCount++;
                        //float BitConverter.ToSingle(new byte[],)
                        UsedUARTCommunication.DataBag.Clear();
                        UsedUARTCommunication.DataBagReadFinish = false;
                    }
                }
                finally
                {
                    UsedUARTCommunication.RWLock_DataBag.ExitReadLock();
                }
            }
        }
        private void ScopeChart_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ScopeMenuStrip.Show(MousePosition.X, MousePosition.Y);
            }
        }
        # region ScopeChart鼠标按键逻辑
        enum MouseStatus
        {
            Down,
            Moving,
            Up
        }
        MouseStatus LeftMouseStatus = MouseStatus.Up;
        MouseStatus MiddleMouseStatus = MouseStatus.Up;
        Point MouseLocation_Last = new Point(0, 0);
        Point MouseLocation_This = new Point(0, 0);
        double t_min_last = 0;
        double t_max_last = 0;
        double d_min_last = 0;
        double d_max_last = 0;
        private void ScopeChart_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (LeftMouseStatus == MouseStatus.Up)
                        LeftMouseStatus = MouseStatus.Moving;
                    break;
                case MouseButtons.Middle:
                    if (MiddleMouseStatus == MouseStatus.Up)
                        MiddleMouseStatus = MouseStatus.Moving;

                    t_min_last = VO.ShowAreaConfigList[0].Time_Min;
                    t_max_last = VO.ShowAreaConfigList[0].Time_Max;
                    d_min_last = VO.ShowAreaConfigList[0].Data_Min;
                    d_max_last = VO.ShowAreaConfigList[0].Data_Max;
                    break;
                default:
                    break;
            }
            MouseLocation_Last = e.Location;
            MouseLocation_This = e.Location;
        }

        private void ScopeChart_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (LeftMouseStatus == MouseStatus.Moving)
                        LeftMouseStatus = MouseStatus.Up;
                    break;
                case MouseButtons.Middle:
                    if (MiddleMouseStatus == MouseStatus.Moving)
                        MiddleMouseStatus = MouseStatus.Up;
                    break;
                default:
                    break;
            }
        }

        private void ScopeChart_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left &&
                e.Button != System.Windows.Forms.MouseButtons.Middle)
                return;
            
            MouseLocation_This = e.Location;
            int x_diff = MouseLocation_This.X - MouseLocation_Last.X;
            int y_diff = MouseLocation_This.Y - MouseLocation_Last.Y;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)//左键拖动平移
            {
                VO.ShowAreaConfigList[0].PanChange(x_diff, y_diff, ScopeChart.Size);
                MouseLocation_Last = MouseLocation_This;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                VO.ShowAreaConfigList[0].PanChange(x_diff, y_diff, ScopeChart.Size);
                double x_changerate = 0.01;
                double y_changerate = 0.01;

                double x_ChangedValue = 1 + Convert.ToDouble(y_diff) * x_changerate;
                double y_ChangedValue = 1 + Convert.ToDouble(y_diff) * y_changerate;

                double t_Min = x_ChangedValue * t_min_last;
                double t_Max = x_ChangedValue * t_max_last;
                double d_Min = y_ChangedValue * d_min_last;
                double d_Max = y_ChangedValue * d_max_last;

                VO.ShowAreaConfigList[0].ConfigShowArea(t_Min, t_Max, d_Min, d_Max);
            }
        }
        #endregion
    }
}
