using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EDC.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Unnamed_LoggingIn(object sender, LoginCancelEventArgs e)
        {
            LoginControl.UserName = LoginControl.UserName.Trim(' ','.',',',';','$','@','#','&','%','?','/',')','(');
        }
    }
}