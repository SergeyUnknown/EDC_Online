using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Net.Mail;
using System.Web.Configuration;
using System.Net;

namespace EDC.Pages.Account
{
    public partial class PasswordRecovery : BasePage
    {
        protected string SiteURL
        {
            get
            {
                return siteURL;
            }
        }
        private string siteURL;
        protected void Page_Load(object sender, EventArgs e)
        {
            siteURL = "http://" + Request.Url.Authority + ResolveUrl("~/");
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(tbAnswer.Text) || string.IsNullOrWhiteSpace(tbQuestion.Text))
            {
                var user = Membership.GetUser(tbUserName.Text);
                if (user != null)
                {
                    tbQuestion.Text = user.PasswordQuestion;
                    FailureText.Text = "";
                }
                else
                    FailureText.Text = Resources.LocalizedText.TheUsernameCouldNotBeFound;
            }
            else
            {
                var user = Membership.GetUser(tbUserName.Text);
                if(user != null)
                {
                    string newPassword = "";
                    try
                    {
                        newPassword = user.ResetPassword(tbAnswer.Text);
                    }
                    catch(System.Web.Security.MembershipPasswordException error)
                    {
                        FailureText.Text = error.Message;
                        return;
                    }
                    

                    MailMessage mail = new MailMessage();
                    mail.To.Add(user.Email);
                    mail.Subject = Resources.LocalizedText.PasswordRecovery;
                    mail.Body = Resources.LocalizedText.NewPassword+": "+newPassword;
                    mail.IsBodyHtml = true;

                    try
                    {
                        SmtpClient smtp = new SmtpClient();
                        smtp.Send(mail);
                    }
                    catch(Exception error)
                    {
                        FailureText.Text = Resources.LocalizedText.ErrorPleaseTryAgain;
                        return;
                    }
                    pLOK.Style.Add("display", "block");
                }
                else
                    FailureText.Text = Resources.LocalizedText.TheUsernameCouldNotBeFound;
            }
        }
    }
}