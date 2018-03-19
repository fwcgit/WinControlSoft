using ControlSoft.src.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlSoft.src.usart
{
    public delegate void UpDeviceStatu(DeviceStatu statu);

    class DataParser : Usart.DataCallBack
    {
        public static DataParser parser = new DataParser();
        private DataParser() { }

        private UpDeviceStatu upStatu;
        public void receiveData(byte[] buffer,int count)
        {

            if (!Utils.CheckCode(buffer)) return;

            for(int i = 0; i <= count; i++)
            {
                System.Console.Out.WriteLine("{0:X}", buffer[i]);
            }
            DeviceStatu deviceStatu = TelProtocol.parserStatu(buffer);
            if (null != upStatu) upStatu(deviceStatu);

        }

        public void startParserData()
        {
            UsartManager.usartManager.addDataListener(this);
        }

        public void stopParserData()
        {
            UsartManager.usartManager.addDataListener(null);
        }

        public void setUpdate(UpDeviceStatu upStatu)
        {
            this.upStatu = upStatu;
        }
    }
}
