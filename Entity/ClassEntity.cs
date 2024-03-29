using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Entity
{
    public class ClassEntity : BaseEntity
    {
        private String name;
        public String Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }
        private String description;
        public String Description
        {
            get { return description; }
            set
            {
                description = value;
            }
        }
        private Int32? warningofstudentcount;
        public Int32? WarningOfStudentCount
        {
            get { return warningofstudentcount; }
            set
            {
                warningofstudentcount = value;
            }
        }
        private Int32? mainteacherid;
        public Int32? MainTeacherId
        {
            get { return mainteacherid; }
            set
            {
                mainteacherid = value;
            }
        }
        private Int32? helperteacherid;
        public Int32? HelperTeacherId
        {
            get { return helperteacherid; }
            set
            {
                helperteacherid = value;
            }
        }
        private String mainteacherinfo;
        public String MainTeacherInfo
        {
            get { return mainteacherinfo; }
            set
            {
                mainteacherinfo = value;
            }
        }
        private String helperteacherInfo;
        public String HelperTeacherInfo
        {
            get { return helperteacherInfo; }
            set
            {
                helperteacherInfo = value;
            }
        }


        public bool IsActiveMainTeacher { get; set; }
        public bool IsActiveHelperTeacer { get; set; }

        public string ClassAndMainTeacherName
        {
            get
            {
                string result = "";
                if (Id > 0)
                {
                    result = name + " - " + mainteacherinfo + " (" + StudentCount + ") ";
                }
                else
                {
                    return name;
                }

                return result;
            }
        }

        public string StudentCount { get; set; }

        public string TeacherOutGoing { get; set; }

        public string StudentIncome { get; set; }

        public string StudentCurrentIncome { get; set; }


    }
}

