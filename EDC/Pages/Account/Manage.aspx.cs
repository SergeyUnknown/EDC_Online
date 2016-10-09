using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using EDC.Models;
using EDC.Models.Repository;

namespace EDC.Account
{
    public partial class Manage : System.Web.UI.Page
    {
        protected string SuccessMessage
        {
            get;
            private set;
        }

        protected string ErrorMessage
        {
            get;
            private set;
        }

        protected bool CanRemoveExternalLogins
        {
            get;
            private set;
        }

        protected void Page_Load()
        {
            if (!IsPostBack)
            {
                var user = System.Web.Security.Membership.GetUser();
                tbUserName.Text = User.Identity.Name;
                tbEmail.Text = user.Email;
                tbQuestion.Text = user.PasswordQuestion;
                

                UserProfileRepository pr = new UserProfileRepository();
                List<UserProfile> profiles = pr.SelectAll().ToList();
                UserProfile userProfile = profiles.FirstOrDefault(x => x.UserProfileID == (Guid)user.ProviderUserKey);
                if (userProfile != null)
                {
                    tbName.Text = userProfile.Name;
                    tbLastname.Text = userProfile.LastName;
                    tbPhone.Text = userProfile.Phone;
                }

                if (!(user.PasswordQuestion == null || user.PasswordQuestion == ""))
                    tbAnswer.Text = "********";
                // Отобразить сообщение об успехе
                var message = Request.QueryString["m"];
                if (message != null)
                {
                    SuccessMessage =
                        message == "ChangePwdSuccess" ? "Пароль изменен."
                        : message == "ChangeQASuccess" ? "Контрольный вопрос и ответ изменены."
                        : message == "ChangeInfo" ? "Данные успешно сохранены"
                        : String.Empty;
                    successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
                }

                var error = Request.QueryString["e"];
                if (error != null)
                {
                    ErrorMessage =
                        error == "power" ? "Недостаточная сложность пароля."
                        : error == "current" ? "Не верно указан текущий пароль."
                        : error == "qa" ? "Ошибка при установке контрольного вопроса и ответа"
                        : String.Empty;
                    errorMessage.Visible = !String.IsNullOrEmpty(ErrorMessage);
                }
            }

        }

        protected static string ConvertToDisplayDateTime(DateTime? utcDateTime)
        {
            // Измените этот метод, чтобы преобразовать дату и время в формате UTC в нужную форму
            // отображения со смещением. Здесь они преобразуются в часовой пояс сервера и форматируются
            // как короткая дата и длинное время с использованием языка и региональных параметров текущего потока.
            return utcDateTime.HasValue ? utcDateTime.Value.ToLocalTime().ToString("G") : "[никогда]";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/");
        }

        protected void btnSaveChange_Click(object sender, EventArgs e)
        {
            var user = Membership.GetUser(User.Identity.Name, false);
            bool changeInfo = false;
            
            if (!string.IsNullOrWhiteSpace(tbEmail.Text)) //сохраняем Email
            {
                user.Email = tbEmail.Text;
                changeInfo = true;
            }
            Membership.UpdateUser(user);

            if (!string.IsNullOrWhiteSpace(tbName.Text) && !string.IsNullOrWhiteSpace(tbLastname.Text) && !string.IsNullOrWhiteSpace(tbPhone.Text))
            {
                UserProfileRepository pr = new UserProfileRepository();

                UserProfile userProfile = pr.GetManyByFilter(x => x.UserProfileID == (Guid)user.ProviderUserKey).FirstOrDefault();
                if(userProfile == null)
                {
                    userProfile = new UserProfile();
                    userProfile.Name = tbName.Text;
                    userProfile.LastName = tbLastname.Text;
                    userProfile.Phone = tbPhone.Text;
                    userProfile.UserProfileID = (Guid)user.ProviderUserKey;
                    pr.Create(userProfile);
                }
                else
                {
                    userProfile.Name = tbName.Text;
                    userProfile.LastName = tbLastname.Text;
                    userProfile.Phone = tbPhone.Text;
                    pr.Update(userProfile);
                }
                
                pr.Save();

            }

            #region changeQA
            bool changedQA = false; //изменён вопрос/ответ
            if (!string.IsNullOrWhiteSpace(tbAnswer.Text) && !string.IsNullOrWhiteSpace(tbQuestion.Text))
            {
                if (!(user.PasswordQuestion == tbQuestion.Text && tbAnswer.Text == "********"))
                {
                    if (user.ChangePasswordQuestionAndAnswer(tbCurrentPassword.Text, tbQuestion.Text, tbAnswer.Text))
                    {
                        changedQA = true;
                    }
                    else
                    {
                        Response.Redirect(ResolveUrl("~/Account/Manage?e=qa"));
                    }
                }
            }
            #endregion

            ///Смена пароля
            string newPassword = tbNewPassword.Text;
            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                if ((newPassword.Length >= Membership.MinRequiredPasswordLength) &&
                    (newPassword.ToCharArray().Count(c => !Char.IsLetterOrDigit(c)) >=
                         Membership.MinRequiredNonAlphanumericCharacters) &&
                    ((Membership.PasswordStrengthRegularExpression.Length == 0) ||
                         System.Text.RegularExpressions.Regex.IsMatch(newPassword, Membership.PasswordStrengthRegularExpression)))
                {
                    if (user.ChangePassword(tbCurrentPassword.Text, newPassword))
                        Response.Redirect(ResolveUrl("~/Account/Manage?m=ChangePwdSuccess"));
                    else
                        Response.Redirect(ResolveUrl("~/Account/Manage?e=current"));
                }
                else
                {
                    Response.Redirect(ResolveUrl("~/Account/Manage?e=power"));
                }
            }
            else
            {
                if(changedQA)
                {
                    Response.Redirect(ResolveUrl("~/Account/Manage?m=ChangeQASuccess"));
                }
                if(changeInfo)
                {
                    Response.Redirect(ResolveUrl("~/Account/Manage?m=ChangeInfo"));
                }
            }
            ///

        }

    }
}