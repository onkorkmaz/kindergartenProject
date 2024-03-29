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
public class OrganizationBusiness{
public DataResultArgs<bool> Set_Organization(OrganizationEntity entity)
{
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
DataResultArgs<bool> resultSet = DataProcess.ExecuteProc(con,cmd, "set_Organization"); 
con.Close();
return resultSet ;
}
}

public DataResultArgs<List<OrganizationEntity>> Get_Organization(SearchEntity entity)
{
DataResultArgs<List<OrganizationEntity>> resultSet = new DataResultArgs<List<OrganizationEntity>>();
SqlCommand cmd = new SqlCommand();
cmd.CommandType = CommandType.StoredProcedure;

cmd.Parameters.AddWithValue("@id", entity.Id);
cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);

using (SqlConnection con = Connection.Conn)
{
con.Open();
DataResultArgs<SqlDataReader> result= DataProcess.ExecuteProcDataReader(con,cmd, "get_Organization"); 
if (result.HasError)
{
resultSet.HasError = result.HasError;
resultSet.ErrorDescription = result.ErrorDescription;
resultSet.ErrorCode = result.ErrorCode;
}
else
{
 SqlDataReader dr = result.Result;
List<OrganizationEntity> lst = new List<OrganizationEntity>();
OrganizationEntity elist;
while(dr.Read())
{
elist = new OrganizationEntity();
elist.Id =   CommonFunctions.GetData<Int32>(dr["id"]);
elist.Name =   CommonFunctions.GetData<String>(dr["name"]);
elist.Description =   CommonFunctions.GetData<String>(dr["description"]);
elist.IsActive =   CommonFunctions.GetData<Boolean?>(dr["isActive"]);
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
