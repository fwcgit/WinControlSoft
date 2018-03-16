using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ControlSoft.src.usart
{
    class Usart
    {
        private string port;
        private SerialPort serialPort;
        private Thread dataThread;
        private bool isReceive = false;
        public DataCallBack dataCallBack;
        private bool start = false;
        private bool stop = false;
        private int dataIndex;
        public void init(string port)
        {
            this.port = port;
        }

        public void configUsart()
        {
            serialPort = new SerialPort();
            serialPort.PortName = port;
            serialPort.BaudRate = 115200;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Parity = Parity.None;
            
        }

        public bool open()
        {
            if (null != serialPort)
            {
                serialPort.Open();
                return serialPort.IsOpen;
            }

            return false;
                
        }

        public void close()
        {
            if (null != serialPort)
            {
                serialPort.Close();
                isReceive = false;
                dataThread = null;
            }

        }

        public interface DataCallBack
        {
            void receiveData(byte[] buffer,int count);
        }
    
        public void setReceiveListener(DataCallBack callback)
        {
            this.dataCallBack = callback;
        }

        public bool isOpen()
        {
            if (null != serialPort)
            {
            
                return serialPort.IsOpen;
            }

            return false;
        }

        public void startReceiveData()
        {
            if(null == dataThread || !isReceive)
            {
                isReceive = true;
                dataThread = new Thread(new ThreadStart(SerialDataHandler));
                dataThread.Start();
            }
            
        }

        public void write(byte[] data)
        {
            if(null != serialPort)
            {
                serialPort.Write(data, 0, data.Length);
            }
        }

        public  void SerialDataHandler()
        {
            byte[] buffer = new byte[100];

            while (isReceive)
            {
                if(null != serialPort && serialPort.IsOpen)
                {
                    
                    byte data =(byte)(serialPort.ReadByte() & 0xff);

                    if(dataIndex >= 100)
                    {
                        start = false;
                        stop = false;
                        dataIndex = 0;
                    }

                    if(data == 0x3b && !start)
                    {
                        start = true;
                        stop = false;
                        dataIndex = 0;
                        buffer[dataIndex] = data;

                    }else if(data == 0x0d && start)
                    {
                        start = false;
                        stop = true;
                        buffer[dataIndex] = data;
                    }
                    else if(start)
                    {
                        buffer[dataIndex] = data;
                    }

                    if (stop)
                    {
                        if (null != dataCallBack) dataCallBack.receiveData(buffer, dataIndex);
                    }
                   
                    dataIndex++;

                }
            }
        }
    }
}
