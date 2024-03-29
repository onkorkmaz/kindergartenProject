using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Entity
{
    public class AdminEntity : BaseEntity
    {
        public AdminEntity()
        {
            if (AuthorityList == null)
            {
                AuthorityList = new Dictionary<AuthorityScreenEnum, bool>();
                AdminProjectTypeRelationEntityList = new List<AdminProjectTypeRelationEntity>();
            }
        }

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

        private Int16 authorityTypeId;

        public Int16 AuthorityTypeId
        {
            get { return authorityTypeId; }
            set { authorityTypeId = value; }
        }

        public string AuthorityDescription { get; set; }

        private Int16 ownerStatus;

        public Int16 OwnerStatus
        {
            get { return ownerStatus; }
            set { ownerStatus = value; }
        }

        public OwnerStatusEnum OwnerStatusEnum
        {
            get
            {
                return (OwnerStatusEnum)OwnerStatus;
            }
        }

        public bool IsDeveleporOrSuperAdmin
        {
            get
            {
                return OwnerStatusEnum == OwnerStatusEnum.Developer || OwnerStatusEnum == OwnerStatusEnum.Admin;
            }
        }


        public string OwnerStatusDescription
        {
            get
            {
                switch (OwnerStatusEnum)
                {
                    case OwnerStatusEnum.None:
                        return "-";
                    case OwnerStatusEnum.Developer:
                        return "Develepor";
                    case OwnerStatusEnum.Admin:
                        return "Üst Admin";
                    case OwnerStatusEnum.Authority:
                        return "Alt Yetki";
                    default:
                        return "Tanımsız";
                }
            }
        }

        Dictionary<AuthorityScreenEnum, bool> AuthorityList;

        public AdminEntity EntranceAdminInfo { get; set; }

        public List<AdminProjectTypeRelationEntity> AdminProjectTypeRelationEntityList { get; set; }

        public string FullName { get; set; }

    }
}

