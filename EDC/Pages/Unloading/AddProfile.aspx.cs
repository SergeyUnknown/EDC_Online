using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EDC.Pages.Unloading
{
    public partial class AddProfile : BasePage
    {
        Models.Repository.EventRepository ER;
        Models.Repository.CRFInEventRepository CRFIER;
        Models.Repository.CRFItemRepository CRFIR;
        Models.Repository.CRFRepository CRFR;
        Models.Repository.UnloadingProfileRepository UPR;
        Models.Repository.UnloadingItemRepository UIR;
        Models.Repository.MedicalCenterRepository MCR;
        Models.Repository.AccessToCenterRepository ATCR;

        public AddProfile()
        {
            ER = new Models.Repository.EventRepository();
            CRFIER = new Models.Repository.CRFInEventRepository();
            CRFIR = new Models.Repository.CRFItemRepository();
            CRFR = new Models.Repository.CRFRepository();
            UPR = new Models.Repository.UnloadingProfileRepository();
            UIR = new Models.Repository.UnloadingItemRepository();
            MCR = new Models.Repository.MedicalCenterRepository();
            ATCR = new Models.Repository.AccessToCenterRepository();
        }

        Models.UnloadingProfile UnloadingProfile
        {
            get
            {
                if (Session["unloadingProfile"] == null)
                    Session["unloadingProfile"] = new Models.UnloadingProfile();
                return Session["unloadingProfile"] as Models.UnloadingProfile;
            }
            set
            {
                Session["unloadingProfile"] = value;
            }
        }

        List<Models.UnloadingItem> UnloadingItems
        {
            get 
            { 
                if(Session["unloadingItems"] == null)
                {
                    Session["unloadingItems"] = new List<Models.UnloadingItem>();
                }
                return Session["unloadingItems"] as List<Models.UnloadingItem>;
            }
            set { Session["unloadingItems"] = value;}
        }

        long SelectedEventID
        {
            get
            {
                if (Session["unloadingSelectedEventID"] == null)
                    Session["unloadingSelectedEventID"] = -1;
                return (long)Session["unloadingSelectedEventID"];
            }
            set
            {
                Session["unloadingSelectedEventID"] = value;
            }
        }

        long SelectedCrfID
        {
            get
            {
                if (Session["unloadingSelectedCrfID"] == null)
                    Session["unloadingSelectedCrfID"] = -1;
                return (long)Session["unloadingSelectedCrfID"];
            }
            set
            {
                Session["unloadingSelectedCrfID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                UnloadingProfile = new Models.UnloadingProfile();
                UnloadingItems = new List<Models.UnloadingItem>();
                SelectedEventID = -1;
                SelectedCrfID = -1;

                var mcs = Core.Core.AccessToCenters(User);
                foreach(var item in mcs)
                {
                    cblCenters.Items.Add(new ListItem(item.Name, item.MedicalCenterID.ToString()));
                }
            }
            else
            {

                if(SelectedEventID >0)
                    LoadCrfs(SelectedEventID);
                if (SelectedCrfID > 0)
                    LoadItems(SelectedCrfID);
            }



            LoadEvents();
        }

        void ClearTable(Table t)
        {
            while(t.Rows.Count>1)
            {
                t.Rows.RemoveAt(1);
            }
        }

        #region Event

        void LoadEvents()
        {
            ClearTable(tEvents);
            List<Models.Event> events = ER.SelectAll().OrderBy(x=>x.Position).ToList();

            foreach(var _event in events)
            {
                var tRow = new TableRow();
                var tCell = new TableCell();

                var cbEvent = new CheckBox();
                cbEvent.Enabled = false;
                if (UnloadingItems.Any(x => x.EventID == _event.EventID))
                    cbEvent.Checked = true;
                var linkEventName = new LinkButton();
                linkEventName.ID = "aEvent_" + _event.EventID;
                linkEventName.Click += linkEventName_Click;
                linkEventName.Text = _event.Name;
                tCell.Controls.Add(cbEvent);
                tCell.Controls.Add(linkEventName);
                tRow.Cells.Add(tCell);
                tEvents.Rows.Add(tRow);
            }
            if (events.Count > 0)
            {
                btnSelectAllEvents.Visible = true;
                btnUnselectAllEvents.Visible = true;
            }
        }

        void linkEventName_Click(object sender, EventArgs e)
        {
            LinkButton lb = sender as LinkButton;
            string strEventID = lb.ID.Substring(lb.ID.IndexOf('_')+1);
            long eventID;
            if(!long.TryParse(strEventID,out eventID))
            {
                throw new FormatException();
            }
            SelectedEventID = eventID;
            SelectedCrfID = -1;
            ClearTable(tItems);
            LoadCrfs(eventID);
        }

        #endregion

        void LoadCrfs(long eventID)
        {
            List<Models.CRFInEvent> crfInEvent = CRFIER.GetManyByFilter(x => x.EventID == eventID).OrderBy(x=>x.Position).ToList();
            ClearTable(tCRFs);
            foreach(var crf in crfInEvent)
            {
                var tRow = new TableRow();
                var tCell = new TableCell();
                //ЧекБокс
                var cb = new CheckBox();
                cb.ID = "cbCRF_" +crf.CRFID;
                cb.Enabled = false;
                if (UnloadingItems.Any(x => x.CRFID == crf.CRFID && x.EventID == SelectedEventID))
                    cb.Checked = true;
                var linkCRFName = new LinkButton();
                linkCRFName.ID = "aCRF_" + crf.CRFID;
                linkCRFName.Text = string.IsNullOrWhiteSpace(crf.CRF.RussianName) ? crf.CRF.Name : crf.CRF.RussianName;
                linkCRFName.Click += linkCRFName_Click;

                tCell.Controls.Add(cb);
                tCell.Controls.Add(linkCRFName);
                tRow.Cells.Add(tCell);
                tCRFs.Rows.Add(tRow);
            }
            if (crfInEvent.Count > 0)
            {
                btnSelectAllCRFs.Visible = true;
                btnUnselectAllCRFs.Visible = true;
            }
        }

        void linkCRFName_Click(object sender, EventArgs e)
        {
            LinkButton lb = sender as LinkButton;
            string strCrfID = lb.ID.Substring(lb.ID.IndexOf('_') + 1);
            long crfID;
            if (!long.TryParse(strCrfID, out crfID))
            {
                throw new FormatException();
            }

            SelectedCrfID = crfID;
            LoadItems(crfID);
        }

        void LoadItems(long crfID)
        {
            List<Models.CRF_Item> itemsInCrf = CRFIR.GetManyByFilter(x => x.CRFID == crfID).ToList();
            ClearTable(tItems);
            foreach (var item in itemsInCrf)
            {
                var tRow = new TableRow();
                var tCell = new TableCell();
                //ЧекБокс
                var cbItem = new CheckBox();
                cbItem.Text = item.DescriptionLabel;
                cbItem.ID = "cbItem_" + item.CRF_ItemID;
                if (UnloadingItems.Any(x => x.EventID == SelectedEventID && x.CRFID == SelectedCrfID && x.ItemID == item.CRF_ItemID))
                    cbItem.Checked = true;
                cbItem.CheckedChanged += cbItem_CheckedChanged;
                cbItem.AutoPostBack = true;
                tCell.Controls.Add(cbItem);
                tRow.Cells.Add(tCell);
                tItems.Rows.Add(tRow);
            }
            if (itemsInCrf.Count > 0)
            {
                btnSelectAllItems.Visible = true;
                btnUnselectAllItems.Visible = true;
            }
        }

        void cbItem_CheckedChanged(object sender, EventArgs e)
        {
            var cb = sender as CheckBox;
            string strItemID = cb.ID.Substring(cb.ID.IndexOf('_')+1);
            long itemID;
            if (!long.TryParse(strItemID, out itemID))
                return;
            if (CRFIR.SelectByID(itemID) == null)
                return;
            if (cb.Checked)
            {
                if (!UnloadingItems.Any(x => x.EventID == SelectedEventID && x.CRFID == SelectedCrfID && x.ItemID == itemID))
                {
                    var unloadingItem = new Models.UnloadingItem();
                    unloadingItem.EventID = SelectedEventID;
                    unloadingItem.CRFID = SelectedCrfID;
                    unloadingItem.ItemID = itemID;
                    UnloadingItems.Add(unloadingItem);
                }
            }
            else
            {
                var ui = UnloadingItems.FirstOrDefault(x => x.EventID == SelectedEventID && x.CRFID == SelectedCrfID && x.ItemID == itemID);
                if(ui != null)
                    UnloadingItems.Remove(ui);
            }

            LoadEvents();
            LoadCrfs(SelectedEventID);
            LoadItems(SelectedCrfID);
        }

        protected void btnSelectAllEvents_Click(object sender, EventArgs e)
        {
            List<Models.Event> events = ER.SelectAll().ToList();

            foreach(var _event in events)
            {
                foreach(var crf in _event.CRFs)
                {
                    AddAllItems(crf.EventID,crf.CRFID);
                }
            }
            LoadEvents();
        }

        protected void btnSelectAllCRFs_Click(object sender, EventArgs e)
        {
            List<Models.CRFInEvent> crfInEvent = CRFIER.GetManyByFilter(x => x.EventID == SelectedEventID).ToList();
            foreach(var crf in crfInEvent)
            {
                AddAllItems(SelectedEventID, crf.CRFID);
            }
            LoadEvents();
            LoadCrfs(SelectedEventID);
        }

        protected void btnSelectAllItems_Click(object sender, EventArgs e)
        {
            AddAllItems(SelectedEventID, SelectedCrfID);
            LoadEvents();
            LoadCrfs(SelectedEventID);
            LoadItems(SelectedCrfID);
        }

        private void AddAllItems(long eventID, long crfID)
        {
            List<Models.CRF_Item> itemsInCrf = CRFIR.GetManyByFilter(x => x.CRFID == crfID).ToList();

            foreach (var item in itemsInCrf)
            {
                if (!UnloadingItems.Any(x => x.EventID == eventID && x.CRFID == crfID && x.ItemID == item.CRF_ItemID))
                {
                    var unloadingItem = new Models.UnloadingItem();
                    unloadingItem.EventID = eventID;
                    unloadingItem.CRFID = crfID;
                    unloadingItem.ItemID = item.CRF_ItemID;

                    UnloadingItems.Add(unloadingItem);
                }
            }
        }

        protected void btnConfigOk_Click(object sender, EventArgs e)
        {
            lblStatus.Visible = false;
            UnloadingProfile.CreatedBy = User.Identity.Name;
            UnloadingProfile.CreationDate = DateTime.Now;
            UnloadingProfile.Name = tbName.Text;
            UnloadingProfile.Description = tbDescription.Text;
            UnloadingProfile.CrfStatus = ddlCrfStatus.SelectedValue;

            if (UPR.GetManyByFilter(x => x.Name == UnloadingProfile.Name).Count() > 0)
            {
                lblStatus.Text = "Уже создан профиль с таким названием";
                lblStatus.Visible = true;
                return;
            }


            divConfig.Visible = false;
            divSelectItems.Visible = true;

            btnCancel2.Visible = btnProfileOk.Visible = true;
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Unloading");
        }

        protected void btnProfileOk_Click(object sender, EventArgs e)
        {
            if (UPR.GetManyByFilter(x => x.Name == UnloadingProfile.Name).Count()>0)
            {
                lblStatus.Text = "Уже создан профиль с таким названием";
                lblStatus.Visible = true;
                return;
            }
            try
            {
                UnloadingProfile.ReadyToUnloading = true;
                var up = UPR.Create(UnloadingProfile);
                UPR.Save();
                
                foreach (ListItem item in cblCenters.Items)
                {
                    if (item.Selected)
                    {
                        int id;
                        if (!int.TryParse(item.Value, out id))
                        {
                            Response.Redirect("~/Unloading");// попытка доступа к другому центру
                        }
                        else
                        {
                            if (Core.Core.HaveAccessToCenter(User, id))
                                UPR.AddCenterToProfile(up.UnloadingProfileID, id);
                            else
                                Response.Redirect("~/Unloading"); // попытка доступа к другому центру
                        }
                    }
                }

                UPR.Save();

                for (int i = 0; i < UnloadingItems.Count;i++ )
                {
                    var item = UnloadingItems[i];
                    item.UnloadingProfileID = up.UnloadingProfileID;
                }

                UIR.Create(UnloadingItems);
                UIR.SaveAsync();
            }
            catch(Exception error)
            {
                lblStatus.Text = error.Message;
                while(error.InnerException != null)
                {
                    lblStatus.Text += ";\r\n" + error.InnerException.Message;
                    error = error.InnerException;
                }
                lblStatus.Visible = true;
                //foreach (var item in UnloadingItems)
                //{
                //    lStatus.Text += item.EventID + " " + item.CRFID + " " + item.ItemID + "\r\n";
                //}
                return;
            }
            Response.Redirect("~/Unloading");
        }

        protected void btnUnselectAllEvents_Click(object sender, EventArgs e)
        {
            UnloadingItems.Clear();
            SelectedEventID = -1;
            SelectedCrfID = -1;
            ClearTable(tCRFs);
            LoadEvents();
        }

        protected void btnUnselectAllCRFs_Click(object sender, EventArgs e)
        {
            UnloadingItems.RemoveAll(x=>x.EventID == SelectedEventID);
            LoadEvents();
            LoadCrfs(SelectedEventID);
        }

        protected void btnUnselectAllItems_Click(object sender, EventArgs e)
        {
            RemoveAllItems(SelectedEventID,SelectedCrfID);
            LoadEvents();
            LoadCrfs(SelectedEventID);
            LoadItems(SelectedCrfID);
        }

        void RemoveAllItems(long eventID, long crfID)
        {
            UnloadingItems.RemoveAll(x => x.EventID == eventID && x.CRFID == crfID);
        }

        public override void Dispose()
        {
            base.Dispose();
        }


    }
}