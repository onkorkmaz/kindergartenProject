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
    public class AuthorityBusiness : BaseBusiness
    {
        public AuthorityBusiness(ProjectType projectType) : base(projectType)
        {

        }

        public DataResultArgs<bool> Set_Authority(AuthorityEntity entity)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DatabaseProcess", entity.DatabaseProcess);
            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@name", entity.Name);
            cmd.Parameters.AddWithValue("@hasAuthority", entity.HasAuthority);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            //cmd.Parameters.AddWithValue("@projectType", base.ProjectTypeInt16);
            cmd.Parameters.AddWithValue("@description", entity.Description);

            DataResultArgs<bool> resultSet = new DataResultArgs<bool>();

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                resultSet = DataProcess.ExecuteProc(con, cmd, "set_Authority");
            }

            return resultSet;
        }

        public DataResultArgs<List<AuthorityEntity>> Get_Authority(SearchEntity entity)
        {
            DataResultArgs<List<AuthorityEntity>> resultSet = new DataResultArgs<List<AuthorityEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(con, cmd, "get_Authority");
                if (result.HasError)
                {
                    resultSet.HasError = result.HasError;
                    resultSet.ErrorDescription = result.ErrorDescription;
                    resultSet.ErrorCode = result.ErrorCode;
                }
                else
                {
                    SqlDataReader dr = result.Result;
                    List<AuthorityEntity> lst = new List<AuthorityEntity>();
                    AuthorityEntity elist;
                    while (dr.Read())
                    {
                        elist = new AuthorityEntity();
                        elist.Id = GeneralFunctions.GetData<Int32>(dr["id"]);
                        elist.Name = GeneralFunctions.GetData<String>(dr["name"]);
                        elist.HasAuthority = GeneralFunctions.GetData<Boolean?>(dr["hasAuthority"]);
                        elist.IsActive = GeneralFunctions.GetData<Boolean?>(dr["isActive"]);
                        elist.ProjectType = (ProjectType)GeneralFunctions.GetData<Int16>(dr["projectType"]);
                        elist.Description = GeneralFunctions.GetData<String>(dr["description"]);
                        lst.Add(elist);
                    }


                    dr.Close();
                    resultSet.Result = lst;
                    con.Close();
                }
            }
            return resultSet;
        }

        public DataResultArgs<AuthorityEntity> Get_AuthorityWithId(int id)
        {
            DataProcess.ControlAdminAuthorization();

            SearchEntity searchEntity = new SearchEntity() { IsDeleted = false, Id = id };
            DataResultArgs<List<AuthorityEntity>> resultSet = Get_Authority(searchEntity);
            DataResultArgs<AuthorityEntity> result = new DataResultArgs<AuthorityEntity>();
            EntityHelper.CopyDataResultArgs<AuthorityEntity>(resultSet, result);
            result.Result = resultSet.Result.FirstOrDefault(o => o.Id == id);

            return result;
        }
    }
}
