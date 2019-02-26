using System;
using System.Collections.Generic;
using System.Text;

namespace BlihMobile.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public User() { }
        public User(string Username, string Password)
        {
            if (Username != null)
                this.Username = Username;
            else
                this.Username = "";
            if (Password != null)
                this.Password = Password;
            else
                this.Password = "";
        }

        public bool CheckLogs()
        {
            if (!this.Username.Equals("") && !this.Password.Equals(""))
                return true;
            else
                return false;
        }
    }
}