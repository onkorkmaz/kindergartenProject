using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Common;

namespace Entity
{
    public class StudentEntity : BaseEntity
    {
        public StudentEntity()
        {
        }

        private String citizenshipnumber;
        public String CitizenshipNumber
        {
            get { return citizenshipnumber; }
            set
            {
                citizenshipnumber = value;
            }
        }

        public SchoolClassEnum SchoolClassEnum { get; set; }

        public string SchoolClassDesc
        {
            get
            {
                switch (SchoolClassEnum)
                {
                    case SchoolClassEnum.Kindergarten_2_Age:
                        return "2 Yaş";
                    case SchoolClassEnum.Kindergarten_3_Age:
                        return "3 Yaş";
                    case SchoolClassEnum.Kindergarten_4_Age:
                        return "4 Yaş";
                    case SchoolClassEnum.Kindergarten_5_Age:
                        return "5 Yaş";
                    case SchoolClassEnum.Kindergarten_6_Age:
                        return "6 Yaş";
                    case SchoolClassEnum.Class_1:
                        return "1.Sınıf";
                    case SchoolClassEnum.Class_2:
                        return "2.Sınıf";
                    case SchoolClassEnum.Class_3:
                        return "3.Sınıf";
                    case SchoolClassEnum.Class_4:
                        return "4.Sınıf";
                    case SchoolClassEnum.Class_5:
                        return "5.Sınıf";
                    case SchoolClassEnum.Class_6:
                        return "6.Sınıf";
                    case SchoolClassEnum.Class_7:
                        return "7.Sınıf";
                    case SchoolClassEnum.Class_8:
                        return "8.Sınıf";
                    case SchoolClassEnum.None:
                        return "";
                    default:
                        return "";
                }
            }
        }

        private String schoolClass;
        public String SchoolClass
        {
            get
            {
                return ((int)SchoolClassEnum).ToString();
            }

        }

        public String CitizenshipNumberStr
        {
            get
            {
                if (string.IsNullOrEmpty(CitizenshipNumber))
                    return "-";
                else
                    return CitizenshipNumber;
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


        private string fullNameForSearch;
        public String FullNameForSearch
        {
            get
            {
                fullNameForSearch = FullName.ToLower().Replace("ı", "i").Replace("ğ", "g").Replace("ö", "o")
                    .Replace("ü", "u").Replace("ş", "s").Replace("ç", "c");

                return fullNameForSearch;
            }
        }
        private String fathername;
        public String FatherName
        {
            get 
            {
                return string.IsNullOrEmpty(fathername) ? "" : fathername; 
            }
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
        private DateTime? birthday;
        public DateTime? Birthday
        {
            get { return birthday; }
            set
            {
                birthday = value;
            }
        }

        public string BirthdayWithFormatddMMyyyy
        {
            get
            {
                string dateFormat = (Birthday.HasValue && Birthday.Value > DateTime.MinValue) ? Birthday.Value.ToString("dd-MM-yyyy") : "";
                return dateFormat;
            }

        }

        public string BirthdayWithFormatyyyyMMdd
        {
            get
            {
                string dateFormat = (Birthday.HasValue && Birthday.Value > DateTime.MinValue)
                    ? Birthday.Value.ToString("yyyy-MM-dd")
                    : "";
                return dateFormat;
            }

        }

        public DateTime? BirthDayCurrentYear
        {
            get
            {
                if (Birthday.HasValue)
                    return new DateTime(DateTime.Today.Year, Birthday.Value.Month, Birthday.Value.Day);
                else
                    return Birthday;
            }
        }


        private String fatherphonenumber;
        public String FatherPhoneNumber
        {
            get
            {
                if (!string.IsNullOrEmpty(fatherphonenumber))
                {
                    if (fatherphonenumber.Trim().Length == 10)
                        return "0" + fatherphonenumber;
                    else if (fatherphonenumber.Trim().Length == 11)
                        return fatherphonenumber;
                }
                return "";
            }
            set
            {
                fatherphonenumber = value;
            }
        }
        private String motherphonenumber;
        public String MotherPhoneNumber
        {
            get
            {
                if (!string.IsNullOrEmpty(motherphonenumber))
                {
                    if (motherphonenumber.Trim().Length == 10)
                        return "0" + motherphonenumber;
                    else if (motherphonenumber.Trim().Length == 11)
                        return motherphonenumber;
                }

                return "";
            }
            set
            {
                motherphonenumber = value;
            }
        }


        public string ParentName
        {
            get
            {
                if (string.IsNullOrEmpty(mothername))
                    return fathername ?? "";
                else
                    return mothername ?? "";
            }
        }

        public string ParentPhoneNumber
        {
            get
            {
                if (string.IsNullOrEmpty(motherphonenumber))
                    return fatherphonenumber ?? "";
                else
                    return motherphonenumber ?? "";
            }
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

        private String notes;
        public String Notes
        {
            get
            {
                return notes;
            }
            set
            {
                notes = value;
            }
        }

        public String NotesStr
        {
            get
            {
                if (string.IsNullOrEmpty(Notes))
                    return "-";
                else
                    return Notes;
            }
        }
        private DateTime? dateofmeeting;
        public DateTime? DateOfMeeting
        {
            get { return dateofmeeting; }
            set
            {
                dateofmeeting = value;
            }
        }

        public string DateOfMeetingWithFormat
        {
            get
            {
                if (DateOfMeeting.HasValue)
                    return CommonFunctions.ToDateWithCulture(dateofmeeting);
                else
                    return "-";
            }
        }

        public string DateOfMeetingWithFormat2
        {
            get
            {
                if (DateOfMeeting.HasValue)
                    return DateOfMeeting.Value.ToString("yyyy-MM-dd");
                else
                    return "";
            }
        }

        public string InterviewWithFormat
        {
            get
            {
                if (InterviewDate.HasValue)
                    return InterviewDate.Value.ToString("yyyy-MM-dd");
                else
                    return "";
            }
        }

        private Decimal? spokenprice;
        public Decimal? SpokenPrice
        {
            get { return spokenprice; }
            set
            {
                spokenprice = value;
            }
        }

        public string SpokenPriceStr
        {
            get
            {
                if (SpokenPrice.HasValue)
                    return spokenprice.Value.ToString("###,###,###.00");
                else
                    return "-";
            }
        }

        public string Email { get; set; }

        public string EmailStr
        {
            get { return !string.IsNullOrEmpty((Email)) ? Email : "-"; }

        }

        public int? ClassId { get; set; }

        public string ClassName { get; set; }

        public string MainTeacherName { get; set; }
        public string MainTeacherSurname { get; set; }

        public string MainTeacher
        {
            get
            {
                return MainTeacherName + " " + MainTeacherSurname;
            }
        }

        public string HelperTeacher
        {
            get
            {
                return HelperTeacherName + " " + HelperTeacherSurname;
            }
        }

        public string HelperTeacherName { get; set; }
        public string HelperTeacherSurname { get; set; }

        public bool IsInterview { get; set; }

        public DateTime? InterviewDate { get; set; }

        public string InterviewDateWithFormat
        {
            get
            {
                if (InterviewDate.HasValue)
                    return InterviewDate.Value.ToString("yyyy-MM-dd");
                else
                    return "";
            }
        }

        public string SearchText
        {
            get
            {
                return CommonFunctions.ReplaceTurkishCharTLower(CitizenshipNumber) +
                    "_" + CommonFunctions.ReplaceTurkishCharTLower(FullName) +
                    "_" + CommonFunctions.ReplaceTurkishCharTLower(FatherName) +
                    "_" + CommonFunctions.ReplaceTurkishCharTLower(MotherName) +
                    "_" + CommonFunctions.ReplaceTurkishCharTLower(FatherPhoneNumber) +
                    "_" + CommonFunctions.ReplaceTurkishCharTLower(MotherPhoneNumber);
            }
        }
    }
}

