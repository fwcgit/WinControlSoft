using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlSoft.src.usart
{
    public class DeviceStatu
    {

        public float temp1, temp2, temp3, temp4, temp5, temp6, temp7, temp8, temp9;
        public int waterMLev1, waterMLev2;
        public int hotLev;

        public DeviceStatu(byte[] buff)
        {
            temp1 = ((buff[1] << 8) | buff[2]) / 100f;
            temp2 = ((buff[3] << 8) | buff[4]) / 100f;
            temp3 = ((buff[5] << 8) | buff[6]) / 100f;
            temp4 = ((buff[7] << 8) | buff[8]) / 100f;
            temp5 = ((buff[9] << 8) | buff[10]) / 100f;
            temp6 = ((buff[11] << 8) | buff[12]) / 100f;
            temp7 = ((buff[13] << 8) | buff[14]) / 100f;
            temp8 = ((buff[15] << 8) | buff[16]) / 100f;
            temp9 = ((buff[17] << 8) | buff[18]) / 100f;
            waterMLev1 = buff[19];
            waterMLev2 = buff[20];
            hotLev = buff[21];

            System.Console.Out.WriteLine("temp1 = "+ temp1);
            System.Console.Out.WriteLine("temp2 = "+ temp2);
            System.Console.Out.WriteLine("temp3 = "+temp3);
            System.Console.Out.WriteLine("temp4 = "+ temp4);
            System.Console.Out.WriteLine("temp5 = "+ temp5);
            System.Console.Out.WriteLine("temp6 = "+ temp6);
            System.Console.Out.WriteLine("temp7 = "+ temp7);
            System.Console.Out.WriteLine("temp8 = "+ temp8);
            System.Console.Out.WriteLine("temp9 = " + temp9);
            System.Console.Out.WriteLine("waterMLev1 = " + waterMLev1);
            System.Console.Out.WriteLine("waterMLev2 = " + waterMLev2);
            System.Console.Out.WriteLine("hotLev = " + hotLev);

        }

       
 
    }

    

}
