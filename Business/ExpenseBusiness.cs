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
    public class ExpenseBusiness : BaseBusiness
    {
        public ExpenseBusiness(ProjectType projectType) : base(projectType)
        {

        }

        public DataResultArgs<string> Set_Expense(ExpenseEntity entity)
        {
            DataResultArgs<string> resultSet = new DataResultArgs<string>();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DatabaseProcess", entity.DatabaseProcess);
                cmd.Parameters.AddWithValue("@id", entity.Id);
                cmd.Parameters.AddWithValue("@name", entity.Name);
                cmd.Parameters.AddWithValue("@amount", entity.Amount);
                cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
                cmd.Parameters.AddWithValue("@isRecursiveMontly", entity.IsRecursiveMontly);
                cmd.Parameters.AddWithValue("@projectType", base.ProjectTypeInt16);
                cmd.Parameters.AddWithValue("@description", entity.Description);


                using (SqlConnection con = Connection.Conn)
                {
                    con.Open();
                    resultSet = DataProcess.ExecuteProcString(con, cmd, "set_Outgoing");
                    con.Close();
                    return resultSet;
                }
            }
            catch (Exception e)
            {
                DataResultArgs<string> result = new DataResultArgs<string>();
                result.HasError = true;
                result.ErrorDescription = e.Message;
                result.Result = e.Message;
                return result;
            }
        }

        public DataResultArgs<List<ExpenseEntity>> Get_Expense(SearchEntity entity)
        {
            DataResultArgs<List<ExpenseEntity>> resultSet = new DataResultArgs<List<ExpenseEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);
            cmd.Parameters.AddWithValue("@projectType", base.ProjectTypeInt16);

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(con,cmd, "get_Outgoing");

                if (result.HasError)
                {
                    resultSet.HasError = result.HasError;
                    resultSet.ErrorDescription = result.ErrorDescription;
                    resultSet.ErrorCode = result.ErrorCode;
                }
                else
                {
                    SqlDataReader dr = result.Result;
                    List<ExpenseEntity> lst = new List<ExpenseEntity>();
                    ExpenseEntity elist;
                    while (dr.Read())
                    {
                        elist = new ExpenseEntity();
                        elist.Id = GeneralFunctions.GetData<Int32>(dr["id"]);
                        elist.Name = GeneralFunctions.GetData<String>(dr["name"]);
                        elist.Amount = GeneralFunctions.GetData<Decimal?>(dr["amount"]);
                        elist.IsActive = GeneralFunctions.GetData<Boolean?>(dr["isActive"]);
                        elist.IsRecursiveMontly = GeneralFunctions.GetData<Boolean?>(dr["isRecursiveMontly"]);
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
