using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EDC.Pages.CRF
{
    public partial class ViewCRF : System.Web.UI.Page
    {
        Models.Repository.CRFRepository CR = new Models.Repository.CRFRepository();
        Models.Repository.CRFSectionRepository CSR = new Models.Repository.CRFSectionRepository();
        Models.Repository.CRFGroupRepository CGR = new Models.Repository.CRFGroupRepository();
        Models.Repository.CRFItemRepository IR = new Models.Repository.CRFItemRepository();

        static long CRFID;
        List<Models.CRF_Section> _sections;
        List<Models.CRF_Group> _groups;
        List<Models.CRF_Item> _items;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CRFID = GetIDFromRequest();
                string tableType = GetTableTypeFromRequest();
                switch (tableType)
                {
                    case "Sections": LoadSections();
                        break;
                    case "Groups": LoadGroups();
                        break;
                    case "Items": LoadItems();
                        break;
                    default: LoadSections();
                        break;
                }
            }

        }

        long GetIDFromRequest()
        {
            long id;
            string strID = (string)RouteData.Values["id"] ?? Request.QueryString["id"];
            if (string.IsNullOrWhiteSpace(strID))
            {
                throw new ArgumentException("Необходимо указать ID CRF");
            }
            if (!long.TryParse(strID, out id))
            {
                throw new ArgumentException("ID указан в неверном формате");
            }
            return id;
        }

        string GetTableTypeFromRequest()
        {
            return (string)RouteData.Values["table"] ?? Request.QueryString["table"];
        }

        void LoadSections()
        {
            gvSections.Visible = true;
            gvGroups.Visible = false;
            gvCRF_Fields.Visible = false;
            _sections = CSR.GetManyByFilter(x=>x.CRFID == CRFID).ToList();
            gvSections.DataSource = _sections;
            gvSections.DataBind();
        }
        void LoadGroups()
        {
            gvSections.Visible = false;
            gvGroups.Visible = true;
            gvCRF_Fields.Visible = false;
            _groups = CGR.GetManyByFilter(x => x.CRFID == CRFID).ToList();
            gvGroups.DataSource = _groups;
            gvGroups.DataBind();
        }
        void LoadItems()
        {
            gvSections.Visible = false;
            gvGroups.Visible = false;
            gvCRF_Fields.Visible = true;
            _items = IR.GetManyByFilter(x => x.CRFID == CRFID).ToList();
            gvCRF_Fields.DataSource = _items;
            gvCRF_Fields.DataBind();
        }

        protected void btnSections_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/CRFs/View/"+CRFID+"/Sections");
        }

        protected void btnGroups_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/CRFs/View/" + CRFID + "/Groups");
        }

        protected void btnItems_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/CRFs/View/" + CRFID + "/Items");
        }

    }
}