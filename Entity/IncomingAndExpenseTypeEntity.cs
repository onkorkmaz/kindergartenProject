using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Entity
{
    public class IncomingAndExpenseTypeEntity : BaseEntity
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
        private Int16? type;
        public Int16? Type
        {
            get { return type; }
            set
            {
                type = value;
            }
        }


        private string typeDesc;
        public string TypeDesc
        {
            get 
            { 
               if(type.HasValue)
                {
                    if (type.Value == 1)
                        return "Gelir";
                    else
                        return "Gider";
                }
                return "";

            }          
        }
    }
}

