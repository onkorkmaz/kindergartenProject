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
        public StudentBusiness(ProjectType projectType) : base(projectType)
        {

        }

        private static Dictionary<Int16, DataResultArgs<List<StudentEntity>>> AllStudentWithCacheWithProjectType = new Dictionary<Int16, DataResultArgs<List<StudentEntity>>>();

        public static void ClearStudentCache(ProjectType projectType)
        {
            if (AllStudentWithCacheWithProjectType != null && AllStudentWithCacheWithProjectType.ContainsKey((Int16)projectType) && AllStudentWithCacheWithProjectType[(Int16)projectType] != null)
            {
                AllStudentWithCacheWithProjectType[(Int16)projectType] = null;
            }
        }

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
                cmd.Parameters.AddWithValue("@classId", entity.ClassId);
                cmd.Parameters.AddWithValue("@schoolClass", entity.SchoolClass);
                cmd.Parameters.AddWithValue("@projectType", (short)base.ProjectType);
                cmd.Parameters.AddWithValue("@isInterview", entity.IsInterview);
                cmd.Parameters.AddWithValue("@interviewDate", entity.InterviewDate);
                //cmd.Parameters.AddWithValue("@className", entity.ClassName);

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
                        entity.Id = GeneralFunctions.GetData<Int32>(currentResultSet.Result);
                    }
                }


                if (entity.IsAddAfterPaymentUnPayment)
                {
                    List<PaymentTypeEntity> paymentTypeList = new PaymentTypeBusiness(entity.ProjectType).Get_PaymentType();

                    foreach (PaymentTypeEntity paymentTypeEntity in paymentTypeList)
                    {
                        for (int i = 1; i < DateTime.Today.Month; i++)
                        {
                            PaymentEntity paymentEntity = new PaymentEntity();
                            paymentEntity.Id = 0;
                            paymentEntity.DatabaseProcess = DatabaseProcess.Add;
                            paymentEntity.Year = (short) DateTime.Today.Year;
                            paymentEntity.StudentId = entity.Id;
                            paymentEntity.Amount = 0;
                            paymentEntity.IsActive = true;
                            paymentEntity.IsDeleted = false;
                            paymentEntity.Month = (short)i;
                            paymentEntity.IsPayment = false;

                            paymentEntity.Amount =
                                ((PaymentTypeEnum) paymentTypeEntity.Id == PaymentTypeEnum.Okul &&
                                 entity.SpokenPrice.HasValue && entity.SpokenPrice.Value > 0)
                                    ? entity.SpokenPrice.Value
                                    : paymentTypeEntity.Amount;
                            paymentEntity.PaymentType = paymentTypeEntity.Id;
                            paymentEntity.AddedOn = DateTime.Now;
                            paymentEntity.ProjectType = entity.ProjectType;

                            new PaymentBusiness(entity.ProjectType).Set_Payment(paymentEntity);
                        }
                    }

                }

                if(AllStudentWithCacheWithProjectType.ContainsKey(base.ProjectTypeInt16))
                    AllStudentWithCacheWithProjectType[base.ProjectTypeInt16] = null;

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
            bool isExist = AllStudentWithCacheWithProjectType.ContainsKey(base.ProjectTypeInt16);
            if (isExist && AllStudentWithCacheWithProjectType[base.ProjectTypeInt16] != null && AllStudentWithCacheWithProjectType[base.ProjectTypeInt16].Result.Count() > 0)
            {
                return AllStudentWithCacheWithProjectType[base.ProjectTypeInt16];
            }
            else
            {
                var result = Get_Student(new SearchEntity() { IsDeleted = false });
                if (isExist)
                {
                    AllStudentWithCacheWithProjectType[base.ProjectTypeInt16] = result;
                }
                else
                {
                    AllStudentWithCacheWithProjectType.Add(base.ProjectTypeInt16, result);

                }
            }

            return AllStudentWithCacheWithProjectType[base.ProjectTypeInt16];
        }

        public  DataResultArgs<List<StudentEntity>> Get_Student(SearchEntity entity)
        {
            var resultSet = Get_StudentDictionary(entity);
            return resultSet[base.ProjectType];
        }

        public Dictionary<ProjectType, DataResultArgs<List<StudentEntity>>> Get_StudentDictionary(SearchEntity entity)
        {
            DataProcess.ControlAdminAuthorization();

            Dictionary<ProjectType, DataResultArgs<List<StudentEntity>>> resultSetDictionary = new Dictionary<ProjectType, DataResultArgs<List<StudentEntity>>>();

            DataResultArgs<List<StudentEntity>> resultSet = new DataResultArgs<List<StudentEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);
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
                    List<StudentEntity> lst = new List<StudentEntity>();
                    while (dr != null && dr.Read())
                    {
                        var studentEntity = GetStudentEntity(dr);
                        lst.Add(studentEntity);
                    }
                    if (dr != null)
                        dr.Close();
                    resultSet.Result = lst;
                }
                con.Close();
            }

            if (resultSetDictionary.ContainsKey(base.ProjectType))
            {
                resultSetDictionary[base.ProjectType] = resultSet;
            }
            else
            {
                resultSetDictionary.Add(base.ProjectType, resultSet);
            }


            return resultSetDictionary;
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
                new PaymentBusiness(base.ProjectType).Get_Payment(entity.Id, DateTime.Today.Year.ToString());
            
            if (!paymentListResult.HasError)
            {
                if (paymentListResult.Result.Any())
                {
                    entity.PaymentList = paymentListResult.Result;
                }
            }

            DataResultArgs<List<PaymentEntity>> nextPaymentListResult =
                new PaymentBusiness(base.ProjectType).Get_Payment(entity.Id, (DateTime.Today.Year + 1).ToString());

            if (!paymentListResult.HasError)
            {
                if (paymentListResult.Result.Any())
                {
                    entity.PaymentList = paymentListResult.Result;
                }
            }

            if (!nextPaymentListResult.HasError)
            {
                if (nextPaymentListResult.Result.Any())
                {
                    foreach (PaymentEntity paymentEntity in nextPaymentListResult.Result)
                    {
                        entity.PaymentList.Add(paymentEntity);
                    }
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
                    StudentEntity elist;
                    while (dr!=null &&  dr.Read())
                    {
                        elist = GetStudentEntity(dr);
                        resultSet.Result = elist;
                    }
                    if (dr != null)
                        dr.Close();
                }
                con.Close();
            }

            return resultSet;
        }

        public StudentEntity Get_StudentWithFullName(string fullName)
        {
            DataProcess.ControlAdminAuthorization();
            List<StudentEntity> list = new StudentBusiness(base.ProjectType).Get_AllStudentWithCache().Result;
            StudentEntity entity = list.FirstOrDefault(o => o.FullNameForSearch == fullName);
            return entity;

        }

        private static StudentEntity GetStudentEntity(SqlDataReader dr)
        {
            DataProcess.ControlAdminAuthorization();

            StudentEntity entity = new StudentEntity();
            entity.Id = GeneralFunctions.GetData<Int32>(dr["id"]);
            entity.CitizenshipNumber = GeneralFunctions.GetData<String>(dr["citizenshipNumber"]);
            entity.Name = GeneralFunctions.GetData<String>(dr["name"]);
            entity.Surname = GeneralFunctions.GetData<String>(dr["surname"]);
            entity.MiddleName = GeneralFunctions.GetData<String>(dr["middleName"]);
            entity.FatherName = GeneralFunctions.GetData<String>(dr["fatherName"]);
            entity.MotherName = GeneralFunctions.GetData<String>(dr["motherName"]);
            entity.Birthday = GeneralFunctions.GetData<DateTime?>(dr["birthday"]);
            entity.FatherPhoneNumber = GeneralFunctions.GetData<String>(dr["fatherPhoneNumber"]);
            entity.MotherPhoneNumber = GeneralFunctions.GetData<String>(dr["motherPhoneNumber"]);
            entity.IsActive = GeneralFunctions.GetData<Boolean>(dr["isActive"]);
            entity.IsStudent = GeneralFunctions.GetData<Boolean>(dr["isStudent"]);
            entity.Notes = GeneralFunctions.GetData<String>(dr["notes"]);
            entity.SchoolClass = GeneralFunctions.GetData<String>(dr["schoolClass"]);
            entity.DateOfMeeting = GeneralFunctions.GetData<DateTime?>(dr["dateOfMeeting"]);
            entity.SpokenPrice = GeneralFunctions.GetData<Decimal?>(dr["spokenPrice"]);
            entity.Email = GeneralFunctions.GetData<String>(dr["email"]);
            entity.ClassId = GeneralFunctions.GetData<Int32?>(dr["classId"]);
            entity.ClassName = GeneralFunctions.GetData<String>(dr["className"]) ?? "";

            entity.IsInterview = GeneralFunctions.GetData<bool>(dr["isInterview"]);
            entity.InterviewDate = GeneralFunctions.GetData<DateTime?>(dr["interviewDate"]);

            entity.MainTeacherName = GeneralFunctions.GetData<String>(dr["mainTeacherName"]) ?? "";
            entity.MainTeacherSurname = GeneralFunctions.GetData<String>(dr["mainTeacherSurname"]) ?? "";

            entity.HelperTeacherName = GeneralFunctions.GetData<String>(dr["helperTeacherName"]) ?? "";
            entity.HelperTeacherSurname = GeneralFunctions.GetData<String>(dr["helperTeacherSurname"]) ?? "";

            return entity;
        }
    }
}
