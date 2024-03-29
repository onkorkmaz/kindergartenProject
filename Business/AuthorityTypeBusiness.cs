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
    public class AuthorityTypeBusiness : BaseBusiness
    {
        static CacheBusines<AuthorityTypeEntity> cache = null;
        public AuthorityTypeBusiness(ProjectType projectType) : base(projectType)
        {
            if (cache == null)
                cache = new CacheBusines<AuthorityTypeEntity>(projectType, CacheType.AuthorityType, 0, false);
        }

        public DataResultArgs<bool> Set_AuthorityType(AuthorityTypeEntity entity)
        {
            DataResultArgs<bool> resultSet = new DataResultArgs<bool>();
            DataProcess.ControlAdminAuthorization(true);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DatabaseProcess", entity.DatabaseProcess);
            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@name", entity.Name);
            cmd.Parameters.AddWithValue("@description", entity.Description);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                resultSet = DataProcess.ExecuteProc(con, cmd, "set_AuthorityType");
                con.Close();
            }
            cache.ClearCache(CacheType.AuthorityType);
            return resultSet;
        }

        public DataResultArgs<List<AuthorityTypeEntity>> Get_AuthorityType(SearchEntity entity)
        {
            DataResultArgs<List<AuthorityTypeEntity>> resultSet = new DataResultArgs<List<AuthorityTypeEntity>>();
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
                DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(con, cmd, "get_AuthorityType");
                if (result.HasError)
                {
                    resultSet.HasError = result.HasError;
                    resultSet.ErrorDescription = result.ErrorDescription;
                    resultSet.ErrorCode = result.ErrorCode;
                }
                else
                {
                    SqlDataReader dr = result.Result;
                    List<AuthorityTypeEntity> lst = new List<AuthorityTypeEntity>();
                    AuthorityTypeEntity elist;
                    while (dr.Read())
                    {
                        elist = new AuthorityTypeEntity();
                        elist.Id = CommonFunctions.GetData<Int32>(dr["id"]);
                        elist.Name = CommonFunctions.GetData<String>(dr["name"]);
                        elist.Description = CommonFunctions.GetData<String>(dr["description"]);
                        elist.IsActive = CommonFunctions.GetData<Boolean?>(dr["isActive"]);
                        lst.Add(elist);
                    }


                    dr.Close();
                    resultSet.Result = lst;
                    cache.AddCacheListInDictionary(CacheType.AuthorityType, lst);
                }
                con.Close();
            }
            return resultSet;
        }

        public DataResultArgs<AuthorityTypeEntity> Get_AuthorityTypeWithId(int id)
        {
            DataProcess.ControlAdminAuthorization();

            SearchEntity searchEntity = new SearchEntity() { IsDeleted = false, Id = id };
            DataResultArgs<List<AuthorityTypeEntity>> resultSet = Get_AuthorityType(searchEntity);

            DataResultArgs<AuthorityTypeEntity> result =EntityHelper.CopyDataResultArgs<AuthorityTypeEntity>(resultSet);
            result.Result = resultSet.Result.FirstOrDefault(o => o.Id == id);

            return result;
        }
    }
}
