﻿using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Entity;

namespace KindergartenProject
{
    public partial class kindergarten : System.Web.UI.MasterPage
    {
        public MenuList SelectedMenuList { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SelectedMenuList != MenuList.Login &&  (Session[CommonConst.Admin] == null || Session[CommonConst.ProjectType] == null) )
            {
                Response.Redirect("/uye-giris");
            }
            else
            {
                if (CurrentContex.Contex == null)
                    CurrentContex.Contex = Session[CommonConst.Admin] as AdminEntity;
            }

            if (KinderGartenWebService.List.ContainsKey(ListKey.SearchValue))
                txtSearchStudent.Text = KinderGartenWebService.List[ListKey.SearchValue];

            setNavbarVisible(SelectedMenuList != MenuList.Login);
        }


        public void SetActiveMenuAttiributes(MenuList selectedMenuList)
        {

            clearMenuActiveStyle(menuPanel);
            clearMenuActiveStyle(menuStudenList);
            clearMenuActiveStyle(menuStudentAdd);
            clearMenuActiveStyle(menuPaymentPlan);
            clearMenuActiveStyle(menuStudentAttendanceBookList);
            clearMenuActiveStyle(menuIncomeAndExpenseList);
            clearMenuActiveStyle(menuIncomeAndExpenseAdd);

            clearMenuActiveStyle(menuSettings);

            clearSubMenuStyle();


            SetMenuAttiributes(menuPanel, selectedMenuList == MenuList.Panel);
            SetMenuAttiributes(menuStudenList, selectedMenuList == MenuList.StudentList);
            SetMenuAttiributes(menuStudentAdd, selectedMenuList == MenuList.StudenAdd);
            SetMenuAttiributes(menuPaymentPlan, selectedMenuList == MenuList.PaymentPlan);
            SetMenuAttiributes(menuStudentAttendanceBookList, selectedMenuList == MenuList.StudentAttendanceBookList);
            SetMenuAttiributes(menuIncomeAndExpenseAdd, selectedMenuList == MenuList.IncomeAndExpenseAdd);
            SetMenuAttiributes(menuIncomeAndExpenseList, selectedMenuList == MenuList.IncomeAndExpenseList);

            SetMenuAttiributes(menuSettings, selectedMenuList == MenuList.PaymentType, menuPaymentType);
            SetMenuAttiributes(menuSettings, selectedMenuList == MenuList.ClassList, menuClassList);
            SetMenuAttiributes(menuSettings, selectedMenuList == MenuList.WorkerList, menuWorkerList);
            SetMenuAttiributes(menuSettings, selectedMenuList == MenuList.IncomeAndExpenseType, menuIncomeAndExpenseType);
            SetMenuAttiributes(menuSettings, selectedMenuList == MenuList.Authority, menuAuthority);
        }

        private void clearSubMenuStyle()
        {
            ui.Attributes.Remove("class");
            ui.Attributes.Add("class", "sidebar-dropdown list-unstyled collapse");
        }
        private void clearMenuActiveStyle(HtmlGenericControl panel)
        {
            panel.Attributes.Remove("class");
            panel.Attributes.Add("class", "sidebar-item");
        }

        private void SetMenuAttiributes(HtmlGenericControl panel, bool isActiveMenu, HtmlGenericControl subMenuId)
        {
            if (isActiveMenu)
            {
                ui.Attributes.Remove("class");
                ui.Attributes.Add("class", "sidebar-dropdown list-unstyled collapse show");
            }
            SetMenuAttiributes(panel, isActiveMenu);
            SetMenuAttiributes(subMenuId, isActiveMenu);
        }

        private void SetMenuAttiributes(HtmlGenericControl panel, bool isActiveMenu)
        {
            if (isActiveMenu)
            {
                panel.Attributes.Remove("class");
                panel.Attributes.Add("class", "sidebar-item active");
            }       
        }

        public void SetVisibleSearchText(bool isVisible)
        {
            txtSearchStudent.Visible = isVisible;
        }

        public void setNavbarVisible(bool isVisible)
        {
            sidebar.Visible = isVisible;
        }
    }

}