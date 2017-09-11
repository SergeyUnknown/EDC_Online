using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace EDC.Pages.Unloading
{
    public partial class Profiles : BasePage
    {
        Models.Repository.UnloadingProfileRepository UPR;
        Models.Repository.UnloadingFileRepository UFR;

        public Profiles()
        {
            UPR = new Models.Repository.UnloadingProfileRepository();
            UFR = new Models.Repository.UnloadingFileRepository();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadProfiles();
        }

        void LoadProfiles()
        {
            gvUnloadingProfiles.DataSource = UPR.SelectAll().ToList();
            gvUnloadingProfiles.DataBind();
        }

        protected void btnAddProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Unloading/Add");
        }

        protected string GetCentersName(Models.UnloadingProfile up)
        {
            System.Text.StringBuilder centersName = new System.Text.StringBuilder();

            if(up.Centers!=null)
            foreach(var center in up.Centers)
            {
                centersName.AppendLine(center.MedicalCenter.Name);
            }

            return centersName.ToString();
        }

        protected void btnUnload_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Unloading/Files?pid="+(sender as ImageButton).CommandArgument);   
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string strProfileID = (sender as ImageButton).CommandArgument;
            int profileID;
            if (!int.TryParse(strProfileID, out profileID))
                return;

            try
            {
                var files = UFR.GetManyByFilter(x => x.UnloadingProfileID == profileID).ToList();

                foreach (var file in files)
                {
                    if (System.IO.File.Exists(file.PathToFile))
                        System.IO.File.Delete(file.PathToFile);
                }

                UPR.Delete(profileID);
                UPR.Save();
            }
            catch (Exception error)
            {
                Response.Write(error.Message);
            }

            LoadProfiles();
        }
    }
}