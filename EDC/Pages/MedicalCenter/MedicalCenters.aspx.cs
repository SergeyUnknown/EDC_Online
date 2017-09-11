using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDC.Models;

namespace EDC.Pages.MedicalCenter
{
    public partial class MedicalCenters : BasePage
    {
        static List<Models.MedicalCenter> _MCs;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                LoadMedicalCenters();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadMedicalCenters();
        }

        void LoadMedicalCenters()
        {

            Models.Repository.MedicalCenterRepository mcr = new Models.Repository.MedicalCenterRepository();
            _MCs = mcr.SelectAll().ToList();
            if(!string.IsNullOrWhiteSpace(tbName.Text))
            {
                _MCs= _MCs.Where(x => x.Name.ToLower().IndexOf(tbName.Text.ToLower()) > -1).ToList();
            }
            if(!string.IsNullOrWhiteSpace(tbRegion.Text))
            {
                _MCs = _MCs.Where(x => x.Region.ToLower().IndexOf(tbRegion.Text.ToLower()) > -1).ToList();
            }
            if(!string.IsNullOrWhiteSpace(tbCity.Text))
            {
                _MCs = _MCs.Where(x => x.City.ToLower().IndexOf(tbCity.Text.ToLower()) > -1).ToList();
            }

            gvMedicalCenters.DataSource = _MCs;
            gvMedicalCenters.DataBind();
        }

        protected void gvMedicalCenters_RowEditing(object sender, GridViewEditEventArgs e)
        {
            Response.Redirect("~/MedicalCenters/Edit/"+_MCs[e.NewEditIndex].MedicalCenterID);
        }

        protected void gvMedicalCenters_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Models.MedicalCenter mc = _MCs[e.RowIndex];
            Models.Repository.MedicalCenterRepository mcr = new Models.Repository.MedicalCenterRepository();
            mcr.Delete(mc.MedicalCenterID);
            mcr.Save();
            
            LoadMedicalCenters();
        }

        protected void RedirectToCreate_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/MedicalCenters/Create");
        }
    }
}