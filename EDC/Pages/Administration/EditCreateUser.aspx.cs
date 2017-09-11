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
    public partial class ViewUser : BasePage
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
                MedicalCenters = MCR.SelectAll().OrderBy(x=>x.MedicalCenterID).ToList();
                foreach (string role in Enum.GetNames(typeof(Core.Roles)))
                {
                    Core.Core.GetRoleRusName(role);
                    ListItem item = new ListItem(Core.Core.GetRoleRusName(role), role);
                    cblUserRole.Items.Add(item);
                }
                MedicalCenters.ForEach(x=> cblCenters.Items.Add(x.Name));

                if (Request.Url.ToString().IndexOf("Edit") > 0)
                {
                    Editing = true;
                    btnOk.Text = Resources.LocalizedText.EditAccount;
                    Title = Resources.LocalizedText.EditAccount;

                    user = Membership.GetUser(GetUserGuidFromRequest());
                    userProfile = pr.SelectByID((Guid)user.ProviderUserKey);
                    cbLock.Checked = !user.IsApproved || user.IsLockedOut;
                    if (!user.IsApproved || user.IsLockedOut)
                        btnReCreatePass.Visible = false;

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
                    btnReCreatePass.Visible = false;
                    btnOk.Text = Resources.LocalizedText.CreateAccount;
                    Title = Resources.LocalizedText.CreateAccount;
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
                if ((!user.IsApproved || user.IsLockedOut) && !cbLock.Checked)
                {
                    user.UnlockUser();
                    user.IsApproved = true;
                }
                if ((user.IsApproved || !user.IsLockedOut) && cbLock.Checked)
                    user.IsApproved = false;
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
                            access.MedicalCenterID = MedicalCenters.First(x => x.Name == item.Text).MedicalCenterID;
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
                        if (item.Selected && userProfile.MedicalCenters.FirstOrDefault(x=>x.MedicalCenterID == mc.MedicalCenterID) == null)
                        {
                            Models.AccessToCenter access = new Models.AccessToCenter();
                            access.MedicalCenterID = mc.MedicalCenterID;
                            access.UserID = (Guid)user.ProviderUserKey;
                            access.CreatedBy = User.Identity.Name;
                            ATCR.Create(access);
                        }
                        else if(!item.Selected && userProfile.MedicalCenters.FirstOrDefault(x=>x.MedicalCenterID == mc.MedicalCenterID) != null)
                        {
                            if (userProfile.CurrentCenterID == mc.MedicalCenterID)
                            {
                                userProfile.CurrentCenterID = null;
                                pr.Update(userProfile);
                                pr.Save();
                            }
                            ATCR.Delete(userProfile.UserProfileID, mc.MedicalCenterID);
                        }
                    }
                    ATCR.Save();

                }
                labelStatus.Text = Resources.LocalizedText.PasswordWasChangedSuccessfully;
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
                        access.MedicalCenterID = MedicalCenters.First(x => x.Name == item.Text).MedicalCenterID;
                        access.UserID = (Guid)newUser.ProviderUserKey;
                        access.CreatedBy = User.Identity.Name;
                        ATCR.Create(access);
                    }
                }
                ATCR.Save();

                labelStatus.Text = Resources.LocalizedText.AccountWasCreatedSuccessfully + "! " + Resources.LocalizedText.Password + ": " + password;
                labelStatus.ForeColor = System.Drawing.Color.Green;
                labelStatus.Visible = true;

                MailMessage msg = new MailMessage("noreply.edc.mdp@gmail.com", newUser.Email);
                msg.Subject = Resources.LocalizedText.AccountWasCreatedSuccessfully;
                msg.Body = Resources.LocalizedText.UserName+": " + newUser.UserName + 
                    "<br/>"+ Resources.LocalizedText.Password +": " + password + "<br/>"
                    + "Ссылка для входа: http://edc.mdp.group/" + ResolveUrl("~/") + "<br/><br/>";
                msg.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;

                smtp.Credentials = new System.Net.NetworkCredential("noreply.edc.mdp@gmail.com", "12332145");

                smtp.Send(msg);
            }
        }

        protected void btnReCreatePass_Click(object sender, EventArgs e)
        {
            string newPass = user.ResetPassword();
            labelStatus.ForeColor = System.Drawing.Color.Green;
            labelStatus.Visible = true;
            labelError.ForeColor = System.Drawing.Color.Red;
            try
            {
                MailMessage msg = new MailMessage("noreply.edc.mdp@gmail.com", user.Email);
                msg.Subject = Resources.LocalizedText.PasswordWasChangedSuccessfully;
                msg.Body = string.Format(Resources.LocalizedText.HelloUserName, user.UserName) +"<br/>"
                    + string.Format("{0} {1}: {2}", Resources.LocalizedText.PasswordWasChangedSuccessfully, Resources.LocalizedText.NewPassword, newPass) +"<br/>"
                    + "Ссылка для входа: http://edc.mdp.group/" + ResolveUrl("~/") + "<br/><br/>";
                msg.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("noreply.edc.mdp@gmail.com", "12332145");

                smtp.Send(msg);
                labelStatus.Text = Resources.LocalizedText.PasswordWasChangedSuccessfully+ "<br/>"+ string.Format(Resources.LocalizedText.MessageNewPassword,newPass) + "<br/><br/>";
            }
            catch(Exception error)
            {
                labelStatus.Text = string.Format("{0} {1}: {2}",Resources.LocalizedText.PasswordWasChangedSuccessfully, Resources.LocalizedText.NewPassword ,newPass);
                labelError.Visible = true;
                labelError.Text = "Ошибка при отправке пароля на почту: "+ error.Message;
            }
        }
    }
}