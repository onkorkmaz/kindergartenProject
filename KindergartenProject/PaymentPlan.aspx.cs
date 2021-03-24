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
    public partial class PaymentPlan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var master = this.Master as kindergarten;
            master.SetActiveMenuAttiributes(MenuList.PaymentPlan);

            divInformation.ListRecordPage = "StudentList.aspx";
            divInformation.NewRecordPage = "AddStudent.aspx";
        }
    }
}