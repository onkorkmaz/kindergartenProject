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
    public class StudentAttendanceBookBusiness : BaseBusiness
    {
        static CacheBusines<StudentAttendanceBookEntity> cache;
        public StudentAttendanceBookBusiness(ProjectType projectType) : base(projectType)
        {
            if (cache == null || cache.ProjectType != projectType)
                cache = new CacheBusines<StudentAttendanceBookEntity>(base.ProjectType, CacheType.AttendanceBook, 0, false);
        }

        public DataResultArgs<StudentAttendanceBookEntity> Set_StudentAttendanceBook(StudentAttendanceBookEntity entity)
        {
            DataResultArgs<StudentAttendanceBookEntity> resultSet = new DataResultArgs<StudentAttendanceBookEntity>();
            DataResultArgs<DataTable> resultSetDt = new DataResultArgs<DataTable>();
            StudentAttendanceBookEntity attendanceBookEntity = new StudentAttendanceBookEntity();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DatabaseProcess", entity.DatabaseProcess);
            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@studentId", entity.StudentId);
            cmd.Parameters.AddWithValue("@arrivalDate", entity.ArrivalDate);
            cmd.Parameters.AddWithValue("@isArrival", entity.IsArrival);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@description", entity.Description);
            cmd.Parameters.AddWithValue("@projectType", base.ProjectTypeInt16);

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                resultSetDt = DataProcess.ExecuteProcDataTable(con, cmd, "set_StudentAttendanceBook_V2");
                con.Close();
            }
            attendanceBookEntity = get_AttendanceBookEntityWithDataTable(resultSetDt.Result);
            resultSet = EntityHelper.CopyDataResultArgs<StudentAttendanceBookEntity>(resultSetDt, resultSet);
            resultSet.Result = attendanceBookEntity;
            cache.UpdateCache(attendanceBookEntity);
            return resultSet;
        }

        public List<StudentAttendanceBookEntity> Get_StudentAttendanceBookWithCache(int studentId)
        {
            if(!cache.IsCacheAvailable)
            {
                Get_StudentAttendanceBook();
            }
            return cache.CacheList.Where(o => o.StudentId == studentId).ToList();
        }

        public DataResultArgs<List<StudentAttendanceBookEntity>> Get_StudentAttendanceBook()
        {
            List<StudentAttendanceBookEntity> lst = new List<StudentAttendanceBookEntity>();
            DataResultArgs<SqlDataReader> result = new DataResultArgs<SqlDataReader>();
            DataResultArgs<List<StudentAttendanceBookEntity>> resultSet = new DataResultArgs<List<StudentAttendanceBookEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@isDeleted", false);
            cmd.Parameters.AddWithValue("@projectType", base.ProjectTypeInt16);

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                result = DataProcess.ExecuteProcDataReader(con, cmd, "get_StudentAttendanceBook");
                
                if (result.HasError)
                {
                    resultSet.HasError = result.HasError;
                    resultSet.ErrorDescription = result.ErrorDescription;
                    resultSet.ErrorCode = result.ErrorCode;
                }
                else
                {
                    SqlDataReader dr = result.Result;
                    
                    StudentAttendanceBookEntity elist;
                    while (dr.Read())
                    {
                        elist = new StudentAttendanceBookEntity();
                        elist.Id = CommonFunctions.GetData<Int32>(dr["id"]);
                        elist.StudentId = CommonFunctions.GetData<Int32?>(dr["studentId"]);
                        elist.ArrivalDate = CommonFunctions.GetData<DateTime?>(dr["arrivalDate"]);
                        elist.IsArrival = CommonFunctions.GetData<Boolean?>(dr["isArrival"]);
                        elist.IsActive = CommonFunctions.GetData<Boolean?>(dr["isActive"]);
                        elist.Description = CommonFunctions.GetData<String>(dr["description"]);
                        lst.Add(elist);
                    }
                    dr.Close();
                    resultSet.Result = lst;
                }
                con.Close();
            }
            cache.AddCacheListInDictionary(CacheType.AttendanceBook, lst);
            return resultSet;
        }

        public StudentAttendanceBookEntity get_AttendanceBookEntityWithDataTable(DataTable dt)
        {
            DataRow dr = dt.Rows[0];
            StudentAttendanceBookEntity elist = new StudentAttendanceBookEntity();
            elist = new StudentAttendanceBookEntity();
            elist.Id = CommonFunctions.GetData<Int32>(dr["id"]);
            elist.StudentId = CommonFunctions.GetData<Int32?>(dr["studentId"]);
            elist.ArrivalDate = CommonFunctions.GetData<DateTime?>(dr["arrivalDate"]);
            elist.IsArrival = CommonFunctions.GetData<Boolean?>(dr["isArrival"]);
            elist.IsActive = CommonFunctions.GetData<Boolean?>(dr["isActive"]);
            elist.Description = CommonFunctions.GetData<String>(dr["description"]);
            return elist;
        }

        public void ClearPaymentCache()
        {
            cache.ClearCache(CacheType.AttendanceBook);
        }
    }
}
