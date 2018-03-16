using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ControlSoft.src.config;
using ControlSoft.src.usart;
using ControlSoft.UI;

namespace ControlSoft
{
    public partial class wMain : Form
    {
        public wMain()
        {
            InitializeComponent();
        }

        private void wMain_Load(object sender, EventArgs e)
        {
            
            for (int i= 1; i <= 9; i++)
            {
                
           
                ((Label)(this.Controls.Find("label" + i, true)[0])).Text = i+"、" + AppConfig.appConfig.getTempName(i + "");

            }

            string[] serialPortList = UsartManager.usartManager.getHostSerialPort();
            comboBox1.Items.AddRange(serialPortList);
            comboBox1.SelectedIndex = 0;


        }

        private void 温度名称ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new TempName().Show();
           
        }

        private void setTempName(Control control)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text.Equals("连接"))
            {
                UsartManager.usartManager.openSerialPort(comboBox1.Text);
               
            }
            else
            {
                UsartManager.usartManager.closeSerialPort();
            }

            bool open = UsartManager.usartManager.isSerialPortOpen();
            if (open)
            {
                UsartManager.usartManager.startReceiveData();

            }
            toolConnectStatus.Text = open ? "已连接" : "未连接";
            button1.Text = open ? "断开" : "连接";
        }
    }


}
