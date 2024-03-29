using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Entity
{
    public class AdminProjectTypeRelationEntity : BaseEntity
    {
        private Int32? adminid;
        public Int32? AdminId
        {
            get { return adminid; }
            set
            {
                adminid = value;
            }
        }
        private Int32? projectTypeId;
        public Int32? ProjectTypeId
        {
            get { return projectTypeId; }
            set
            {
                projectTypeId = value;
            }
        }

        public string ProjectTypeName { get; set; }

        private Boolean? hasauthority;
        public Boolean? HasAuthority
        {
            get { return hasauthority; }
            set
            {
                hasauthority = value;
            }
        }
    }
}

