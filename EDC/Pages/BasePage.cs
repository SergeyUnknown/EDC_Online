using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Threading;
using System.Globalization;

namespace EDC
{
    public class BasePage : Page
    {
        protected override void InitializeCulture()
        {
            HttpCookie cookieLang = Request.Cookies["lang"];
            string culture = "ru-RU";
            
            if(cookieLang!= null)
            {
                switch(cookieLang.Value)
                {
                    case "en":
                    case "en-US":
                        {
                            culture = "en-US";
                            break;
                        }
                    default:
                        {
                            culture = "ru-RU";
                            break;
                        }
                }
            }
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(culture);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);

            base.InitializeCulture();
        }
    }
}