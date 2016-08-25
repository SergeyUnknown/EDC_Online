using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;

namespace EDC.Pages.Administration
{
    public partial class Users : System.Web.UI.Page
    {
        static MembershipUserCollection users;
        static int pageSize = 50;
        static int recordCount = 0;

        Models.Repository.UserProfileRepository PR = new Models.Repository.UserProfileRepository();

        protected int CountUserOnline
        {
            get;
            private set;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                CountUserOnline = Membership.GetNumberOfUsersOnline();
                gvUsers.DataSource = GetDataTableUsers();
                gvUsers.DataBind();
            }
            LoadDTDataItem();
        }

        private int GetPageFromRequest()
        {
            int page;
            string reqValue = (string)RouteData.Values["page"] ?? Request.QueryString["page"];
            return reqValue != null && int.TryParse(reqValue, out page) && page > 0 ? page : 1;
        }

        //номер текущей страницы
        protected int CurrentPage
        {
            get
            {
                int page;
                page = GetPageFromRequest();
                return page > MaxPageCount ? MaxPageCount : page;
            }
        }

        //максимальный номер страницы
        protected int MaxPageCount
        {
            get
            {
                return (int)Math.Ceiling((decimal)Membership.GetAllUsers().Count / pageSize);
            }
        } 

        //получаем список пользователей на страницу
        protected MembershipUserCollection GetUsers()
        {
            int totalUsers;
            users = Membership.GetAllUsers(CurrentPage - 1, pageSize, out totalUsers);
            return users;
        }

        protected DataTable GetDataTableUsers()
        {
            DataTable dt = new DataTable("Users");

            DataColumn userNameColumn = new DataColumn("UserName",typeof(string));
            dt.Columns.Add(userNameColumn);

            DataColumn emailColumn = new DataColumn("Email", typeof(string));
            dt.Columns.Add(emailColumn);

            DataColumn nameColumn = new DataColumn("Name", typeof(string));
            dt.Columns.Add(nameColumn);

            DataColumn surnameColumn = new DataColumn("Lastname", typeof(string));
            dt.Columns.Add(surnameColumn);

            DataColumn phoneColumn = new DataColumn("Phone", typeof(string));
            dt.Columns.Add(phoneColumn);

            DataColumn roleColumn = new DataColumn("Role", typeof(string));
            dt.Columns.Add(roleColumn);

            DataColumn lockedColumn = new DataColumn("Locked", typeof(string));
            dt.Columns.Add(lockedColumn);


            MembershipUserCollection muc = GetUsers();
            foreach (MembershipUser user in muc)
            {
                Models.Repository.UserProfileRepository pr = new Models.Repository.UserProfileRepository();
                var u = pr.SelectByUserID((Guid)user.ProviderUserKey);
                DataRow dr = dt.NewRow();

                    dr["UserName"] = user.UserName;
                    dr["Email"] = user.Email;
                    if (u != null)
                {
                    dr["Name"] = u.Name;
                    dr["Lastname"] = u.LastName;
                    dr["Phone"] = u.Phone;
                }
                string roles = "";
                string[] arrRolesForUser = Roles.GetRolesForUser(user.UserName);
                int rolesCountForUser = arrRolesForUser.Count();

                for (int i = 0; i < rolesCountForUser; i++)
                {
                    if (i == rolesCountForUser - 1)
                    {
                        roles += Core.GetRoleRusName(arrRolesForUser[i]);
                    }
                    else
                    {
                        roles += Core.GetRoleRusName(arrRolesForUser[i]) + ", ";
                    }
                }
                dr["Role"] = roles;

                if (user.IsLockedOut || !user.IsApproved)
                    dr["Locked"] = "Да";
                else
                    dr["Locked"] = "Нет";

                dt.Rows.Add(dr);
            }

            return dt;
        }

        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e) //добавить удаление из БД
        {
            try
            {
                string userName = e.Values["UserName"].ToString();
                MembershipUser user = Membership.GetUser(userName);
                if (Membership.DeleteUser(userName))
                {
                    PR.Delete(PR.GetManyByFilter(x => x.UserID == (Guid)user.ProviderUserKey).First().UserProfileID);
                    PR.Save();

                    gvUsers.DataSource = GetDataTableUsers();
                    gvUsers.DataBind();
                    labelStatus.ForeColor = System.Drawing.Color.Green;
                    labelStatus.Text = "Пользователь успешно удалён!";
                }
            }
            catch (Exception error)
            {
                labelStatus.ForeColor = System.Drawing.Color.Red;
                labelStatus.Text = error.Message;
            }
        }

        protected void gvUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            Response.Redirect("~/Administration/EditUser/" + users[gvUsers.Rows[e.NewEditIndex].Cells[0].Text].ProviderUserKey);
        }

        protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ControlCollection cc = e.Row.Cells[7].Controls;
                if (cc.Count > 0)
                {
                    (cc[2] as ImageButton).Attributes.Add("onclick", "return confirm('Вы действительно хотите удалить данного пользователя?')");
                }
            }
        }

        protected void dtInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            pageSize = dtInfo.DropDownSelectedValue;
            int maxRecordsOnPage = (CurrentPage * pageSize) > recordCount ? recordCount : (CurrentPage * pageSize);

            //EntryesInfo.Text = string.Format(Localization.Records, ((CurrentPage - 1) * pageSize + 1), maxRecordsOnPage, recordCount);
            LoadDTDataItem();
        }

        void LoadDTDataItem()
        {
            string pageInfo = String.Format(Localization.Page, CurrentPage, MaxPageCount);
            DownTableDataItem dtDataItem = new DownTableDataItem(
                "Добавить пользователя",
                "~/Administration/CreateUser",
                CurrentPage,
                MaxPageCount,
                "~/Administration/Users/",
                Core.DropDownListItems25,
                pageSize, pageInfo);

            dtInfo.DataItem = dtDataItem;
            dtInfo.DataBind();
        }
    }
}