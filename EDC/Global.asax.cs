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

            bool drop = false;

            if(drop)
                Database.SetInitializer(new DropCreateDatabaseAlways<Models.EDCContext>()); //дроп БД при запуске
            else
                Database.SetInitializer<Models.EDCContext>(new Models.EDCInitializer()); //всё норм

            

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
                Membership.CreateUser(user1, @"!23QWeASd");    //создание учетой записи администратор
                Roles.AddUserToRole(user1, Core.Roles.Administrator.ToString());  //назначение ей прав администратора
            }

            Models.Repository.AppSettingRepository ASR = new Models.Repository.AppSettingRepository();
            ASR.SelectByID(Core.Core.STUDY_NAME);
            ASR.SelectByID(Core.Core.STUDY_PROTOCOL);
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
