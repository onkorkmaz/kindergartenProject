﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using  Common;

namespace KindergartenProject
{
    public partial class Exit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[CommonConst.Admin] != null)
            {
                Session[CommonConst.Admin] = null;
                Session.Abandon();
                Response.Redirect("Login.aspx");
            }
        }
    }
}