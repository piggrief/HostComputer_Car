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

using UartComunication;

namespace HostComputer.UI
{
    public partial class UARTConfig : Skin_Mac
    {
        public UART UARTToConfig = new UART();

        public UARTConfig()
        {
            InitializeComponent();
        }
        public UARTConfig(UART ConfigUart)
        {
            InitializeComponent();
            UARTToConfig = ConfigUart;
        }

        private void BaudLabel_Click(object sender, EventArgs e)
        {

        }
        private void UARTConfigSomeInit()
        {            
            BaudComboBox.SelectedIndex = 2;//115200
        }
        private void UARTConfig_Load(object sender, EventArgs e)
        {
            UARTConfigSomeInit();
        }
    }
}
