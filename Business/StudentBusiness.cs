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
    public class StudentBusiness
    {
        public static DataResultArgs<List<StudentEntity>> AllStudentWithCache = null;

        public DataResultArgs<StudentEntity> Set_Student(StudentEntity entity)
        {
            try
            {
                DataProcess.ControlAdminAuthorization(true);

                if (entity.Id > 0 && entity.DatabaseProcess == DatabaseProcess.Add)
                    entity.DatabaseProcess = DatabaseProcess.Update;

                DataResultArgs<StudentEntity> resultSet = new DataResultArgs<StudentEntity>();
                resultSet.Result = entity;

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DatabaseProcess", entity.DatabaseProcess);
                cmd.Parameters.AddWithValue("@id", entity.Id);
                cmd.Parameters.AddWithValue("@citizenshipNumber", entity.CitizenshipNumber);
                cmd.Parameters.AddWithValue("@name", entity.Name);
                cmd.Parameters.AddWithValue("@surname", entity.Surname);
                cmd.Parameters.AddWithValue("@middleName", entity.MiddleName);
                cmd.Parameters.AddWithValue("@fatherName", entity.FatherName);
                cmd.Parameters.AddWithValue("@motherName", entity.MotherName);
                cmd.Parameters.AddWithValue("@birthday", entity.Birthday);
                cmd.Parameters.AddWithValue("@fatherPhoneNumber", entity.FatherPhoneNumber);
                cmd.Parameters.AddWithValue("@motherPhoneNumber", entity.MotherPhoneNumber);
                cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
                cmd.Parameters.AddWithValue("@isStudent", entity.IsStudent);
                cmd.Parameters.AddWithValue("@notes", entity.Notes);
                cmd.Parameters.AddWithValue("@dateOfMeeting", entity.DateOfMeeting);
                cmd.Parameters.AddWithValue("@spokenPrice", entity.SpokenPrice);
                cmd.Parameters.AddWithValue("@email", entity.Email);
                using (SqlConnection con = Connection.Conn)
                {
                    con.Open();
                    DataResultArgs<string> currentResultSet = DataProcess.ExecuteProcString(con, cmd, "set_Student");
                    con.Close();

                    resultSet.HasError = currentResultSet.HasError;
                    resultSet.ErrorDescription = currentResultSet.ErrorDescription;
                    resultSet.MyException = currentResultSet.MyException;

                    if (entity.DatabaseProcess == DatabaseProcess.Add && !currentResultSet.HasError)
                    {
                        resultSet.Result.Id = GeneralFunctions.GetData<Int32>(currentResultSet.Result);
                    }
                }

                AllStudentWithCache = null;
                Get_AllStudentWithCache();

                return resultSet;
            }
            catch (Exception e)
            {
                DataResultArgs<StudentEntity> result = new DataResultArgs<StudentEntity>();
                result.HasError = true;
                result.ErrorDescription = e.Message;
                return result;
            }
        }

        public DataResultArgs<List<StudentEntity>> Get_AllStudentWithCache()
        {
            return AllStudentWithCache ?? (AllStudentWithCache = Get_Student(new SearchEntity() {IsDeleted = false}));
            //return Get_Student(new SearchEntity() {IsDeleted = false});

        }

        public DataResultArgs<List<StudentEntity>> Get_Student(SearchEntity entity)
        {
            DataProcess.ControlAdminAuthorization();

            DataResultArgs<List<StudentEntity>> resultSet = new DataResultArgs<List<StudentEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);
            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(con,cmd, "get_Student");
                if (result.HasError)
                {
                    resultSet.HasError = result.HasError;
                    resultSet.ErrorDescription = result.ErrorDescription;
                    resultSet.ErrorCode = result.ErrorCode;
                }
                else
                {
                    SqlDataReader dr = result.Result;
                    List<StudentEntity> lst = new List<StudentEntity>();
                    while (dr.Read())
                    {
                        var studentEntity = GetEntity(dr);
                        lst.Add(studentEntity);
                    }

                    dr.Close();
                    resultSet.Result = lst;
                }
                con.Close();
            }

            return resultSet;
        }

        public DataResultArgs<StudentEntity> Get_Student(int studentId)
        {
            DataProcess.ControlAdminAuthorization();

            DataResultArgs<StudentEntity> resultSet = new DataResultArgs<StudentEntity>();

            DataResultArgs<List<StudentEntity>> studentList = Get_Student(new SearchEntity() {Id = studentId});

            if (studentList.HasError)
            {
                resultSet.HasError = studentList.HasError;
                resultSet.ErrorDescription = studentList.ErrorDescription;
                resultSet.MyException = studentList.MyException;
            }
            else
            {
                resultSet.Result = studentList.Result.FirstOrDefault();
            }

            return resultSet;
        }

        public StudentEntity Get_StudentWithPaymentList(int id)
        {
            DataProcess.ControlAdminAuthorization();

            StudentEntity entity = Get_Student(new SearchEntity() {Id = id}).Result[0];

            DataResultArgs<List<PaymentEntity>> paymentListResult =
                new PaymentBusiness().Get_Payment(entity.Id.Value, DateTime.Today.Year.ToString());
            if (!paymentListResult.HasError)
            {
                if (paymentListResult.Result.Any())
                {
                    entity.PaymentList = paymentListResult.Result;
                }
            }

            return entity;
        }


        public DataResultArgs<List<StudentEntity>> Get_Student()
        {
            return Get_Student(new SearchEntity() {IsDeleted = false});
        }

        public DataResultArgs<StudentEntity> Get_Student(string citizenshipNumber)
        {
            DataProcess.ControlAdminAuthorization();

            DataResultArgs<StudentEntity> resultSet = new DataResultArgs<StudentEntity>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@citizenshipNumber", citizenshipNumber);

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(con, cmd, "get_Student");
                if (result.HasError)
                {
                    resultSet.HasError = result.HasError;
                    resultSet.ErrorDescription = result.ErrorDescription;
                    resultSet.ErrorCode = result.ErrorCode;
                }
                else
                {
                    SqlDataReader dr = result.Result;
                    StudentEntity elist;
                    while (dr.Read())
                    {
                        elist = GetEntity(dr);
                        resultSet.Result = elist;
                    }

                    dr.Close();
                }
                con.Close();
            }

            return resultSet;
        }

        private static StudentEntity GetEntity(SqlDataReader dr)
        {
            DataProcess.ControlAdminAuthorization();

            StudentEntity elist = new StudentEntity();
            elist.Id = GeneralFunctions.GetData<Int32>(dr["id"]);
            elist.CitizenshipNumber = GeneralFunctions.GetData<String>(dr["citizenshipNumber"]);
            elist.Name = GeneralFunctions.GetData<String>(dr["name"]);
            elist.Surname = GeneralFunctions.GetData<String>(dr["surname"]);
            elist.MiddleName = GeneralFunctions.GetData<String>(dr["middleName"]);
            elist.FatherName = GeneralFunctions.GetData<String>(dr["fatherName"]);
            elist.MotherName = GeneralFunctions.GetData<String>(dr["motherName"]);
            elist.Birthday = GeneralFunctions.GetData<DateTime?>(dr["birthday"]);
            elist.FatherPhoneNumber = GeneralFunctions.GetData<String>(dr["fatherPhoneNumber"]);
            elist.MotherPhoneNumber = GeneralFunctions.GetData<String>(dr["motherPhoneNumber"]);
            elist.IsActive = GeneralFunctions.GetData<Boolean>(dr["isActive"]);
            elist.IsStudent = GeneralFunctions.GetData<Boolean>(dr["isStudent"]);
            elist.Notes = GeneralFunctions.GetData<String>(dr["notes"]);
            elist.DateOfMeeting = GeneralFunctions.GetData<DateTime?>(dr["dateOfMeeting"]);
            elist.SpokenPrice = GeneralFunctions.GetData<Decimal?>(dr["spokenPrice"]);
            elist.Email = GeneralFunctions.GetData<String>(dr["email"]);
            return elist;
        }
    }
}
