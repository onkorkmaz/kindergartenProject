using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Entity
{
    public class StudentDetailEntity : BaseEntity
    {
        private Int32 studentid;
        public Int32 StudentId
        {
            get { return studentid; }
            set
            {
                studentid = value;
            }
        }
        private Boolean ispaymentpassive;
        public Boolean IsPaymentPassive
        {
            get { return ispaymentpassive; }
            set
            {
                ispaymentpassive = value;
            }
        }
    }
}

