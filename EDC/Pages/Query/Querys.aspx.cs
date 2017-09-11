using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDC.Core;

namespace EDC.Pages.Query
{
    public partial class Querys : BasePage
    {
        Models.Repository.QueryRepository NR = new Models.Repository.QueryRepository();
        Models.Repository.UserProfileRepository UPR = new Models.Repository.UserProfileRepository();

        static int pageSize = 50;
        static int recordCount = 0;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.IsInRole(Core.Roles.Data_Manager.ToString()))
                dtInfo.ViewButton = false;
            if(!IsPostBack)
            {
                string reqSubjectNumber = Request.QueryString["subjectNumber"] ?? "";
                string reqforUser = Request.QueryString["forUser"] ?? "";
                string reqQueryStatus = Request.QueryString["queryStatus"] ?? "";
                LoadNotes(reqSubjectNumber,reqforUser,reqQueryStatus);
                LoadDTDataItem();
            }
        }

        #region LoadQueries
        void LoadNotes(string subjectNumber = "", string forUser = "", string queryStatus = "")
        {
            Models.UserProfile currentUserProfile = UPR.SelectByID(System.Web.Security.Membership.GetUser().ProviderUserKey);
            List<Models.Query> queries = NR.GetManyByFilter(x=>x.MedicalCenterID == currentUserProfile.CurrentCenterID).ToList();
            
            if (currentUserProfile == null || currentUserProfile.GetCurrentCenterID() == null)
                return;

            queries = Core.Core.FilterQueriesAccess(queries,User);

            tQueryStatistic.Rows[2].Cells[0].InnerText = queries.Count(x => x.Status == QueryStatus.New).ToString();
            tQueryStatistic.Rows[2].Cells[1].InnerText = queries.Count(x => x.Status == QueryStatus.Updated).ToString();
            tQueryStatistic.Rows[2].Cells[2].InnerText = queries.Count(x => x.Status == QueryStatus.Closed).ToString();
            tQueryStatistic.Rows[2].Cells[3].InnerText = queries.Count.ToString();
            queries = FilterQueries(queries, subjectNumber, forUser, queryStatus);
            queries = queries.OrderByDescending(x => x.QueryID).Skip((CurrentPage - 1) * pageSize).Take(pageSize).ToList();
            gvNotes.DataSource = queries;
            gvNotes.DataBind();
        }

        List<Models.Query> FilterQueries(List<Models.Query> queries, string subjectNumber = "", string forUser = "", string queryStatus = "")
        {
            if (subjectNumber != "")
                queries = queries.Where(x=> x.Subject.Number.ToLower().IndexOf(subjectNumber.ToLower())>-1).ToList();
            if (forUser != "")
                queries = queries.Where(x => x.To.ToLower().IndexOf(forUser.ToLower())>-1).ToList();
            if(queryStatus!="")
            {
                switch(queryStatus.ToLower())
                {
                    case "new":
                        {
                            queries = queries.Where(x => x.Status == QueryStatus.New).ToList();
                            return queries;
                        }
                    case "updated":
                        {
                            queries = queries.Where(x => x.Status == QueryStatus.Updated).ToList();
                            return queries;
                        }
                    case "closed":
                        {
                            queries = queries.Where(x => x.Status == QueryStatus.Closed).ToList();
                            return queries;
                        }
                }
            }
            return queries;
        }
        #endregion

        private int GetPageFromRequest()
        {
            int page;
            string reqValue = (string)RouteData.Values["page"] ?? Request.QueryString["page"];
            return reqValue != null && int.TryParse(reqValue, out page) && page > 0 ? page : 1;
        }

        //номер текущей страницы
        protected int CurrentPage
        {
            get
            {
                int page;
                page = GetPageFromRequest();
                return page > MaxPageCount ? MaxPageCount : page;
            }
        }

        //максимальный номер страницы
        protected int MaxPageCount
        {
            get
            {
                return (int)Math.Ceiling((decimal)NR.SelectAll().Count() / pageSize);
            }
        }

        protected void dtInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            pageSize = dtInfo.DropDownSelectedValue;
            int maxRecordsOnPage = (CurrentPage * pageSize) > recordCount ? recordCount : (CurrentPage * pageSize);

            LoadDTDataItem();
        }

        void LoadDTDataItem()
        {
            DownTableDataItem dtDataItem = new DownTableDataItem(
                CurrentPage,
                MaxPageCount,
                "~/Querys/",
                Core.Core.DropDownListItems25,
                pageSize);

            dtInfo.DataItem = dtDataItem;
            dtInfo.DataBind();
        }

        protected string GetCRFName(Models.CRF CRF)
        {
            if (CRF != null)
            {
                string rusName = CRF.RussianName;
                string engName = CRF.Name;
                if (string.IsNullOrWhiteSpace(rusName))
                    return engName;
                else
                    return rusName;
            }
            else
                return "";
        }

        protected string GetRedirectURL(Models.SubjectsItem si,Models.Query query)
        {
            if (si == null)
                return "";
            else
                return ResolveClientUrl(string.Format("~/Subjects/{0}/{1}/{2}?mpename={3}&q={4}", si.SubjectID, si.EventID, si.CRFID, si.IndexID > 0 ? si.Item.Identifier + "_" + si.IndexID : si.Item.Identifier,query.QueryID));
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadNotes(tbSubjectNumber.Text,tbForUser.Text,ddlQueryStatus.SelectedValue);
        }
    }
}