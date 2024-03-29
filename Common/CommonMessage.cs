﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class CommonConst
    {
        public const string EmptyAmount = " - ";
        public const string Admin = "Admin";
        public const string ProjectType = "ProjectType";

        public const string Url = "https://benimdunyamogrencitakip.com";

        public const string TL = "###,###,##0.##";
    }

    public static class RecordMessage
    {
        public const string Add = "Kayıt başarılı bir şekilde eklenmiştir.";
        public const string Update = "Kayıt başarılı bir şekilde güncellenmiştir.";
        public const string Delete = "Kayıt başarılı bir şekilde silinmiştir.";
    }

    public static class ButtonText
    {
        public const string Submit = "Kaydet";
        public const string Update = "Güncelle";
    }

    public static class ListKey
    {
        public const string SearchValue = "searchValue";
    }

    public static class IncomeAndExpenseSubType
    {
        public const int Income = 1;
        public const int Expense = 2;
        public const int WorkerExpense = 3;

    }
}
