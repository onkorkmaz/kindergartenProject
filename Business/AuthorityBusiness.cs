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
        static CacheBusines<AuthorityEntity> cache;
        public AuthorityBusiness(ProjectType projectType,int adminId) : base(projectType)
        {
            if (cache == null || cache.ProjectType != projectType)
                cache = new CacheBusines<AuthorityEntity>(projectType, CacheType.Authority, adminId, true);
        }

        public DataResultArgs<bool> Set_Authority(AuthorityEntity entity)
        {
            DataResultArgs<bool> resultSet = new DataResultArgs<bool>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DatabaseProcess", entity.DatabaseProcess);
            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@authorityScreenId", entity.AuthorityScreenId);
            cmd.Parameters.AddWithValue("@authorityTypeId", entity.AuthorityTypeId);
            cmd.Parameters.AddWithValue("@hasAuthority", entity.HasAuthority);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                resultSet = DataProcess.ExecuteProc(con, cmd, "set_Authority");
                con.Close();
                cache.ClearCache(CacheType.Authority);
            }

            return resultSet;
        }

        public AuthorityEntity GetAuthorityWithScreenAndTypeId(AuthorityScreenEnum authorityScreenEnum,short authorityTypeId)
        {
            AuthorityEntity entity = new AuthorityEntity();
            DataProcess.ControlAdminAuthorization();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@isActive", true);
            cmd.Parameters.AddWithValue("@isDeleted", false);
            cmd.Parameters.AddWithValue("@authorityScreenId", (int)authorityScreenEnum);
            cmd.Parameters.AddWithValue("@authorityTypeId", authorityTypeId);
            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(con, cmd, "get_Authority");
                if (!result.HasError)
                {
                    SqlDataReader dr = result.Result;
                    AuthorityEntity elist;
                    while (dr.Read())
                    {
                        entity = getEntity(dr);
                    }

                    dr.Close();
                }
                con.Close();
            }

            //cache.CacheList.Add(entity);

            return entity;
        }

        public AuthorityEntity GetAuthorityWithTypeId(int authorityTypeId)
        {
            AuthorityEntity entity = new AuthorityEntity();
            DataProcess.ControlAdminAuthorization();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@isActive", true);
            cmd.Parameters.AddWithValue("@isDeleted", false);
            cmd.Parameters.AddWithValue("@authorityTypeId", authorityTypeId);
            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(con, cmd, "get_Authority");
                if (!result.HasError)
                {
                    SqlDataReader dr = result.Result;
                    while (dr.Read())
                    {
                        entity = getEntity(dr);
                    }

                    dr.Close();
                }
                con.Close();
            }

            //cache.CacheList.Add(entity);

            return entity;
        }

        private static AuthorityEntity getEntity(SqlDataReader dr)
        {
            AuthorityEntity elist = new AuthorityEntity();
            elist.Id = CommonFunctions.GetData<Int32>(dr["id"]);
            elist.AuthorityScreenId = CommonFunctions.GetData<Int32>(dr["authorityScreenId"]);
            elist.AuthorityScreenName = CommonFunctions.GetData<string>(dr["authorityScreenName"]);                                                                         
            elist.AuthorityTypeId = CommonFunctions.GetData<Int32>(dr["authorityTypeId"]);
            elist.HasAuthority = CommonFunctions.GetData<Boolean>(dr["hasAuthority"]);
            elist.IsActive = CommonFunctions.GetData<Boolean>(dr["isActive"]);
            return elist;
        }

        public DataResultArgs<List<AuthorityEntity>> Get_Authority(SearchEntity entity)
        {
            DataResultArgs<List<AuthorityEntity>> resultSet = new DataResultArgs<List<AuthorityEntity>>();
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
                        elist = getEntity(dr);
                        lst.Add(elist);
                    }

                    dr.Close();
                    resultSet.Result = lst;
                    cache.AddCacheListInDictionary(CacheType.Authority, lst);
                }
                con.Close();
            }
            return resultSet;
        }

        public List<AuthorityEntity> Get_ActiveAuthority(short authorityTypeId)
        {
            List<AuthorityEntity> resultSet = new List<AuthorityEntity>();

            List<AuthorityScreenEntity> authorityScreenList = new AuthorityScreenBusiness(base.ProjectType).Get_AuthorityScreen(new SearchEntity() { IsActive = true, IsDeleted = false }).Result;

            foreach (AuthorityScreenEntity screen in authorityScreenList)
            {
                if (cache.IsCacheAvailable && cache.CacheList.FirstOrDefault(o => o.AuthorityScreenId == screen.Id && o.AuthorityTypeId == authorityTypeId) != null)
                {
                    resultSet.Add(cache.CacheList.FirstOrDefault(o => o.AuthorityScreenId == screen.Id && o.AuthorityTypeId == authorityTypeId));
                }
                else
                {
                    AuthorityEntity db_AuthorityEntity = GetAuthorityWithScreenAndTypeId((AuthorityScreenEnum)screen.Id, authorityTypeId);
                    if (db_AuthorityEntity.Id > 0)
                    {
                        cache.CacheList.Add(db_AuthorityEntity);
                        resultSet.Add(db_AuthorityEntity);
                    }
                    else
                    {
                        AuthorityEntity entity = new AuthorityEntity();
                        entity.AuthorityScreenId = screen.Id;
                        entity.AuthorityScreenName = screen.Name;
                        entity.AuthorityTypeId = authorityTypeId;
                        //entity.AuthorityTypeName = new AuthorityTypeBusiness(base.ProjectType).Get_AuthorityTypeWithId(entity.AuthorityTypeId).Result.Name;
                        entity.IsActive = true;
                        entity.IsDeleted = false;
                        entity.HasAuthority = false;
                        entity.ProjectType = base.ProjectType;
                        entity.Id = 0;
                        resultSet.Add(entity);
                        cache.CacheList.Add(entity);
                    }
                }
            }
            return resultSet;
        }
    }
}
