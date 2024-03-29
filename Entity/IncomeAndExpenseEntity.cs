using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Entity
{
    public class IncomeAndExpenseEntity : BaseEntity
    {
        private Int32 Incomeandexpensetypeid;
        public Int32 IncomeAndExpenseTypeId
        {
            get { return Incomeandexpensetypeid; }
            set
            {
                Incomeandexpensetypeid = value;
            }
        }
        private String Incomeandexpensetypename;
        public String IncomeAndExpenseTypeName
        {
            get { return Incomeandexpensetypename; }
            set
            {
                Incomeandexpensetypename = value;
            }
        }
        private Int16? Incomeandexpensetype;
        public Int16? IncomeAndExpenseType
        {
            get { return Incomeandexpensetype; }
            set
            {
                Incomeandexpensetype = value;
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


        public string AmountStr
        {
            get
            {
                return Amount.ToString(CommonConst.TL);
            }
        }

        private String description;
        public String Description
        {
            get 
            { 
                return description; 
            }
            set
            {
                description = value;
            }
        }

        private Int32? workerId;
        public Int32? WorkerId
        {
            get { return workerId; }
            set
            {
                workerId = value;
            }
        }

        public DateTime ProcessDate{ get; set; }

        public string ProcessDateWithFormat
        {
            get
            {
                return CommonFunctions.ToDateWithCulture(ProcessDate);
            }
        }

        public string ProcessDateWithFormatyyyyMMdd
        {
            get
            {
                return ProcessDate.ToString("yyyy-MM-dd");        
            }
        }

        public string WorkerName { get; set; }
        public string WorkerSurname { get; set; }


        public String Title
        {
            get 
            { 
                return WorkerName +" " + WorkerSurname;
            }
            
        }
    }
}

