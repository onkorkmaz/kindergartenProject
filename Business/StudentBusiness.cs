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
        public DataResultArgs<bool> Set_Student(StudentEntity entity)
        {
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
            cmd.Parameters.AddWithValue("@birthdate", entity.Birthdate);
            cmd.Parameters.AddWithValue("@fatherPhoneNumber", entity.FatherPhoneNumber);
            cmd.Parameters.AddWithValue("@motherPhoneNumber", entity.MotherPhoneNumber);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@isStudent", entity.IsStudent);

            DataResultArgs<bool> resultSet = DataProcess.ExecuteProc(cmd, "set_Student");

            AllStudentWithCache = null;
            Get_AllStudentWithCache();

            return resultSet;
        }

        public DataResultArgs<List<StudentEntity>> Get_AllStudentWithCache()
        {
            if (AllStudentWithCache == null)
            {
                AllStudentWithCache = Get_Student(new SearchEntity() { IsDeleted = false });
            }
            return AllStudentWithCache;
        }

        public DataResultArgs<List<StudentEntity>> Get_Student(SearchEntity entity)
        {
            DataResultArgs<List<StudentEntity>> resultSet = new DataResultArgs<List<StudentEntity>>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);

            DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(cmd, "get_Student");
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
                StudentEntity elist;
                while (dr.Read())
                {
                    elist = new StudentEntity();
                    elist.Id = GeneralFunctions.GetData<Int32>(dr["id"]);
                    elist.CitizenshipNumber = GeneralFunctions.GetData<String>(dr["citizenshipNumber"]);
                    elist.Name = GeneralFunctions.GetData<String>(dr["name"]);
                    elist.Surname = GeneralFunctions.GetData<String>(dr["surname"]);
                    elist.MiddleName = GeneralFunctions.GetData<String>(dr["middleName"]);
                    elist.FatherName = GeneralFunctions.GetData<String>(dr["fatherName"]);
                    elist.MotherName = GeneralFunctions.GetData<String>(dr["motherName"]);
                    elist.Birthdate = GeneralFunctions.GetData<DateTime?>(dr["birthdate"]);
                    elist.FatherPhoneNumber = GeneralFunctions.GetData<String>(dr["fatherPhoneNumber"]);
                    elist.MotherPhoneNumber = GeneralFunctions.GetData<String>(dr["motherPhoneNumber"]);
                    elist.IsActive = GeneralFunctions.GetData<Boolean>(dr["isActive"]);
                    elist.IsStudent = GeneralFunctions.GetData<Boolean>(dr["isStudent"]);
                    elist.AddedOn = GeneralFunctions.GetData<DateTime>(dr["addedOn"]);

                    lst.Add(elist);
                }


                dr.Close();
                resultSet.Result = lst;
            }
            return resultSet;
        }

        public DataResultArgs<StudentEntity> Get_Student(string citizenshipNumber)
        {
            DataResultArgs<StudentEntity> resultSet = new DataResultArgs<StudentEntity>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@citizenshipNumber", citizenshipNumber);

            DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(cmd, "get_Student");
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
                    elist = new StudentEntity();
                    elist.Id = GeneralFunctions.GetData<Int32>(dr["id"]);
                    elist.CitizenshipNumber = GeneralFunctions.GetData<String>(dr["citizenshipNumber"]);
                    elist.Name = GeneralFunctions.GetData<String>(dr["name"]);
                    elist.Surname = GeneralFunctions.GetData<String>(dr["surname"]);
                    elist.MiddleName = GeneralFunctions.GetData<String>(dr["middleName"]);
                    elist.FatherName = GeneralFunctions.GetData<String>(dr["fatherName"]);
                    elist.MotherName = GeneralFunctions.GetData<String>(dr["motherName"]);
                    elist.Birthdate = GeneralFunctions.GetData<DateTime?>(dr["birthdate"]);
                    elist.FatherPhoneNumber = GeneralFunctions.GetData<String>(dr["fatherPhoneNumber"]);
                    elist.MotherPhoneNumber = GeneralFunctions.GetData<String>(dr["motherPhoneNumber"]);
                    elist.IsActive = GeneralFunctions.GetData<Boolean>(dr["isActive"]);
                    elist.IsStudent = GeneralFunctions.GetData<Boolean>(dr["isStudent"]);

                    resultSet.Result = elist;
                }
                dr.Close();
            }
            return resultSet;
        }

    }
}
