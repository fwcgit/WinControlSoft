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
        public static byte PROTOCOL_HEAD_END        = 0x0D;

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
    }
}
