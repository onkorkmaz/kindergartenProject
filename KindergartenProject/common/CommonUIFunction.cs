﻿using Business;
using Common;
using Entity;
using KindergartenProject.userControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace KindergartenProject
{
    public class CommonUIFunction : System.Web.UI.MasterPage
    {
        public static List<SeasonEntity> GetSeasonList(int year)
        {
            List<SeasonEntity> monthList = new List<SeasonEntity>();
            monthList.Add(new SeasonEntity(0, year, 9, "Eylül"));
            monthList.Add(new SeasonEntity(1, year, 10, "Ekim"));
            monthList.Add(new SeasonEntity(2, year, 11, "Kasım"));
            monthList.Add(new SeasonEntity(3, year, 12, "Aralık"));
            monthList.Add(new SeasonEntity(4, year + 1, 1, "Ocak"));
            monthList.Add(new SeasonEntity(5, year + 1, 2, "Şubat"));
            monthList.Add(new SeasonEntity(6, year + 1, 3, "Mart"));
            monthList.Add(new SeasonEntity(7, year + 1, 4, "Nisan"));
            monthList.Add(new SeasonEntity(8, year + 1, 5, "Mayıs"));
            monthList.Add(new SeasonEntity(9, year + 1, 6, "Haziran"));
            monthList.Add(new SeasonEntity(10, year + 1, 7, "Temmuz"));
            monthList.Add(new SeasonEntity(11, year + 1, 8, "Ağustus"));

            return monthList;
        }

        private void controlMenuVisibleForAuthority(HtmlGenericControl genericControl, List<AuthorityScreenEnum> authorityList)
        {
            bool authorityExists = false;

            foreach (AuthorityScreenEnum enm in authorityList)
            {
                AuthorityEntity entity = new AuthorityBusiness(new BasePage()._ProjectType, new BasePage()._AdminEntity.Id).GetAuthorityWithScreenAndTypeId(enm, new BasePage()._AdminEntity.AuthorityTypeId);
                if (entity != null)
                {
                    authorityExists = entity.HasAuthority;
                    break;
                }
            }
            genericControl.Visible = authorityExists;
        }

        internal void SetVisibility(AuthorityScreenEnum authorityScreen, HtmlGenericControl genericControl)
        {
            if (new BasePage()._AdminEntity.IsDeveleporOrSuperAdmin)
            {
                genericControl.Visible = true;
                return;
            }

            List<AuthorityScreenEnum> authortityList = new List<AuthorityScreenEnum>();
            authortityList = new List<AuthorityScreenEnum>();
            authortityList.Add(authorityScreen);
            controlMenuVisibleForAuthority(genericControl, authortityList);
        }

        public static void loadClass(ProjectType projectType , ref DropDownList drpClassList, ref divInformation _divInformation,bool isAddUnSignedItem)
        {
            DataResultArgs<List<ClassEntity>> resultSet = new ClassBusiness(projectType).Get_ClassForStudent();

            if (resultSet.HasError)
            {
                _divInformation.ErrorText = resultSet.ErrorDescription;
                return;
            }
            else
            {
                List<ClassEntity> classList = resultSet.Result;
                List<ClassEntity> list = new List<ClassEntity>();

                list.Add(new ClassEntity() { Id = -1, Name = "-" });
                if (resultSet.Result != null)
                {

                    foreach (ClassEntity entity in classList)
                    {
                        list.Add(entity);
                    }
                }

                if (isAddUnSignedItem)
                {
                    list.Add(new ClassEntity() { Id = -2, Name = "Atanmayanlar" });
                }

                drpClassList.DataSource = list;
                drpClassList.DataValueField = "Id";
                drpClassList.DataTextField = "ClassAndMainTeacherName";
                drpClassList.DataBind();
            }
        }

    }
}