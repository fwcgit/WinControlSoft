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
        private bool start      = false;
        private bool stop       = false;
        List<DataCallBack> calllbacks = new List<DataCallBack>();

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

        public void addCallBak(DataCallBack callback)
        {
             if(!calllbacks.Contains(callback))
            {
                calllbacks.Add(callback);
            }
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

        public void write(byte[] data,int len)
        {
            if (null != serialPort)
            {
                serialPort.Write(data, 0, len);
            }
        }

        public  void SerialDataHandler()
        {
            byte[] buffer = new byte[4096];
            int datalen = 0;

            while (isReceive)
            {
                if(null != serialPort && serialPort.IsOpen)
                {

                    byte data = 0;
                    try
                    {
                        data = (byte)(serialPort.ReadByte() & 0xff);
                        System.Console.Out.WriteLine("0x{0:X} ", data);
                    }
                    catch(Exception e)
                    {
                        System.Console.Out.WriteLine(e.Message);
                    }
                  
                    if(dataIndex >= 4096)
                    {
                        start = false;
                        stop = false;
                        dataIndex = 0;
                    }

                    if (start)
                    {
                        buffer[dataIndex] = data;
                    }

                    if (data == 0xa0 && !start)
                    {
                        start = true;
                        stop = false;
                        dataIndex = 0;
                        datalen = 0;
                        buffer[dataIndex] = data;
                    }
                    else if (start && dataIndex == 3)
                    {
                        datalen = buffer[3];
                        datalen = datalen << 8;
                        datalen = datalen | buffer[2];
                    }

                    if (start && dataIndex >= 3 && dataIndex - 3 == datalen)
                    {
                        stop = true;
                    }

                    if (stop)
                    {
                        start = false;
                        stop = false;
                        
                        foreach (DataCallBack cb in calllbacks)
                        {
                            if (null != cb) cb.receiveData(buffer, dataIndex);
                        }
                    }

                    dataIndex++;

                }
            }
        }
    }
}
