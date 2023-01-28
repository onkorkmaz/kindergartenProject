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
    public class AdminBusiness : BaseBusiness
    {
        public AdminBusiness(ProjectType projectType) : base(projectType)
        {

        }
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
                cmd.Parameters.AddWithValue("@lastLoginDate", entity.LastLoginDate);
                cmd.Parameters.AddWithValue("@isSuperAdmin", entity.IsSuperAdmin);
                cmd.Parameters.AddWithValue("@isDeveloper", entity.IsDeveloper);
                cmd.Parameters.AddWithValue("@authorityTypeId", entity.AuthorityTypeId);

                cmd.Parameters.AddWithValue("@projectType", base.ProjectTypeInt16);

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
            cmd.Parameters.AddWithValue("@projectType", base.ProjectTypeInt16);

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
            cmd.Parameters.AddWithValue("@projectType", base.ProjectTypeInt16);

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
                    while (dr!=null &&  dr.Read())
                    {
                        elist = new AdminEntity();
                        elist.Id = CommonFunctions.GetData<Int32>(dr["id"]);
                        elist.UserName = CommonFunctions.GetData<String>(dr["userName"]);
                        elist.Password = CommonFunctions.GetData<String>(dr["password"]);
                        elist.IsActive = CommonFunctions.GetData<Boolean?>(dr["isActive"]);
                        elist.IsSuperAdmin = CommonFunctions.GetData<Boolean>(dr["isSuperAdmin"]);
                        elist.IsDeveloper = CommonFunctions.GetData<Boolean>(dr["isDeveloper"]);
                        elist.LastLoginDate = CommonFunctions.GetData<DateTime?>(dr["lastLoginDate"]);
                        elist.AuthorityTypeId = CommonFunctions.GetData<Int16?>(dr["authorityTypeId"]);
                        elist.ProjectType = (ProjectType)CommonFunctions.GetData<Int16>(dr["projectType"]);
                        lst.Add(elist);
                    }
                    if (dr != null)
                        dr.Close();
                    resultSet.Result = lst;
                }
                con.Close();
            }
        }
    }
}
