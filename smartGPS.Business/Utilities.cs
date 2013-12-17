using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace smartGPS.Business
{
    public class Utilities
    {
        public static String encryptPassword(String password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public static double? parseDouble(String value)
        {
            try
            {
                return Double.Parse(value, CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static readonly DateTime UnixEpochStart =  DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);

        public static DateTime ToDateTimeFromEpoch(long epochTime)
        {
            DateTime result = UnixEpochStart.AddMilliseconds(epochTime);
            return result;
        }

        public static long ToEpochFromDateTime(DateTime date)
        {
            TimeSpan ts = date.ToUniversalTime() - UnixEpochStart;
            return (long)Math.Floor(ts.TotalMilliseconds);
        }
    }
}