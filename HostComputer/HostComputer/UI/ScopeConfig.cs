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

namespace HostComputer.UI
{
    public partial class ScopeConfig : Skin_Mac
    {
        /// <summary>
        /// 曲线数量
        /// </summary>
        int SeriesCount = 1;
        public ScopeConfig(int SeriesCountSet)
        {
            InitializeComponent();
            SeriesCount = SeriesCountSet;
        }
    }
}
