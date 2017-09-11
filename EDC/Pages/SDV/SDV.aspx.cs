using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EDC.Pages.SDV
{
    public partial class SDV : BasePage
    {
        Models.Repository.SubjectsCRFRepository SCrfR = new Models.Repository.SubjectsCRFRepository();
        Models.Repository.UserProfileRepository UPR = new Models.Repository.UserProfileRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadTable();
        }

        void LoadTable()
        {
            var up = UPR.SelectByID(System.Web.Security.Membership.GetUser(User.Identity.Name).ProviderUserKey);
            if (up == null || up.CurrentCenterID == null)
                return;
            List<Models.SubjectsCRF> approvedSubjectsCRF = SCrfR.GetManyByFilter(x => x.Subject.MedicalCenterID == up.CurrentCenterID && x.IsEnd && !x.IsCheckAll).ToList();
            if (approvedSubjectsCRF.Count == 0)
                lblInfo.Visible = true;
            else
            {
                gvApprovedSubjectsCRF.DataSource = approvedSubjectsCRF;
                gvApprovedSubjectsCRF.DataBind();
            }
        }
        protected string GetRedirectURL(object obj)
        {
            Models.SubjectsCRF subjectCrf = obj as Models.SubjectsCRF;
            return ResolveClientUrl(string.Format("~/Subjects/{0}/{1}/{2}",subjectCrf.SubjectID,subjectCrf.EventID,subjectCrf.CRFID));
        }
    }
}