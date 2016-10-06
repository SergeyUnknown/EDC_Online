using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EDC
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        Models.Repository.AppSettingRepository ASR = new Models.Repository.AppSettingRepository();

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(HttpContext.Current.User.IsInRole(Core.Roles.Auditor.ToString()) || !HttpContext.Current.User.Identity.IsAuthenticated))
            {
                liQueryAndNotes.Visible = true;
            }
            if (HttpContext.Current.User.IsInRole(Core.Roles.Administrator.ToString()) || HttpContext.Current.User.IsInRole(Core.Roles.Data_Manager.ToString()))
            {
                liAdmin.Visible = true;
                liData.Visible = true;
                liMonitor.Visible = true;
                liQueryAndNotes.Visible = true;
            }

            if (HttpContext.Current.User.IsInRole(Core.Roles.Monitor.ToString()))
            {
                liMonitor.Visible = true;
            }

            HttpCookie cookie = Request.Cookies["LeftMenuVisible"];
            if(cookie!=null)
            {
                if(cookie.Value == "false")
                {
                    sectionLeftMenu.Style.Add("display", "none");
                    sectionMainMenu.Style.Add("max-width","100%");
                    sectionMainMenu.Style.Add("margin-left", "0px");
                    navigationMenu.Style.Add("padding-left", "14px");
                    navigationMenu.Style.Add("width", "calc(100% - 14px)");
                }
            }
            ddlCurrentCenter.Items.Add("...");
        }

        protected string StudyName
        {
            get { return ASR.SelectByID(Core.STUDY_NAME).Value; }
            set { }
        }
    }
}