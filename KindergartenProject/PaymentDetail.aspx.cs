﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using Entity;
using Business;
using System.Text;

namespace KindergartenProject
{
    public partial class PaymentDetail : System.Web.UI.Page
    {
        private const string studentDoesNotFound = "Ödeme detayı için öğrenci seçmelisiniz !!!";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int year = DateTime.Today.Year;

                for(int i = -1;i<5;i++)
                {
                    drpYear.Items.Add(new ListItem((year + i).ToString(), (year + i).ToString()));
                }
                drpYear.SelectedValue = year.ToString();
            }

            divInformation.InformationVisible = false;
            divInformation.ListRecordPage = "/odeme-plani";

            var master = this.Master as kindergarten;
            master.SetActiveMenuAttiributes(MenuList.PaymentPlan);
            master.SetVisibleSearchText(false);


            object Id = Page.RouteData.Values["student_id"];


            if (Id == null)
            {
                divInformation.ErrorText = studentDoesNotFound;
                divInformation.ErrorLinkText = "Ödeme Listesi için tıklayınız ...";
                divInformation.ErrorLink = "/odeme-plani";
            }
            else
            {
                string IdDecrypt = Cipher.Decrypt(Id.ToString());
                hdnId.Value = IdDecrypt;
                int id = GeneralFunctions.GetData<int>(IdDecrypt);
                if (id <= 0)
                {
                    divInformation.ErrorText = studentDoesNotFound;
                    divInformation.ErrorLinkText = "Ödeme Listesi için tıklayınız ...";
                    divInformation.ErrorLink = "/odeme-plani";
                }
                else
                {
                    StudentEntity entity = new StudentBusiness().Get_Student(new SearchEntity() { Id = id }).Result[0];

                    lblStudentInto.Text = "<a href = \"/ogrenci-guncelle/" + entity.EncryptId + "\">" +
                                          entity.FullName.ToUpper() + "</a> &nbsp;&nbsp;&nbsp;";
                    lblStudentInto.Text += "<a href= '/email-gonder/" + entity.EncryptId +
                                           "'><img title='Mail Gönder' src ='img/icons/email.png'/></a>";

                }
            }
        }
    }
}