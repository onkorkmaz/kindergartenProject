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
    public class AdminBusiness
    {
        public DataResultArgs<bool> Set_Admin(AdminEntity entity)
        {
            try
            {
                DataProcess.ControlAdminAuthorization(true);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DatabaseProcess", entity.DatabaseProcess);
                cmd.Parameters.AddWithValue("@id", entity.Id);
                cmd.Parameters.AddWithValue("@userName", entity.UserName);
                cmd.Parameters.AddWithValue("@password", entity.Password);
                cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
                cmd.Parameters.AddWithValue("@LastLoginDate", entity.LastLoginDate);
                cmd.Parameters.AddWithValue("@AdminType", entity.AdminType);

                using (SqlConnection con = Connection.Conn)
                {
                    con.Open();
                    DataResultArgs<bool> resultSet = DataProcess.ExecuteProc(con, cmd, "set_Admin");
                    con.Close();
                    return resultSet;
                }
            }
            catch (Exception e)
            {
                DataResultArgs<bool> result = new DataResultArgs<bool>();
                result.HasError = true;
                result.ErrorDescription = e.Message;
                return result;
            }
        }

        public DataResultArgs<AdminEntity> Get_Admin(string userName, string password)
        {

            DataResultArgs<List<AdminEntity>> resultSet = new DataResultArgs<List<AdminEntity>>();

            DataResultArgs<AdminEntity> returnResultSet = new DataResultArgs<AdminEntity>();


            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@userName", userName);
            cmd.Parameters.AddWithValue("@password", password);

            GetList(cmd, resultSet);

            if (resultSet.HasError)
            {
                returnResultSet.HasError = resultSet.HasError;
                returnResultSet.ErrorCode = resultSet.ErrorCode;
                returnResultSet.ErrorDescription = resultSet.ErrorDescription;
                returnResultSet.MyException = resultSet.MyException;
            }
            else
            {
                returnResultSet.Result = resultSet.Result.FirstOrDefault();
            }

            return returnResultSet;
        }

        public DataResultArgs<List<AdminEntity>> Get_Admin(SearchEntity entity)
        {

            DataResultArgs<List<AdminEntity>> resultSet = new DataResultArgs<List<AdminEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);

            GetList(cmd, resultSet);

            return resultSet;
        }

        private static void GetList(SqlCommand cmd, DataResultArgs<List<AdminEntity>> resultSet)
        {

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(con,cmd, "get_Admin");
                if (result.HasError)
                {
                    resultSet.HasError = result.HasError;
                    resultSet.ErrorDescription = result.ErrorDescription;
                    resultSet.ErrorCode = result.ErrorCode;
                }
                else
                {
                    SqlDataReader dr = result.Result;
                    List<AdminEntity> lst = new List<AdminEntity>();
                    AdminEntity elist;
                    while (dr.Read())
                    {
                        elist = new AdminEntity();
                        elist.Id = GeneralFunctions.GetData<Int32?>(dr["id"]);
                        elist.UserName = GeneralFunctions.GetData<String>(dr["userName"]);
                        elist.Password = GeneralFunctions.GetData<String>(dr["password"]);
                        elist.IsActive = GeneralFunctions.GetData<Boolean?>(dr["isActive"]);
                        elist.LastLoginDate = GeneralFunctions.GetData<DateTime?>(dr["LastLoginDate"]);
                        elist.AdminType = GeneralFunctions.GetData<Int16?>(dr["AdminType"]);
                        lst.Add(elist);
                    }

                    dr.Close();
                    resultSet.Result = lst;
                }
                con.Close();
            }
        }
    }
}
