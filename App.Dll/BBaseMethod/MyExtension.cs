using System;
using System.Collections.Generic;
using System.Text;

namespace App.Dll
{
    public static class MyExtension
    {


        public static int ToInt(this object value)
        {
            int aa = 0;
            if (!int.TryParse(value.ToString(), out aa))
            {
                return 0;
            }
            else
            {
                return aa;
            }
        }

        public static bool ToBool(this object value)
        {
            bool aa = false;
            switch (value.ToString().ToLower())
            {
                case "false": return false;
                case "true": return true;
            }
            if (!bool.TryParse(value.ToString(), out aa))
            {
                return false;
            }
            else
                return true;
        }

        public static DateTime ToDateTime(this object value)
        {
            return value.ToString().ToDateTime(DateTime.MinValue);
        }
        public static DateTime? ToNullableDateTime(this object value)
        {
            if (string.IsNullOrEmpty(value.ToString()))
                return null;
            else
                return value.ToDateTime();
        }
        public static DateTime ToDateTime(this string value, DateTime defaultResult)
        {
            DateTime time;
            if (DateTime.TryParse(value, out time))
            {
                return time;
            }
            return defaultResult;
        }
        public static string getAppValue(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }
    }
}
