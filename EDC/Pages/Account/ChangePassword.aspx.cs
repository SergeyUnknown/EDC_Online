using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Text.RegularExpressions;

namespace EDC.Pages.Account
{
    public partial class ChangePassword : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            FailureText.Text = "";
            char[] incorrectChar = {'"','%',' '};
            string strUserID = Request.Params["i"];
            if (strUserID == null)
                throw new ArgumentNullException();
            Guid userID = Guid.Parse(strUserID);
            var mu = Membership.GetUser(userID);
            if (mu == null)
                throw new ArgumentException();
            if(Membership.ValidateUser(mu.UserName,tbOldPassword.Text.Replace("%","").Replace("\"","").Replace("'","")))
            {
                string newPass = tbNewPassword.Text;
                if (newPass != tbOldPassword.Text)
                {
                    if (newPass == tbConfirmPassword.Text)
                    {
                        if (Regex.IsMatch(newPass, Membership.PasswordStrengthRegularExpression))
                        {
                            if(mu.ChangePassword(tbOldPassword.Text, newPass))
                            {
                                //lOk.Text = "Пароль успешно изменён! Через несколько секунд Вы будете автоматически перенаправлены на главную страницу";
                                pLOK.Style.Add("display","block");
                                fsLoginForm.Style.Add("display", "none");
                                timerRedirect.Enabled = true;
                            }
                            else
                                FailureText.Text = "Ошибка при смене пароля. Попробуйте ещё раз.";
                        }
                        else
                        {
                            FailureText.Text = "Указал недостаточно сложный пароль";
                        }
                    }
                    else
                    {
                        FailureText.Text = "Пароль и подтверждение пароля не совпадают";
                    }
                }
                else
                {
                    FailureText.Text = "Старый и новый пароли не должны совпадать";
                }
            }
            else
                FailureText.Text = "Неверно указан текущий пароль";

        }

        protected void timerRedirect_Tick(object sender, EventArgs e)
        {
            Response.Redirect("~/");
        }

    }
}