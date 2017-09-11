using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EDC.Pages.Unloading
{
    public partial class ProfileFiles : BasePage
    {
        int ProfileID
        {
            get
            {
                return (int)Session["profileID"];
            }
            set 
            {
                Session["profileID"] = value; 
            }
        }

        Models.Repository.UnloadingFileRepository UFR;

        public ProfileFiles()
        {
            UFR = new Models.Repository.UnloadingFileRepository();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                GetProfileIDFromRequest();
                LoadFiles();
            }
        }

        void GetProfileIDFromRequest()
        {
            string strProfileID = Request.Params.Get("pid");
            int profileID;
            if (!int.TryParse(strProfileID, out profileID))
                throw new ArgumentException("Некорректно указан ID профиля");
            ProfileID = profileID;
        }

        void LoadFiles()
        {
            gvUnloadingFiles.DataSource = UFR.GetManyByFilter(x=>x.UnloadingProfileID == ProfileID).ToList();
            gvUnloadingFiles.DataBind();
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            ImageButton btn = sender as ImageButton;
            if (btn == null)
                throw new ArgumentNullException("btn is null");
            string strFileID = btn.CommandArgument;
            int fileID;
            if (!int.TryParse(strFileID, out fileID))
                return;
            var uf = UFR.SelectByID(fileID);
            Response.Clear();
            Response.ContentType = uf.Type;
            Response.AppendHeader("Content-Disposition", "attachment;filename="+uf.FileName);
            Response.WriteFile(uf.PathToFile);
            Response.Flush();
            Response.End();
        }

        protected void gvUnloadingFiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var files = UFR.GetManyByFilter(x => x.UnloadingProfileID == ProfileID).ToList();
            var uf = files[e.RowIndex];

            if (System.IO.File.Exists(uf.PathToFile))
                System.IO.File.Delete(uf.PathToFile);
            UFR.Delete(uf.UnloadingFileID);
            UFR.Save();

            LoadFiles();
        }

        protected void btnRunExcelUnloading_Click(object sender, EventArgs e)
        {
            try
            {
                int upID = ProfileID;

                using (UnloadingServiceReference.UnloadingServiceClient client = new UnloadingServiceReference.UnloadingServiceClient())
                {
                    client.ExportToExcel(upID, User.Identity.Name);
                }
            }
            catch (Exception error)
            {
                throw new Exception("error", error);
            }
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        protected void btnRunODMv132Unloading_Click(object sender, EventArgs e)
        {

        }
    }
}