using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EDC.Pages.Event
{
    public partial class ConfigurationEvent : BasePage
    {
        Models.Repository.EventRepository ER = new Models.Repository.EventRepository();
        Models.Repository.CRFInEventRepository ECRFR = new Models.Repository.CRFInEventRepository();
        Models.Repository.CRFRepository CRFR = new Models.Repository.CRFRepository();

        //текущее событие
        Models.Event _event
        {
            get
            {
                if (Session["ce_event"] == null)
                    Session["ce_event"] = new Models.Event();
                return (Models.Event)Session["ce_event"];
            }
            set
            {
                Session["ce_event"] = value;
            }
        }

        //Добавленные CRF/Элементы связующей таблицы
        List<Models.CRFInEvent> _eCRFs
        {
            get
            {
                if (Session["ce_eCRFs"] == null)
                    Session["ce_eCRFs"] = new List<Models.CRFInEvent>();
                return Session["ce_eCRFs"] as List<Models.CRFInEvent>;
            }
            set
            {
                Session["ce_eCRFs"] = value;
            }
        }

        //все CRF
        List<Models.CRF> _CRFs
        {
            get
            {
                if (Session["ce_CRFs"] == null)
                    Session["ce_CRFs"] = new List<Models.CRF>();
                return Session["ce_CRFs"] as List<Models.CRF>;
            }
            set
            {
                Session["ce_CRFs"] = value;
            }
        }

        //CRF которые нужно добавить 
        List<Models.CRF> _addingCRFs
        {
            get
            {
                if (Session["ce_addingCRFs"] == null)
                    Session["ce_addingCRFs"] = new List<Models.CRF>();
                return Session["ce_addingCRFs"] as List<Models.CRF>;
            }
            set
            {
                Session["ce_addingCRFs"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.IsInRole(Core.Roles.Administrator.ToString()))
                Response.Redirect("~/");
            if (!IsPostBack)
            {
                _addingCRFs = new List<Models.CRF>();
                divAddCRF.Visible = false;
                _event = ER.SelectByID(GetIDFromRequest());
                if (_event == null)
                    throw new NullReferenceException();
                Title = "Настройка События " + _event.Name;

                LoadEventsCRFs();
            }
        }

        void LoadEventsCRFs()
        {
            long eventID = _event.EventID;
            _eCRFs = ECRFR.GetManyByFilter(x => x.EventID == eventID).OrderBy(x=>x.Position).ToList();
            CheckCRFs();
            gvCRFs.DataSource = _eCRFs;
            gvCRFs.DataBind();

        }

        void CheckCRFs()
        {
            for(int i =0;i< _eCRFs.Count;i++)
            {
                if(_eCRFs[i].Position != i+1)
                {
                    _eCRFs[i].Position = i + 1;
                    ECRFR.Update(_eCRFs[i]);
                }
            }
            ECRFR.Save();
            long eventID = _event.EventID;
            _eCRFs = ECRFR.GetManyByFilter(x => x.EventID == eventID).OrderBy(x => x.Position).ToList();
        }

        long GetIDFromRequest()
        {
            long id;
            string strID = (string)RouteData.Values["id"] ?? Request.QueryString["id"];
            if (string.IsNullOrWhiteSpace(strID))
            {
                throw new ArgumentException("Необходимо указать ID события");
            }
            if (!long.TryParse(strID, out id))
            {
                throw new ArgumentException("ID указан в неверном формате");
            }
            return id;
        }

        protected void gvCRFs_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            long eventID = _event.EventID;
            int pos = _eCRFs[e.RowIndex].Position;

            List<Models.CRFInEvent> _editedECRF = ECRFR.GetManyByFilter(x => x.EventID == eventID && x.Position > pos).ToList();
            for (int i = 0; i < _editedECRF.Count;i++ )
            {
                _editedECRF[i].Position--;
                ECRFR.Update(_editedECRF[i]);
            }
                ECRFR.Delete(_eCRFs[e.RowIndex].CRFID, _eCRFs[e.RowIndex].EventID);
            ECRFR.Save();
            LoadEventsCRFs();
        }

        protected void gvCRFs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlPos = (DropDownList)e.Row.FindControl("ddlPosition");
                for (int i = 0; i < _eCRFs.Count; i++)
                {
                    ddlPos.Items.Add((i + 1).ToString());
                }
                ddlPos.SelectedIndex = e.Row.RowIndex;

            }
        }
        protected void ddlPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.Parent.Parent;

            Models.CRFInEvent _eCRF = ECRFR.SelectByID(_eCRFs[row.RowIndex].CRFID, _eCRFs[row.RowIndex].EventID);
            int newPos = ddl.SelectedIndex + 1;

            if (newPos < _eCRF.Position)
            {
                List<Models.CRFInEvent> editedEventsCRFs = 
                    ECRFR.GetManyByFilter(
                    x => x.EventID == _event.EventID && 
                        x.Position >= newPos && 
                        x.Position < _eCRF.Position).OrderBy(x => x.Position).ToList();

                for (int i = 0; i < editedEventsCRFs.Count; i++)
                {
                    editedEventsCRFs[i].Position = newPos + (i + 1);
                    ECRFR.Update(editedEventsCRFs[i]);
                }
            }
            else
            {

                List<Models.CRFInEvent> editedEventsCRFs = 
                    ECRFR.GetManyByFilter(x => 
                        x.EventID == _event.EventID && 
                        x.Position <= newPos && 
                        x.Position > _eCRF.Position).OrderBy(x => x.Position).ToList();

                for (int i = 0; i < editedEventsCRFs.Count; i++)
                {
                    editedEventsCRFs[i].Position = newPos - editedEventsCRFs.Count + i;
                    ECRFR.Update(editedEventsCRFs[i]);
                }
            }

            List<Models.CRFInEvent> allCrfInEvent = ECRFR.GetManyByFilter(x => x.EventID == _event.EventID).OrderBy(x => x.Position).ToList();

            _eCRF.Position = newPos;
            try
            {
                ECRFR.Update(_eCRF);
                ECRFR.Save();
            }
            catch (Exception error)
            {
                //вывод
            }
            LoadEventsCRFs();
            
        }

        protected void btnAddCRF_Click(object sender, EventArgs e)
        {
            divAddCRF.Visible = true;
            divAddedCRF.Visible = false;
            LoadNewCRF();
        }

        private void LoadNewCRF()
        {
            _CRFs = CRFR.SelectAll().ToList();
            _CRFs = _CRFs.FindAll(x=>!_eCRFs.Any(y=>y.CRFID== x.CRFID)).OrderBy(x=>x.RussianName).ThenBy(x=>x.Name).ToList();
            gvNewCRFs.DataSource = _CRFs;
            gvNewCRFs.DataBind();

        }

        protected void btnSaveAdd_Click(object sender, EventArgs e)
        {
            divAddCRF.Visible = false;
            divAddedCRF.Visible = true;

            int crfCount = _eCRFs.Count;
            for (int i = 0; i < _addingCRFs.Count; i++)
            {
                Models.CRFInEvent item = new Models.CRFInEvent();
                item.CRFID = _addingCRFs[i].CRFID;
                item.EventID = _event.EventID;
                item.Position = crfCount + i + 1;
                ECRFR.Create(item);
            }
            ECRFR.Save();
            LoadEventsCRFs();
        }

        protected void btnSaveCancel_Click(object sender, EventArgs e)
        {
            divAddCRF.Visible = false;
            divAddedCRF.Visible = true;
        }

        protected void cbAdd_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if(cb.Checked)
            {
                int posIndex = 1;
                if (_eCRFs != null)
                    posIndex += _eCRFs.Count;
                if (_addingCRFs != null)
                    posIndex += _addingCRFs.Count;

                GridViewRow gvr = (GridViewRow)cb.Parent.Parent;
                Label lbl = (Label)gvr.FindControl("lblPosition");
                lbl.Text = posIndex.ToString();

                
                _addingCRFs.Add(_CRFs[gvr.RowIndex]);
            }
            else
            {
                GridViewRow gvr = (GridViewRow)cb.Parent.Parent;
                Label lbl = (Label)gvr.FindControl("lblPosition");
                int pos = int.Parse(lbl.Text);

                if (_eCRFs != null)
                    pos -= _eCRFs.Count;
                _addingCRFs.RemoveAt(pos-1);

                LoadNewCRF();
            }
        }

        protected void gvNewCRFs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType== DataControlRowType.DataRow)
            {
                CheckBox cb = (CheckBox)e.Row.FindControl("cbAdd");
                Label lblName = (Label)e.Row.FindControl("lblName");
                lblName.Attributes.Add("onclick", string.Format("labelNameClick('#{0}');", cb.ClientID));


                int pos = _addingCRFs.FindIndex(x=>x.Name==((Models.CRF)e.Row.DataItem).Name);
                if(pos>=0)
                {
                    cb.Checked = true;

                    Label lbl = (Label)e.Row.FindControl("lblPosition");

                    if (_eCRFs != null)
                        pos += _eCRFs.Count;
                    pos += 1;
                    lbl.Text = pos.ToString();
                    
                }
            }
        }

        protected void gvCRFs_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            e.Cancel = true;
            Response.Redirect("~/CRFs/View/" + _eCRFs[e.NewSelectedIndex].CRFID);
        }

    }
}