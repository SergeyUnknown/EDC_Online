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
                if (item!=null)
                {
                    rbStatus.SelectedValue = item.Value;
                }

                item = ASR.SelectByID("studyName");
                if (item != null)
                {
                    tbStudyName.Text = item.Value;
                }

                item = ASR.SelectByID("protocolID");
                if (item != null)
                {
                    tbProtocolID.Text = item.Value;
                }
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
            else
            {
                item = new Models.AppSetting();
                item.AppSettingID = "appStatus";
                item.Value = rbStatus.SelectedValue;
                ASR.Create(item);
            }

            item = ASR.SelectByID("studyName");
            if (item != null)
            {
                item.Value = tbStudyName.Text;
                ASR.Update(item);
            }
            else
            {
                item = new Models.AppSetting();
                item.AppSettingID = "studyName";
                item.Value = tbStudyName.Text;
                ASR.Create(item);
            }

            item = ASR.SelectByID("protocolID");
            if (item != null)
            {
                item.Value = tbProtocolID.Text;
                ASR.Update(item);
            }
            else
            {
                item = new Models.AppSetting();
                item.AppSettingID = "protocolID";
                item.Value = tbProtocolID.Text;
                ASR.Create(item);
            }

            ASR.Save();
        }
    }
}