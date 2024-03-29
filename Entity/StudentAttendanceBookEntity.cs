using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Entity
{
    public class StudentAttendanceBookEntity : BaseEntity
    {
        private Int32? studentid;
        public Int32? StudentId
        {
            get { return studentid; }
            set
            {
                studentid = value;
            }
        }
        private DateTime? arrivaldate;
        public DateTime? ArrivalDate
        {
            get { return arrivaldate; }
            set
            {
                arrivaldate = value;
            }
        }
        private Boolean? isarrival;
        public Boolean? IsArrival
        {
            get { return isarrival; }
            set
            {
                isarrival = value;
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

        public int Year { 
            get
            {
                if (ArrivalDate.HasValue && ArrivalDate.Value > DateTime.MinValue)
                    return ArrivalDate.Value.Year;
                return 0;
            } 
        }

        public int Month
        {
            get
            {
                if (ArrivalDate.HasValue && ArrivalDate.Value > DateTime.MinValue)
                    return ArrivalDate.Value.Month;
                return 0;
            }
        }

        public int Day
        {
            get
            {
                if (ArrivalDate.HasValue && ArrivalDate.Value > DateTime.MinValue)
                    return ArrivalDate.Value.Day;
                return 0;
            }
        }
    }
}

