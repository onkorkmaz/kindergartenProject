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
    public class StudentBusiness : BaseBusiness
    {
        public static Dictionary<ProjectType, List<StudentEntity>> studentCache = new Dictionary<ProjectType, List<StudentEntity>>();

        public StudentBusiness(ProjectType projectType) : base(projectType)
        {
        }

        public static void ClearStudentCache(ProjectType projectType)
        {
            studentCache[projectType] = new List<StudentEntity>();
        }

        public DataResultArgs<StudentEntity> DeleteStudent(StudentEntity entity)
        {
            DataResultArgs<StudentEntity> resultSet = new DataResultArgs<StudentEntity>();

            DataResultArgs<bool> paymentResultSet = new PaymentBusiness(base.ProjectType).SetStatusUnpaymentRecords(entity.Id, false, true);
            if (paymentResultSet.HasError)
            {
                resultSet.HasError = true;
                resultSet.ErrorDescription = paymentResultSet.ErrorDescription;
                return resultSet;
            }

            entity.DatabaseProcess = DatabaseProcess.Deleted;
            resultSet = Set_Student(entity);

            if (!resultSet.HasError)
            {
                studentCache[base.ProjectType] = new List<StudentEntity>();
                Get_StudentList();
            }

            return resultSet;

        }

        public DataResultArgs<StudentEntity> AddStudent(StudentEntity entity)
        {
            entity.DatabaseProcess = DatabaseProcess.Add;
            var resultSet = Set_Student(entity);

            if (!resultSet.HasError)
            {
                studentCache[base.ProjectType] = new List<StudentEntity>();
                Get_StudentList();
            }
            return resultSet;
        }

        public DataResultArgs<StudentEntity> UpdateStudent(StudentEntity entity)
        {
            DataResultArgs<StudentEntity> resultSet = new DataResultArgs<StudentEntity>();
            DataResultArgs<bool> paymentResultSet = new PaymentBusiness(base.ProjectType).SetStatusUnpaymentRecords(entity.Id, entity.IsActive.Value, false);

            if (paymentResultSet.HasError)
            {
                resultSet.HasError = true;
                resultSet.ErrorDescription = paymentResultSet.ErrorDescription;
                return resultSet;
            }

            entity.DatabaseProcess = DatabaseProcess.Update;
            resultSet = Set_Student(entity);

            if (!resultSet.HasError)
            {
                studentCache[base.ProjectType] = new List<StudentEntity>();
                Get_StudentList();
            }
            return resultSet;
        }

        private DataResultArgs<StudentEntity> Set_Student(StudentEntity entity)
        {
            try
            {
                DataProcess.ControlAdminAuthorization(true);

                if (entity.Id > 0 && entity.DatabaseProcess == DatabaseProcess.Add)
                    entity.DatabaseProcess = DatabaseProcess.Update;

                DataResultArgs<StudentEntity> resultSet = new DataResultArgs<StudentEntity>();
                DataResultArgs<string> resultSetId = new DataResultArgs<string>();

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
                cmd.Parameters.AddWithValue("@classId", entity.ClassId);
                cmd.Parameters.AddWithValue("@schoolClass", entity.SchoolClass);
                cmd.Parameters.AddWithValue("@projectType", (short)base.ProjectType);
                cmd.Parameters.AddWithValue("@isInterview", entity.IsInterview);
                cmd.Parameters.AddWithValue("@interviewDate", (entity.InterviewDate > DateTime.MinValue) ? entity.InterviewDate : null);

                using (SqlConnection con = Connection.Conn)
                {
                    con.Open();
                    resultSetId = DataProcess.ExecuteProcString(con, cmd, "set_Student");
                    con.Close();
                }

                if (!resultSetId.HasError)
                {
                    entity.Id = CommonFunctions.GetData<int>(resultSetId.Result);
                }
                            
                resultSet = EntityHelper.CopyDataResultArgs<StudentEntity>(resultSetId, resultSet);
                resultSet.Result = entity;
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

        public DataResultArgs<List<StudentEntity>> Get_StudentList()
        {
            DataResultArgs<List<StudentEntity>> resultSet = new DataResultArgs<List<StudentEntity>>();
            DataProcess.ControlAdminAuthorization();

            if (studentCache.ContainsKey(base.ProjectType) && studentCache[ProjectType].Count > 0)
            {
                resultSet.Result = studentCache[ProjectType];
                return resultSet;
            }

            List<StudentEntity> lst = new List<StudentEntity>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@isDeleted", false);
            cmd.Parameters.AddWithValue("@projectType", (short)base.ProjectType);

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

                    while (dr != null && dr.Read())
                    {
                        var studentEntity = GetStudentEntity(dr);
                        lst.Add(studentEntity);
                    }
                    if (dr != null)
                        dr.Close();

                    if (studentCache.ContainsKey(base.ProjectType))
                    {
                        studentCache[base.ProjectType] = lst;
                    }
                    else
                    {
                        studentCache.Add(base.ProjectType, lst);
                    }
                    resultSet.Result = lst;
                }
                con.Close();
            }
            return resultSet;
        }

        private StudentEntity GetStudentEntity(SqlDataReader dr)
        {
            DataProcess.ControlAdminAuthorization();

            StudentEntity entity = new StudentEntity();
            entity.Id = CommonFunctions.GetData<Int32>(dr["id"]);
            entity.CitizenshipNumber = CommonFunctions.GetData<String>(dr["citizenshipNumber"]);
            entity.Name = CommonFunctions.GetData<String>(dr["name"]);
            entity.Surname = CommonFunctions.GetData<String>(dr["surname"]);
            entity.MiddleName = CommonFunctions.GetData<String>(dr["middleName"]);
            entity.FatherName = CommonFunctions.GetData<String>(dr["fatherName"]);
            entity.MotherName = CommonFunctions.GetData<String>(dr["motherName"]);
            entity.Birthday = CommonFunctions.GetData<DateTime?>(dr["birthday"]);
            entity.FatherPhoneNumber = CommonFunctions.GetData<String>(dr["fatherPhoneNumber"]);
            entity.MotherPhoneNumber = CommonFunctions.GetData<String>(dr["motherPhoneNumber"]);
            entity.IsActive = CommonFunctions.GetData<Boolean>(dr["isActive"]);
            entity.IsStudent = CommonFunctions.GetData<Boolean>(dr["isStudent"]);
            entity.Notes = CommonFunctions.GetData<String>(dr["notes"]);
            entity.SchoolClassEnum = (SchoolClassEnum)CommonFunctions.GetData<int>(dr["schoolClass"]);
            entity.DateOfMeeting = CommonFunctions.GetData<DateTime?>(dr["dateOfMeeting"]);
            entity.SpokenPrice = CommonFunctions.GetData<Decimal?>(dr["spokenPrice"]);
            entity.Email = CommonFunctions.GetData<String>(dr["email"]);
            entity.ClassId = CommonFunctions.GetData<Int32?>(dr["classId"]);
            entity.AddedOn = CommonFunctions.GetData<DateTime>(dr["addedOn"]);
            entity.ClassName = CommonFunctions.GetData<String>(dr["className"]) ?? "";

            entity.IsInterview = CommonFunctions.GetData<bool>(dr["isInterview"]);
            entity.InterviewDate = CommonFunctions.GetData<DateTime?>(dr["interviewDate"]);

            entity.MainTeacherName = CommonFunctions.GetData<String>(dr["mainTeacherName"]) ?? "";
            entity.MainTeacherSurname = CommonFunctions.GetData<String>(dr["mainTeacherSurname"]) ?? "";

            entity.HelperTeacherName = CommonFunctions.GetData<String>(dr["helperTeacherName"]) ?? "";
            entity.HelperTeacherSurname = CommonFunctions.GetData<String>(dr["helperTeacherSurname"]) ?? "";
            entity.ProjectType = base.ProjectType;

            return entity;
        }

        public DataResultArgs<StudentEntity> Get_StudentWithId(int id)
        {
            DataProcess.ControlAdminAuthorization();

            DataResultArgs<StudentEntity> resultSet = new DataResultArgs<StudentEntity>();

            resultSet.Result = studentCache[base.ProjectType].FirstOrDefault(o => o.Id == id);

            return resultSet;
        }


        public DataResultArgs<StudentEntity> Get_Student(string citizenshipNumber)
        {
            DataProcess.ControlAdminAuthorization();
            DataResultArgs<StudentEntity> resultSet = new DataResultArgs<StudentEntity>();

            resultSet.Result = Get_StudentList().Result.FirstOrDefault(o => o.CitizenshipNumber == citizenshipNumber);
            return resultSet;
        }

        public DataResultArgs<StudentEntity> Get_StudentWithFullName(string fullName)
        {
            DataProcess.ControlAdminAuthorization();
            DataResultArgs<StudentEntity> resultSet = new DataResultArgs<StudentEntity>();

            resultSet.Result = Get_StudentList().Result.FirstOrDefault(o => o.FullNameForSearch == fullName);
            return resultSet;
        }
    }
}
