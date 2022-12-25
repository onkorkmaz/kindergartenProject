using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Entity
{
    public class ExpenseEntity : BaseEntity
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

        public String Description
        {
            get { return name; }
            set
            {
                name = value;
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
        private Boolean? isrecursivemontly;
        public Boolean? IsRecursiveMontly
        {
            get { return isrecursivemontly; }
            set
            {
                isrecursivemontly = value;
            }
        }

        private short? expenseType;
        public short? ExpenseType
        {
            get { return expenseType; }
            set
            {
                expenseType = value;
            }
        }
    }
}

