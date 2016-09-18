using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;

namespace EDC.Pages.Administration
{
    public partial class ConfigurationTrials : System.Web.UI.Page
    {
        Models.Repository.AppSettingRepository ASR = new Models.Repository.AppSettingRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var item = ASR.SelectByID("appStatus");
                rbStatus.SelectedValue = item.Value;

                item = ASR.SelectByID("studyName");
                tbStudyName.Text = item.Value;

                item = ASR.SelectByID("protocolID");
                tbProtocolID.Text = item.Value;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var item = ASR.SelectByID("appStatus");
            if (item != null)
            {
                item.Value = rbStatus.SelectedValue;
                ASR.Update(item);
            }

            item = ASR.SelectByID("studyName");
            if (item != null)
            {
                item.Value = tbStudyName.Text;
                ASR.Update(item);
            }

            item = ASR.SelectByID("protocolID");
            if (item != null)
            {
                item.Value = tbProtocolID.Text;
                ASR.Update(item);
            }

            ASR.Save();
        }
    }
}