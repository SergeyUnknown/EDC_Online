using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using EDC.Models;
using EDC.Core.Rule;

namespace EDC.Pages.Subject
{
    public partial class SubjectsCRFPage : BasePage
    {
        #region Repositories
        Models.Repository.SubjectRepository SR = new Models.Repository.SubjectRepository();
        Models.Repository.SubjectsEventRepoitory SER = new Models.Repository.SubjectsEventRepoitory();
        Models.Repository.SubjectsCRFRepository SCR = new Models.Repository.SubjectsCRFRepository();
        Models.Repository.SubjectsItemRepoitory SIR = new Models.Repository.SubjectsItemRepoitory();

        Models.Repository.CRFRepository CRFR = new Models.Repository.CRFRepository();
        Models.Repository.CRFInEventRepository CIER = new Models.Repository.CRFInEventRepository();
        Models.Repository.CRFItemRepository CIR = new Models.Repository.CRFItemRepository();
        Models.Repository.QueryRepository NR = new Models.Repository.QueryRepository();
        Models.Repository.EventRepository ER = new Models.Repository.EventRepository();
        Models.Repository.AppSettingRepository ASR = new Models.Repository.AppSettingRepository();
        Models.Repository.AuditsRepository AR = new Models.Repository.AuditsRepository();

        Models.Repository.AuditsEditReasonsRepository AERR = new Models.Repository.AuditsEditReasonsRepository();
        #endregion

        Models.CRF _crf
        {
            get { return (Models.CRF)Session["_crf"]; }
            set { Session["_crf"] = value; }
        }
        
        Models.Subject _subject
        {
            get { return (Models.Subject)Session["_subject"]; }
            set { Session["_subject"] = value; }
        }
        AjaxControlToolkit.ModalPopupExtender _mpe
        {
            get { return (AjaxControlToolkit.ModalPopupExtender)Session["_mpe"]; }
            set { Session["_mpe"] = value; }
        }
        Models.SubjectsCRF _sCRF
        {
            get { return (Models.SubjectsCRF)Session["_sCRF"]; }
            set { Session["_sCRF"] = value; }
        }
        Dictionary<string, int> RowCountInSection
        {
            get
            {
                if (Session["RowCountInSection"] == null)
                {
                    Session["RowCountInSection"] = new Dictionary<string, int>();
                    return new Dictionary<string, int>();
                }
                else
                    return (Dictionary<string, int>)Session["RowCountInSection"];
            }
            set
            {
                Session["RowCountInSection"] = value; 
            }
        }
        Models.SubjectsItem _si
        {
            get { return (Models.SubjectsItem)Session["_si"]; }
            set { Session["_si"] = value; }
        }
        long _subjectID
        {
            get 
            {
                long temp;
                if (long.TryParse(Convert.ToString(Session["_subjectID"]), out temp))
                    return temp;
                else
                    return 0;
            }
            set { Session["_subjectID"] = value; }
        }
        long _eventID
        {
            get { return Session["_eventID"] == null ? 0 : (long)Session["_eventID"]; }
            set { Session["_eventID"] = value; }
        }
        long _crfID
        {
            get { return Session["_crfID"] == null ? 0 : (long)Session["_crfID"]; }
            set { Session["_crfID"] = value; }
        }

        int answerRowIndex
        {
            get { return Session["answerRowIndex"] == null ? -1 : (int)Session["answerRowIndex"]; }
            set { Session["answerRowIndex"] = value; }
        }
        int closeRowIndex
        {
            get { return Session["closeRowIndex"] == null ? -1 : (int)Session["closeRowIndex"]; }
            set { Session["closeRowIndex"] = value; }
        }
        bool readOnly
        {
            get
            {
                return Session["readOnly"] == null ? false : (bool)Session["readOnly"];
            }
            set { Session["readOnly"] = value; }
        }

        bool editing
        {
            get
            {
                return Session["editing"] == null ? false : (bool)Session["editing"];
            }
            set { Session["editing"] = value; }
        }

        bool mpeFirstLoad
        {
            get
            {
                return Session["mpeFirstLoad"] == null ? true : (bool)Session["mpeFirstLoad"];
            }
            set { Session["mpeFirstLoad"] = value; }
        }

        int[] queryStatistic = new int[3];


        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                mpeFirstLoad = true;
                GetInfoFromRequest();
                _crf = CRFR.SelectByID(_crfID);
                _subject = SR.SelectByID(_subjectID);
                _sCRF = SCR.SelectByID(_subjectID, _eventID, _crfID);
                Models.Event _event = ER.SelectByID(_eventID);
                ConfigRedirectButtons();
                lbInfo.Text = string.Format(Resources.LocalizedText.Subject_Title_CRF, _subject.Number, _event.Name, string.IsNullOrWhiteSpace(_crf.RussianName) ? _crf.Name : _crf.RussianName);
                RowCountInSection = new Dictionary<string, int>();
                answerRowIndex = -1;
                closeRowIndex = -1;
                if (Request.Cookies["activeTabIndex"] != null)
                    Request.Cookies["activeTabIndex"].Value = "0";
                editing = false;
                _mpe = null;
            }
            if (_subject.IsDeleted 
                || _subject.IsLock
                || _subject.IsStopped
                || ASR.SelectByID(Core.Core.APP_STATUS).Value == Core.AppStatus.Disable.ToString() 
                || !(User.IsInRole(Core.Roles.Investigator.ToString()) 
                || User.IsInRole(Core.Roles.Principal_Investigator.ToString())) 
                || (_sCRF != null && (_sCRF.IsEnd || _sCRF.IsApproved || _sCRF.IsCheckAll)))
            {
                if (!editing)
                {
                    readOnly = true;
                }
                else
                {
                    readOnly = false;
                    btnEdit.Visible = false;
                }
            }
            else
            {
                readOnly = false;
            }
            Title = string.Format(Resources.LocalizedText.Subject_, _subject.Number);

            LoadForm(_crf);

            string mpeName = GetMPENameFromRequest();
            if (mpeName != null && mpeFirstLoad)
            {
                long? queryID = GetQueryIDFromRequest();
                    if(queryID != null)
                    {
                        var query = NR.SelectByID(queryID);
                        if(query!=null)
                            LoadMessages(query);
                    }

                LoadMPE("mpe_" + mpeName);
                divMessageLog.Style.Add("display","block");
                mpeFirstLoad = false;
            }

            ConfigActionButtons();
        }

        /// <summary>
        /// Настройка видимости кнопок переходов
        /// </summary>
        void ConfigRedirectButtons()
        {
            #region buttonGoSubjects
            List<Models.Subject> subjectsInCenter = SR.GetManyByFilter(x=>x.MedicalCenterID == _subject.MedicalCenterID).OrderBy(x => x.SubjectID).ToList();
            if (subjectsInCenter.Count > 0)
            {
                int currentSubjectIndex = subjectsInCenter.FindIndex(x => x.SubjectID == _subjectID);
                if (currentSubjectIndex == 0)
                    btnPrevSubject.Visible = false;
                else
                {
                    string str = "checkChanged('" + ResolveUrl(string.Format("~/Subjects/{0}/{1}/{2}", subjectsInCenter[currentSubjectIndex - 1].SubjectID, _eventID, _crfID)) + "')";
                    btnPrevSubject.Attributes.Add("onclick", str);
                    string subjectNumber = subjectsInCenter[currentSubjectIndex - 1].Number;
                    btnPrevSubject.ToolTip = "К предыдущему субъекту (" + subjectNumber + ")";
                }
                if (currentSubjectIndex == subjectsInCenter.Count - 1)
                    btnNextSubject.Visible = false;
                else
                {
                    string str = "checkChanged('" + ResolveUrl(string.Format("~/Subjects/{0}/{1}/{2}", subjectsInCenter[currentSubjectIndex + 1].SubjectID, _eventID, _crfID)) + "')";
                    btnNextSubject.Attributes.Add("onclick", str);
                    string subjectNumber = subjectsInCenter[currentSubjectIndex + 1].Number;
                    btnNextSubject.ToolTip = "К следующему субъекту (" + subjectNumber + ")";

                }
            }
            else
            {
                btnNextSubject.Visible = false;
                btnPrevSubject.Visible = false;
            }
            #endregion

            Models.CRFInEvent CIE = CIER.SelectByID(_crfID, _eventID);
            List<Models.Event> events = ER.SelectAll().OrderBy(x => x.Position).ToList();
            List<Models.CRFInEvent> CIEs = CIER.GetManyByFilter(x => x.EventID == _eventID).OrderBy(x => x.Position).ToList();
            int currentEventIndex = events.IndexOf(events.Find(x => x.EventID == _eventID));
            if (CIEs.Count > 0)
            {
                int currentCRFIndex = CIEs.FindIndex(x => x.EventID == _eventID && x.CRFID == _crfID);
                if (currentCRFIndex == 0) //Если ИРК первая в списке
                {
                    if (currentEventIndex != 0) //если событие не первое в списке
                    {
                        long prevEventID = events[currentEventIndex - 1].EventID;
                        List<Models.CRFInEvent> cies = CIER.GetManyByFilter(x => x.EventID == prevEventID).OrderBy(x => x.Position).ToList();
                        if (cies.Count > 0)
                        {
                            btnPrevCRFInEvent.Attributes.Add("onclick", "checkChanged('" + ResolveClientUrl(string.Format("~/Subjects/{0}/{1}/{2}", _subjectID, prevEventID, cies.Last().CRFID)) + "')");
                            btnPrevCRFInEvent.ToolTip = "К предыдущему событию (" + events[currentEventIndex - 1].Name + ")";
                        }
                        else
                            btnNextCRFInEvent.Visible = false;
                    }
                    else
                        btnPrevCRFInEvent.Visible = false;
                }
                else
                {
                    btnPrevCRFInEvent.Attributes.Add("onclick", "checkChanged('" + ResolveClientUrl(string.Format("~/Subjects/{0}/{1}/{2}", _subjectID, _eventID, CIEs[currentCRFIndex - 1].CRFID)) + "')");
                    string crfName = string.IsNullOrWhiteSpace(CIEs[currentCRFIndex - 1].CRF.RussianName) ? CIEs[currentCRFIndex - 1].CRF.Name : CIEs[currentCRFIndex - 1].CRF.RussianName;
                    btnPrevCRFInEvent.ToolTip = "К предыдущей форме (" + crfName + ")";
                }
                if (currentCRFIndex == CIEs.Count - 1)
                {
                    if (currentEventIndex != events.Count-1) //если событие не первое в списке
                    {
                        long nextEventID = events[currentEventIndex + 1].EventID;
                        List<Models.CRFInEvent> cies = CIER.GetManyByFilter(x => x.EventID == nextEventID).OrderBy(x => x.Position).ToList();
                        if (cies.Count > 0)
                        {
                            btnNextCRFInEvent.Attributes.Add("onclick", "checkChanged('" + ResolveClientUrl(string.Format("~/Subjects/{0}/{1}/{2}", _subjectID, nextEventID, cies.First().CRFID)) + "')");
                            btnNextCRFInEvent.ToolTip = "К следующему событию (" + events[currentEventIndex + 1].Name + ")";
                        }
                        else
                            btnNextCRFInEvent.Visible = false;
                    }
                    else
                        btnNextCRFInEvent.Visible = false;
                }
                else
                {
                    btnNextCRFInEvent.Attributes.Add("onclick", "checkChanged('" + ResolveUrl(string.Format("~/Subjects/{0}/{1}/{2}", _subjectID, _eventID, CIEs[currentCRFIndex + 1].CRFID)) + "')");
                    string crfName = string.IsNullOrWhiteSpace(CIEs[currentCRFIndex + 1].CRF.RussianName) ? CIEs[currentCRFIndex + 1].CRF.Name : CIEs[currentCRFIndex + 1].CRF.RussianName;
                    btnNextCRFInEvent.ToolTip = "К следующей форме (" + crfName + ")";
                }

            }
            else
            {
                btnPrevCRFInEvent.Visible = false;
                btnNextCRFInEvent.Visible = false;
            }
        }


        /// <summary>
        /// Настройка видимости кнопок действий
        /// </summary>
        void ConfigActionButtons()
        {
            btnEnd.Visible = btnEdit.Visible = btnApproved.Visible = btnCheckAll.Visible = false;
            btnEdit.Text = Resources.LocalizedText.Edit;
            HttpCookie cookie = Request.Cookies["activeTabIndex"];
            int tabIndex = 0;
            if (cookie != null)
            {
                string sTabIndex = cookie.Value;
                if (!int.TryParse(sTabIndex, out tabIndex))
                    tabIndex = 0;
            }
            if (_sCRF != null && (tcCRF.Tabs.Count==1 || tabIndex == tcCRF.Tabs.Count-1) && !(_sCRF.IsDeleted || _sCRF.IsLock || _sCRF.IsStopped))
            {
                string[] userRoles = System.Web.Security.Roles.GetRolesForUser();
                if ((userRoles.Contains(Core.Roles.Investigator.ToString()) || userRoles.Contains(Core.Roles.Principal_Investigator.ToString())) && !_sCRF.IsEnd)
                    btnEnd.Visible = true;
                else if (userRoles.Contains(Core.Roles.Investigator.ToString()))
                {
                    if (_sCRF.IsEnd && !_sCRF.IsApproved && !(_sCRF.IsLock ||  _sCRF.IsStopped || _sCRF.IsDeleted))
                        btnEdit.Visible = true;
                }
                else if (userRoles.Contains(Core.Roles.Monitor.ToString()))
                {
                    if (_sCRF.IsEnd && !_sCRF.IsCheckAll)
                    {
                        List<SubjectsItem> sis = SIR.GetManyByFilter(x => x.EventID == _eventID && x.SubjectID == _subjectID && x.CRFID == _crfID).ToList();
                        //Если есть не закрытые Query
                        if (sis == null || sis.Count == 0 || sis.All(x => x.Queries.Count == 0 || x.Queries.All(y => y.Status == Core.QueryStatus.Closed)))
                        {
                            btnCheckAll.Visible = true;
                        }
                    }
                    else if(_sCRF.IsCheckAll && !_sCRF.IsApproved)
                    {
                        btnEdit.Visible = true;
                        btnEdit.Text = Resources.LocalizedText.CancelSDV;
                    }
                }
                else if (userRoles.Contains(Core.Roles.Principal_Investigator.ToString()))
                {
                    if (_sCRF.IsCheckAll && !_sCRF.IsApproved)
                        btnApproved.Visible = true;
                    
                    btnEdit.Visible = true;
                }
            }
            if (btnEdit.Visible && btnEdit.Parent.FindControl("btnUnvisibleEdit")==null)
            {
                LinkButton btnUnvisible = new LinkButton();
                btnUnvisible.ID = "btnUnvisibleEdit";
                btnUnvisible.CssClass = "buttonHide";
                var p = btnEdit.Parent;
                p.Controls.AddAt(p.Controls.IndexOf(btnEdit), btnUnvisible);

                AjaxControlToolkit.ModalPopupExtender mpe = new AjaxControlToolkit.ModalPopupExtender();
                mpe.TargetControlID = btnUnvisible.ID;
                mpe.PopupControlID = "pnlEditReason";
                mpe.CancelControlID = "btnCloseEditReason";
                mpe.ID = "mpeEditReason";
                p.Controls.Add(mpe);
            }
            if (btnApproved.Visible && btnEdit.Parent.FindControl("btnUnvisibleLogin") == null)
            {
                LinkButton btnUnvisible = new LinkButton();
                btnUnvisible.ID = "btnUnvisibleLogin";
                btnUnvisible.CssClass = "buttonHide";
                var p = btnEdit.Parent;
                p.Controls.AddAt(p.Controls.IndexOf(btnApproved), btnUnvisible);

                AjaxControlToolkit.ModalPopupExtender mpe = new AjaxControlToolkit.ModalPopupExtender();
                mpe.TargetControlID = btnUnvisible.ID;
                mpe.PopupControlID = "pnlLogin";
                mpe.CancelControlID = "btnCloseLogin";
                mpe.ID = "mpeLogin";
                p.Controls.Add(mpe);
            }
            if(_sCRF != null)
                if (_sCRF.IsDeleted)
                {
                    lblStatus.Text = string.Format("{0}", Resources.LocalizedText.Removed);
                    btnStatus.Attributes.Add("class", "ActionIc Delete");
                }
                else if (_subject.IsLock)
                {
                    lblStatus.Text = string.Format("{0}", Resources.LocalizedText.Locked);
                    btnStatus.Attributes.Add("class", "ActionIc Lock");
                }
                else if (_sCRF.IsStopped)
                {
                    lblStatus.Text = string.Format("{0}", Resources.LocalizedText.Stopped);
                    btnStatus.Attributes.Add("class", "ActionIc Stopped");
                }
                else if (_sCRF.IsApproved)
                {
                    lblStatus.Text = lblStatus.Text = string.Format("{0}", Resources.LocalizedText.Signed);
                    btnStatus.Attributes.Add("class", "ActionIc Approve");
                }
                else if (_sCRF.IsCheckAll)
                {
                    lblStatus.Text = lblStatus.Text = string.Format("{0}", Resources.LocalizedText.SDVComplete);
                    btnStatus.Attributes.Add("class", "ActionIc CheckAll");
                }
                else if (_sCRF.IsEnd)
                {
                    lblStatus.Text = lblStatus.Text = string.Format("{0}", Resources.LocalizedText.Completed);
                    btnStatus.Attributes.Add("class", "ActionIc End");
                }
            if (lblStatus.Text == "")
                btnStatus.Visible = false;
            else
                btnStatus.Visible = true;
        }

        #region GetInfoFromRequest
        void GetInfoFromRequest()
        {
            GetSubjectIDFromRequest();
            GetCrfIDFromRequest();
            GetEventIDFromRequest();
        }

        private void GetSubjectIDFromRequest()
        {
            long subjectId;
            string reqValue = (string)RouteData.Values["subjectid"] ?? Request.QueryString["subjectid"];
            if(reqValue != null && long.TryParse(reqValue, out subjectId) && subjectId>0)
            {
                _subjectID = subjectId;
            }
            else
            {
                throw new ArgumentNullException("Не указан ID субъекта");
            }
        }
        private void GetCrfIDFromRequest()
        {
            long CRFID;
            string reqValue = (string)RouteData.Values["crfid"] ?? Request.QueryString["crfid"];
            if (reqValue != null && long.TryParse(reqValue, out CRFID) && CRFID > 0)
            {
                _crfID = CRFID;
            }
            else
            {
                throw new ArgumentNullException("Не указан ID CRF");
            }
        }
        private void GetEventIDFromRequest()
        {
            long CRFID;
            string reqValue = (string)RouteData.Values["eventid"] ?? Request.QueryString["eventid"];
            if (reqValue != null && long.TryParse(reqValue, out CRFID) && CRFID > 0)
            {
                _eventID = CRFID;
            }
            else
            {
                throw new ArgumentNullException("Не указан ID события");
            }
        }
        private string GetMPENameFromRequest()
        {
            return (string)RouteData.Values["mpename"] ?? Request.QueryString["mpename"];
        }
        private long? GetQueryIDFromRequest()
        {
            long queryID;
            string reqValue = (string)RouteData.Values["q"] ?? Request.QueryString["q"];
            if (reqValue != null && long.TryParse(reqValue, out queryID) && queryID > 0)
            {
                return queryID;
            }
            return null;
            
        }

        #endregion

        void LoadForm(Models.CRF _crf)
        {
            tcCRF.Tabs.Clear();
            foreach (Models.CRF_Section section in _crf.Sections)
            {
                if(!RowCountInSection.ContainsKey(section.Label))
                {
                    RowCountInSection.Add(section.Label, 0);
                }
                AjaxControlToolkit.TabPanel tp = new AjaxControlToolkit.TabPanel();
                tp.HeaderText = section.Title;
                tp.ID = "tp_" + section.Label;

                int columnCount = section.Items.Max(x => x.ColumnNumber);

                Table tableUngrouped = new Table(); //таблица итемов без группы
                tableUngrouped.CssClass = "normTable";
                TableRow trUngrouped = new TableRow(); //строка итемов

                Table tableGrouped = new Table();           //широкая таблица
                tableGrouped.CssClass = "bigTable";
                TableRow trGroupedHeaders = new TableRow(); //заголовки
                TableRow trsGroupedValues = new TableRow(); //значения


                Button btnSave = new Button();  //Кнопка сохранить
                btnSave.ID = "btnSave";
                btnSave.Click += btnSave_Click;
                btnSave.Text = Resources.LocalizedText.Save;
                btnSave.ValidationGroup = section.Label.Replace(" ","");

                Button btnAdd = new Button();   //кнопка добавить
                btnAdd.ID = "btnAdd";
                btnAdd.Click +=btnAdd_Click;
                btnAdd.Text = Resources.LocalizedText.Add;
                
                List<CRF_Item> ungroupedItems = section.Items
                    .Where(x => x.Ungrouped)
                    .OrderBy(x=>x.CRF_ItemID).ToList(); //итемы без группы

                List<CRF_Item> groupedItems = section.Items
                    .Where(x => !x.Ungrouped)
                    .OrderBy(x=>x.CRF_ItemID).ToList(); //итемы в группе

                #region NoGroup
                
                for (int i = 0; i < ungroupedItems.Count;i++ )
                {
                    var item = ungroupedItems[i];

                    if (!string.IsNullOrWhiteSpace(item.Header)) //Header
                    {
                        Label header = new Label();
                        header.Text = item.Header;
                        TableRow _tr = new TableRow();
                        TableCell _tc = new TableCell();
                        header.CssClass = "CRFItemHeader";
                        _tc.ColumnSpan = columnCount;
                        _tc.Controls.Add(header);
                        _tr.Cells.Add(_tc);
                        tableUngrouped.Rows.Add(_tr);
                    }
                    if (!string.IsNullOrWhiteSpace(item.Subheader)) //subHeader
                    {
                        Label subheader = new Label();
                        subheader.Text = item.Subheader;
                        TableRow _tr = new TableRow();
                        TableCell _tc = new TableCell();
                        subheader.CssClass = "CRFItemsubHeader";
                        _tc.Controls.Add(subheader);
                        _tr.Cells.Add(_tc);
                        tableUngrouped.Rows.Add(_tr);
                    }
                    TableCell tc = new TableCell();
                    if (!string.IsNullOrWhiteSpace(item.LeftItemText)) //LeftItemText
                    {
                        Label LIT = new Label();
                        LIT.Text = item.LeftItemText;
                        LIT.CssClass = "CRFLeftItem";
                        tc.CssClass = "CRFCellLeftItem";
                        tc.Controls.Add(LIT);
                    }

                    GetAddedControl(item, ref tc,-1,null); //запись в tc нужных контролов

                    if (!string.IsNullOrWhiteSpace(item.Units)) //Units
                    {
                        Label Units = new Label();
                        Units.Text = item.Units;
                        tc.Controls.Add(Units);
                    }
                    if (!string.IsNullOrWhiteSpace(item.RightItemText)) //RightItemText
                    {
                        Label RIT = new Label();
                        RIT.Text = item.RightItemText;
                        tc.Controls.Add(RIT);
                    }

                    if (item.Required) //если обязательное
                    {
                        RequiredFieldValidator RFV = new RequiredFieldValidator();
                        RFV.ControlToValidate = item.Identifier;
                        RFV.Display = ValidatorDisplay.Dynamic;
                        RFV.ValidationGroup = section.Label.Replace(" ","");
                        RFV.ErrorMessage = Resources.LocalizedText.RequiredField;
                        RFV.CssClass = "field-validation-error";
                        tc.Controls.Add(RFV);
                    }

                    trUngrouped.Cells.Add(tc);

                    if (i < ungroupedItems.Count - 1) //если элемент не последний
                    {
                        if (section.Items[i + 1].ColumnNumber <= section.Items[i].ColumnNumber)
                        {
                            tableUngrouped.Rows.Add(trUngrouped);
                            trUngrouped = new TableRow();
                        }

                    }
                    else //иначе
                    {
                        tableUngrouped.Rows.Add(trUngrouped);
                    }
                }

                if (tableUngrouped.Rows.Count > 0)
                {
                    tp.Controls.Add(tableUngrouped);
                    ScriptManager.GetCurrent(this).RegisterPostBackControl(btnSave);
                }
                #endregion

                #region Group
                if (groupedItems.Count > 0)
                {
                    List<SubjectsItem> _items = SIR
                        .GetManyByFilter(x => x.SubjectID == _subjectID && x.EventID == _eventID && x.CRFID == _crfID && x.IsGrouped).ToList();

                    int rowCount = _items.Count > 0 ? _items.Max(x => x.IndexID) : 0;

                    int groupedIndex = rowCount+1; //индекс строки в группе
                    TableRow[] addedRows = new TableRow[rowCount]; //добавленные строки
                    for (int i = 0; i < rowCount; i++)
                    {
                        addedRows[i] = new TableRow();
                    }

                    for (int i = 0; i < groupedItems.Count; i++)
                    {
                        var item = groupedItems[i];

                        TableCell tc = new TableCell();
                        if (!string.IsNullOrWhiteSpace(item.LeftItemText)) //LeftItemText
                        {
                            Label LIT = new Label();
                            LIT.Text = item.LeftItemText;
                            LIT.CssClass = "CRFLeftItem";
                            tc.CssClass = "CRFCellLeftItem";
                            tc.Controls.Add(LIT);
                        }
                        trGroupedHeaders.Cells.Add(tc);
                        trGroupedHeaders.CssClass = "bigHeight";
                        #region readCurrentValues

                        List<SubjectsItem> SIs = SIR
                            .GetManyByFilter(x => x.SubjectID == _subjectID && x.EventID == _eventID && x.CRFID == _crfID && x.ItemID == item.CRF_ItemID)
                            .OrderBy(x => x.IndexID).ToList(); //значения в этом столбце

                        if (SIs.Count > 0)
                        {
                            for (int k = 0; k < SIs.Count; k++)
                            {
                                tc = new TableCell();
                                GetAddedControl(item, ref tc, SIs[k].IndexID, SIs[k].Value);
                                addedRows[k].Cells.Add(tc);
                            }
                        }

                        #endregion

                        ////////////////////Поле ввода данных/////////////////
                        tc = new TableCell();
                        GetAddedControl(item, ref tc, groupedIndex, null);
                        if(!readOnly)
                            trsGroupedValues.Cells.Add(tc);
                        //////////////////////////////////////////////////
                    }

                    tableGrouped.Rows.Add(trGroupedHeaders); //добавление шапки
                    if(addedRows.Length>0)
                        tableGrouped.Rows.AddRange(addedRows); //введенные данные
                    tableGrouped.Rows.Add(trsGroupedValues); //добавление пустых полей

                    //////////////////////отрисовка добавленных строк/////////////////////////
                    TableRow addingTR = new TableRow();
                    for (int j = 0; j < RowCountInSection[section.Label]; j++)
                    {
                        for (int i = 0; i < groupedItems.Count; i++)
                        {
                            TableCell tc = new TableCell();
                            GetAddedControl(groupedItems[i], ref tc, groupedIndex+j+1, null);
                            addingTR.Cells.Add(tc);

                            if (groupedItems[i].Required) //если обязательное
                            {
                                RequiredFieldValidator RFV = new RequiredFieldValidator();
                                RFV.ControlToValidate = groupedItems[i].Identifier + "_" + (groupedIndex + j + 1).ToString();
                                RFV.Display = ValidatorDisplay.Dynamic;
                                RFV.ErrorMessage = "Данное поле обязательно для заполнения";
                                RFV.CssClass = "field-validation-error";
                                tc.Controls.Add(RFV);
                            }
                        }
                        tableGrouped.Rows.AddAt(groupedIndex + j + 1, addingTR);
                        addingTR = new TableRow();
                    }

                    ////////////////////////////////////////////////////

                    ////////////////кнопка Добавить/////////////////////
                    if (!readOnly)
                    {
                        TableCell tcg = new TableCell();
                        trGroupedHeaders = new TableRow();
                        tcg.Controls.Add(btnAdd);
                        trGroupedHeaders.Cells.Add(tcg);
                        tableGrouped.Rows.Add(trGroupedHeaders);
                        ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnAdd);
                    }
                    ///////////////////////////////////////////////////
                }
                #endregion

                tp.ScrollBars = ScrollBars.Auto; //скролбар
                tcCRF.ScrollBars = ScrollBars.Auto; //скролбар
                tp.Controls.Add(tableGrouped); //таблицу в tabPanel
                Panel panel = new Panel();
                panel.HorizontalAlign = HorizontalAlign.Right;
                panel.Controls.Add(btnSave);
                if(!readOnly)
                    tp.Controls.Add(panel);
                tcCRF.Tabs.Add(tp); //панель в tabConteiner

                tQueryStatistic.Rows[2].Cells[0].InnerHtml = queryStatistic[0].ToString();
                tQueryStatistic.Rows[2].Cells[1].InnerHtml = queryStatistic[1].ToString();
                tQueryStatistic.Rows[2].Cells[2].InnerHtml = queryStatistic[2].ToString();
                tQueryStatistic.Rows[2].Cells[3].InnerHtml = queryStatistic.Sum().ToString();

            }
        }

        /// <summary>
        /// Получение модального окна с Query для поля.
        /// </summary>
        void GetModalPopup(ref TableCell tc, string id,CRF_Item item, int index)
        {
            var si = SIR.SelectByID(_subjectID, _eventID, _crfID, item.CRF_ItemID, index);
            List<Models.Query> queries = GetQueries(si);
            queryStatistic[0] += queries.Count(x => x.Status == Core.QueryStatus.New);
            queryStatistic[1] += queries.Count(x => x.Status == Core.QueryStatus.Updated);
            queryStatistic[2] += queries.Count(x => x.Status == Core.QueryStatus.Closed);
            Button btnNotes = new Button();
            btnNotes.ID = "btnNotes_" + id;
            btnNotes.Click += btnNotes_Click;
            if (queries.Count == 0)
                btnNotes.CssClass = "notes";
            else if(queries.Count>0 && queries.Any(x=> x.Status == Core.QueryStatus.New))
                btnNotes.CssClass = "notes notesNew";
            else if (queries.Count > 0 && queries.Any(x => x.Status == Core.QueryStatus.Updated))
                btnNotes.CssClass = "notes notesUpdate";
            else
                btnNotes.CssClass = "notes notesClose";
            tc.Controls.Add(btnNotes);

            LinkButton btnUnvisible = new LinkButton();
            btnUnvisible.ID = "btnUnvisible_" + id;
            btnUnvisible.CssClass = "buttonHide";
            tc.Controls.Add(btnUnvisible);

            AjaxControlToolkit.ModalPopupExtender mpe = new AjaxControlToolkit.ModalPopupExtender();
            mpe.TargetControlID = btnUnvisible.ID;
            mpe.PopupControlID = "pnlModalPopup";
            mpe.CancelControlID = "btnCloseWindow";
            mpe.ID = "mpe_" + id;
            tc.Controls.Add(mpe);
        }

        //Получение уже добавленных контролов
        void GetAddedControl(CRF_Item item, ref TableCell tc, int index, string value)
        {
            Control addedControl = new Control();
            string id = index > 0 ? item.Identifier + "_" + index : item.Identifier;

            value = value == null ? ReadCRFItemValue(item, index) : value;

            switch (item.ResponseType)
            {
                case Core.ResponseType.Text:
                    {
                        TextBox tb = new TextBox();
                        #region switch item.DateType
                        switch (item.DataType)
                        {
                            case Core.DataType.INT:
                                {
                                    tb.TextMode = TextBoxMode.Number;
                                    tb.Attributes.Add("min", "0");
                                    break;
                                }
                            case Core.DataType.REAL:
                                {
                                    AjaxControlToolkit.FilteredTextBoxExtender FTBE = new AjaxControlToolkit.FilteredTextBoxExtender();
                                    FTBE.TargetControlID = id;
                                    FTBE.ValidChars = ".";
                                    FTBE.FilterType = AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.Custom;
                                    tc.Controls.Add(FTBE);
                                    break;
                                }
                            case Core.DataType.DATE:
                                {
                                    AjaxControlToolkit.CalendarExtender CE = new AjaxControlToolkit.CalendarExtender();
                                    CE.TargetControlID = id;
                                    CE.Format = "dd.MM.yyyy";
                                    tc.Controls.Add(CE);

                                    AjaxControlToolkit.FilteredTextBoxExtender FTBE = new AjaxControlToolkit.FilteredTextBoxExtender();
                                    FTBE.TargetControlID = id;
                                    FTBE.ValidChars = ".";
                                    FTBE.FilterType = AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.Custom;
                                    tc.Controls.Add(FTBE);
                                    break;
                                }
                        }
                        #endregion
                        if (value != null)
                            tb.Text = value;
                        tb.Attributes.Add("onchange", @"setChanged()");
                        tb.Enabled = !readOnly;
                        addedControl = tb;
                        break;
                    }
                case Core.ResponseType.Textarea:
                    {
                        TextBox tb = new TextBox();
                        tb.TextMode = TextBoxMode.MultiLine;
                        if (value != null)
                            tb.Text = value;
                        tb.Attributes.Add("onchange", @"setChanged()");
                        tb.Enabled = !readOnly;
                        addedControl = tb;
                        break;
                    }
                case Core.ResponseType.MultiSelect:
                case Core.ResponseType.Checkbox:
                    {
                        CheckBoxList cb = new CheckBoxList();
                        cb.Items.AddRange(GetListItems(item).ToArray());
                        cb.CssClass = "response-Item " + item.ResponseLayout;
                        if (value != null)
                        {
                            string[] values = value.Split(',');
                            foreach (ListItem li in cb.Items)
                            {
                                if (values.Contains(li.Value))
                                    li.Selected = true;
                                li.Attributes.Add("onchange", @"setChanged()");
                            }
                        }
                        cb.Enabled = !readOnly;
                        addedControl = cb;
                        break;
                    }
                case Core.ResponseType.Radio:
                    {
                        RadioButtonList rb = new RadioButtonList();
                        rb.Items.AddRange(GetListItems(item).ToArray());
                        rb.CssClass = "response-Item " + item.ResponseLayout;
                        if (value != null)
                            rb.SelectedValue = value;
                        rb.Attributes.Add("onchange", @"setChanged()");
                        rb.Enabled = !readOnly;
                        addedControl = rb;
                        break;
                    }
                case Core.ResponseType.SingleSelect:
                    {
                        DropDownList ddl = new DropDownList();
                        ddl.Items.AddRange(GetListItems(item).ToArray());
                        if (value != null)
                            ddl.SelectedValue = value;
                        ddl.Enabled = !readOnly;
                        addedControl = ddl;
                        break;
                    }
            }
            addedControl.ID = id;

            tc.Controls.Add(addedControl);

            if (item.Required) //если обязательное
            {
                Label lbl = new Label();
                lbl.Text = "*";
                tc.Controls.Add(lbl);
            }
            if (value != null)
            {
                GetModalPopup(ref tc, id,item,index);
            }
        }

        string ReadCRFItemValue(CRF_Item item,int index)
        {
            SubjectsItem si = SIR.SelectByID(_subjectID, _eventID, _crfID, item.CRF_ItemID, -1);
            if (si != null)
                return si.Value;
            else
                return null;
        }

        List<ListItem> GetListItems(Models.CRF_Item item)
        {
            List<ListItem> listItems = new List<ListItem>();
            string[] responseOptionText = item.ResponseOptionText.Replace("\\,", "^^").Replace("\\","").Split(',');
            string[] responseValues = item.ResponseValuesOrCalculation.Split(',');
            for (int i = 0; i < responseOptionText.Length; i++)
            {
                ListItem li = new ListItem(responseOptionText[i].Replace("^^", ","), responseValues[i]);
                listItems.Add(li);
            }
            return listItems;
        }

        void GetInfo(Control form, Models.CRF_Section section)
        {

            List<CRF_Item> groupedItems = section.Items
                .Where(x => !x.Ungrouped)
                .OrderBy(x => x.CRF_ItemID).ToList(); //итемы в группе

            List<CRF_Item> ungroupedItems = section.Items
                .Where(x => x.Ungrouped)
                .OrderBy(x => x.CRF_ItemID).ToList(); //итемы без группы

            #region foreach UngroupedItems
            foreach (var item in ungroupedItems)
            {
                Control _control;
                _control = form.FindControl(item.Identifier);
                string value = GetValueFromControl(_control);
                SubjectsItem si = SIR.SelectByID(_subjectID, _eventID, _crfID, item.CRF_ItemID,-1);
                Models.Audit audit = new Models.Audit();

                if (si == null)
                {
                    si = new SubjectsItem();
                    si.SubjectID = _subjectID;
                    si.EventID = _eventID;
                    si.CRFID = _crfID;
                    si.ItemID = item.CRF_ItemID;
                    si.IsGrouped = false;
                    si.IndexID = -1;
                    si.Value = value;
                    si.CreatedBy = User.Identity.Name;
                    si = SIR.Create(si);
                    SIR.Save();

                    audit.NewValue = value;
                }
                else
                {
                    if (si.Value == value)
                        continue;

                    audit.OldValue = si.Value;
                    audit.NewValue = value;
                    
                    si.Value = value;
                    si.CreatedBy = User.Identity.Name;
                    SIR.Update(si);
                }

                audit.UserName = User.Identity.Name;
                audit.UserID = (Guid)System.Web.Security.Membership.GetUser(User.Identity.Name).ProviderUserKey;
                audit.SubjectID = _subjectID;
                audit.EventID = _eventID;
                audit.CRFID = _crfID;
                audit.ItemID = item.CRF_ItemID;
                audit.IndexID = -1;
                audit.ActionDate = DateTime.Now;
                audit.FieldName = item.Name;
                audit.ActionType = Core.AuditActionType.SubjectItem;
                audit.ChangesType = Core.AuditChangesType.Create;
                AR.Create(audit);

                AR.Save();
                SIR.Save();
            }
            #endregion

            if (groupedItems.Count>0)
            {
                Control _control;
                _control = form.FindControl(groupedItems[0].Identifier + "_1");
                Table groupedTable = _control.Parent.Parent.Parent as Table;
                int valuesRowCount = groupedTable.Rows.Count - 2;

                for (int i = 0; i < valuesRowCount; i++)
                {
                    string[] values = new string[groupedItems.Count];
                    bool allNull = true;

                    for (int j = 0; j < groupedItems.Count; j++)
                    {
                        CRF_Item item = groupedItems[j];
                        _control = form.FindControl(item.Identifier + "_" + (i + 1).ToString());
                        values[j] = GetValueFromControl(_control);
                        if (!string.IsNullOrWhiteSpace(values[j]))
                            allNull = false;
                    }
                    if (allNull)
                        continue;

                    for(int j =0;j<groupedItems.Count;j++)
                    {
                        CRF_Item item = groupedItems[j];
                        string value = values[j];
                        SubjectsItem si = SIR.SelectByID(_subjectID, _eventID, _crfID, item.CRF_ItemID, i + 1);
                        Models.Audit audit = new Models.Audit();

                        if (si == null)
                        {
                            si = new SubjectsItem();
                            si.SubjectID = _subjectID;
                            si.EventID = _eventID;
                            si.CRFID = _crfID;
                            si.ItemID = item.CRF_ItemID;
                            si.Value = value;
                            si.IndexID = i + 1;
                            si.IsGrouped = true;
                            si.CreatedBy = User.Identity.Name;
                            SIR.Create(si);
                            SIR.Save();

                            audit.NewValue = value;
                        }
                        else
                        {
                            if (si.Value == value)
                                continue;

                            audit.OldValue = si.Value;
                            audit.NewValue = value;

                            si.Value = value;
                            si.CreatedBy = User.Identity.Name;
                            SIR.Update(si);

                        }

                        audit.UserName = User.Identity.Name;
                        audit.UserID = (Guid)System.Web.Security.Membership.GetUser(User.Identity.Name).ProviderUserKey;
                        audit.SubjectID = _subjectID;
                        audit.EventID = _eventID;
                        audit.CRFID = _crfID;
                        audit.ItemID = item.CRF_ItemID;
                        audit.IndexID = si.IndexID;
                        audit.ActionDate = DateTime.Now;
                        audit.FieldName = item.Name;
                        audit.ActionType = Core.AuditActionType.SubjectItem;
                        audit.ChangesType = Core.AuditChangesType.Update;
                        AR.Create(audit);

                        SIR.Save();
                        AR.Save();
                    }
                }
            }
        }

        #region AutoQuery
        void GetQuery(CRF_Section section)
        {
            foreach(var crf_item in section.Items)
            {
                //список правил данного итема
                List<Models.Rule> rulesForItem = crf_item.Rules.Where(x=> x.EventID == null || x.EventID == _eventID).ToList(); 
                
                //список созданных замечаний
                List<Models.Query> itemNotes = crf_item.Notes
                    .Where(x=> 
                        x.EventID == _eventID
                        && x.SubjectID == _subjectID
                        && (x.Status== Core.QueryStatus.New || x.Status== Core.QueryStatus.Updated))
                        .ToList();

                rulesForItem = rulesForItem.Where(x => !itemNotes.Any(y => y.Messages[0].Text == x.ErrorMessage)).ToList();
                //текущая цель (событие, ИРК, группа, итем)
                var currentItemTarget = new Core.Rule.Target(){EventID= _eventID, CRFID=crf_item.CRFID, GroupID=(long)crf_item.GroupID, ItemID= crf_item.CRF_ItemID};
                foreach(var rule in rulesForItem)
                {
                    List<Core.Rule.IToken> ruleTokens = new List<Core.Rule.IToken>();
                    rule.Tokens.ForEach(x=> ruleTokens.Add((Core.Rule.IToken)x));
                    List<Core.Rule.Rule_Error> errorList = new List<Core.Rule.Rule_Error>();
                    //список токенов данного правила
                    Core.Rule.RuleParse.UpdateCrfTokens(ruleTokens, currentItemTarget, errorList);

                    //список итемов данного пациента
                    List<SubjectsItem> subjectsItems = SIR.GetManyByFilter(x=> x.SubjectID==_subjectID && x.EventID==_eventID && x.CRFID==crf_item.CRFID && x.ItemID == crf_item.CRF_ItemID).ToList();

                    foreach (SubjectsItem subjectItem in subjectsItems)
                    {
                        bool isEmpty = false;
                        //////////////////////////////////////////////////////////////////
                        Stack<Core.Rule.IToken> stack = new Stack<Core.Rule.IToken>();
                        foreach (Core.Rule.IToken token in ruleTokens)
                        {
                            bool isError = false;

                            if (token.Type == TokenType.Constant || token.Type == TokenType.Item)
                            {
                                stack.Push(token);
                                continue;
                            }
                            if (token.Type == TokenType.Operation)
                            {
                                if (stack.Count < 2)
                                    throw new ArgumentException("В стеке менее двух элементов. Квери не может быть установлено"+Environment.NewLine+
                                    "Верхний элемент: "
                                    +stack.Peek()== null?"Отсутствует":(stack.Peek().Type.ToString()+" "+stack.Peek().Value.ToString())
                                    +Environment.NewLine
                                    +rule.ErrorMessage 
                                    +Environment.NewLine 
                                    +rule.Expression);
                                else
                                {
                                    IToken t2 = stack.Pop();
                                    IToken t1 = stack.Pop();

                                    bool comparing = TokenValueComparing(t1, t2, currentItemTarget, subjectItem, token.Value,out isError,out isEmpty);
                                    if (isEmpty)
                                        break;
                                    if (isError)
                                        throw new Exception("Ошибка при сравнение двух элементов. Квери не может быть установлено"
                                            +Environment.NewLine
                                            +rule.ErrorMessage 
                                            +Environment.NewLine 
                                            +rule.Expression);
                                    else
                                        stack.Push(new Core.Rule.Token(comparing.ToString(),TokenType.Constant));
                                }
                            }
                        }
                        if (isEmpty)
                            continue;
                        if (stack.Count != 1)
                        {
                            if(stack.Count == 0)
                                throw new Exception("Нет элементов в стеке. Квери не может быть установлено" 
                                            + Environment.NewLine
                                            + rule.ErrorMessage
                                            + Environment.NewLine
                                            + rule.Expression);
                            else
                                throw new Exception("После всех операция в стеке осталось " + stack.Count + " элементов. Квери не может быть установлено" 
                                            + Environment.NewLine
                                            + rule.ErrorMessage
                                            + Environment.NewLine
                                            + rule.Expression);
                        }

                        IToken lastT = stack.Pop();
                        if (lastT.Value == rule.IfExpressionEvaluates.ToString())
                        {
                            Models.Query newNote = new Models.Query();
                            newNote.MedicalCenterID = SR.SelectByID(_subjectID).MedicalCenterID;
                            newNote.CreationDate = DateTime.Now;
                            newNote.SubjectID = _subjectID;
                            newNote.EventID = _eventID;
                            newNote.CRFID = rule.CRFID;
                            newNote.ItemID = rule.ItemID;
                            newNote.IndexID = subjectItem.IndexID; //TEST
                            newNote.From = Resources.LocalizedText.System;
                            newNote.To = subjectItem.CreatedBy;
                            newNote.Type = Core.NoteType.Query;
                            newNote.Header = "Автоматическая заметка №" + (NR.SelectAll().Count() + 1).ToString();
                            newNote = NR.Create(newNote);
                            NR.Save();
                            NR = new Models.Repository.QueryRepository();

                            var qm = new Models.QueryMessage();
                            qm.Text = rule.ErrorMessage;
                            qm.From = Resources.LocalizedText.System;
                            qm.To = subjectItem.CreatedBy;
                            NR.AddMessage(newNote.QueryID,qm,true);
                            NR.Save();
                        }


                        ////////////////////////////////////////////////////////////////////
                    }
                }
            }
        }

        bool TokenValueComparing(IToken t1, IToken t2, Target currentItemTarget, SubjectsItem subjectItem, string operation, out bool isError, out bool isEmpty)
        {
            isError = false;
            isEmpty = false;

            string sV1 = GetTokenRealyValue(t1, currentItemTarget, subjectItem);
            string sV2 = GetTokenRealyValue(t2, currentItemTarget, subjectItem);

            if(string.IsNullOrWhiteSpace(sV1) || string.IsNullOrWhiteSpace(sV2))
            {
                if (operation != "==" && operation != "!=")
                {
                    isEmpty = true;
                    return false;
                }
                else
                {

                }
            }

            double dV1 = 0;
            double dV2 = 0;

            bool isDV1 = double.TryParse(sV1, out dV1);
            bool isDV2 = double.TryParse(sV2, out dV2);

            if(isDV1 ^ isDV2)
            {
                if(!isDV1)
                {
                    sV1 = sV1.Replace(".", ",");
                    isDV1 = double.TryParse(sV1, out dV1);
                }
                else
                {
                    sV2 = sV2.Replace(".", ",");
                    isDV2 = double.TryParse(sV2, out dV2);
                }
            }

            DateTime dtV1 = new DateTime();
            DateTime dtV2 = new DateTime();

            bool isDTV1 = DateTime.TryParse(sV1, out dtV1);
            bool isDTV2 = DateTime.TryParse(sV2, out dtV2);

            switch (operation.ToUpper())
            {
                case "==":
                    {
                        return sV1 == sV2;
                    }
                case "!=":
                    {
                        return sV1 != sV2;
                    }
                case "<":
                    {
                        if (isDV1 && isDV2)
                            return dV1 < dV2;
                        else
                            if (isDTV1 && isDTV2)
                                return dtV1 < dtV2;
                            else
                                isError = true;
                        break;
                    }
                case "<=":
                    {
                        if (isDV1 && isDV2)
                            return dV1 <= dV2;
                        else
                            if (isDTV1 && isDTV2)
                                return dtV1 <= dtV2;
                            else
                                isError = true;
                        break;
                    }
                case ">":
                    {
                        if (isDV1 && isDV2)
                            return dV1 > dV2;
                        else
                            if (isDTV1 && isDTV2)
                                return dtV1 > dtV2;
                            else
                                isError = true;
                        break;
                    }
                case ">=":
                    {

                        if (isDV1 && isDV2)
                            return dV1 >= dV2;
                        else
                            if (isDTV1 && isDTV2)
                                return dtV1 >= dtV2;
                            else
                                isError = true;
                        break;
                    }
                case "OR":
                case "AND":
                    {
                        bool bValue1;
                        bool bValue2;
                        bool isBValue1 = bool.TryParse(sV1, out bValue1);
                        bool isBValue2 = bool.TryParse(sV2, out bValue2);
                        if (!(isBValue1 && isBValue2))
                        {
                            isError = true;
                            break;
                        }
                        else
                            if(operation.ToUpper() == "OR")
                                return bValue1 || bValue2;
                            else if (operation.ToUpper() == "AND")
                                return bValue1 && bValue2;
                        break;
                    }
            }
            return false;
            
        }

        string GetTokenRealyValue(IToken token, Target currentItemTarget,SubjectsItem subjectItem)
        {
            if (token.Type == TokenType.Constant)
                return token.Value;
            else
            {
                CrfToken crfT = token as CrfToken;
                List<SubjectsItem> sis;
                if ((crfT.EventID == null || crfT.EventID == currentItemTarget.EventID) && crfT.CrfID == currentItemTarget.CRFID && crfT.GroupID == currentItemTarget.GroupID)
                {
                    if (crfT.EventID != null)
                        sis = SIR.GetManyByFilter(x => x.EventID == crfT.EventID && x.SubjectID == subjectItem.SubjectID && x.CRFID == crfT.CrfID && x.ItemID == crfT.ItemID && x.IndexID == subjectItem.IndexID).ToList();
                    else
                        sis = SIR.GetManyByFilter(x => x.EventID == currentItemTarget.EventID && x.SubjectID == subjectItem.SubjectID && x.CRFID == crfT.CrfID && x.ItemID == crfT.ItemID && x.IndexID == subjectItem.IndexID).ToList();
                }
                else
                {
                    if (crfT.EventID != null)
                        sis = SIR.GetManyByFilter(x => x.EventID == crfT.EventID && x.SubjectID == subjectItem.SubjectID && x.CRFID == crfT.CrfID && x.ItemID == crfT.ItemID).ToList();
                    else
                        sis = SIR.GetManyByFilter(x => x.EventID == currentItemTarget.EventID && x.SubjectID == subjectItem.SubjectID && x.CRFID == crfT.CrfID && x.ItemID == crfT.ItemID).ToList();
                }

                if (sis.Count == 0)
                    return "";
                else
                    return sis[0].Value;
            }
        }

        #endregion

        string GetValueFromControl(Control control)
        {
            Type type = control.GetType();
            string value = "";
            switch (type.Name)
            {
                case "TextBox":
                    {
                        TextBox tb = control as TextBox;
                        value = tb.Text;
                        break;
                    }
                case "RadioButtonList":
                    {
                        RadioButtonList rbl = control as RadioButtonList;
                        value = rbl.SelectedValue;
                        break;
                    }
                case "CheckBoxList":
                    {
                        CheckBoxList cbl = control as CheckBoxList;
                        foreach(ListItem item in cbl.Items)
                        {
                            if(item.Selected)
                            {
                                value += "," + item.Value;
                            }
                        }
                        value = value.TrimStart(',');
                        break;
                    }
                case "DropDownList":
                    {
                        DropDownList ddl = control as DropDownList;
                        value = ddl.SelectedValue;
                        break;
                    }
            }
            return value;
        }

        void AddGroupedRow(Control form, Models.CRF_Section section)
        {
            Table tInfo = form as Table;
            int rIndex = tInfo.Rows.Count-1;

            List<CRF_Item> groupedItems = section.Items
                .Where(x => !x.Ungrouped)
                .OrderBy(x => x.CRF_ItemID).ToList(); //итемы в группе
            TableRow addingTR = new TableRow();
            for (int i = 0; i < groupedItems.Count; i++)
            {
                TableCell tc = new TableCell();
                GetAddedControl(groupedItems[i], ref tc, rIndex, null);
                addingTR.Cells.Add(tc);

                if (groupedItems[i].Required) //если обязательное
                {
                    RequiredFieldValidator RFV = new RequiredFieldValidator();
                    RFV.ControlToValidate = groupedItems[i].Identifier + "_" + (rIndex).ToString();
                    RFV.Display = ValidatorDisplay.Dynamic;
                    RFV.ErrorMessage = "Данное поле обязательно для заполнения";
                    RFV.CssClass = "field-validation-error";
                    tc.Controls.Add(RFV);
                }
            }
            tInfo.Rows.AddAt(rIndex, addingTR);
            RowCountInSection[section.Label] = RowCountInSection[section.Label] + 1;
        }

        /// <summary>
        /// Нажата кнопка сохранить
        /// </summary>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Control tempControl = ((Button)sender).Parent.Parent;
            string panelName = (tempControl as AjaxControlToolkit.TabPanel).HeaderText;
            CRF_Section section = _crf.Sections.First(x => x.Title == panelName);

            lblStatus.Text = "";
            btnStatus.Visible = false;

            //обновления статуса Ввод начат////
            _sCRF = SCR.SelectByID(_subjectID, _eventID, _crfID);
            if(_sCRF == null)
            {
                _sCRF = new SubjectsCRF();
                _sCRF.SubjectID = _subjectID;
                _sCRF.EventID = _eventID;
                _sCRF.CRFID = _crfID;
                SCR.Create(_sCRF);
                SCR.Save();
                _sCRF = SCR.SelectByID(_subjectID, _eventID, _crfID);
            }

            ///////////////////////////////////
            var se = SER.SelectByID(_subjectID, _eventID);
            if (se == null)
            {
                se = new SubjectsEvent();
                se.SubjectID = _subjectID;
                se.EventID = _eventID;
                SER.Create(se);
                SER.Save();
            }

            if (!_sCRF.IsStart)
            {
                _sCRF.IsStart = true;
                _sCRF.IsStartBy = User.Identity.Name;
                SCR.Update(_sCRF);

                ChangeAuditCrfStatus(string.Empty,Resources.LocalizedText.DataEntryStarted);

                SCR.Save();
                _sCRF = SCR.SelectByID(_subjectID, _eventID, _crfID);
            }
            if(_sCRF.IsEnd)
            {
                ChangeAuditCrfStatus(Resources.LocalizedText.Completed,string.Empty);

                _sCRF.IsEnd = false;
                _sCRF.IsEndBy = "";
                _sCRF.IsEndDate = null;

                if (_sCRF.IsApproved)
                {
                    ChangeAuditCrfStatus(Resources.LocalizedText.Signed, string.Empty);

                    _sCRF.IsApproved = false;
                    _sCRF.IsApprovedBy = "";
                    _sCRF.IsApprovedDate = null;

                }
                if (_sCRF.IsCheckAll)
                {
                    ChangeAuditCrfStatus(Resources.LocalizedText.SDVComplete, string.Empty);

                    _sCRF.IsCheckAll = false;
                    _sCRF.IsCheckAllBy = "";
                    _sCRF.IsCheckAllDate = null;
                }
                

                SCR.Update(_sCRF);
                SCR.Save();
            }
            _sCRF = SCR.SelectByID(_subjectID, _eventID, _crfID);



            _crf = CRFR.SelectByID(_crf.CRFID); //не очень понятно зачем=(
            
            GetInfo(tempControl, section); //считывание информации из полей и запись в БД.
            GetQuery(section);
            LoadForm(_crf);

            ConfigActionButtons();

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Control tempControl = ((Button)sender).Parent.Parent.Parent;
            string panelName = (tempControl.Parent as AjaxControlToolkit.TabPanel).HeaderText;
            CRF_Section section = _crf.Sections.First(x => x.Title == panelName);
            AddGroupedRow(tempControl, section);
        }

        protected void btnNotes_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string mpeName ="mpe" + btn.ID.Replace("btnNotes", "");
            AjaxControlToolkit.ModalPopupExtender mpe = btn.Parent.FindControl(mpeName) as AjaxControlToolkit.ModalPopupExtender;
            LoadMPE(mpeIn: mpe);
        }

        void SetDefaultBtnVisibleForMPE()
        {
            divCreate.Visible = false;
            divMessageLog.Style.Add("display","none");
            btnSaveWindow.Visible = false;
            if ((!_sCRF.IsCheckAll) && (User.IsInRole(Core.Roles.Auditor.ToString()) || User.IsInRole(Core.Roles.Data_Manager.ToString()) || User.IsInRole(Core.Roles.Monitor.ToString())))
            {
                btnCreateNote.Visible = true;
            }
            else
                btnCreateNote.Visible = false;
            tbHeader.Visible = true;
            lbHeader.Visible = true;
        }
        void LoadMPE(string mpeNameIn = "", AjaxControlToolkit.ModalPopupExtender mpeIn=null)
        {
            AjaxControlToolkit.ModalPopupExtender mpe = new AjaxControlToolkit.ModalPopupExtender();
            string mpeName = "";
            if (mpeNameIn != "")
            {
                foreach(AjaxControlToolkit.TabPanel tab in tcCRF.Tabs)
                {
                    mpe = tab.FindControl(mpeNameIn) as AjaxControlToolkit.ModalPopupExtender;
                    if (mpe != null)
                        break;
                }
                //mpe = tcCRF.FindControl(mpeNameIn) as AjaxControlToolkit.ModalPopupExtender; //.ActiveTab.FindControl(mpeNameIn)
                if (mpe != null)
                    _mpe = mpe;
                else
                    throw new ArgumentNullException("Не найдено с указанным id");
                mpeName = mpeNameIn;
            }
            else
                if (mpeIn != null)
                {
                    mpeName = mpeIn.ID;
                    _mpe = mpeIn;
                    mpe = mpeIn;
                }

            SetDefaultBtnVisibleForMPE();

            string itemIdentifier;
            int index;

            GetItemIDIndex(mpeName, out itemIdentifier, out index);

            Models.CRF_Item ci = CIR.GetManyByFilter(x => x.Identifier == itemIdentifier).FirstOrDefault();
            if(ci==null)
            {
                throw new Exception("Не удалось найти Итем с указанным идентификатором");
            }
            Models.SubjectsItem si = SIR.SelectByID(_subjectID, _eventID, _crfID, ci.CRF_ItemID, index);

            if (si == null)
                return;
            else
                _si = si;

            LoadQuery(si);

            _mpe.Show();
        }

        void GetItemIDIndex(string mpeName, out string itemIdentifier, out int index)
        {
            string strIndex = mpeName.Substring(mpeName.LastIndexOf('_') + 1);
            if (!int.TryParse(strIndex, out index))
                index = -1;

            itemIdentifier = mpeName.Replace("mpe_", "");
            if (itemIdentifier.IndexOf('_') > 0 && index>=0)
            {
                itemIdentifier = itemIdentifier.Remove(itemIdentifier.IndexOf('_'));
            }

        }
        void LoadQuery(Models.SubjectsItem si)
        {
            List<Models.Query> queries = GetQueries(si);
            gvNotes.DataSource = queries;
            gvNotes.DataBind();
        }
        List<Models.Query> GetQueries(SubjectsItem si)
        {
            List<Models.Query> queries = NR
                .GetManyByFilter(x =>
                    x.SubjectID == _subjectID &&
                    x.EventID == _eventID &&
                    x.CRFID == _crfID &&
                    x.ItemID == si.ItemID &&
                    x.IndexID == si.IndexID).ToList();

            queries = Core.Core.FilterQueriesAccess(queries, User);

            return queries;
        }

        protected void gvNotes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                bool answer = true;
                bool close = true;
                Models.Query _query = (gvNotes.DataSource as List<Models.Query>)[e.Row.RowIndex];


                if (!(User.IsInRole(Core.Roles.Auditor.ToString()) || User.IsInRole(Core.Roles.Data_Manager.ToString())))
                    close = false;
                if (User.IsInRole(Core.Roles.Administrator.ToString()))
                    answer = false;
                if(User.IsInRole(Core.Roles.Monitor.ToString()))
                {
                    if(_query.From != User.Identity.Name)
                    {
                        answer = false;
                        close = false;
                    }
                    else
                    {
                        close = true;
                    }

                }

                if (_query.Status == Core.QueryStatus.Closed || _query.Status == Core.QueryStatus.Note)
                {
                    //ответить
                    answer = false;

                    //закрыть
                    close = false;
                }

                //ответить
                (e.Row.Cells[10].Controls[1] as Button).Visible = answer;

                //закрыть
                (e.Row.Cells[11].Controls[1] as Button).Visible = close;
            }
        }
        
        protected void btnPopUpSeeMessage_Click(object sender, EventArgs e)
        {
            Button _btn = sender as Button;
            int rowIndex = (_btn.Parent.Parent as GridViewRow).RowIndex;

            string itemIdentifier;
            int index;
            GetItemIDIndex(_mpe.ID, out itemIdentifier, out index);
            Models.CRF_Item ci = CIR.GetManyByFilter(x => x.Identifier == itemIdentifier).FirstOrDefault();
            Models.SubjectsItem si = SIR.SelectByID(_subjectID, _eventID, _crfID, ci.CRF_ItemID, index);

            var queries = GetQueries(si);

            var query = queries[rowIndex];

            LoadMessages(query);

            LoadMPE(_mpe.ID);
            divMessageLog.Style.Add("display","block");

        }

        void LoadMessages(Models.Query query)
        {
            while(tQueries.Rows.Count>1)
            {
                tQueries.Rows.RemoveAt(tQueries.Rows.Count-1);
            }
            
            foreach (var message in query.Messages)
            {
                System.Web.UI.HtmlControls.HtmlTableRow row = new System.Web.UI.HtmlControls.HtmlTableRow();

                var tc = new System.Web.UI.HtmlControls.HtmlTableCell();
                tc.InnerText = message.CreationDate.ToShortDateString();
                row.Cells.Add(tc);

                tc = new System.Web.UI.HtmlControls.HtmlTableCell();
                tc.InnerText = query.Header;
                row.Cells.Add(tc);

                tc = new System.Web.UI.HtmlControls.HtmlTableCell();
                tc.InnerText = message.From;
                row.Cells.Add(tc);

                tc = new System.Web.UI.HtmlControls.HtmlTableCell();
                tc.InnerText = message.Text;
                row.Cells.Add(tc);

                tQueries.Rows.Add(row);
            }
        }

        /// <summary>
        /// Нажата кнопка ответить на сообщение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAnswer_Click(object sender, EventArgs e)
        {
            LoadMPE(_mpe.ID);
            divCreate.Visible = true;
            lbHeader.Visible = false;
            tbHeader.Visible = false;
            btnSaveWindow.Visible = true;
            rfvHeader.EnableClientScript = false;
            lbNoteText.Text = Resources.LocalizedText.Message;
            Button _btn = sender as Button;
            int rowIndex = (_btn.Parent.Parent as GridViewRow).RowIndex;
            answerRowIndex = rowIndex;
        }

        /// <summary>
        /// Нажата кнопка закрыть заметку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCloseNote_Click(object sender, EventArgs e)
        {
            Button _btn = sender as Button;
            closeRowIndex = (_btn.Parent.Parent as GridViewRow).RowIndex;
            LoadMPE(_mpe.ID);
            divCreate.Visible = true;
            lbHeader.Visible = false;
            tbHeader.Visible = false;
            btnSaveWindow.Visible = true;
            rfvHeader.EnableClientScript = false;
            lbNoteText.Text = Resources.LocalizedText.ReasonOfQueryResolution;
            btnSaveWindow.Text = Resources.LocalizedText.ResolveQuery;

        }

        void CloseQuery(Models.Query query)
        {

            if (query == null)
                throw new ArgumentNullException();
            query.Status = Core.QueryStatus.Closed;
            query.ClosedBy = User.Identity.Name;
            query.ClosedDate = DateTime.Now;
            query.ClosedText = tbNoteText.Text;
            NR.Update(query);
            NR.Save();
        }

        protected void btnCreateNote_Click(object sender, EventArgs e)
        {
            if(_mpe!=null)
                LoadMPE(_mpe.ID);
            divCreate.Visible = true;
            btnSaveWindow.Visible = true;
            btnCreateNote.Visible = false;
            rfvHeader.EnableClientScript = true;
            lbNoteText.Text = Resources.LocalizedText.QueryMessage;
        }
        protected void btnSaveWindow_Click(object sender, EventArgs e)
        {
            Models.Query note = new Models.Query();

            if(closeRowIndex > -1)
            {
                var queries = GetQueries(_si);
                CloseQuery(queries[closeRowIndex]);
            }
            else
            if (answerRowIndex == -1)
            {
                note.To = _si.CreatedBy;
                note.Header = tbHeader.Text;
                note.Status = Core.QueryStatus.New;
                note.Type = Core.NoteType.Note;
                note.From = User.Identity.Name;
                note.CreationDate = DateTime.Now;
                note.SubjectID = _subjectID;
                note.EventID = _eventID;
                note.CRFID = _crfID;
                note.ItemID = _si.ItemID;
                note.IndexID = _si.IndexID;
                note.MedicalCenterID = SR.SelectByID(_subjectID).MedicalCenterID;
                note = NR.Create(note);
                NR.Save();
                NR = new Models.Repository.QueryRepository();

                Models.QueryMessage qm = new QueryMessage();
                qm.From = User.Identity.Name;
                qm.To = _si.CreatedBy;
                qm.Text = tbNoteText.Text;
                NR.AddMessage(note.QueryID, qm,true);

            }
            else
            {
                List<Models.Query> Notes = NR
                    .GetManyByFilter(x => x.SubjectID == _subjectID && x.EventID == _eventID && x.CRFID == _crfID && x.ItemID == _si.ItemID && x.IndexID == _si.IndexID).ToList();
                Models.Query answeringNote = NR.SelectByID(Notes[answerRowIndex].QueryID);

                Models.QueryMessage qm = new QueryMessage();
                qm.From = User.Identity.Name;
                qm.To = answeringNote.From;
                qm.Text = tbNoteText.Text;
                NR.AddMessage(Notes[answerRowIndex].QueryID,qm,false);
            }

            NR.Save();

            tbHeader.Text = "";
            tbNoteText.Text = "";
            answerRowIndex = -1;
            LoadMPE(_mpe.ID);
        }

        private void ChangeAuditCrfStatus(string oldValue, string newValue)
        {
            var audit = new Models.Audit();
            audit.UserName = User.Identity.Name;
            audit.UserID = (Guid)System.Web.Security.Membership.GetUser(User.Identity.Name).ProviderUserKey;
            audit.SubjectID = _subjectID;
            audit.EventID = _eventID;
            audit.CRFID = _crfID;
            audit.ActionDate = DateTime.Now;
            audit.ActionType = Core.AuditActionType.SubjectCRFStatus;
            audit.ChangesType = Core.AuditChangesType.Update;
            audit.OldValue = oldValue;
            audit.NewValue = newValue;
            AR.Create(audit);
            AR.Save();
        }

        #region btnChangeCRFStatus
        protected void btnEnd_Click(object sender, EventArgs e)
        {
            _sCRF = SCR.SelectByID(_subjectID, _eventID, _crfID);
            List<SubjectsItem> sis = SIR.GetManyByFilter(x => x.EventID == _eventID && x.SubjectID == _subjectID && x.CRFID == _crfID).ToList();
            //Если есть не закрытые Query
            if (!(sis == null || (sis.Count > 0 && sis.All(x => x.Queries.Count == 0 || (x.Queries.Count > 0 && x.Queries.All(y => y.Status == Core.QueryStatus.Closed))))))
            {
                return;
            }
            _sCRF.IsEnd = true;
            _sCRF.IsEndBy = User.Identity.Name;
            _sCRF.IsEndDate = DateTime.Now;
            SCR.Update(_sCRF);

            ChangeAuditCrfStatus(string.Empty,"Завершено");

            SCR.Save();

            readOnly = true;
            LoadForm(_crf);
            ConfigActionButtons();
        }
        protected void btnApproved_Click(object sender, EventArgs e)
        {
            lblLoginStatus.Visible = false;
            tbUserName.Text = "";
            tbPassword.Text = "";
            AjaxControlToolkit.ModalPopupExtender mpe = btnApproved.Parent.FindControl("mpeLogin") as AjaxControlToolkit.ModalPopupExtender;
            mpe.Show();
        }

        protected void btnCheckAll_Click(object sender, EventArgs e)
        {
            _sCRF = SCR.SelectByID(_subjectID, _eventID, _crfID);
            List<SubjectsItem> sis = SIR.GetManyByFilter(x=>x.EventID == _eventID && x.SubjectID == _subjectID && x.CRFID == _crfID).ToList();
            //Если есть не закрытые Query
            if(!(sis==null || (sis.Count>0 && sis.Any(x=>x.Queries.Count == 0 || (x.Queries.Count>0 && x.Queries.Any(y=>y.Status == Core.QueryStatus.Closed))))))
            {
                return;
            }
            _sCRF.IsCheckAll = true;
            _sCRF.IsCheckAllBy = User.Identity.Name;
            _sCRF.IsCheckAllDate = DateTime.Now;
            SCR.Update(_sCRF);
            SCR.Save();
            _sCRF = SCR.SelectByID(_subjectID, _eventID, _crfID);

            ChangeAuditCrfStatus(string.Empty,"Сверено");

            ConfigActionButtons();
        }
        #endregion

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (btnEdit.Text != Resources.LocalizedText.Edit)
                lblEditReason.Text = Resources.LocalizedText.ReasonForSDVCancelling;
            AjaxControlToolkit.ModalPopupExtender mpe = btnEdit.Parent.FindControl("mpeEditReason") as AjaxControlToolkit.ModalPopupExtender;

            mpe.Show();
        }

        protected void btnSaveEditReason_Click(object sender, EventArgs e)
        {
            Models.AuditEditReason aer = new AuditEditReason();
            aer.ActionDate = DateTime.Now;
            aer.CRFID = _crfID;
            aer.EventID = _eventID;
            aer.SubjectID = _subjectID;
            aer.UserName = User.Identity.Name;
            aer.UserID = (Guid)System.Web.Security.Membership.GetUser(User.Identity.Name).ProviderUserKey;
            aer.EditReason = tbEditReason.Text;
            AERR.Create(aer);
            AERR.Save();
            if (btnEdit.Text == "Изменить")
            {
                editing = true;
                readOnly = false;
                LoadForm(_crf);
            }
            else
            {
                _sCRF = SCR.SelectByID(_subjectID, _eventID, _crfID);
                _sCRF.IsCheckAll = false;
                _sCRF.IsCheckAllBy = "";
                _sCRF.IsCheckAllDate = null;
                SCR.Update(_sCRF);
                SCR.Save();
                _sCRF = SCR.SelectByID(_subjectID, _eventID, _crfID);
                ConfigActionButtons();
            }
        }

        protected void btnOkLogin_Click(object sender, EventArgs e)
        {
            if(Membership.ValidateUser(tbUserName.Text,tbPassword.Text))
            {
                _sCRF = SCR.SelectByID(_subjectID, _eventID, _crfID);

                _sCRF.IsApproved = true;
                _sCRF.IsApprovedBy = User.Identity.Name;
                _sCRF.IsApprovedDate = DateTime.Now;
                SCR.Update(_sCRF);

                ChangeAuditCrfStatus(string.Empty, "Подписано");
                
                SCR.Save();

                _sCRF = SCR.SelectByID(_subjectID, _eventID, _crfID);
                ConfigActionButtons();
            }
            else
            {
                lblLoginStatus.Visible = true;
                AjaxControlToolkit.ModalPopupExtender mpe = btnApproved.Parent.FindControl("mpeLogin") as AjaxControlToolkit.ModalPopupExtender;
                mpe.Show();
            }
        }

    }
}