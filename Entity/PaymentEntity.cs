using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
namespace Entity
{
    public class PaymentEntity : BaseEntity
    {
        private string encryptStudentId;
        public string EncryptStudentId
        {
            get { return encryptStudentId; }
            set
            {
                encryptStudentId = value;
            }
        }

        private Int32? studentid;
        public Int32? StudentId
        {
            get { return studentid; }
            set
            {
                studentid = value;
            }
        }
        private Int16? year;


        public Int16? Year
        {
            get { return year; }
            set
            {
                year = value;
            }
        }
        private Int16? month;
        public Int16? Month
        {
            get { return month; }
            set
            {
                month = value;
            }
        }
        private Decimal? amount;
        public Decimal? Amount
        {
            get { return amount; }
            set
            {
                amount = value;
            }
        }
        private Boolean? ispayment;
        public Boolean? IsPayment
        {
            get { return ispayment; }
            set
            {
                ispayment = value;
            }
        }
        private DateTime? paymentdate;
        public DateTime? PaymentDate
        {
            get { return paymentdate; }
            set
            {
                paymentdate = value;
            }
        }
        private Int32? paymenttype;
        public Int32? PaymentType
        {
            get { return paymenttype; }
            set
            {
                paymenttype = value;
            }
        }

        public Boolean IsNotPayable { get; set; }

        public string AmountDesc
        {
            get
            {
                if (Amount.HasValue && Amount.Value > 0)
                    return Amount.Value.ToString("###,###,###.##", CultureInfo.CurrentCulture);
                else
                {
                    return "";
                }
            }
        }
    }

    public class StudentListAndPaymentTypeInfo
    {
        public StudentListAndPaymentTypeInfo()
        {
            PaymentTypeList = new List<PaymentTypeEntity>();
            StudentList = new List<StudentEntity>();
        }

        public List<PaymentTypeEntity> PaymentTypeList { get; set; }

        public  List<StudentEntity> StudentList { get; set; }

        public int Year
        {
            get { return DateTime.Now.Year; }
        }

        public int Month
        {
            get { return DateTime.Now.Month; }
        }
    }

    public class EmailPaymentEntity
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public int PaymentTypeId { get; set; }

        public string AmountDescription { get; set; }

        public bool IsPayment { get; set; }

    }
}

