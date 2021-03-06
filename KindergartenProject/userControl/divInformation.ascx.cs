﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KindergartenProject.userControl
{
    public partial class divInformation : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Information.Visible = false;
                lblError.Text = "";
                lblSuccess.Text = "";
            }
        }

        private bool _informationVisible;
        public bool InformationVisible
        {
            set
            {
                _informationVisible = value;
                Information.Visible = _informationVisible;
            }
            get
            {
                return _informationVisible;
            }
        }

        private string _errorText;
        public string ErrorText
        {
            get 
            {
                return _errorText;
            }
            set
            {
                _errorText = value;
                SetErrorText(_errorText);
            }
        }

        private string _successfulText;
        public string SuccessfulText
        {
            get
            {
                return _successfulText;
            }
            set
            {
                _successfulText = value;
                SetSuccessfulText(_successfulText);
            }
        }

        public string NewRecordPage { get; set; }
        public string ListRecordPage { get; set; }

        private void SetSuccessfulText(string text)
        {
            Information.Visible = true;
            divError.Visible = false;
            divSuccuess.Visible = true;
            lblError.Text = "";
            lblSuccess.Text = text;
        }

        private void SetErrorText(string text)
        {
            Information.Visible = true;
            divError.Visible = true;
            divSuccuess.Visible = false;
            lblError.Text = text;
            lblSuccess.Text = "";
        }

        protected void lnkNewRecord_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/" + NewRecordPage + "");
        }

        protected void lnkRecordList_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/" + ListRecordPage + "");
        }
    }
}