using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Entity
{
    public class IncomeAndExpenseTypeEntity : BaseEntity
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
        private Int16 type;
        public Int16 Type
        {
            get { return type; }
            set
            {
                type = value;
            }
        }

        public string TypeDesc
        {
            get
            {
                if (type == 1)
                    return "Gelir";
                else if (type == 2)
                    return "Gider";
                else
                    return "";

            }
        }
    }
}

