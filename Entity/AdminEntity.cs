using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

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

        private Int16? authorityTypeId;

        public Int16? AuthorityTypeId
        {
            get { return authorityTypeId; }
            set { authorityTypeId = value; }
        }

        public AuthorityTypeEnum AuthorityTypeEnum
        {
            get
            {
                if (authorityTypeId.HasValue)
                    return (AuthorityTypeEnum)authorityTypeId.Value;

                return AuthorityTypeEnum.None;
            }
        }

        public bool IsDeveloper { get; set; }

        public bool IsSuperAdmin { get; set; }
    }


    public class CurrentContex
    {
        public static AdminEntity Contex;

    }

}

