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
    public class PaymentTypeBusiness : BaseBusiness
    {
        private static Dictionary<short, DataResultArgs<List<PaymentTypeEntity>>> staticPaymentTypeList;

        public PaymentTypeBusiness(ProjectType projectType) : base(projectType)
        {
            staticPaymentTypeList = new Dictionary<short, DataResultArgs<List<PaymentTypeEntity>>>();
        }


        public DataResultArgs<bool> Set_PaymentType(PaymentTypeEntity entity)
        {
            try
            {
                DataProcess.ControlAdminAuthorization(true);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DatabaseProcess", entity.DatabaseProcess);
                cmd.Parameters.AddWithValue("@id", entity.Id);
                cmd.Parameters.AddWithValue("@name", entity.Name);
                cmd.Parameters.AddWithValue("@amount", entity.Amount);
                cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
                cmd.Parameters.AddWithValue("@projectType", (short)base.ProjectType);

                using (SqlConnection con = Connection.Conn)
                {
                    con.Open();
                    DataResultArgs<bool> resultSet = DataProcess.ExecuteProc(con, cmd, "set_PaymentType");
                    con.Close();

                    if (staticPaymentTypeList.ContainsKey(base.ProjectTypeInt16))
                    {
                        staticPaymentTypeList[base.ProjectTypeInt16] = null;
                    }


                    return resultSet;
                }
            }
            catch (Exception ex)
            {
                DataResultArgs<bool> result = new DataResultArgs<bool>();
                result.HasError = true;
                result.ErrorDescription = ex.Message;
                return result;
            }
        }

        public DataResultArgs<PaymentTypeEntity> Get_PaymentTypeWithId(int id)
        {
            DataProcess.ControlAdminAuthorization();

            DataResultArgs<List<PaymentTypeEntity>> resultSet = Get_PaymentType(new SearchEntity()
                {IsActive = true, IsDeleted = false});

            DataResultArgs<PaymentTypeEntity> result = new DataResultArgs<PaymentTypeEntity>();

            EntityHelper.CopyDataResultArgs<PaymentTypeEntity>(resultSet, result);

            result.Result = resultSet.Result.FirstOrDefault(o => o.Id == id);

            return result;

        }

        public DataResultArgs<List<PaymentTypeEntity>> Get_PaymentType(SearchEntity entity)
        {
            DataProcess.ControlAdminAuthorization();

            if (!staticPaymentTypeList.ContainsKey(base.ProjectTypeInt16))
            {

                DataResultArgs<List<PaymentTypeEntity>> resultSet = new DataResultArgs<List<PaymentTypeEntity>>();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", entity.Id);
                cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
                cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);
                cmd.Parameters.AddWithValue("@projectType", (short)base.ProjectType);

                using (SqlConnection con = Connection.Conn)
                {
                    con.Open();
                    DataResultArgs<SqlDataReader> result =
                        DataProcess.ExecuteProcDataReader(con, cmd, "get_PaymentType");
                    if (result.HasError)
                    {
                        resultSet.HasError = result.HasError;
                        resultSet.ErrorDescription = result.ErrorDescription;
                        resultSet.ErrorCode = result.ErrorCode;
                    }
                    else
                    {
                        SqlDataReader dr = result.Result;
                        List<PaymentTypeEntity> lst = new List<PaymentTypeEntity>();
                        PaymentTypeEntity elist;
                        while (dr!=null &&  dr.Read())
                        {
                            elist = new PaymentTypeEntity();
                            elist.Id = GeneralFunctions.GetData<Int32>(dr["id"]);
                            elist.Name = GeneralFunctions.GetData<String>(dr["name"]);
                            elist.Amount = GeneralFunctions.GetData<Decimal?>(dr["amount"]);
                            elist.IsActive = GeneralFunctions.GetData<Boolean?>(dr["isActive"]);
                            elist.UpdatedOn = GeneralFunctions.GetData<DateTime>(dr["updatedOn"]);
                            elist.ProjectType = (ProjectType)GeneralFunctions.GetData<Int16>(dr["projectType"]);
 
                            lst.Add(elist);
                        }

                        if (dr != null)
                            dr.Close();
                        
                        resultSet.Result = lst;
                    }
                    con.Close();
                }

                staticPaymentTypeList.Add(base.ProjectTypeInt16, resultSet);
            }

            return staticPaymentTypeList[base.ProjectTypeInt16];
        }


        public List<PaymentTypeEntity> Get_PaymentType()
        {
            return Get_PaymentType(new SearchEntity() {IsActive = true, IsDeleted = false}).Result;
        }
    }
}
