using ControlSoft.src.usart;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ControlSoft.src.firmware
{
    

    class TelFirmware : Usart.DataCallBack
    {

        public interface IDownload
        {
            void progress(long val, long max);
            void start(long max);

        }
        public static TelFirmware rf = new TelFirmware();
        public IDownload downloadCallback;
        private string filePath;
        private Thread thread;
        private bool readNext = false;
        private bool isRun = false;

        private TelFirmware()
        {
            UsartManager.usartManager.addDataListener(this);
        }

        public void setDownloadCallback(IDownload callback)
        {
            this.downloadCallback = callback;
        }

        public void stop()
        {
            if (thread != null)
            {
                isRun = false;
                Thread.Sleep(500);
            }
        }
        public void start(string path)
        {
            this.filePath = path;

            if (thread != null)
            {
                isRun = false;
                Thread.Sleep(500);
            }

            thread = new Thread(new ThreadStart(fileDownload));
            readNext = false;
            isRun = true;
            thread.Start();

           
            byte[] data = System.Text.Encoding.ASCII.GetBytes("firmware update");
            UsartManager.usartManager.writeData(data, data.Length);
        }

        private void callbackProgress(long val, long max)
        {
            if (val != -1)
            {
                if (downloadCallback != null)
                {
                    downloadCallback.progress(val, max);
                }
            }
            else
            {
                if (downloadCallback != null)
                {
                    downloadCallback.start(max);
                }
            }
        }

        public void fileDownload()
        {

            long total = 0;
            FileStream fs = new FileStream(filePath, FileMode.Open);
            callbackProgress(-1, fs.Length);

            while (isRun)
            {
                Thread.Sleep(100);

                if (readNext)
                {
                    readNext = false;

                    byte[] data = new byte[2048 + 1 + 1 + 2];
                    data[0] = 0xa0;
                    data[1] = 0x80;

                    int count = fs.Read(data, 4, 2048);
                    total = total + count;
                    callbackProgress(total, fs.Length);

                    data[2] = (byte)(count & 0x000000ff);
                    data[3] = (byte)((count>>8) & 0x000000ff);

                    if (count <= 0)
                    {
                        data[1] = 0x1a;
                        data[2] = 0;
                        data[3] = 0;

                        isRun = false;

                    }
                    UsartManager.usartManager.writeData(data,4+count);
                }
            }

            fs.Close();
        }

        public void receiveData(byte[] buffer, int count)
        {
            if (buffer[0] == 0xa0)
            {
                if (buffer[1] == 0x80)
                {
                    readNext = true;
                }
            }
        }
    }
}
