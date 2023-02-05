using System;
using System.Collections.Generic;
using Entity;
using System.Data.SqlClient;
using System.Data;
using Common;
namespace Business
{
    public class StudentDetailBusiness : BaseBusiness
    {
        public StudentDetailBusiness(ProjectType projectType) : base(projectType)
        {

        }
        public DataResultArgs<bool> Set_StudentDetail(StudentDetailEntity entity)
        {
            DataResultArgs<bool> resultSet = new DataResultArgs<bool>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DatabaseProcess", entity.DatabaseProcess);
            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@studentId", entity.StudentId);
            cmd.Parameters.AddWithValue("@isPaymentPassive", entity.IsPaymentPassive);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@projectType", base.ProjectTypeInt16);
            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                resultSet = DataProcess.ExecuteProc(con, cmd, "set_StudentDetail");
            }
            return resultSet;
        }

        public DataResultArgs<List<StudentDetailEntity>> Get_StudentDetail(SearchEntity entity)
        {
            DataResultArgs<List<StudentDetailEntity>> resultSet = new DataResultArgs<List<StudentDetailEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);
            cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);
            cmd.Parameters.AddWithValue("@projectType", base.ProjectTypeInt16);
            cmd.Parameters.AddWithValue("@studentId", entity.StudentId);


            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(con, cmd, "get_StudentDetail");
                if (result.HasError)
                {
                    resultSet.HasError = result.HasError;
                    resultSet.ErrorDescription = result.ErrorDescription;
                    resultSet.ErrorCode = result.ErrorCode;
                }
                else
                {
                    SqlDataReader dr = result.Result;
                    List<StudentDetailEntity> lst = new List<StudentDetailEntity>();
                    StudentDetailEntity elist;
                    while (dr.Read())
                    {
                        elist = new StudentDetailEntity();
                        elist.Id = CommonFunctions.GetData<Int32>(dr["id"]);
                        elist.StudentId = CommonFunctions.GetData<Int32>(dr["studentId"]);
                        elist.IsPaymentPassive = CommonFunctions.GetData<Boolean>(dr["isPaymentPassive"]);
                        elist.IsActive = CommonFunctions.GetData<Boolean>(dr["isActive"]);
                        elist.ProjectType = (ProjectType)CommonFunctions.GetData<Int16>(dr["projectType"]);
                        lst.Add(elist);
                    }


                    dr.Close();
                    resultSet.Result = lst;
                    con.Close();
                }
            }
            return resultSet;
        }

    }
}
