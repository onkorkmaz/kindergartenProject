using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
namespace Entity
{
    public class StudentEntity : BaseEntity
    {
        private String citizenshipnumber;
        public String CitizenshipNumber
        {
            get { return citizenshipnumber; }
            set
            {
                citizenshipnumber = value;
            }
        }
        private String name;
        public String Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }
        private String surname;
        public String Surname
        {
            get { return surname; }
            set
            {
                surname = value;
            }
        }
        private String middlename;
        public String MiddleName
        {
            get { return middlename; }
            set
            {
                middlename = value;
            }
        }

        private String fullName;
        public String FullName
        {
            get 
            {
                fullName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Name.Trim());
                if (!string.IsNullOrEmpty(MiddleName))
                    fullName += " " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(MiddleName.Trim());

                fullName += " " + (string.IsNullOrEmpty(Surname) ? "" : Surname.Trim().ToUpper());

                return fullName;
            }         
        }

        private String fathername;
        public String FatherName
        {
            get { return fathername; }
            set
            {
                fathername = value;
            }
        }
        private String mothername;
        public String MotherName
        {
            get { return mothername; }
            set
            {
                mothername = value;
            }
        }
        private DateTime? birthdate;
        public DateTime? Birthdate
        {
            get { return birthdate; }
            set
            {
                birthdate = value;
            }
        }

        private string dateFormat;
        public string DateFormat
        {
            get
            {
                dateFormat = (Birthdate.HasValue && Birthdate.Value > DateTime.MinValue) ? Birthdate.Value.ToString("dd-MM-yyyy") : "";
                return dateFormat;
            }
        }

        private String fatherphonenumber;
        public String FatherPhoneNumber
        {
            get { return fatherphonenumber; }
            set
            {
                fatherphonenumber = value;
            }
        }
        private String motherphonenumber;
        public String MotherPhoneNumber
        {
            get { return motherphonenumber; }
            set
            {
                motherphonenumber = value;
            }
        }

        public string FatherInfo 
        { 
            get 
            {
                return getParentInfo(FatherName, FatherPhoneNumber);
            } 
        }

        public string MotherInfo
        {
            get
            {
                return getParentInfo(MotherName, MotherPhoneNumber);
            }
        }

        private string getParentInfo(string parentName, string parentPhoneNumber)
        {
            string info = "";

            if (!string.IsNullOrEmpty(parentName))
            {
                info += CultureInfo.CurrentCulture.TextInfo.ToTitleCase(parentName.Trim());
            }

            if (!string.IsNullOrEmpty(parentPhoneNumber))
            {
                if (!string.IsNullOrEmpty(info))
                    info += " - ";

                info += parentPhoneNumber.Trim();
            }

            return info;
        }

        private Boolean isstudent;
        public Boolean IsStudent
        {
            get { return isstudent; }
            set
            {
                isstudent = value;
            }
        }
    }
}

