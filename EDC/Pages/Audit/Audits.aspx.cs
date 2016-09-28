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
                string userName = Request.QueryString["userName"];
                if (!string.IsNullOrEmpty(userName))
                    tbUserName.Text = userName;
                string dateMin = Request.QueryString["dateMin"];
                if (!string.IsNullOrEmpty(dateMin))
                    tbDateMin.Text = dateMin;
                string dateMax = Request.QueryString["dateMax"];
                if (!string.IsNullOrEmpty(userName))
                    tbDateMin.Text = dateMax;

                LoadAudits(userName, dateMin, dateMax);
            }
        }

        int GetPageFromRequest()
        {
            string sPageNumber = Request.QueryString["page"];
            int number = 0;
            if (sPageNumber != null)
                if (int.TryParse(sPageNumber, out number) && number>0 && number<MaxPage())
                    return number;
            return 1;
        }

        int MaxPage()
        {
            return (int)Math.Ceiling(AR.SelectAll().Count()/15.0);
        }

        void LoadAudits(string userName, string dateMin, string dateMax)
        {
            lblMessage.Visible = false;
            DateTime tempDate;

            List<Models.Audit> audits = AR.SelectAll().ToList();
            bool dataAvailable = audits.Count > 0;

            gvAudits.DataSource = null;
            gvAudits.DataBind();
            if(!dataAvailable)
            {
                lblMessage.Text = "Нет данных";
                lblMessage.Visible = true;
                return;
            }

            if (!string.IsNullOrEmpty(userName))
                audits = audits.FindAll(x => x.UserName == userName);
            if (dateMin != null && DateTime.TryParse(dateMin, out tempDate))
                audits = audits.FindAll(x => x.ActionDate >= tempDate);
            if (dateMax != null && DateTime.TryParse(dateMax, out tempDate))
                audits = audits.FindAll(x => x.ActionDate <= tempDate);

            audits = audits.Skip((GetPageFromRequest() - 1) * 15).Take(15).ToList();
            if (audits.Count == 0)
            {
                lblMessage.Text = "Данные удовлетворяющие заданным условиям не найдены";
                lblMessage.Visible = true;
            }
            else
            {
                gvAudits.DataSource = audits;
                gvAudits.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadAudits(tbUserName.Text, tbDateMin.Text, tbDateMax.Text);
        }
    }
}