using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebsite.Helper
{
    public class Colors
    {
        public static string selected = "color:#fd7e14;";
        public static string LightColorTitle = "color:#ffffff;";
        public static string LightColorText = "color:#faf9d4;";
        public static string DarkText = "color:#8f6903;";
        public static string Yellow = "color:#ffc107;";

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

    
}