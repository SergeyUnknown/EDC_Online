using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;

namespace EDC.Pages.Administration
{
    public partial class ConfigurationTrials : BasePage
    {
        Models.Repository.AppSettingRepository ASR = new Models.Repository.AppSettingRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var item = ASR.SelectByID(Core.Core.APP_STATUS);
                rbStatus.SelectedValue = item.Value;

                item = ASR.SelectByID(Core.Core.STUDY_NAME);
                tbStudyName.Text = item.Value;

                item = ASR.SelectByID(Core.Core.STUDY_PROTOCOL);
                tbProtocolID.Text = item.Value;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var item = ASR.SelectByID(Core.Core.APP_STATUS);

            item.Value = rbStatus.SelectedValue;
            ASR.Update(item);


            item = ASR.SelectByID(Core.Core.STUDY_NAME);

            item.Value = tbStudyName.Text;
            ASR.Update(item);


            item = ASR.SelectByID(Core.Core.STUDY_PROTOCOL);

            item.Value = tbProtocolID.Text;
            ASR.Update(item);
            

            ASR.Save();
        }
    }
}