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
    public class AdminProjectTypeRelationBusiness
    {
        public DataResultArgs<bool> Set_AdminProjectTypeRelation(AdminProjectTypeRelationEntity entity)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DatabaseProcess", entity.DatabaseProcess);
            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@adminId", entity.AdminId);
            cmd.Parameters.AddWithValue("@projectTypeId", entity.ProjectTypeId);
            cmd.Parameters.AddWithValue("@hasAuthority", entity.HasAuthority);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<bool> resultSet = DataProcess.ExecuteProc(con, cmd, "dbo.set_AdminProjectTypeRelation");
                con.Close();
                return resultSet;
            }
        }

        public DataResultArgs<AdminProjectTypeRelationEntity> Get_AdminProjectTypeRelation(int adminId, ProjectType projectTypeEnum)
        {
            SearchEntity searchEntity = new SearchEntity() { IsActive = true ,ProjectType = projectTypeEnum };
            DataResultArgs<List<AdminProjectTypeRelationEntity>> resultSetList = Get_AdminProjectTypeRelation(searchEntity, adminId);

            DataResultArgs<AdminProjectTypeRelationEntity> resultSet = EntityHelper.CopyDataResultArgs<AdminProjectTypeRelationEntity>(resultSetList);

            return resultSet;
        }

        public DataResultArgs<List<AdminProjectTypeRelationEntity>> Get_AdminProjectTypeRelation(SearchEntity entity, int? adminId)
        {
            DataResultArgs<List<AdminProjectTypeRelationEntity>> resultSet = new DataResultArgs<List<AdminProjectTypeRelationEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);
            if (entity.ProjectType != ProjectType.None)
            {
                cmd.Parameters.AddWithValue("@projectTypeId", (int)entity.ProjectType);
            }
            cmd.Parameters.AddWithValue("@adminId", adminId);

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(con, cmd, "get_AdminProjectTypeRelation");
                if (result.HasError)
                {
                    resultSet.HasError = result.HasError;
                    resultSet.ErrorDescription = result.ErrorDescription;
                    resultSet.ErrorCode = result.ErrorCode;
                }
                else
                {
                    SqlDataReader dr = result.Result;
                    List<AdminProjectTypeRelationEntity> lst = new List<AdminProjectTypeRelationEntity>();
                    AdminProjectTypeRelationEntity elist;
                    while (dr.Read())
                    {
                        elist = new AdminProjectTypeRelationEntity();
                        elist.Id = CommonFunctions.GetData<Int32>(dr["id"]);
                        elist.AdminId = CommonFunctions.GetData<Int32?>(dr["adminId"]);
                        elist.ProjectTypeId = CommonFunctions.GetData<Int32?>(dr["projectTypeId"]);
                        elist.HasAuthority = CommonFunctions.GetData<Boolean?>(dr["hasAuthority"]);
                        elist.IsActive = CommonFunctions.GetData<Boolean?>(dr["isActive"]);
                        elist.ProjectTypeName = CommonFunctions.GetData<string>(dr["projectTypeName"]);

                        lst.Add(elist);
                    }


                    dr.Close();
                    resultSet.Result = lst;
                }
                con.Close();
                return resultSet;
            }
        }

    }
}
