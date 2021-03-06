using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;
using System.Data.SqlClient;
using System.Data;
using Common;
namespace Business
{
    public class PaymentTypeBusiness
    {
        public DataResultArgs<List<PaymentTypeEntity>> staticResultSet = new DataResultArgs<List<PaymentTypeEntity>>();

        public DataResultArgs<bool> Set_PaymentType(PaymentTypeEntity entity)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DatabaseProcess", entity.DatabaseProcess);
            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@name", entity.Name);
            cmd.Parameters.AddWithValue("@amount", entity.Amount);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);

            DataResultArgs<bool> resultSet = DataProcess.ExecuteProc(cmd, "set_PaymentType");

            if (!resultSet.HasError)
                staticResultSet = new DataResultArgs<List<PaymentTypeEntity>>();

            return resultSet;
        }

        public DataResultArgs<List<PaymentTypeEntity>> Get_PaymentType(SearchEntity entity)
        {
            if (staticResultSet.Result != null && staticResultSet.Result.Count > 0)
                return staticResultSet;

            staticResultSet = new DataResultArgs<List<PaymentTypeEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);

            DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(cmd, "get_PaymentType");
            if (result.HasError)
            {
                staticResultSet.HasError = result.HasError;
                staticResultSet.ErrorDescription = result.ErrorDescription;
                staticResultSet.ErrorCode = result.ErrorCode;
            }
            else
            {
                SqlDataReader dr = result.Result;
                List<PaymentTypeEntity> lst = new List<PaymentTypeEntity>();
                PaymentTypeEntity elist;
                while (dr.Read())
                {
                    elist = new PaymentTypeEntity();
                    elist.Id = GeneralFunctions.GetData<Int32?>(dr["id"]);
                    elist.Name = GeneralFunctions.GetData<String>(dr["name"]);
                    elist.Amount = GeneralFunctions.GetData<Decimal?>(dr["amount"]);
                    elist.IsActive = GeneralFunctions.GetData<Boolean?>(dr["isActive"]);
                    elist.UpdatedOn = GeneralFunctions.GetData<DateTime>(dr["updatedOn"]);
                    lst.Add(elist);
                }


                dr.Close();
                staticResultSet.Result = lst;
            }
            return staticResultSet;
        }

    }
}
