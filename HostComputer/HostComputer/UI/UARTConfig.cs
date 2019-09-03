using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Windows.Forms;

using CCWin;

using UartComunication;

namespace HostComputer.UI
{
    public partial class UARTConfig : Skin_Mac
    {
        public HostComputerForm MainForm;
        public UARTConfig()
        {
            InitializeComponent();
        }
        public UARTConfig(HostComputerForm FormBuff)
        {
            InitializeComponent();
            MainForm = FormBuff;
        }

        private void BaudLabel_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 串口端口号Combox刷新
        /// </summary>
        public void PortComboBoxFresh()
        {
            string[] ports = SerialPort.GetPortNames();
            PortComboBox.Items.Clear();
            foreach (string port in ports)
            {
                PortComboBox.Items.Add(port);
            }
            if (PortComboBox.Items.Count == 0)
            {
                PortComboBox.Text = "";
            }
        }
        /// <summary>
        /// 一些控件初始化
        /// </summary>
        private void UARTConfigSomeInit()
        {            
            BaudComboBox.SelectedIndex = 2;//115200
            DataBitComboBox.SelectedIndex = 0;
            StopBitComboBox.SelectedIndex = 0;
            CheckBitComboBox.SelectedIndex = 0;
            PortComboBoxFresh();
            if (PortComboBox.Items.Count > 0)
                PortComboBox.SelectedIndex = 0;
        }
        private void UARTConfig_Load(object sender, EventArgs e)
        {
            UARTConfigSomeInit();
        }
        /// <summary>
        /// 下拉端口选择框更新端口号
        /// </summary>
        private void PortComboBox_Click(object sender, EventArgs e)
        {
            PortComboBoxFresh();
        }
        /// <summary>
        /// 配置恢复默认
        /// </summary>
        private void ConfigDefaultBTN_Click(object sender, EventArgs e)
        {
            UARTConfigSomeInit();
        }
        /// <summary>
        /// 根据相关控件的选项更新UART对象
        /// </summary>
        private void ConfigUARTInfo()
        {
            MainForm.UsedUart.SetSerialPort(PortComboBox.Text,
                                        int.Parse(BaudComboBox.Text),
                                        int.Parse(DataBitComboBox.Text),
                                        int.Parse(StopBitComboBox.Text));
            MainForm.toolStripLabel1.Text = "端口号：" + PortComboBox.Text + " | ";
            MainForm.toolStripLabel2.Text = "波特率：" + BaudComboBox.Text + " | ";
            MainForm.toolStripLabel3.Text = "数据位：" + DataBitComboBox.Text + " | ";
            MainForm.toolStripLabel4.Text = "停止位：" + StopBitComboBox.Text + " | ";
            MainForm.toolStripLabel5.Text = "";
        }

        private void ConfigOKBTN_Click(object sender, EventArgs e)
        {
            ConfigUARTInfo();
            this.Close();
        }

        private void ConfigCancelBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
