using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlSoft.src.usart
{
    class DeviceStatu
    {

        public float temp1, temp2, temp3, temp4, temp5, temp6, temp7, temp8, temp9;
        public int waterMLev1, waterMLev2;
        public int hotLev;

        public DeviceStatu(byte[] buff)
        {
            
        }
       
    }
}
