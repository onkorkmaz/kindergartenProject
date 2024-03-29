using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Entity
{
    public class AuthorityEntity : BaseEntity
    {
        private Int32 authorityscreenid;
        public Int32 AuthorityScreenId
        {
            get { return authorityscreenid; }
            set
            {
                authorityscreenid = value;
            }
        }

        private string authorityscreenname;
        public string AuthorityScreenName
        {
            get { return authorityscreenname; }
            set
            {
                authorityscreenname = value;
            }
        }


        private Int32 authoritytypeid;
        public Int32 AuthorityTypeId
        {
            get { return authoritytypeid; }
            set
            {
                authoritytypeid = value;
            }
        }

        private string authoritytypename;
        public string AuthorityTypeName
        {
            get { return authoritytypename; }
            set
            {
                authoritytypename = value;
            }
        }


        private Boolean hasauthority;
        public Boolean HasAuthority
        {
            get { return hasauthority; }
            set
            {
                hasauthority = value;
            }
        }
    }
}

