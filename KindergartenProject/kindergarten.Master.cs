﻿using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace KindergartenProject
{
    public partial class kindergarten : System.Web.UI.MasterPage
    {
        public MenuList SelectedMenuList { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (KinderGartenWebService.list.ContainsKey(ListKey.SearchValue))
                txtSearchStudent.Text = KinderGartenWebService.list[ListKey.SearchValue];
        }

        public void SetActiveMenuAttiributes(MenuList selectedMenuList)
        {
            SetMenuAttiributes(menuPanel, selectedMenuList == MenuList.Panel);
            SetMenuAttiributes(menuStudenList, selectedMenuList == MenuList.StudentList);
            SetMenuAttiributes(menuAddStudent, selectedMenuList == MenuList.AddStudenList);
            SetMenuAttiributes(menuPaymentPlan, selectedMenuList == MenuList.PaymentPlan);
            SetMenuAttiributes(menuSettings, selectedMenuList == MenuList.PaymentType, menuPaymentType);

        }

        private void SetMenuAttiributes(HtmlGenericControl panel, bool isActiveMenu, HtmlGenericControl subMenuId)
        {
            if (isActiveMenu)
            {
                ui.Attributes.Remove("class");
                ui.Attributes.Add("class", "sidebar-dropdown list-unstyled collapse show");
            }
            else
            {
                ui.Attributes.Remove("class");
                ui.Attributes.Add("class", "sidebar-dropdown list-unstyled collapse");
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
            else
            {
                panel.Attributes.Remove("class");
                panel.Attributes.Add("class", "sidebar-item");
            }
        }
    }
}