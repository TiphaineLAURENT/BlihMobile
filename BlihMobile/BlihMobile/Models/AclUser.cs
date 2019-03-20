using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BlihMobile.Models
{
    public class AclUser
    {
        public string name { get; set; }
        public bool read { get; set; }
        public bool write { get; set; }
        public bool execute { get; set; }
    }
}
