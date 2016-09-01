using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;


namespace Backend.Kernel
{
    public static class Utility
    {

        #region 數值轉換

        public static decimal ToDecimal(this object value)
        {
            if (value == null) return default(decimal);
            decimal d;
            var b = decimal.TryParse(value.ToString(), out d);
            return d;
        }

        public static int ToInt(this object value)
        {
            if (value == null) return default(int);
            if (value is bool)
            {
                var bValue = (bool)value;
                return bValue ? 1 : 0;
            }
            int i;
            var b = int.TryParse(value.ToString(), out i);
            return i;
        }

        public static short ToShort(this object value)
        {
            if (value == null) return default(short);
            short i;
            var b = short.TryParse(value.ToString(), out i);
            return i;
        }

        public static float ToFloat(this object value)
        {
            if (value == null) return default(float);
            float i;
            var b = float.TryParse(value.ToString(), out i);
            return i;
        }

        public static double ToDouble(this object value)
        {
            if (value == null) return default(double);
            double i;
            var b = double.TryParse(value.ToString(), out i);
            return i;
        }

        public static readonly DateTime MinDate = new DateTime(1900, 1, 1);

        public static DateTime ToDateTime(this object value, string format = "")
        {
            if (value == null) return MinDate;
            DateTime d;
            if (string.IsNullOrWhiteSpace(format))
            {
                var b = DateTime.TryParse(value.ToString(), out d);
                if (!b)
                {
                    d = MinDate;
                }
            }
            else
            {
                d = DateTime.ParseExact(value.ToString(), format, Thread.CurrentThread.CurrentCulture);
            }
            return d;
        }

        public static bool ToBoolean(this object value)
        {
            if (value == null) return false;
            return "true".Equals(value.ToString(), StringComparison.OrdinalIgnoreCase) || value.ToInt() == 1;
        }

        public static string ToIntText(this bool value)
        {
            return value ? "1" : "0";
        }

        public static short ToShortValue(this bool value)
        {
            return (short)(value ? 1 : 0);
        }

        public static byte[] ToBytes(this Stream s)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = s.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static byte[] GetBytes(this string s)
        {
            #region 舊轉byte編碼
            //if (string.Equals(BackendUser.CurrentLanguage, "zh-tw", StringComparison.OrdinalIgnoreCase))
            //{
            //    encoding = Encoding.GetEncoding("BIG5");
            //}
            //else
            //{
            //encoding = Encoding.UTF8;
            //}
            //return encoding.GetBytes(s);
            #endregion

            Encoding encoding = Encoding.UTF8;
            byte[] buffer = encoding.GetBytes(s);

            byte[] outBuffer = new byte[buffer.Length + 3];
            outBuffer[0] = 0xEF;//有BOM,解决乱码
            outBuffer[1] = 0xBB;
            outBuffer[2] = 0xBF;
            Array.Copy(buffer, 0, outBuffer, 3, buffer.Length);


            return outBuffer;
        }

        public static string ToDecimalString(this object source)
        {
            var value = source.ToDecimal();
            if (value.Equals(0)) return "0";
            return value.ToString("N2");
        }

        public static string ToIntString(this object source)
        {
            return source.ToInt().ToString("N0");
        }

        #endregion


        public static object InvokeMethod(this object obj, string methodName, params object[] parameters)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                throw new Exception("Invoke method name null");
            }
            var mi = obj.GetType().GetMethod(methodName);
            var result = mi.Invoke(obj, parameters);
            return result;
        }
    }

}
