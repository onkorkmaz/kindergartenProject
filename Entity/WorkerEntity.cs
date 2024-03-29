using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Entity
{
    public class WorkerEntity : BaseEntity
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
        private String surname;
        public String Surname
        {
            get { return surname; }
            set
            {
                surname = value;
            }
        }
        private Boolean? ismanager;
        public Boolean? IsManager
        {
            get { return ismanager; }
            set
            {
                ismanager = value;
            }
        }
        private Decimal? price;
        public Decimal? Price
        {
            get { return price; }
            set
            {
                price = value;
            }
        }

        public string PriceStr
        {
            get 
            {
                if (price.HasValue)
                    return Price.Value.ToString(CommonConst.TL);

                return "";
            }
            
        }

        public string Title
        {
            get
            {
                string title = Name + " " + surname;
                if (IsManager.HasValue && IsManager.Value)
                    title += " - Yönetici";

                return title;
            }
        }

        public string PhoneNumber { get; set; }

        public bool IsTeacher { get; set; }
    }
}

