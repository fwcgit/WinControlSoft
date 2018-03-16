using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlSoft.src.usart
{
    class UsartManager
    {
        public static UsartManager usartManager = new UsartManager();

        DataParser dataParser;
        private UsartManager() {
            dataParser = new DataParser();
        }

        private Usart usart = new Usart();

        public bool openSerialPort(String port)
        {
            usart.init(port);
            usart.configUsart();
            return usart.open();
        }


        public void closeSerialPort()
        {
            usart.close();
            dataParser.stopParserData();
        }

        public void writeData(byte[] data)
        {
            usart.write(data);
        }

        public bool isSerialPortOpen()
        {
            bool b = usart.isOpen();
            if (b) dataParser.startParserData();
            return b;
        }

        public void startReceiveData()
        {
            usart.startReceiveData();
        }

        public void addDataListener(Usart.DataCallBack callback)
        {
            usart.setReceiveListener(callback);
        }

        public string[] getHostSerialPort()
        {
            return SerialPort.GetPortNames();
        }
    }
}
