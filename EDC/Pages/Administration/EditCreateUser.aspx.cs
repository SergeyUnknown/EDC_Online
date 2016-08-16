using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace EDC.Pages.Administration
{
    public partial class ViewUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                cblUserRole.Items.Clear();
                foreach (string role in Enum.GetNames(typeof(Core.Roles)))
                {
                    Core.GetRoleRusName(role);
                    ListItem item = new ListItem(Core.GetRoleRusName(role), role);
                    cblUserRole.Items.Add(item);
                }

            }

            if(Request.Url.ToString().IndexOf("Edit")>0)
            {
                btnOk.Text = "Изменить аккаунт";

                MembershipUser user = Membership.GetUser(GetUserGuidFromRequest());
                Models.Repository.ProfileRepository pr = new Models.Repository.ProfileRepository();
                var userProfile = pr.SelectByUserID((Guid)user.ProviderUserKey);

                tbEmail.Text = user.Email;
                tbUserName.Text = user.UserName;

                string[] rolesUser = Roles.GetRolesForUser(user.UserName);

                rolesUser.ToList().ForEach(x => cblUserRole.Items.FindByValue(x).Selected = true);

                if(userProfile != null)
                {
                    tbName.Text = userProfile.Name;
                    tbLastname.Text = userProfile.LastName;
                    tbPhone.Text = userProfile.Phone;
                }

            }
            else
            {
                btnOk.Text = "Создать аккаунт";

            }


        }

        Guid GetUserGuidFromRequest()
        {
            string userIDstr = (string)RouteData.Values["id"] ?? Request.QueryString["id"];
            if (string.IsNullOrWhiteSpace(userIDstr))
                throw new ArgumentException("Необходимо указать ID редактируемого пользователя");
            return Guid.Parse(userIDstr);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Administration/Users");
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {

        }
    }
}