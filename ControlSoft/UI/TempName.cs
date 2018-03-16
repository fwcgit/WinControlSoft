using ControlSoft.src.config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlSoft.UI
{
    public partial class TempName : Form
    {
        public TempName()
        {
            InitializeComponent();
        }

        private void TempName_Load(object sender, EventArgs e)
        {
            for(int i = 1; i <= 9; i++)
            {

                ((TextBox)(this.Controls.Find("textBox" + i, false)[0])).Text = AppConfig.appConfig.getTempName(i+"");
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= 9; i++)
            {
                string name = ((TextBox)(this.Controls.Find("textBox" + i, false)[0])).Text;
                AppConfig.appConfig.setTempName(i + "", name);
            }

            MessageBox.Show("修改成功");

            Close();
        }
    }
}
