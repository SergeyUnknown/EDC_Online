using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EDC.Pages.SDV
{
    public partial class SDV : System.Web.UI.Page
    {
        Models.Repository.SubjectsCRFRepository SCrfR = new Models.Repository.SubjectsCRFRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadTable();
        }

        void LoadTable()
        {
            List<Models.SubjectsCRF> approvedSubjectsCRF = SCrfR.GetManyByFilter(x => x.IsApprove && !x.IsCheckAll).ToList();
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