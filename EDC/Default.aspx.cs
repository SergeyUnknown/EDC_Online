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
    public partial class _Default : BasePage
    {

        Models.Repository.AppSettingRepository ASR;
        protected void Page_Load(object sender, EventArgs e)
        {
            ASR = new Models.Repository.AppSettingRepository();

            string user1 = Core.Roles.Administrator.ToString();
            if (Membership.GetUser(user1) == null)
            {
                Membership.CreateUser(user1, @"!23QWeASd");    //создание учетой записи администратор
                Roles.AddUserToRole(user1, Core.Roles.Administrator.ToString());  //назначение ей прав администратора
            }
        }

        protected string Protocol
        {
            get
            {
                return ASR.SelectByID(Core.Core.STUDY_PROTOCOL).Value;
            }
        }

        protected string StudyName
        {
            get
            {
                return ASR.SelectByID(Core.Core.STUDY_NAME).Value;
            }
        }
    }
}