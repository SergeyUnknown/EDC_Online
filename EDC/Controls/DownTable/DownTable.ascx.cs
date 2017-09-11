using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EDC.Controls
{
    public partial class DownTable : System.Web.UI.UserControl
    {
        public bool ViewButton
        {
            get { return LinkButtonAddNew.Visible; }
            set { LinkButtonAddNew.Visible = value; }
        }

        static int _pageSize;

        public int DropDownSelectedValue
        {
            get { return _pageSize; }
        }

        public event EventHandler SelectedIndexChanged;

        protected virtual void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedIndexChanged != null)
            {
                SelectedIndexChanged.Invoke(this, e);
            }
        }

        public DownTableDataItem DataItem { get; set; }

        public override void DataBind()
        {
            LinkButtonFirstPage.PostBackUrl = DataItem.FirstPageURL;
            LinkButtonPrevPage.PostBackUrl = DataItem.PrevPageURL;
            LinkButtonNextPage.PostBackUrl = DataItem.NextPageURL;
            LinkButtonLastPage.PostBackUrl = DataItem.LastPageURL;
            LinkButtonAddNew.PostBackUrl = DataItem.ButtonURL;
            LinkButtonAddNew.ToolTip = DataItem.ButtonText;
            pageInfo.Text = DataItem.PageInfo;
            ddlItemCount.DataSource = DataItem.DropDownItems;
            ddlItemCount.DataBind();
            ddlItemCount.SelectedValue = DataItem.DropDownSelectedValue.ToString();
            _pageSize = DataItem.DropDownSelectedValue;
            Image5.ImageUrl = DataItem.ButtonImageURL;

            HttpCookie cookie = new HttpCookie("EDC Cookie");
            cookie["PrevPage"] = Request.Url.ToString();
            Response.Cookies.Add(cookie);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (DataItem != null)
            {
                LinkButtonFirstPage.PostBackUrl = DataItem.FirstPageURL;
                LinkButtonPrevPage.PostBackUrl = DataItem.PrevPageURL;
                LinkButtonNextPage.PostBackUrl = DataItem.NextPageURL;
                LinkButtonLastPage.PostBackUrl = DataItem.LastPageURL;
                LinkButtonAddNew.PostBackUrl = DataItem.ButtonURL;
                LinkButtonAddNew.ToolTip = DataItem.ButtonText;
                pageInfo.Text = DataItem.PageInfo;
                ddlItemCount.DataSource = DataItem.DropDownItems;
                ddlItemCount.DataBind();
                ddlItemCount.SelectedValue = DataItem.DropDownSelectedValue.ToString();
                _pageSize = DataItem.DropDownSelectedValue;
                Image5.ImageUrl = DataItem.ButtonImageURL;
            }
        }

        protected void ddlItemCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strPageSize = "";
            DropDownList PageSizeList = ((DropDownList)sender);
            strPageSize = PageSizeList.SelectedValue;

            int.TryParse(strPageSize, out _pageSize);

            OnSelectedIndexChanged(sender, e);
        }
    }
}