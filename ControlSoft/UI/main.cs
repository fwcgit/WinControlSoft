using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ControlSoft.src.bean;
using ControlSoft.src.config;
using ControlSoft.src.usart;
using ControlSoft.UI;

namespace ControlSoft
{
    public partial class wMain : Form
    {
        private SynchronizationContext synchronizationContext;
        private Button[] Water1Lev = new Button[5];
        private Button[] Water2Lev = new Button[5];
        private Button[] HotLev = new Button[5];

        public wMain()
        {
            InitializeComponent();
            synchronizationContext = SynchronizationContext.Current;

            Water1Lev[0] = buttonWaterLow;
            Water1Lev[1] = buttonWaterMid;
            Water1Lev[2] = buttonWaterHigh;
            Water1Lev[3] = buttonWaterAuto;
            Water1Lev[4] = buttonWaterClose;

            Water2Lev[0] = buttonWaterTwoLow;
            Water2Lev[1] = buttonWaterTwoMid;
            Water2Lev[2] = buttonWaterTwoHigh;
            Water2Lev[3] = buttonWaterTwoAuto;
            Water2Lev[4] = buttonWaterTwoClose;

            HotLev[0] = buttonHotLow;
            HotLev[1] = buttonHotMid;
            HotLev[2] = buttonHotHigh;
            HotLev[3] = buttonHotAuto;
            HotLev[4] = buttonHotClose;
        }

        private void wMain_Load(object sender, EventArgs e)
        {
            
            for (int i= 1; i <= 9; i++)
            {
                
           
                ((Label)(this.Controls.Find("label" + i, true)[0])).Text = i+"、" + AppConfig.appConfig.getTempName(i + "");

            }

            this.listView1.Columns.Add("进程名", listView1.Width - 5, HorizontalAlignment.Center); //一步添加

            SoftList softList = AppConfig.appConfig.getSoftMonitoring();

            listView1.BeginUpdate();
            foreach (SoftBean soft in softList.softs)
            {
                ListViewItem lvi = new ListViewItem();

                lvi.Text = soft.name;
                listView1.Items.Add(lvi);

            }

            listView1.EndUpdate();

            string[] serialPortList = UsartManager.usartManager.getHostSerialPort();
            comboBox1.Items.AddRange(serialPortList);
            comboBox1.SelectedIndex = 0;

            DataParser.parser.setUpdate(upStatusData);
        }

        private void 温度名称ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new TempName().ShowDialog();
           
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

        public void upStatusData(DeviceStatu statu)
        {
            synchronizationContext.Post(UpdateUi, statu);
        }

        public void UpdateUi(object statu)
        {
            DeviceStatu deviceStatu = (DeviceStatu)statu;
            labelTemp1.Text = deviceStatu.temp1 + "℃";
            labelTemp2.Text = deviceStatu.temp2 + "℃";
            labelTemp3.Text = deviceStatu.temp3 + "℃";
            labelTemp4.Text = deviceStatu.temp4 + "℃";
            labelTemp5.Text = deviceStatu.temp5 + "℃";
            labelTemp6.Text = deviceStatu.temp6 + "℃";
            labelTemp7.Text = deviceStatu.temp7 + "℃";
            labelTemp8.Text = deviceStatu.temp8 + "℃";
            labelTemp9.Text = deviceStatu.temp9 + "℃";

            upWaterLev1(deviceStatu.waterMLev1);
            upWaterLev2(deviceStatu.waterMLev2);
            upHotLev1(deviceStatu.hotLev);
        }

        private void upWaterLev1(int val)
        {
            int select = getLev(val);

            for(int i = 0;i < Water1Lev.Length; i++)
            {
                Button btn = Water1Lev[i];
                if(select == i)
                {
                    btn.BackColor = Color.Blue;
                }
                else
                {
                    btn.BackColor = Control.DefaultBackColor;
                }
            }
        }

        private void upWaterLev2(int val)
        {
            int select = getLev(val);

            for (int i = 0; i < Water2Lev.Length; i++)
            {
                Button btn = Water2Lev[i];
                if (select == i)
                {
                    btn.BackColor = Color.Blue;
                }
                else
                {
                    btn.BackColor = Control.DefaultBackColor;
                }
            }
        }

        private void upHotLev1(int val)
        {

            int select = getLev(val);

            for (int i = 0; i < HotLev.Length; i++)
            {
                Button btn = HotLev[i];
                if (select == i)
                {
                    btn.BackColor = Color.Blue;
                }
                else
                {
                    btn.BackColor = Control.DefaultBackColor;
                }
            }
        }

        private int getLev(int val)
        {
            int lev = 0;
            if(val > 0 && val <= 3)
            {
                lev = 0;
            }
            else if(val > 3 && val <= 6)
            {
                lev = 1;
            }
            else if (val > 6 && val <= 10)
            {
                lev = 2;
            }
            else if (val >= 11 && val < 12)
            {
                lev = 3;
            }
            else if (val >= 12)
            {
                lev =  4;
            }
            return lev;
        }

        private void buttonHotHigh_Click(object sender, EventArgs e)
        {
            UsartManager.usartManager.writeData(TelProtocol.CmdMLev(TelProtocol.PROTOCOL_CMD_HOT, 10));
        }

        private void buttonHotMid_Click(object sender, EventArgs e)
        {
            UsartManager.usartManager.writeData(TelProtocol.CmdMLev(TelProtocol.PROTOCOL_CMD_HOT, 6));

        }

        private void buttonHotLow_Click(object sender, EventArgs e)
        {
            UsartManager.usartManager.writeData(TelProtocol.CmdMLev(TelProtocol.PROTOCOL_CMD_HOT, 3));

        }

        private void buttonHotAuto_Click(object sender, EventArgs e)
        {
            UsartManager.usartManager.writeData(TelProtocol.CmdMLev(TelProtocol.PROTOCOL_CMD_HOT, 11));

        }

        private void buttonHotClose_Click(object sender, EventArgs e)
        {
            UsartManager.usartManager.writeData(TelProtocol.CmdMLev(TelProtocol.PROTOCOL_CMD_HOT, 12));

        }

        private void buttonWaterHigh_Click(object sender, EventArgs e)
        {
            UsartManager.usartManager.writeData(TelProtocol.CmdMLev(TelProtocol.PROTOCOL_CMD_WATER_M_1, 10));

        }

        private void buttonWaterMid_Click(object sender, EventArgs e)
        {
            UsartManager.usartManager.writeData(TelProtocol.CmdMLev(TelProtocol.PROTOCOL_CMD_WATER_M_1, 6));

        }

        private void buttonWaterLow_Click(object sender, EventArgs e)
        {
            UsartManager.usartManager.writeData(TelProtocol.CmdMLev(TelProtocol.PROTOCOL_CMD_WATER_M_1, 3));

        }

        private void buttonWaterAuto_Click(object sender, EventArgs e)
        {
            UsartManager.usartManager.writeData(TelProtocol.CmdMLev(TelProtocol.PROTOCOL_CMD_WATER_M_1, 11));

        }

        private void buttonWaterClose_Click(object sender, EventArgs e)
        {
            UsartManager.usartManager.writeData(TelProtocol.CmdMLev(TelProtocol.PROTOCOL_CMD_WATER_M_1, 12));

        }

        private void buttonWaterTwoHigh_Click(object sender, EventArgs e)
        {
            UsartManager.usartManager.writeData(TelProtocol.CmdMLev(TelProtocol.PROTOCOL_CMD_WATER_M_2, 10));

        }

        private void buttonWaterTwoMid_Click(object sender, EventArgs e)
        {
            UsartManager.usartManager.writeData(TelProtocol.CmdMLev(TelProtocol.PROTOCOL_CMD_WATER_M_2, 6));
        }

        private void buttonWaterTwoLow_Click(object sender, EventArgs e)
        {
            UsartManager.usartManager.writeData(TelProtocol.CmdMLev(TelProtocol.PROTOCOL_CMD_WATER_M_2, 3));
        }

        private void buttonWaterTwoAuto_Click(object sender, EventArgs e)
        {
            UsartManager.usartManager.writeData(TelProtocol.CmdMLev(TelProtocol.PROTOCOL_CMD_WATER_M_2, 11));
        }

        private void buttonWaterTwoClose_Click(object sender, EventArgs e)
        {
            UsartManager.usartManager.writeData(TelProtocol.CmdMLev(TelProtocol.PROTOCOL_CMD_WATER_M_2, 12));
        }

        private void 软件监控ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SoftMonitoring softMonitoring = new SoftMonitoring();
            softMonitoring.ShowDialog();
        }

        private void 固件升级ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FirmwareDownload fd = new FirmwareDownload();
            fd.ShowDialog();
        }
    }



}
