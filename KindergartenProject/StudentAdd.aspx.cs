﻿using Business;
using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KindergartenProject
{
    public partial class StudentAdd : BasePage
    {
        public StudentAdd() : base(AuthorityScreenEnum.Ogrenci_Islem)
        {
        }

        #region VARIABLES
        StudentBusiness business = null;
        List<StudentEntity> lst;
        #endregion VARIABLES

        public const string paymentDetail = "Ödeme Detayı";

        #region PROPERTIES
        private StudentEntity currentRecord;
        public StudentEntity CurrentRecord
        {
            set
            {
                currentRecord = value;
                hdnId.Value = currentRecord.Id.ToString();
                txtTckn.Text = currentRecord.CitizenshipNumber;
                txtName.Text = currentRecord.Name;
                txtSurname.Text = currentRecord.Surname;
                txtMiddleName.Text = currentRecord.MiddleName;
                txtFatherName.Text = currentRecord.FatherName;
                txtMotherName.Text = currentRecord.MotherName;
                if (currentRecord.Birthday.HasValue)
                {
                    txtBirthday.Text = currentRecord.Birthday.Value.ToString("yyyy-MM-dd");
                }
                txtFatherPhoneNumber.Text = currentRecord.FatherPhoneNumber;
                txtMotherPhoneNumber.Text = currentRecord.MotherPhoneNumber;
                chcIsActive.Checked = (currentRecord.IsActive.HasValue) ? currentRecord.IsActive.Value : false;
                drpStudentState.SelectedValue = (currentRecord.IsStudent) ? "0" : "1";

                hdnStudentState.Value = (currentRecord.IsStudent) ? "0" : "1";

                //if (currentRecord.DateOfMeeting.HasValue)
                //{
                //    txtDateOfMeeting.Text = currentRecord.DateOfMeeting.Value.ToString("yyyy-MM-dd");
                //}

                if (currentRecord.InterviewDate.HasValue)
                {
                    txtInterviewDate.Text = currentRecord.InterviewDate.Value.ToString("yyyy-MM-dd");
                }

                chcInterview.Checked = currentRecord.IsInterview;

                txtNotes.Text = currentRecord.Notes;
                txtSpokenPrice.Text = currentRecord.SpokenPrice.ToString();
                txtEmail.Text = currentRecord.Email;

                if (currentRecord.ClassId > 0)
                {
                    drpClassList.SelectedValue = currentRecord.ClassId.ToString();
                    hdnCurrentClassId.Value = currentRecord.ClassId.ToString();
                }

                if (CommonFunctions.GetData<int>(currentRecord.SchoolClass) > 0)
                {
                    drpSchoolClass.SelectedIndex = CommonFunctions.GetData<int>(currentRecord.SchoolClass);
                }


            }
        }
        #endregion PROPERTIES

        #region CONTRUCTOR && PAGE_LOAD
        protected void Page_Load(object sender, EventArgs e)
        {
            DataResultArgs<StudentEntity> resultSet = new DataResultArgs<StudentEntity>();
            business = new StudentBusiness(_ProjectType);

            divInformation.ListRecordPage = "/ogrenci-listesi";
            divInformation.NewRecordPage = "/ogrenci-ekle";

            divInformation.InformationVisible = false;

            var master = this.Master as kindergarten;
            master.SetActiveMenuAttiributes(MenuList.StudenAdd);
            master.SetVisibleSearchText(false);
            this.Title = this.Title + " - " + master.SetTitle(_ProjectType);

            btnPaymentDetail.Visible = false;
            btnDelete.Visible = false;

            object idObject = Page.RouteData.Values["student_id"];
            int idInt = CommonFunctions.GetData<int>(idObject);
            if (idObject != null && idInt > 0)
            {
                resultSet = new StudentBusiness(_ProjectType).Get_StudentWithId(idInt);
            }

            if (!Page.IsPostBack)
            {
                if (_ProjectType != ProjectType.BenimDunyamEgitimMerkeziIstiklalCaddesi)
                {
                    divSchoolClass.Visible = false;
                }

                txtInterviewDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                DataResultArgs<List<ClassEntity>> resultSetClassList = new ClassBusiness(_ProjectType).Get_Class(new SearchEntity() { IsActive = true, IsDeleted = false });
                if (resultSetClassList.HasError)
                {
                    divInformation.ErrorText = resultSetClassList.ErrorDescription;
                    return;
                }
                else
                {
                    List<ClassEntity> classList = resultSetClassList.Result;
                    List<ClassEntity> list = new List<ClassEntity>();

                    list.Add(new ClassEntity() { Id = -1, Name = "Seçiniz..." });
                    if (resultSetClassList.Result != null)
                    {

                        foreach (ClassEntity entity in classList)
                        {
                            list.Add(entity);
                        }
                    }

                    drpClassList.DataSource = list;
                    drpClassList.DataValueField = "Id";
                    drpClassList.DataTextField = "ClassAndMainTeacherName";
                    drpClassList.DataBind();
                }

                if (resultSet.HasError)
                {
                    divInformation.ErrorText = resultSet.ErrorDescription;
                }
                else if (resultSet.Result != null)
                {
                    CurrentRecord = resultSet.Result;
                }

                int classId = CommonFunctions.GetData<int>(drpClassList.SelectedValue);
                if (classId > 0)
                {
                    lblMaxStudentCount.Text = new KinderGartenWebService().CalculateRecordedStudentCount(classId.ToString());
                }
            }

            if (resultSet.HasError)
            {
                divInformation.ErrorText = resultSet.ErrorDescription;
            }
            else if (resultSet.Result != null)
            {
                StudentEntity _sEntity = resultSet.Result;
                btnSubmit.Text = ButtonText.Update;
                btnPaymentDetail.Visible = _sEntity.IsStudent;
                btnDelete.Visible = true;

                if (_sEntity.IsInterview)
                {
                    interviewDate.Style.Remove("display");
                }
            }
        }


        #endregion CONTRUCTOR && PAGE_LOAD

        #region METHODS

        private StudentEntity getStudentEntity()
        {
            StudentEntity entity = new StudentEntity();
            entity.Id = CommonFunctions.GetData<Int32>(hdnId.Value);
            entity.CitizenshipNumber = txtTckn.Text;
            entity.Name = txtName.Text;
            entity.Surname = txtSurname.Text;
            entity.MiddleName = txtMiddleName.Text;
            entity.FatherName = txtFatherName.Text;
            entity.MotherName = txtMotherName.Text;

            if (!string.IsNullOrEmpty(txtBirthday.Text))
            {
                entity.Birthday = CommonFunctions.GetData<DateTime>(txtBirthday.Text);
            }
            entity.FatherPhoneNumber = txtFatherPhoneNumber.Text;
            entity.MotherPhoneNumber = txtMotherPhoneNumber.Text;
            entity.IsActive = chcIsActive.Checked;
            entity.IsStudent = drpStudentState.SelectedValue == "0";
            entity.Notes = txtNotes.Text;
            //entity.DateOfMeeting = GeneralFunctions.GetData<DateTime>(txtDateOfMeeting.Text);
            entity.SpokenPrice = CommonFunctions.GetData<decimal>(txtSpokenPrice.Text);
            entity.Email = txtEmail.Text;
            entity.ClassId = CommonFunctions.GetData<int>(drpClassList.SelectedValue);
            entity.IsInterview = chcInterview.Checked;
            entity.InterviewDate = CommonFunctions.GetData<DateTime>(txtInterviewDate.Text);

            if (drpSchoolClass.SelectedIndex > 0)
            {
                entity.SchoolClassEnum = (SchoolClassEnum)CommonFunctions.GetData<int>(drpSchoolClass.SelectedValue);
            }

            return entity;
        }

        private bool hasUnPaymentRefund(int id)
        {
            bool hasUnPaymentRefund = new PaymentBusiness(_ProjectType).HasUnPaymentRefund(id);
            return hasUnPaymentRefund;
        }
        #endregion METHODS

        #region EVENTS
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Button btn = ((Button)sender);
            bool insert = CommonFunctions.GetData<int>(hdnId.Value) <= 0;
            DatabaseProcess databaseProcess = (insert) ? DatabaseProcess.Add : DatabaseProcess.Update;
            StudentEntity entity = getStudentEntity();
            entity.DatabaseProcess = databaseProcess;

            if (databaseProcess == DatabaseProcess.Add)
            {
                DataResultArgs<StudentEntity> resultSet = business.AddStudent(entity);
                showInfoMessage(resultSet, DatabaseProcess.Add);
            }
            else if (databaseProcess == DatabaseProcess.Update)
            {
                DataResultArgs<StudentEntity> resultSet = business.UpdateStudent(entity);
                showInfoMessage(resultSet, DatabaseProcess.Update);
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ogrenci-ekle");
        }


        #endregion EVENTS

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int id = CommonFunctions.GetData<int>(hdnId.Value);

            if (id > 0)
            {
                if (!chcIsActive.Checked)
                {
                    bool isNotValid = hasUnPaymentRefund(id);
                    if (isNotValid)
                    {
                        divInformation.ErrorText = "Öğrencinin ödenmemiş aidatları bulunmaktadır.";
                        return;
                    }
                }

                StudentEntity entity = getStudentEntity();
                DataResultArgs<StudentEntity> resultSet = business.DeleteStudent(entity);
                showInfoMessage(resultSet, DatabaseProcess.Deleted);

            }
        }

        private void showInfoMessage(DataResultArgs<StudentEntity> resultSet, DatabaseProcess databaseProcess)
        {
            if (resultSet.HasError)
            {
                divInformation.ErrorText = resultSet.ErrorDescription;
                return;
            }
            else
            {
                StudentEntity entity = resultSet.Result;
                if (databaseProcess == DatabaseProcess.Deleted)
                {
                    divInformation.SuccessfulText = RecordMessage.Delete;
                    pnlBody.Enabled = false;
                }
                else
                {
                    divInformation.SuccessfulText = (databaseProcess == DatabaseProcess.Add) ? RecordMessage.Add : RecordMessage.Update;
                    btnSubmit.Text = ButtonText.Submit;
                    pnlBody.Enabled = false;

                    if (entity.IsStudent)
                    {
                        divInformation.SetAnotherText("<a href = \"/odeme-plani-detay/" + resultSet.Result.Id + "\">" + paymentDetail + "</a>");
                    }
                    else
                    {
                        btnPaymentDetail.Visible = false;
                    }
                }
            }
        }

        protected void btnPayment_Click(object sender, EventArgs e)
        {
            object Id = Page.RouteData.Values["student_id"];

            if (Id != null)
            {
                string IdDecrypt = Cipher.Decrypt(Id.ToString());

                int id = CommonFunctions.GetData<int>(IdDecrypt);
                if (id > 0)
                {
                    string link = "~/odeme-plani-detay/" + Cipher.Encrypt(id.ToString());
                    Response.Redirect(link);
                }
            }
        }
    }
}