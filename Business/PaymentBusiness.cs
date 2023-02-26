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
    public class PaymentBusiness : BaseBusiness
    {
        CacheBusines<PaymentEntity> cache;
        public PaymentBusiness(ProjectType projectType) : base(projectType)
        {
            cache = new CacheBusines<PaymentEntity>(projectType, CacheType.PaymentList);
        }
        public DataResultArgs<PaymentEntity> Set_Payment(PaymentEntity entity)
        {
            DataResultArgs<PaymentEntity> resultSet = new DataResultArgs<PaymentEntity>();
            DataResultArgs<DataTable> resultSetDt = new DataResultArgs<DataTable>();
            PaymentEntity paymentEntity = new PaymentEntity();
            try
            {
                DataProcess.ControlAdminAuthorization(true);

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DatabaseProcess", entity.DatabaseProcess);
                cmd.Parameters.AddWithValue("@id", entity.Id);
                cmd.Parameters.AddWithValue("@studentId", entity.StudentId);
                cmd.Parameters.AddWithValue("@year", entity.Year);
                cmd.Parameters.AddWithValue("@month", entity.Month);
                cmd.Parameters.AddWithValue("@amount", entity.Amount);
                cmd.Parameters.AddWithValue("@isPayment", entity.IsPayment);
                cmd.Parameters.AddWithValue("@paymentDate", entity.PaymentDate);
                cmd.Parameters.AddWithValue("@paymentType", entity.PaymentType);
                cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
                cmd.Parameters.AddWithValue("@projectType", base.ProjectTypeInt16);

                using (SqlConnection con = Connection.Conn)
                {
                    con.Open();
                    resultSetDt = DataProcess.ExecuteProcDataTable(con, cmd, "set_Payment_V2");
                    con.Close();
                }

                paymentEntity = get_PaymentEntityWithDataTable(resultSetDt.Result);
                cache.UpdateCache(paymentEntity);
                resultSet = EntityHelper.CopyDataResultArgs<PaymentEntity>(resultSetDt, resultSet);
                resultSet.Result = paymentEntity;
                return resultSet;

            }
            catch (Exception e)
            {
                DataResultArgs<PaymentEntity> result = new DataResultArgs<PaymentEntity>();
                result.HasError = true;
                result.ErrorDescription = e.Message;
                return result;
            } 
        }

        internal DataResultArgs<bool> SetStatusUnpaymentRecords(int studentId, bool isActive, bool isDeleted)
        {
            DataResultArgs<bool> resultSet = new DataResultArgs<bool>();
            DataProcess.ControlAdminAuthorization();

            List<PaymentEntity> lstPayment = Get_Payment(studentId).Result;

            foreach (PaymentEntity entity in lstPayment)
            {
                if (entity.IsPayment != true && (entity.Year > DateTime.Today.Year || (entity.Year == DateTime.Today.Year && entity.Month > DateTime.Today.Month)))
                {
                    entity.DatabaseProcess = (isDeleted) ? DatabaseProcess.Deleted : DatabaseProcess.Update;
                    entity.IsActive = isActive;
                    entity.IsDeleted = isDeleted;
                    entity.Amount = 0;
                    DataResultArgs<PaymentEntity> result = Set_Payment(entity);
                    if (result.HasError)
                    {
                        resultSet = EntityHelper.CopyDataResultArgs<PaymentEntity>(result, resultSet);
                        return resultSet;
                    }
                }
            }

            return resultSet;
        }

        public DataResultArgs<List<PaymentEntity>> Get_Payment()
        {
            DataResultArgs<List<PaymentEntity>> resultSet = new DataResultArgs<List<PaymentEntity>>();
            DataProcess.ControlAdminAuthorization();

            if (cache.IsCacheAvailable)
            {
                resultSet.Result = cache.CacheList;
                return resultSet;
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@isDeleted", false);
            cmd.Parameters.AddWithValue("@projectType", (short)base.ProjectType);

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result = DataProcess.ExecuteProcDataReader(con, cmd, "get_Payment");
                if (result.HasError)
                {
                    resultSet.HasError = result.HasError;
                    resultSet.ErrorDescription = result.ErrorDescription;
                    resultSet.ErrorCode = result.ErrorCode;
                }
                else
                {
                    List<PaymentEntity> lst = GetList(result);
                    resultSet.Result = lst;
                    cache.AddCache(CacheType.PaymentList, lst);
                }

                con.Close();
            }

            return resultSet;
        }


        public DataResultArgs<List<PaymentEntity>> Get_Payment(int studentId, int year)
        {
            DataProcess.ControlAdminAuthorization();

            DataResultArgs<List<PaymentEntity>> resultSet = new DataResultArgs<List<PaymentEntity>>();

            if (!cache.IsCacheAvailable)
            {
                Get_Payment();
                new LogBusiness(base.ProjectType).InsertLog("Ödeme DB Alındı");
            }

            resultSet.Result = cache.CacheList.Where(o => o.StudentId == studentId && o.Year == year).ToList();           
            return resultSet;

        }

        public DataResultArgs<List<PaymentEntity>> Get_Payment(int studentId, int year,int month)
        {
            DataProcess.ControlAdminAuthorization();

            DataResultArgs<List<PaymentEntity>> resultSet = new DataResultArgs<List<PaymentEntity>>();

            if (!cache.IsCacheAvailable)
            {
                Get_Payment();
                new LogBusiness(base.ProjectType).InsertLog("Ödeme DB Alındı");
            }

            resultSet.Result = cache.CacheList.Where(o => o.StudentId == studentId && o.Year == year && o.Month == month).ToList();
            return resultSet;

        }

        public DataResultArgs<PaymentEntity> Get_Payment(int studentId, int year, int month, int paymentType)
        {
            DataProcess.ControlAdminAuthorization();

            if (!cache.IsCacheAvailable)
            {
                Get_Payment();
                new LogBusiness(base.ProjectType).InsertLog("Ödeme DB Alındı");
            }

            DataResultArgs<PaymentEntity> resultSet = new DataResultArgs<PaymentEntity>();

            resultSet.Result = cache.CacheList.FirstOrDefault(o => o.StudentId == studentId && o.Year == year && o.Month == month && o.PaymentType == paymentType);
            new LogBusiness(base.ProjectType).InsertLog("ödemeyi cache den aldı");
            return resultSet;
        }

        public DataResultArgs<List<PaymentEntity>> Get_Payment(int studentId)
        {
            DataProcess.ControlAdminAuthorization();

            if (!cache.IsCacheAvailable)
            {
                Get_Payment();
                new LogBusiness(base.ProjectType).InsertLog("Ödeme DB Alındı");
            }

            DataResultArgs<List<PaymentEntity>> resultSet = new DataResultArgs<List<PaymentEntity>>();

            resultSet.Result = cache.CacheList.Where(o => o.StudentId == studentId).ToList();
            return resultSet;
        }

        private static List<PaymentEntity> GetList(DataResultArgs<SqlDataReader> result)
        {
            DataProcess.ControlAdminAuthorization();

            SqlDataReader dr = result.Result;
            List<PaymentEntity> lst = new List<PaymentEntity>();
            while (dr != null && dr.Read())
            {
                var paymentEntity = new PaymentEntity
                {
                    Id = CommonFunctions.GetData<Int32>(dr["id"]),
                    StudentId = CommonFunctions.GetData<Int32?>(dr["studentId"]),
                    Year = CommonFunctions.GetData<Int16?>(dr["year"]),
                    Month = CommonFunctions.GetData<Int16?>(dr["month"]),
                    Amount = CommonFunctions.GetData<Decimal?>(dr["amount"]),
                    IsPayment = CommonFunctions.GetData<Boolean?>(dr["isPayment"]),
                    PaymentDate = CommonFunctions.GetData<DateTime?>(dr["paymentDate"]),
                    PaymentType = CommonFunctions.GetData<Int32?>(dr["paymentType"]),
                    IsActive = CommonFunctions.GetData<Boolean?>(dr["isActive"]),
                    ProjectType = (ProjectType)CommonFunctions.GetData<Int16>(dr["projectType"])

                };
                lst.Add(paymentEntity);
            }
            if (dr != null)
                dr.Close();
            return lst;
        }


        private static PaymentEntity get_PaymentEntityWithDataTable(DataTable dt)
        {
            PaymentEntity paymentEntity = new PaymentEntity();

            if (dt == null || dt.Rows.Count == 0)
                return paymentEntity;

            foreach (DataRow dr in dt.Rows)
            {
                paymentEntity = new PaymentEntity
                {
                    Id = CommonFunctions.GetData<Int32>(dr["id"]),
                    StudentId = CommonFunctions.GetData<Int32?>(dr["studentId"]),
                    Year = CommonFunctions.GetData<Int16?>(dr["year"]),
                    Month = CommonFunctions.GetData<Int16?>(dr["month"]),
                    Amount = CommonFunctions.GetData<Decimal?>(dr["amount"]),
                    IsPayment = CommonFunctions.GetData<Boolean?>(dr["isPayment"]),
                    PaymentDate = CommonFunctions.GetData<DateTime?>(dr["paymentDate"]),
                    PaymentType = CommonFunctions.GetData<Int32?>(dr["paymentType"]),
                    IsActive = CommonFunctions.GetData<Boolean?>(dr["isActive"]),
                    ProjectType = (ProjectType)CommonFunctions.GetData<Int16>(dr["projectType"])

                };
            }
            return paymentEntity;
        }

        public DataResultArgs<List<ExpenseSummary>> Get_ExpenseSummaryWithMonthAndYear(int year, int month)
        {
            DataProcess.ControlAdminAuthorization();

            DataResultArgs<List<ExpenseSummary>> resultSet = new DataResultArgs<List<ExpenseSummary>>();
            List<ExpenseSummary> list = new List<ExpenseSummary>();
            SqlCommand cmd = new SqlCommand();
            ExpenseSummary summary = new ExpenseSummary();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@projectType", (short)base.ProjectType);
            cmd.Parameters.AddWithValue("@year", year);
            cmd.Parameters.AddWithValue("@month", month);

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result =
                    DataProcess.ExecuteProcDataReader(con, cmd, "Get_ExpenseSummaryWithMonthAndYear");
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
                        summary = new ExpenseSummary
                        {
                            ExpenseTypeId = CommonFunctions.GetData<int>(dr["expenseTypeId"]),
                            ExpenseTypeName = CommonFunctions.GetData<string>(dr["expenseTypeName"]),
                            ExpenseAmount = CommonFunctions.GetData<decimal>(dr["expenseAmount"]),
                            ProjectType = base.ProjectType,
                            Month = month,
                            Year = year
                        };

                        list.Add(summary);
                    }
                    con.Close();
                }

                resultSet.Result = list;
            }

            return resultSet;
        }

        public DataResultArgs<List<PaymentSummary>> Get_IncomeAndExpenseSummaryWithMonthAndYear(int year, int month)
        {
            DataProcess.ControlAdminAuthorization();

            DataResultArgs<List<PaymentSummary>> resultSet = new DataResultArgs<List<PaymentSummary>>();
            List<PaymentSummary> list = new List<PaymentSummary>();
            SqlCommand cmd = new SqlCommand();
            PaymentSummary summary = new PaymentSummary();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@projectType", (short)base.ProjectType);
            cmd.Parameters.AddWithValue("@year", year);
            cmd.Parameters.AddWithValue("@month", month);

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result =
                    DataProcess.ExecuteProcDataReader(con, cmd, "Get_IncomeAndExpenseSummaryWithMonthAndYear");
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
                        summary = new PaymentSummary
                        {
                            IncomeForStudentPayment = CommonFunctions.GetData<decimal>(dr["incomeForStudentPayment"]),
                            WaitingIncomeForStudentPayment = CommonFunctions.GetData<decimal>(dr["waitingIncomeForStudentPayment"]),
                            IncomeWithoutStudentPayment = CommonFunctions.GetData<decimal>(dr["incomeWithoutStudentPayment"]),

                            WorkerExpenses = CommonFunctions.GetData<decimal>(dr["workerExpenses"]),
                            ExpenseWithoutWorker = CommonFunctions.GetData<decimal>(dr["expenseWithoutWorker"]),
                            CurrentBalance = CommonFunctions.GetData<decimal>(dr["currentBalance"]),
                            TotalBalance = CommonFunctions.GetData<decimal>(dr["totalBalance"]),

                            ProjectType = (ProjectType)CommonFunctions.GetData<Int16>(dr["projectType"]),
                            Month = month,
                            Year = year
                        };

                        list.Add(summary);
                    }
                    con.Close();
                }

                resultSet.Result = list;
            }

            return resultSet;
        }

        public DataResultArgs<List<PaymentSummaryDetail>> Get_IncomeAndExpenseSummaryDetailWithMonthAndYear(int year, int month)
        {
            DataProcess.ControlAdminAuthorization();

            DataResultArgs<List<PaymentSummaryDetail>> resultSet = new DataResultArgs<List<PaymentSummaryDetail>>();
            List<PaymentSummaryDetail> list = new List<PaymentSummaryDetail>();
            SqlCommand cmd = new SqlCommand();
            PaymentSummaryDetail summary = new PaymentSummaryDetail();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@projectType", (short)base.ProjectType);
            cmd.Parameters.AddWithValue("@year", year);
            cmd.Parameters.AddWithValue("@month", month);

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result =
                    DataProcess.ExecuteProcDataReader(con, cmd, "Get_IncomeAndExpenseSummaryDetailWithMonthAndYear");
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
                        summary = new PaymentSummaryDetail
                        {
                            IsPayment = CommonFunctions.GetData<bool>(dr["isPayment"]),
                            PaymentTypeName = CommonFunctions.GetData<string>(dr["paymentTypeName"]),
                            Amount = CommonFunctions.GetData<decimal>(dr["amount"]),
                            ProjectType = base.ProjectType,
                            Month = month,
                            Year = year
                        };

                        list.Add(summary);
                    }
                    con.Close();
                }

                resultSet.Result = list;
            }

            return resultSet;
        }

        public void ClearPaymentCache()
        {
            cache.ClearCache(CacheType.PaymentList);
        }
    }
}
