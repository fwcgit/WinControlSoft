using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlSoft.src.usart
{
    class DataParser : Usart.DataCallBack
    {
        public void receiveData(byte[] buffer,int count)
        {
            for(int i = 0; i <= count; i++)
            {
                System.Console.Out.WriteLine("{0:X}", buffer[i]);
            }
          
        }

        public void startParserData()
        {
            UsartManager.usartManager.addDataListener(this);
        }

        public void stopParserData()
        {
            UsartManager.usartManager.addDataListener(null);
        }
    }
}
