using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EDC.Pages.CRF
{
    public partial class ViewCRF : BasePage
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
                lblOID.Text = CR.SelectByID(CRFID).Identifier;
                LoadTables();
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

        void LoadTables()
        {
            _sections = CSR.GetManyByFilter(x => x.CRFID == CRFID).ToList();
            gvSections.DataSource = _sections;
            gvSections.DataBind();

            _groups = CGR.GetManyByFilter(x => x.CRFID == CRFID).ToList();
            gvGroups.DataSource = _groups;
            gvGroups.DataBind();
            gvGroups.Style.Add("display","none");

            _items = IR.GetManyByFilter(x => x.CRFID == CRFID).ToList();
            gvCRF_Fields.DataSource = _items;
            gvCRF_Fields.DataBind();
            gvCRF_Fields.Style.Add("display", "none");
        }

    }
}