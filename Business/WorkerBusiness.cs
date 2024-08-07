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
    public class WorkerBusiness : BaseBusiness
    {
        public WorkerBusiness(ProjectType projectType) : base(projectType)
        {

        }

        public DataResultArgs<bool> Set_Worker(WorkerEntity entity)
        {
            try
            {
                DataProcess.ControlAdminAuthorization(true);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DatabaseProcess", entity.DatabaseProcess);
                cmd.Parameters.AddWithValue("@id", entity.Id);
                cmd.Parameters.AddWithValue("@name", entity.Name);
                cmd.Parameters.AddWithValue("@surname", entity.Surname);
                cmd.Parameters.AddWithValue("@isManager", entity.IsManager);
                cmd.Parameters.AddWithValue("@price", entity.Price);
                cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
                cmd.Parameters.AddWithValue("@phoneNumber", entity.PhoneNumber);
                cmd.Parameters.AddWithValue("@isTeacher", entity.IsTeacher);
                cmd.Parameters.AddWithValue("@projectType", (short)base.ProjectType);

                using (SqlConnection con = Connection.Conn)
                {
                    con.Open();
                    DataResultArgs<bool> resultSet = DataProcess.ExecuteProc(con, cmd, "set_Worker");
                    con.Close();

                    return resultSet;
                }
            }
            catch (Exception ex)
            {
                DataResultArgs<bool> resultSet = new DataResultArgs<bool>();
                resultSet.HasError = true;
                resultSet.ErrorDescription = ex.Message;
                resultSet.MyException = ex;
                return resultSet;
            }

        }

        public DataResultArgs<WorkerEntity> Get_Worker_WithId(int id, bool? isTeacher)
        {
            DataProcess.ControlAdminAuthorization(true);
            var resultSet = Get_Worker(new SearchEntity() { Id = id }, isTeacher);

            DataResultArgs<WorkerEntity> result =EntityHelper.CopyDataResultArgs<WorkerEntity>(resultSet);

            result.Result = resultSet.Result.FirstOrDefault(o => o.Id == id);

            return result;
        }

        public DataResultArgs<List<WorkerEntity>> Get_Worker(SearchEntity entity,bool? isTeacher)
        {
            DataProcess.ControlAdminAuthorization(true);
            DataResultArgs<List<WorkerEntity>> resultSet = new DataResultArgs<List<WorkerEntity>>();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", entity.Id);
                cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
                cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);
                cmd.Parameters.AddWithValue("@projectType", (short)base.ProjectType);

                if (isTeacher.HasValue)
                    cmd.Parameters.AddWithValue("@isTeacher", isTeacher);
                DataResultArgs<SqlDataReader> result = new DataResultArgs<SqlDataReader>();
                using (SqlConnection con = Connection.Conn)
                {
                    con.Open();
                    result = DataProcess.ExecuteProcDataReader(con, cmd, "get_Worker");


                    if (result.HasError)
                    {
                        resultSet.HasError = result.HasError;
                        resultSet.ErrorDescription = result.ErrorDescription;
                        resultSet.ErrorCode = result.ErrorCode;
                    }
                    else
                    {
                        SqlDataReader dr = result.Result;
                        List<WorkerEntity> lst = new List<WorkerEntity>();
                        WorkerEntity elist;
                        while (dr!=null &&  dr.Read())
                        {
                            elist = new WorkerEntity();
                            elist.Id = CommonFunctions.GetData<Int32>(dr["id"]);
                            elist.Name = CommonFunctions.GetData<String>(dr["name"]);
                            elist.Surname = CommonFunctions.GetData<String>(dr["surname"]);
                            elist.IsManager = CommonFunctions.GetData<Boolean?>(dr["isManager"]);
                            elist.Price = CommonFunctions.GetData<Decimal?>(dr["price"]);
                            elist.IsActive = CommonFunctions.GetData<Boolean?>(dr["isActive"]);
                            elist.UpdatedOn = CommonFunctions.GetData<DateTime>(dr["updatedOn"]);
                            elist.PhoneNumber = CommonFunctions.GetData<String>(dr["phoneNumber"]);
                            elist.IsTeacher = CommonFunctions.GetData<bool>(dr["isTeacher"]);

                            if (string.IsNullOrEmpty(elist.PhoneNumber))
                                elist.PhoneNumber = "";

                            lst.Add(elist);
                        }

                        if (dr != null)
                            dr.Close();
                        con.Close();
                        resultSet.Result = lst;
                    }
                }
                return resultSet;
            }
            catch (Exception ex)
            {
                resultSet.HasError = true;
                resultSet.ErrorDescription = ex.Message;
                resultSet.MyException = ex;
                return resultSet;
            }
        }
    }
}
