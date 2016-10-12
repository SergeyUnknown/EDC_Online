using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Net.Mail;

namespace EDC.Pages.Administration
{
    public partial class ViewUser : System.Web.UI.Page
    {
        static bool Editing = false;
        Models.Repository.UserProfileRepository pr = new Models.Repository.UserProfileRepository();
        Models.Repository.MedicalCenterRepository MCR = new Models.Repository.MedicalCenterRepository();
        Models.Repository.AccessToCenterRepository ATCR = new Models.Repository.AccessToCenterRepository();
        static Models.UserProfile userProfile;
        static MembershipUser user;
        static List<Models.MedicalCenter> MedicalCenters;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                user = null;
                cblUserRole.Items.Clear();
                cblCenters.Items.Clear();
                liCenters.Visible = true;
                MedicalCenters = MCR.SelectAll().OrderBy(x=>x.MedialCenterID).ToList();
                foreach (string role in Enum.GetNames(typeof(Core.Roles)))
                {
                    Core.GetRoleRusName(role);
                    ListItem item = new ListItem(Core.GetRoleRusName(role), role);
                    cblUserRole.Items.Add(item);
                }
                MedicalCenters.ForEach(x=> cblCenters.Items.Add(x.Name));

                if (Request.Url.ToString().IndexOf("Edit") > 0)
                {
                    Editing = true;
                    btnOk.Text = "Изменить аккаунт";
                    Title = "Редактирование аккаунта";

                    user = Membership.GetUser(GetUserGuidFromRequest());
                    userProfile = pr.SelectByID((Guid)user.ProviderUserKey);

                    tbEmail.Text = user.Email;
                    tbUserName.Text = user.UserName;
                    tbUserName.ReadOnly = true;

                    string[] rolesUser = Roles.GetRolesForUser(user.UserName);

                    rolesUser.ToList().ForEach(x => cblUserRole.Items.FindByValue(x).Selected = true);
                    if (userProfile != null)
                    {
                        tbName.Text = userProfile.Name;
                        tbLastname.Text = userProfile.LastName;
                        tbPhone.Text = userProfile.Phone;
                        if(userProfile.MedicalCenters!=null)
                        {
                            userProfile.MedicalCenters.ForEach(x=> cblCenters.Items.FindByText(x.MedicalCenter.Name).Selected = true);
                        }
                    }

                }
                else
                {
                    btnOk.Text = "Создать аккаунт";
                    Title = "Создание аккаунта";
                    Editing = false;

                }

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
            if(Editing)
            {
                user.Email = tbEmail.Text;
                Membership.UpdateUser(user);
                if(userProfile == null)
                {
                    Models.UserProfile up = new Models.UserProfile();
                    up.Name = tbName.Text;
                    up.Phone = tbPhone.Text;
                    up.LastName = tbLastname.Text;
                    up.UserProfileID = (Guid)user.ProviderUserKey;
                    pr.Create(up);
                    pr.Save();
                    pr = new Models.Repository.UserProfileRepository();
                }

                foreach(ListItem item in cblUserRole.Items)
                {
                    if(item.Selected && !Roles.IsUserInRole(user.UserName,item.Value))
                    {
                        Roles.AddUserToRole(user.UserName, item.Value);
                        continue;
                    }
                    else
                        if (!item.Selected && Roles.IsUserInRole(user.UserName, item.Value))
                        {
                            Roles.RemoveUserFromRole(user.UserName, item.Value);
                            continue;
                        }
                }
                userProfile = pr.SelectByID((Guid)user.ProviderUserKey);

                if(userProfile.MedicalCenters.Count == 0)
                {
                    foreach(ListItem item in cblCenters.Items)
                    {
                        if(item.Selected)
                        {
                            Models.AccessToCenter access = new Models.AccessToCenter();
                            access.MedicalCenterID = MedicalCenters.First(x => x.Name == item.Text).MedialCenterID;
                            access.UserID = (Guid)user.ProviderUserKey;
                            access.CreatedBy = User.Identity.Name;
                            ATCR.Create(access);
                        }
                    }
                    ATCR.Save();
                }
                else
                {
                    foreach (ListItem item in cblCenters.Items)
                    {
                        Models.MedicalCenter mc = MedicalCenters.First(x => x.Name == item.Text);
                        if (item.Selected && userProfile.MedicalCenters.FirstOrDefault(x=>x.MedicalCenterID == mc.MedialCenterID) == null)
                        {
                            Models.AccessToCenter access = new Models.AccessToCenter();
                            access.MedicalCenterID = mc.MedialCenterID;
                            access.UserID = (Guid)user.ProviderUserKey;
                            access.CreatedBy = User.Identity.Name;
                            ATCR.Create(access);
                        }
                        else if(!item.Selected && userProfile.MedicalCenters.FirstOrDefault(x=>x.MedicalCenterID == mc.MedialCenterID) != null)
                        {
                            if (userProfile.CurrentCenterID == mc.MedialCenterID)
                            {
                                userProfile.CurrentCenterID = null;
                                pr.Update(userProfile);
                                pr.Save();
                            }
                            ATCR.Delete(userProfile.UserProfileID, mc.MedialCenterID);
                        }
                    }
                    ATCR.Save();

                }
                labelStatus.Text = "Пользователь успешно изменён!";
                labelStatus.ForeColor = System.Drawing.Color.Green;
                labelStatus.Visible = true;
            }
            else
            {
                string password = Membership.GeneratePassword(8, 2);
                MembershipUser newUser = Membership.CreateUser(tbUserName.Text, password, tbEmail.Text);
                Models.UserProfile up = new Models.UserProfile();
                up.Name = tbName.Text;
                up.Phone = tbPhone.Text;
                up.LastName = tbLastname.Text;
                up.UserProfileID = (Guid)newUser.ProviderUserKey;
                pr.Create(up);
                pr.Save();
                ATCR = new Models.Repository.AccessToCenterRepository();
                foreach (ListItem item in cblUserRole.Items)
                {
                    if (item.Selected)
                    {
                        Roles.AddUserToRole(newUser.UserName, item.Value);
                        continue;
                    }
                }

                foreach (ListItem item in cblCenters.Items)
                {
                    if (item.Selected)
                    {
                        Models.AccessToCenter access = new Models.AccessToCenter();
                        access.MedicalCenterID = MedicalCenters.First(x => x.Name == item.Text).MedialCenterID;
                        access.UserID = (Guid)newUser.ProviderUserKey;
                        access.CreatedBy = User.Identity.Name;
                        ATCR.Create(access);
                    }
                }
                ATCR.Save();

                labelStatus.Text = "Аккаунт успешно создан! Пароль: " + password;
                labelStatus.ForeColor = System.Drawing.Color.Green;
                labelStatus.Visible = true;
                MailMessage msg = new MailMessage("noreply.edc.mdp@gmail.com", newUser.Email);
                msg.Subject = "Аккаунт успешно создан";
                msg.Body = "Ваш логин: " + newUser.UserName + "<br/>Ваш пароль: " + password;
                msg.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;

                smtp.Credentials = new System.Net.NetworkCredential("noreply.edc.mdp@gmail.com", "12332145");

                smtp.Send(msg);
            }
        }
    }
}