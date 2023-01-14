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
    public class IncomingAndExpenseBusiness : BaseBusiness
    {
        public IncomingAndExpenseBusiness(ProjectType projectType) : base(projectType)
        {

        }

        public DataResultArgs<bool> Set_IncomingAndExpense(IncomingAndExpenseEntity entity)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DatabaseProcess", entity.DatabaseProcess);
            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@incomingAndExpenseTypeId", entity.IncomingAndExpenseTypeId);
            cmd.Parameters.AddWithValue("@amount", entity.Amount);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@projectType", base.ProjectType);
            cmd.Parameters.AddWithValue("@description", entity.Description);

            DataResultArgs<bool> resultSet = new DataResultArgs<bool>();
            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                resultSet = DataProcess.ExecuteProc(con, cmd, "set_IncomingAndExpense");
                con.Close();
            }
            return resultSet;
        }

        public DataResultArgs<List<IncomingAndExpenseEntity>> Get_IncomingAndExpense(SearchEntity entity)
        {
            DataResultArgs<List<IncomingAndExpenseEntity>> resultSet = new DataResultArgs<List<IncomingAndExpenseEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);
            cmd.Parameters.AddWithValue("@projectType", base.ProjectTypeInt16);

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(con,cmd, "get_IncomingAndExpense");
                if (result.HasError)
                {
                    resultSet.HasError = result.HasError;
                    resultSet.ErrorDescription = result.ErrorDescription;
                    resultSet.ErrorCode = result.ErrorCode;
                }
                else
                {
                    SqlDataReader dr = result.Result;
                    List<IncomingAndExpenseEntity> lst = new List<IncomingAndExpenseEntity>();
                    IncomingAndExpenseEntity elist;
                    while (dr.Read())
                    {
                        elist = new IncomingAndExpenseEntity();
                        elist.Id = GeneralFunctions.GetData<Int32>(dr["id"]);
                        elist.IncomingAndExpenseTypeId = GeneralFunctions.GetData<Int32>(dr["incomingAndExpenseTypeId"]);
                        elist.IncomingAndExpenseTypeName = GeneralFunctions.GetData<String>(dr["incomingAndExpenseTypeName"]);
                        elist.IncomingAndExpenseType = GeneralFunctions.GetData<Int16?>(dr["incomingAndExpenseType"]);
                        elist.Amount = GeneralFunctions.GetData<Decimal>(dr["amount"]);
                        elist.IsActive = GeneralFunctions.GetData<Boolean>(dr["isActive"]);
                        elist.ProjectType = (ProjectType)GeneralFunctions.GetData<Int16>(dr["projectType"]);
                        elist.Description = GeneralFunctions.GetData<String>(dr["description"]);
                        lst.Add(elist);
                    }


                    dr.Close();
                    con.Close();
                    resultSet.Result = lst;

                }
            }
            return resultSet;
        }

    }
}
