using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace EDC
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string user1 = Core.Roles.Administrator.ToString();
            if (Membership.GetUser(user1) == null)
            {
                Membership.CreateUser(user1, @"!23QWeASd");    //создание учетой записи администратор
                Roles.AddUserToRole(user1, Core.Roles.Administrator.ToString());  //назначение ей прав администратора
            }

            string user2 = Core.Roles.Investigator.ToString(); //исследователь
            if (Membership.GetUser(user2) == null)
            {
                Membership.CreateUser(user2, @"!23QWeASd");
                Roles.AddUserToRole(user2, Core.Roles.Investigator.ToString());
            }

            string user3 = Core.Roles.Monitor.ToString(); //монитор
            if (Membership.GetUser(user3) == null)
            {
                Membership.CreateUser(user3, @"!23QWeASd");
                Roles.AddUserToRole(user3, Core.Roles.Monitor.ToString());
            }

            string user4 = Core.Roles.Data_Manager.ToString(); //дата менеджер
            if (Membership.GetUser(user4) == null)
            {
                Membership.CreateUser(user4, @"!23QWeASd");
                Roles.AddUserToRole(user4, Core.Roles.Data_Manager.ToString());
            }

            string user5 = Core.Roles.Principal_Investigator.ToString(); //главный исследователь
            if (Membership.GetUser(user5) == null)
            {
                Membership.CreateUser(user5, @"!23QWeASd");
                Roles.AddUserToRole(user5, Core.Roles.Principal_Investigator.ToString());
            }

            string user6 = Core.Roles.Auditor.ToString(); //Аудитор
            if (Membership.GetUser(user6) == null)
            {
                Membership.CreateUser(user6, @"!23QWeASd");
                Roles.AddUserToRole(user6, Core.Roles.Auditor.ToString());
            }
        }

    }
}