using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace EDC
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute(null, "NotesAndQuery", "~/Pages/Note/NotesAndQuery.aspx");

            routes.MapPageRoute(null, "ConfigurationTrials", "~/Pages/Administration/ConfigurationTrials.aspx");

            routes.MapPageRoute(null, "SubjectsMatrix/{page}", "~/Pages/Subject/SubjectsMatrix.aspx");
            routes.MapPageRoute(null, "SubjectsMatrix", "~/Pages/Subject/SubjectsMatrix.aspx");

            routes.MapPageRoute(null, "Subjects/{subjectid}/{eventid}/{crfid}", "~/Pages/Subject/SubjectsCRFPage.aspx");
            routes.MapPageRoute(null, "Subjects/Edit/{id}", "~/Pages/Subject/CreateEditSubject.aspx");
            routes.MapPageRoute(null, "Subjects/Add", "~/Pages/Subject/CreateEditSubject.aspx");
            routes.MapPageRoute(null, "Subjects/{page}", "~/Pages/Subject/Subjects.aspx");
            routes.MapPageRoute(null, "Subjects", "~/Pages/Subject/Subjects.aspx");

            routes.MapPageRoute(null, "Events/Configuration/{id}", "~/Pages/Event/ConfigurationEvent.aspx");
            routes.MapPageRoute(null, "Events/Edit/{id}", "~/Pages/Event/CreateEditEvent.aspx");
            routes.MapPageRoute(null, "Events/Add", "~/Pages/Event/CreateEditEvent.aspx");
            routes.MapPageRoute(null, "Events/{page}", "~/Pages/Event/Events.aspx");
            routes.MapPageRoute(null, "Events", "~/Pages/Event/Events.aspx");

            routes.MapPageRoute(null, "CRFs/View/{id}/{table}", "~/Pages/CRF/ViewCRF.aspx");
            routes.MapPageRoute(null, "CRFs/View/{id}", "~/Pages/CRF/ViewCRF.aspx");
            routes.MapPageRoute(null, "CRFs/Add", "~/Pages/CRF/AddCRF.aspx");
            routes.MapPageRoute(null, "CRFs/{page}", "~/Pages/CRF/CRFs.aspx");
            routes.MapPageRoute(null, "CRFs", "~/Pages/CRF/CRFs.aspx");

            routes.MapPageRoute(null, "MedicalCenters/Edit/{id}", "~/Pages/MedicalCenter/CreateEditMC.aspx");
            routes.MapPageRoute(null, "MedicalCenters/Create", "~/Pages/MedicalCenter/CreateEditMC.aspx");
            routes.MapPageRoute(null, "MedicalCenters", "~/Pages/MedicalCenter/MedicalCenters.aspx");

            routes.MapPageRoute(null, "Account/Login", "~/Pages/Account/Login.aspx");
            routes.MapPageRoute(null, "Account/Manage", "~/Pages/Account/Manage.aspx");

            routes.MapPageRoute(null, "Administration/CreateUser", "~/Pages/Administration/EditCreateUser.aspx");
            routes.MapPageRoute(null, "Administration/EditUser/{id}", "~/Pages/Administration/EditCreateUser.aspx");
            routes.MapPageRoute(null, "Administration/Users/{page}", "~/Pages/Administration/Users.aspx");
            routes.MapPageRoute(null, "Administration/Users", "~/Pages/Administration/Users.aspx");
            routes.MapPageRoute(null, "Administration/Errors", "~/Pages/Administration/ErrorLog.aspx");
            routes.MapPageRoute(null, "Administration", "~/Pages/Administration/Users.aspx");

            routes.MapPageRoute(null, "", "~/Default.aspx");

            routes.RouteExistingFiles = true;
            routes.EnableFriendlyUrls();
        }
    }
}
