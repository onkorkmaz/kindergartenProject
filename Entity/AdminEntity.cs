using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class AdminEntity : BaseEntity
    {
        private String userName;

        public String UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private String password;

        public String Password
        {
            get { return password; }
            set { password = value; }
        }

        private DateTime? lastlogindate;

        public DateTime? LastLoginDate
        {
            get { return lastlogindate; }
            set { lastlogindate = value; }
        }

        private Int16? admintype;

        public Int16? AdminType
        {
            get { return admintype; }
            set { admintype = value; }
        }
    }
}

