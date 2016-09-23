using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EDC.Pages.Audit
{
    public partial class Audits : System.Web.UI.Page
    {
        Models.Repository.AuditsRepository AR = new Models.Repository.AuditsRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                gvAudits.DataSource = AR.SelectAll().ToList();
                gvAudits.DataBind();
            }
        }
    }
}