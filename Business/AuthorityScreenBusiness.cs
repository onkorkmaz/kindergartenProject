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
    public class AuthorityScreenBusiness : BaseBusiness
    {
        static CacheBusines<AuthorityScreenEntity> cache;
        public AuthorityScreenBusiness(ProjectType projectType) : base(projectType)
        {
            if (cache == null || cache.ProjectType != projectType)
                cache = new CacheBusines<AuthorityScreenEntity>(projectType, CacheType.AuthorityScreen, 0, false);
        }

        public DataResultArgs<bool> Set_AuthorityScreen(AuthorityScreenEntity entity)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DatabaseProcess", entity.DatabaseProcess);
            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@name", entity.Name);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@projectType", entity.ProjectType);
            cmd.Parameters.AddWithValue("@description", entity.Description);

            DataResultArgs<bool> resultSet = new DataResultArgs<bool>();

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                resultSet = DataProcess.ExecuteProc(con, cmd, "set_AuthorityScreen");
                con.Close();
                cache.ClearCache(CacheType.AuthorityScreen);
            }

            return resultSet;
        }

        public DataResultArgs<List<AuthorityScreenEntity>> Get_AuthorityScreen(SearchEntity entity)
        {
            DataResultArgs<List<AuthorityScreenEntity>> resultSet = new DataResultArgs<List<AuthorityScreenEntity>>();
            DataProcess.ControlAdminAuthorization();

            if (cache.IsCacheAvailable)
            {
                resultSet.Result = cache.CacheList;
                return resultSet;
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(con, cmd, "get_AuthorityScreen");
                if (result.HasError)
                {
                    resultSet.HasError = result.HasError;
                    resultSet.ErrorDescription = result.ErrorDescription;
                    resultSet.ErrorCode = result.ErrorCode;
                }
                else
                {
                    SqlDataReader dr = result.Result;
                    List<AuthorityScreenEntity> lst = new List<AuthorityScreenEntity>();
                    AuthorityScreenEntity elist;
                    while (dr.Read())
                    {
                        elist = new AuthorityScreenEntity();
                        elist.Id = CommonFunctions.GetData<Int32>(dr["id"]);
                        elist.Name = CommonFunctions.GetData<String>(dr["name"]);
                        elist.IsActive = CommonFunctions.GetData<Boolean?>(dr["isActive"]);
                        elist.ProjectType = (ProjectType)CommonFunctions.GetData<Int16>(dr["projectType"]);
                        elist.Description = CommonFunctions.GetData<String>(dr["description"]);
                        lst.Add(elist);
                    }
                    dr.Close();
                    resultSet.Result = lst;
                    con.Close();
                    cache.AddCacheListInDictionary(CacheType.AuthorityScreen, lst);
                }
            }
            return resultSet;
        }

        public DataResultArgs<AuthorityScreenEntity> Get_AuthorityScreenWithId(int id)
        {
            DataProcess.ControlAdminAuthorization();

            SearchEntity searchEntity = new SearchEntity() { IsDeleted = false, Id = id };
            DataResultArgs<List<AuthorityScreenEntity>> resultSet = Get_AuthorityScreen(searchEntity);

             DataResultArgs<AuthorityScreenEntity> result =EntityHelper.CopyDataResultArgs<AuthorityScreenEntity>(resultSet);
            result.Result = resultSet.Result.FirstOrDefault(o => o.Id == id);

            return result;
        }
    }
}
