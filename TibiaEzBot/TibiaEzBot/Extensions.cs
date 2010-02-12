using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TibiaEzBot
{
    public static class Extensions
    {
		public static sbyte[,] Direction = new sbyte[8, 2] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 }, { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } };
		
		public static bool IsAdjacent(this Point p, Point point)
		{
			for (int i = 0; i < 8; i++)
	        {
				if(p.X + Direction[i, 0] == point.X && p.Y + Direction[i, 1] == point.Y)
					return true;
			}
			
			return false;
		}
		
        public static String ToHexString(this byte[] bytes)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var b in bytes)
            {
                builder.Append(b.ToString("X2"));
                builder.Append(" ");
            }

            return builder.ToString().TrimEnd();
        }

        public static string ToIPString(this byte[] value)
        {
            string ret = "";
            for (int i = 0; i < value.Length; i++)
                ret += value[i] + ".";

            return ret.TrimEnd('.');
        }

        public static byte[] ToByteArray(this uint[] unsignedIntegers)
        {
            byte[] temp = new byte[unsignedIntegers.Length * 4];
            for (int i = 0; i < unsignedIntegers.Length; i++)
                Array.Copy(BitConverter.GetBytes(unsignedIntegers[i]), 0, temp, i * 4, 4);

            return temp;
        }

        public static byte[] ToByteArray(this string s)
        {
            List<byte> value = new List<byte>();
            foreach (char c in s.ToCharArray())
                value.Add(c.ToByte());

            return value.ToArray();
        }

        public static byte ToByte(this char value)
        {
            return (byte)value;
        }
    }
}
