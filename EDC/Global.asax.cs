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

            Database.SetInitializer<Models.EDCContext>(new Models.EDCInitializer()); //всё норм

            //Database.SetInitializer(new DropCreateDatabaseAlways<Models.EDCContext>()); //дроп БД при запуске
            

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
                string user1 = Core.Roles.Administrator.ToString();
                Membership.CreateUser(user1, "12345");    //создание учетой записи администратор
                Roles.AddUserToRole(user1, Core.Roles.Administrator.ToString());  //назначение ей прав администратора

                string user2 = Core.Roles.Investigator.ToString(); //исследователь
                Membership.CreateUser(user2, "12345");
                Roles.AddUserToRole(user2, Core.Roles.Investigator.ToString());

                string user3 = Core.Roles.Monitor.ToString(); //монитор
                Membership.CreateUser(user3, "12345");
                Roles.AddUserToRole(user3, Core.Roles.Monitor.ToString());

                string user4 = Core.Roles.Data_Manager.ToString(); //дата манагер
                Membership.CreateUser(user4, "12345");
                Roles.AddUserToRole(user4, Core.Roles.Data_Manager.ToString());

                string user5 = Core.Roles.Principal_Investigator.ToString(); //главный исследователь
                Membership.CreateUser(user5, "12345");
                Roles.AddUserToRole(user5, Core.Roles.Principal_Investigator.ToString());

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
