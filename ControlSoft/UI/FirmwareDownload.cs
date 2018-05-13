using ControlSoft.src.firmware;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlSoft.UI
{
    public partial class FirmwareDownload : Form 
    {
        bool isDownload = false;
        private SynchronizationContext synchronizationContext;
        public FirmwareDownload()
        {
            InitializeComponent();
            synchronizationContext = SynchronizationContext.Current;
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Title = "选择固件文件";
            dialog.Filter = "固件文件(*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {

                labPath.Text = dialog.FileName;
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {

            if (!isDownload)
            {
                isDownload = true;
                TelFirmware.rf.setDownloadCallback(new DownloadCallback(synchronizationContext,progressBar1));
                TelFirmware.rf.start(labPath.Text);
            }
            else
            {
                isDownload = false;
                TelFirmware.rf.stop();
            }
        }


        public class DownloadCallback : TelFirmware.IDownload
        {
            private SynchronizationContext synchronizationContextb;
            private ProgressBar pb;
            public DownloadCallback(SynchronizationContext ctx, ProgressBar pb)
            {
                this.synchronizationContextb = ctx;
                this.pb = pb;
            }


            public void updateProgress(Object per)
            {
                if((int)per == -2)
                {
                    pb.Value = 0;
                }
                else
                {
                    pb.Value = (int)per;
                }

               

            }
            public void progress(long val, long max)
            {
                float pro = val / max;
                int per = (int)(pro * 100.0);

                if (val == max)
                {
                    per = -2;
                }
                synchronizationContextb.Post(updateProgress,per);
                
            }

            public void start(long max)
            {
                
            }
        }
    }
}
