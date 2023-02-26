﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Entity
{
    public class EntityHelper
    {

        public static DataResultArgs<bool> CopyDataResultArgs<T>(DataResultArgs<T> source, DataResultArgs<bool> destination)
        {
            destination.ErrorCode = source.ErrorCode;
            destination.ErrorDescription = source.ErrorDescription;
            destination.HasError = source.HasError;
            destination.MyException = source.MyException;
            return destination;
        }

        public static DataResultArgs<T> CopyDataResultArgs<T>(DataResultArgs<string> source, DataResultArgs<T> destination)
        {
            destination.ErrorCode = source.ErrorCode;
            destination.ErrorDescription = source.ErrorDescription;
            destination.HasError = source.HasError;
            destination.MyException = source.MyException;
            return destination;
        }

        public static DataResultArgs<T> CopyDataResultArgs<T>(DataResultArgs<List<T>> source, DataResultArgs<T> destination)
        {
            destination.ErrorCode = source.ErrorCode;
            destination.ErrorDescription = source.ErrorDescription;
            destination.HasError = source.HasError;
            destination.MyException = source.MyException;
            return destination;
        }

        public static DataResultArgs<T> CopyDataResultArgs<T>(DataResultArgs<DataTable> source, DataResultArgs<T> destination)
        {
            destination.ErrorCode = source.ErrorCode;
            destination.ErrorDescription = source.ErrorDescription;
            destination.HasError = source.HasError;
            destination.MyException = source.MyException;
            return destination;
        }
    }
}
