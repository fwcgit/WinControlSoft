using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlSoft.src.utils
{
    class Utils
    {
        
        public static bool CheckCode(byte[] data)
        {
            int sum = 0;
            for(int i = 1;i < data.Length; i++)
            {
                sum += data[i];
            }

            return ((byte)sum & 0xff) == data[data.Length-1];
        }
    }
}
