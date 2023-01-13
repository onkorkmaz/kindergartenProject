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
    public class IncomingAndExpenseTypeBusiness : BaseBusiness
    {
        public IncomingAndExpenseTypeBusiness(ProjectType projectType) : base(projectType)
        {

        }
        public DataResultArgs<bool> Set_IncomingAndExpenseType(IncomingAndExpenseTypeEntity entity)
        {
            DataProcess.ControlAdminAuthorization();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DatabaseProcess", entity.DatabaseProcess);
            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@name", entity.Name);
            cmd.Parameters.AddWithValue("@type", entity.Type);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@projectType", base.ProjectTypeInt16);
            DataResultArgs<bool> resultSet = new DataResultArgs<bool>();

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                resultSet = DataProcess.ExecuteProc(con, cmd, "set_IncomingAndExpenseType");
                con.Close();
            }
            return resultSet;
        }

        public DataResultArgs<List<IncomingAndExpenseTypeEntity>> Get_IncomingAndExpenseType(SearchEntity entity)
        {
            DataProcess.ControlAdminAuthorization();

            DataResultArgs<List<IncomingAndExpenseTypeEntity>> resultSet = new DataResultArgs<List<IncomingAndExpenseTypeEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);
            cmd.Parameters.AddWithValue("@projectType", base.ProjectTypeInt16);


            List<IncomingAndExpenseTypeEntity> lst = new List<IncomingAndExpenseTypeEntity>();
            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(con, cmd, "get_IncomingAndExpenseType");
                if (result.HasError)
                {
                    resultSet.HasError = result.HasError;
                    resultSet.ErrorDescription = result.ErrorDescription;
                    resultSet.ErrorCode = result.ErrorCode;
                }
                else
                {
                    SqlDataReader dr = result.Result;

                    IncomingAndExpenseTypeEntity elist;
                    while (dr.Read())
                    {
                        elist = new IncomingAndExpenseTypeEntity();
                        elist.Id = GeneralFunctions.GetData<Int32>(dr["id"]);
                        elist.Name = GeneralFunctions.GetData<String>(dr["name"]);
                        elist.Type = GeneralFunctions.GetData<Int16?>(dr["type"]);
                        elist.IsActive = GeneralFunctions.GetData<Boolean?>(dr["isActive"]);
                        elist.ProjectType = (ProjectType)GeneralFunctions.GetData<Int16>(dr["projectType"]);
                        elist.UpdatedOn = GeneralFunctions.GetData<DateTime>(dr["updatedOn"]);

                        lst.Add(elist);
                    }

                    dr.Close();
                    con.Close();
                }
                resultSet.Result = lst;
            }
            return resultSet;
        }


        public DataResultArgs<IncomingAndExpenseTypeEntity> Get_IncomingAndExpenseType_WithId(int id)
        {
            DataProcess.ControlAdminAuthorization(true);
            var resultSet = Get_IncomingAndExpenseType(new SearchEntity() { Id = id });

            DataResultArgs<IncomingAndExpenseTypeEntity> result = new DataResultArgs<IncomingAndExpenseTypeEntity>();

            EntityHelper.CopyDataResultArgs<IncomingAndExpenseTypeEntity>(resultSet, result);

            result.Result = resultSet.Result.FirstOrDefault(o => o.Id == id);

            return result;
        }

    }
}
