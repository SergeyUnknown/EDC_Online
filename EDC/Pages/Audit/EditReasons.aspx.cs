using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EDC.Pages.Audit
{
    public partial class EditReasons : System.Web.UI.Page
    {
        Models.Repository.AuditsEditReasonsRepository AERR = new Models.Repository.AuditsEditReasonsRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadEditReasons();
        }

        void LoadEditReasons()
        {
            gvEditReason.DataSource = AERR.SelectAll().ToList();
            gvEditReason.DataBind();
        }

        protected string GetCRFName(Models.SubjectsCRF sCRF)
        {
            if (sCRF != null)
            {
                string rusName = sCRF.CRF.RussianName;
                string engName = sCRF.CRF.Name;
                if (string.IsNullOrWhiteSpace(rusName))
                    return engName;
                else
                    return rusName;
            }
            else
                return "";
        }
    }
}