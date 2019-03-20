using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace BlihMobile.Models
{
    public class Constants
    {
        public static bool IsDev = true;

        public static Color BackGroundColorLoginPage = Color.FromHex("#227f7d");
        public static Color BackGroundColorPage = Color.White;

        public static string Username { get; set; }
        public static string Password { get; set; }

        public static string SelectedRepo { get; set; }
        public static string Reason { get; set; }

        public static List<AclUser> AclUsers { get; set; }
    }
}
