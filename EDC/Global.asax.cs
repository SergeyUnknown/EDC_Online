using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using EDC;
using System.Data.Entity;

namespace EDC
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Код, выполняемый при запуске приложения
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Database.SetInitializer<Models.EDCContext>(new Models.EDCInitializer());

            //Database.SetInitializer(new DropCreateDatabaseAlways<Models.EDCContext>());
            

            // Код, выполняемый при запуске приложения
            foreach(string item in Enum.GetNames(typeof(Core.Roles)))
            {
                if (!Roles.RoleExists(item))
                {
                    Roles.CreateRole(item);
                }
            }


            if (Membership.GetAllUsers().Count == 0)
            {
                string adminName = "Administrator";
                Membership.CreateUser(adminName, "!23qweAsd");    //создание учетой записи администратор
                Roles.AddUserToRole(adminName, Core.Roles.Administrator.ToString());  //назначение ей прав администратора
            }
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Код, выполняемый при завершении работы приложения

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Код, выполняемый при появлении необработанной ошибки

        }
    }
}
