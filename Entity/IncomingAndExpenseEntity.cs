using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Entity
{
    public class IncomingAndExpenseEntity : BaseEntity
    {
        private Int32 incomingandexpensetypeid;
        public Int32 IncomingAndExpenseTypeId
        {
            get { return incomingandexpensetypeid; }
            set
            {
                incomingandexpensetypeid = value;
            }
        }
        private String incomingandexpensetypename;
        public String IncomingAndExpenseTypeName
        {
            get { return incomingandexpensetypename; }
            set
            {
                incomingandexpensetypename = value;
            }
        }
        private Int16? incomingandexpensetype;
        public Int16? IncomingAndExpenseType
        {
            get { return incomingandexpensetype; }
            set
            {
                incomingandexpensetype = value;
            }
        }
        private Decimal amount;
        public Decimal Amount
        {
            get { return amount; }
            set
            {
                amount = value;
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
    }
}

