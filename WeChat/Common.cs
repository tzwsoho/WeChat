using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Windows.Forms;

namespace WeChat
{
    static public class Common
    {
        static public long UnixtimeStamp()
        {
            return DateTime.Now.Subtract(DateTime.Parse("1970-01-01")).Ticks / TimeSpan.TicksPerMillisecond;
        }

        static public long ReverseTimestamp()
        {
            return 0x80000000 - (DateTime.Now.Subtract(DateTime.Parse("1970-01-01")).Ticks / TimeSpan.TicksPerSecond);
        }

        static public string DeviceID()
        {
            string str_device_id = "e";
            Random rnd = new Random();
            for (int i = 0; i < 15; i++)
            {
                str_device_id += rnd.Next(10);
            }

            return str_device_id;
        }

        static public string EncodeJson(object objToEncode)
        {
            MemoryStream ms = new MemoryStream();
            DataContractJsonSerializer s = new DataContractJsonSerializer(objToEncode.GetType());
            s.WriteObject(ms, objToEncode);

            byte[] bytes_json = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(bytes_json, 0, bytes_json.Length);
            ms.Close();

            return Encoding.UTF8.GetString(bytes_json);
        }

        static public object DecodeJson(string szJson, Type objType)
        {
            try
            {
                byte[] bytesObject = Encoding.UTF8.GetBytes(szJson);
                MemoryStream ms = new MemoryStream(bytesObject);
                DataContractJsonSerializer s = new DataContractJsonSerializer(objType);
                object objRet = s.ReadObject(ms);
                ms.Close();

                return objRet;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
