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
        private const int cacheMinutes = 60;
        //private static List<PaymentEntity> cacheListPaymentX = new List<PaymentEntity>();

        private List<PaymentEntity> cacheListPayment
        {
            get
            {
                if (isCacheAvailable)
                {
                    return dict_CacheListPayment[base.ProjectType].List;
                }
                return null;
            }
        }

        private static Dictionary<ProjectType, CacheEntity<PaymentEntity>> dict_CacheListPayment = new Dictionary<ProjectType, CacheEntity<PaymentEntity>>();


        private bool isCacheAvailable
        {
            get
            {
                if (dict_CacheListPayment.ContainsKey(base.ProjectType))
                {
                    var result = dict_CacheListPayment[base.ProjectType];
                    List<PaymentEntity> cacheListPayment = result.List;
                    return cacheListPayment != null && cacheListPayment.Count > 0 && result.EndDate > DateTime.Now;
                }
                return false;
            }
        }

        public PaymentBusiness(ProjectType projectType) : base(projectType)
        {
        }
        public DataResultArgs<PaymentEntity> Set_Payment(PaymentEntity entity)
        {
            DataResultArgs<PaymentEntity> resultSet = new DataResultArgs<PaymentEntity>();
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
                cmd.Parameters.AddWithValue("@isChangeAmountPaymentNotOK", entity.IsChangeAmountPaymentNotOK);
                cmd.Parameters.AddWithValue("@projectType", base.ProjectTypeInt16);

                using (SqlConnection con = Connection.Conn)
                {
                    con.Open();
                    DataResultArgs<DataTable> resultSetDt = DataProcess.ExecuteProcDataTable(con, cmd, "set_Payment_V2");
                    con.Close();

                    PaymentEntity paymentEntity = get_PaymentEntityWithDataTable(resultSetDt.Result);

                    if(cacheListPayment!=null)
                    {
                        if (isCacheAvailable && entity.DatabaseProcess == DatabaseProcess.Add)
                        {
                            if (cacheListPayment.Last().Id > paymentEntity.Id)
                            {
                                entity.DatabaseProcess = DatabaseProcess.Update;
                            }
                        }

                        if(entity.DatabaseProcess== DatabaseProcess.Add)
                        {
                            cacheListPayment.Add(paymentEntity);
                        }
                        else if (entity.DatabaseProcess == DatabaseProcess.Update)
                        {
                            int index = cacheListPayment.FindIndex(o => o.Id == paymentEntity.Id);
                            cacheListPayment.RemoveAt(index);
                            cacheListPayment.Insert(index, paymentEntity);
                        }
                        else if (entity.DatabaseProcess == DatabaseProcess.Deleted)
                        {
                            int index = cacheListPayment.FindIndex(o => o.Id == paymentEntity.Id);
                            cacheListPayment.RemoveAt(index);
                        }
                    }

                    resultSet = EntityHelper.CopyDataResultArgs<PaymentEntity>(resultSetDt, resultSet);
                    resultSet.Result = paymentEntity;

                    return resultSet;
                }
            }
            catch (Exception e)
            {
                DataResultArgs<PaymentEntity> result = new DataResultArgs<PaymentEntity>();
                result.HasError = true;
                result.ErrorDescription = e.Message;
                return result;
            }
        }

        public DataResultArgs<List<PaymentEntity>> Get_Payment(SearchEntity entity)
        {
            DataResultArgs<List<PaymentEntity>> resultSet = new DataResultArgs<List<PaymentEntity>>();
            DataProcess.ControlAdminAuthorization();

            if (isCacheAvailable)
            {
                if (entity.Id <= 0)
                {
                    resultSet.Result = cacheListPayment;
                }
                else
                {
                    resultSet.Result = cacheListPayment.Where(o => o.Id == entity.Id).ToList();
                }
                return resultSet;
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@isActive", entity.IsActive);
            cmd.Parameters.AddWithValue("@isDeleted", entity.IsDeleted);
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
                    if (dict_CacheListPayment.ContainsKey(base.ProjectType))
                    {
                        dict_CacheListPayment[base.ProjectType] = new CacheEntity<PaymentEntity>();
                        dict_CacheListPayment[base.ProjectType].List = lst;
                    }
                    else
                    {
                        CacheEntity<PaymentEntity> cache = new CacheEntity<PaymentEntity>();
                        cache.List = lst;
                        dict_CacheListPayment.Add(base.ProjectType, cache);
                    }
                }

                con.Close();
            }

            return resultSet;
        }

        public DataResultArgs<List<PaymentEntity>> Get_Payment(int studentId, string year)
        {
            DataProcess.ControlAdminAuthorization();

            DataResultArgs<List<PaymentEntity>> resultSet = new DataResultArgs<List<PaymentEntity>>();

            if (isCacheAvailable && cacheListPayment.Any(o => o.StudentId == studentId && o.Year == CommonFunctions.GetData<int>(year)))
            {
                resultSet.Result = cacheListPayment.Where(o => o.StudentId == studentId && o.Year == CommonFunctions.GetData<int>(year)).ToList();
                return resultSet;
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@studentId", studentId);
            cmd.Parameters.AddWithValue("@year", year);
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
                }

                con.Close();
            }

            return resultSet;
        }

        public DataResultArgs<List<PaymentEntity>> Get_Payment(int studentId)
        {
            DataProcess.ControlAdminAuthorization();

            DataResultArgs<List<PaymentEntity>> resultSet = new DataResultArgs<List<PaymentEntity>>();

            if (isCacheAvailable && cacheListPayment.Any(o => o.StudentId == studentId))
            {
                resultSet.Result = cacheListPayment.Where(o => o.StudentId == studentId).ToList();
                return resultSet;
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@studentId", studentId);
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
                }

                con.Close();
            }

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

        internal DataResultArgs<bool> SetStatusUnpaymentRecords(int studentId, bool isActive,bool isDeleted)
        {
            DataResultArgs<bool> resultSet = new DataResultArgs<bool>();
            DataProcess.ControlAdminAuthorization();

            List<PaymentEntity> lstPayment = Get_Payment(studentId).Result;

            foreach(PaymentEntity entity in lstPayment)
            {
                if (entity.IsPayment != true && (entity.Year > DateTime.Today.Year || (entity.Year == DateTime.Today.Year && entity.Month > DateTime.Today.Month)))
                {
                    entity.DatabaseProcess = (isDeleted) ? DatabaseProcess.Deleted : DatabaseProcess.Update;
                    entity.IsActive = isActive;
                    entity.IsDeleted = isDeleted;
                    entity.Amount = 0;
                    DataResultArgs<PaymentEntity> result = Set_Payment(entity);
                    if(result.HasError)
                    {
                        resultSet = EntityHelper.CopyDataResultArgs<PaymentEntity>(result, resultSet);
                        return resultSet;
                    }
                }
            }

            return resultSet;
        }

        public DataResultArgs<List<PaymentEntity>> Get_PaymentForCurrentMonth()
        {
            DataProcess.ControlAdminAuthorization();
            DataResultArgs<List<PaymentEntity>> resultSet = new DataResultArgs<List<PaymentEntity>>();

            if (isCacheAvailable)
            {
                resultSet.Result = cacheListPayment.Where(o => o.IsActive == true && o.IsDeleted != true && o.Year == DateTime.Today.Year && o.Month == DateTime.Today.Month && (int)o.ProjectType == base.ProjectTypeInt16).ToList();
                return resultSet;
            }
            else
            {
                Get_Payment(new SearchEntity() { IsActive = true, IsDeleted = false });
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@projectType", base.ProjectTypeInt16);

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result =
                    DataProcess.ExecuteProcDataReader(con, cmd, "Get_PaymentForCurrentMonth");
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
                }

                con.Close();
            }

            return resultSet;
        }

        public DataResultArgs<List<PaymentEntity>> Get_LastTwoMonths()
        {
            DataProcess.ControlAdminAuthorization();
            DataResultArgs<List<PaymentEntity>> resultSet = new DataResultArgs<List<PaymentEntity>>();

            if (isCacheAvailable)
            {
                DateTime today = DateTime.Today;
                DateTime lastMonthDate = DateTime.Today.AddMonths(-1);

                List<PaymentEntity> list = cacheListPayment.Where(o => (int)o.ProjectType == base.ProjectTypeInt16 && o.IsActive == true && o.IsDeleted != true && (o.Year == today.Year && o.Month == today.Month)).ToList();

                resultSet.Result = list;
                return resultSet;
            }
            else
            {
                Get_Payment(new SearchEntity() { IsActive = true, IsDeleted = false });
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@projectType", (short)base.ProjectType);

            using (SqlConnection con = Connection.Conn)
            {
                con.Open();
                DataResultArgs<SqlDataReader> result =
                    DataProcess.ExecuteProcDataReader(con, cmd, "get_LastTwoMonth");
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
                }

                con.Close();
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
            dict_CacheListPayment[base.ProjectType] = new CacheEntity<PaymentEntity>();
        }

        public PaymentEntity Get_PaymentWidthId(int id)
        {
            if (isCacheAvailable)
            {
                return cacheListPayment.FirstOrDefault(o => o.Id == id);
            }
            else
            {
                return Get_Payment(new SearchEntity() { Id = id }).Result.FirstOrDefault();
            }
        }
    }
}
