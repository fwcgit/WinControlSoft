using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlSoft.src.usart
{
    class TelProtocol
    {
        public static byte PROTOCOL_HEAD_START      = 0x3B;
        public static byte PROTOCOL_HEAD_END_SIZE   = 26;

        public static byte PROTOCOL_UP_TEMP         = 0x40;
        public static byte PROTOCOL_UP_WATER_M_1    = 0x41;
        public static byte PROTOCOL_UP_WATER_M_2    = 0x43;
        public static byte PROTOCOL_UP_HOT          = 0x44;
        public static byte PROTOCOL_CMD_TEMP        = 0x45;
        public static byte PROTOCOL_CMD_WATER_M_1   = 0x46;
        public static byte PROTOCOL_CMD_WATER_M_2   = 0x47;
        public static byte PROTOCOL_CMD_HOT         = 0x48;
        public static byte PROTOCOL_UP_STATU        = 0x49;

        public static DeviceStatu parserStatu(byte[] buff)
        {
            DeviceStatu statu = new DeviceStatu(buff);
            return statu;
        }

        public static byte[] CmdMLev(int type,int lev)
        {
            byte[] buffer = new byte[6];
            buffer[0] = PROTOCOL_HEAD_START;
            buffer[1] = 5;
            buffer[2] = (byte)type;
            buffer[3] = (byte)lev; ;
            buffer[4] = getCodeCheck(buffer,0,2);

            return buffer;
        }

        public static byte getCodeCheck(byte[] data,int start,int stop)
        {
            int val = 0;
            for(int i = start; i <= stop; i++)
            {
                val += data[i];
            }
            return (byte)val;
        }
    }
}
