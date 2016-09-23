using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDC.Models;

namespace EDC.Pages.Subject
{
    public partial class SubjectsCRFPage : System.Web.UI.Page
    {
        Models.Repository.CRFRepository CRFR = new Models.Repository.CRFRepository();
        Models.Repository.CRFItemRepository CIR = new Models.Repository.CRFItemRepository();
        Models.Repository.SubjectsItemRepoitory SIR = new Models.Repository.SubjectsItemRepoitory();
        Models.Repository.SubjectsCRFRepository SCR = new Models.Repository.SubjectsCRFRepository();
        Models.Repository.SubjectRepository SR = new Models.Repository.SubjectRepository();
        Models.Repository.CRFInEventRepository CIER = new Models.Repository.CRFInEventRepository();
        Models.Repository.NoteRepository NR = new Models.Repository.NoteRepository();
        Models.Repository.EventRepository ER = new Models.Repository.EventRepository();
        Models.Repository.AppSettingRepository ASR = new Models.Repository.AppSettingRepository();

        static Models.CRF _crf;
        static Models.Subject _subject;
        static Models.SubjectsCRF _sCRF;
        static long _subjectID;
        static long _eventID;
        static long _crfID;
        static Dictionary<string, int> RowCountInSection = new Dictionary<string, int>();

        static AjaxControlToolkit.ModalPopupExtender _mpe;

        static Models.SubjectsItem _si;

        static int answerRowIndex;

        static bool readOnly = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                GetInfoFromRequest();
                _crf = CRFR.SelectByID(_crfID);
                _subject = SR.SelectByID(_subjectID);
                _sCRF = SCR.SelectByID(_subjectID, _eventID, _crfID);
                Models.Event _event = ER.SelectByID(_eventID);
                ConfigButtonVisible();
                Title = string.Format("Настройка ИРК субъекта {0}", _subject.Number);
                lbInfo.Text = string.Format("Визит: \"{0}\", ИРК: \"{1}\", субъект: {2}",_event.Name, string.IsNullOrWhiteSpace(_crf.RussianName) ? _crf.Name : _crf.RussianName, _subject.Number);
                RowCountInSection = new Dictionary<string, int>();
                answerRowIndex = -1;

                if (ASR.SelectByID(Core.APP_STATUS).Value == Core.AppStatus.Disable.ToString())
                    readOnly = true;


                if (User.IsInRole(Core.Roles.Principal_Investigator.ToString()) && (_sCRF != null && _sCRF.IsEnd) && !_sCRF.IsApprove)
                {
                    btnApproved.Visible = true;
                }

                if (User.IsInRole(Core.Roles.Monitor.ToString()))
                {
                    readOnly = true;
                    if ((_sCRF != null && _sCRF.IsApprove) && !_sCRF.IsCheckAll)
                        btnCheckAll.Visible=true;
                }

                if(User.IsInRole(Core.Roles.Auditor.ToString()))
                {
                    readOnly = true;
                }

            }
            LoadForm(_crf);

            

            if (User.IsInRole(Core.Roles.Investigator.ToString()) || User.IsInRole(Core.Roles.Principal_Investigator.ToString()) && _sCRF !=null && !_sCRF.IsEnd)
            {
                if (ViewState["ActiveTabIndex"] != null && (int)ViewState["ActiveTabIndex"]== tcCRF.Tabs.Count-1)
                    btnEnd.Visible = _sCRF.IsEnd?false: true;
            }
            
        }

        void ConfigButtonVisible()
        {
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


            Models.CRFInEvent CIE = CIER.SelectByID(_crfID, _eventID);
            List<Models.CRFInEvent> CIEs = CIER.GetManyByFilter(x => x.EventID == _eventID).OrderBy(x => x.Position).ToList();
            if (CIEs.Count > 0)
            {
                int currentCRFIndex = CIEs.FindIndex(x => x.EventID == _eventID && x.CRFID == _crfID);
                if (currentCRFIndex == 0)
                    btnPrevCRFInEvent.Visible = false;
                else
                {
                    btnPrevCRFInEvent.Attributes.Add("onclick", "checkChanged('" + ResolveClientUrl(string.Format("~/Subjects/{0}/{1}/{2}", _subjectID, _eventID, CIEs[currentCRFIndex - 1].CRFID)) + "')");
                    string crfName = string.IsNullOrWhiteSpace(CIEs[currentCRFIndex - 1].CRF.RussianName) ? CIEs[currentCRFIndex - 1].CRF.Name : CIEs[currentCRFIndex - 1].CRF.RussianName;
                    btnPrevCRFInEvent.ToolTip = "К предыдущей форме (" + crfName + ")";
                }
                if (currentCRFIndex == CIEs.Count - 1)
                    btnNextCRFInEvent.Visible = false;
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
                TableRow trUngrouped = new TableRow(); //строка итемов

                Table tableGrouped = new Table();           //широкая таблица
                TableRow trGroupedHeaders = new TableRow(); //заголовки
                TableRow trsGroupedValues = new TableRow(); //значения


                Button btnSave = new Button();  //Кнопка сохранить
                btnSave.ID = "btnSave";
                btnSave.Click += btnSave_Click;
                btnSave.Text = "Сохранить";

                Button btnAdd = new Button();   //кнопка добавить
                btnAdd.ID = "btnAdd";
                btnAdd.Click +=btnAdd_Click;
                btnAdd.Text = "Добавить";
                
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
                        RFV.ErrorMessage = "Данное поле обязательно для заполнения";
                        RFV.CssClass = "field-validation-error";
                        tc.Controls.Add(RFV);
                    }

                    trUngrouped.Cells.Add(tc);

                    if (i < section.Items.Count - 1) //если элемент не последний
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
                            GetAddedControl(groupedItems[i], ref tc, j+2, null);
                            addingTR.Cells.Add(tc);

                            if (groupedItems[i].Required) //если обязательное
                            {
                                RequiredFieldValidator RFV = new RequiredFieldValidator();
                                RFV.ControlToValidate = groupedItems[i].Identifier + "_" + (j + 2).ToString();
                                RFV.Display = ValidatorDisplay.Dynamic;
                                RFV.ErrorMessage = "Данное поле обязательно для заполнения";
                                RFV.CssClass = "field-validation-error";
                                tc.Controls.Add(RFV);
                            }
                        }
                        tableGrouped.Rows.AddAt(j + 2, addingTR);
                        addingTR = new TableRow();
                    }

                    ////////////////////////////////////////////////////

                    ////////////////кнопка Добавить/////////////////////
                    TableCell tcg = new TableCell();
                    trGroupedHeaders = new TableRow();
                    tcg.Controls.Add(btnAdd);
                    trGroupedHeaders.Cells.Add(tcg);
                    tableGrouped.Rows.Add(trGroupedHeaders);
                    tableGrouped.CssClass = "bigTable";
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnAdd);
                    ///////////////////////////////////////////////////
                }
                #endregion

                tp.ScrollBars = ScrollBars.Auto; //скролбар
                tcCRF.ScrollBars = ScrollBars.Auto; //скролбар
                tp.Controls.Add(tableGrouped); //таблицу в tabPanel
                tp.Controls.Add(btnSave);
                tcCRF.Tabs.Add(tp); //панель в tabConteiner

            }
        }

        void GetModalPopup(ref TableCell tc, string id)
        {
            Button btnNotes = new Button();
            btnNotes.ID = "btnNotes_" + id;
            btnNotes.Click += btnNotes_Click;
            btnNotes.CssClass = "nodes";
            tc.Controls.Add(btnNotes);

            LinkButton btnUnvisible = new LinkButton();
            btnUnvisible.ID = "btnUnvisible_" + id;
            btnUnvisible.CssClass = "buttonHide";
            tc.Controls.Add(btnUnvisible);

            AjaxControlToolkit.ModalPopupExtender mpe = new AjaxControlToolkit.ModalPopupExtender();
            mpe.TargetControlID = btnUnvisible.ID;
            mpe.PopupControlID = "pnlModalPopup";
            //mpe.OkControlID = "btnSaveWindow";
            mpe.CancelControlID = "btnCloseWindow";
            mpe.ID = "mpe_" + id;
            tc.Controls.Add(mpe);
        }

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
                GetModalPopup(ref tc, id);
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

            foreach (var item in ungroupedItems)
            {
                Control _control;
                _control = form.FindControl(item.Identifier);
                string value = GetValueFromControl(_control);
                SubjectsItem si = SIR.SelectByID(_subjectID, _eventID, _crfID, item.CRF_ItemID,-1);
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
                    SIR.Create(si);
                }
                else
                {
                    si.Value = value;
                    si.CreatedBy = User.Identity.Name;
                    SIR.Update(si);
                }
                SIR.Save();
            }

            if(groupedItems.Count>0)
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
                        }
                        else
                        {
                            si.Value = value;
                            si.CreatedBy = User.Identity.Name;
                            SIR.Update(si);
                        }
                        SIR.Save();
                    }
                }
            }
        }

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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Control tempControl = ((Button)sender).Parent;
            string panelName = (tempControl as AjaxControlToolkit.TabPanel).HeaderText;
            CRF_Section section = _crf.Sections.First(x => x.Title == panelName);
            GetInfo(tempControl, section);

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
            _sCRF.IsStart = true;
            _sCRF.IsStartBy = User.Identity.Name;

            SCR.Update(_sCRF);
            SCR.Save();
            _sCRF = SCR.SelectByID(_subjectID, _eventID, _crfID);
            ///////////////////////////////////
            _crf = CRFR.SelectByID(_crf.CRFID);
            LoadForm(_crf);
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
            LoadMPE(mpe);
        }

        void LoadMPE(string mpeName)
        {
            AjaxControlToolkit.ModalPopupExtender mpe = tcCRF.ActiveTab.FindControl(mpeName) as AjaxControlToolkit.ModalPopupExtender;
            _mpe = mpe;
            ///////////default//////////////
            divCreate.Visible = false;
            btnSaveWindow.Visible = false;
            btnCreateNote.Visible = true;
            tbHeader.Visible = true;
            lbHeader.Visible = true;


            string strIndex = mpeName.Substring(mpeName.LastIndexOf('_') + 1);
            int rowIndex;
            if (!int.TryParse(strIndex, out rowIndex))
                rowIndex = -1;

            string itemIdentifier = mpe.ID.Replace("mpe_", "");
            if (itemIdentifier.IndexOf('_') > 0)
            {
                itemIdentifier = itemIdentifier.Remove(itemIdentifier.IndexOf('_'));
            }
            Models.CRF_Item ci = CIR.GetManyByFilter(x => x.Identifier == itemIdentifier).FirstOrDefault();

            Models.SubjectsItem si = SIR.SelectByID(_subjectID, _eventID, _crfID, ci.CRF_ItemID, rowIndex);
            if (si == null)
                return;
            else
                _si = si;
            List<Models.Note> Notes = NR
                .GetManyByFilter(x => x.SubjectID == _subjectID && x.EventID == _eventID && x.CRFID == _crfID && x.ItemID == si.ItemID && x.IndexID == si.IndexID).ToList();
            gvNotes.DataSource = Notes;
            gvNotes.DataBind();

            _mpe.Show();
        }

        void LoadMPE(AjaxControlToolkit.ModalPopupExtender mpe)
        {
            string mpeName = mpe.ID;
            _mpe = mpe;
            ///////////default//////////////
            divCreate.Visible = false;
            btnSaveWindow.Visible = false;
            btnCreateNote.Visible = true;
            tbHeader.Visible = true;
            lbHeader.Visible = true; 
            
            string strIndex = mpeName.Substring(mpeName.LastIndexOf('_') + 1);
            int rowIndex;
            if (!int.TryParse(strIndex, out rowIndex))
                rowIndex = -1;

            string itemIdentifier = mpe.ID.Replace("mpe_", "");
            if (itemIdentifier.IndexOf('_') > 0)
            {
                itemIdentifier = itemIdentifier.Remove(itemIdentifier.IndexOf('_'));
            }
            Models.CRF_Item ci = CIR.GetManyByFilter(x => x.Identifier == itemIdentifier).FirstOrDefault();

            Models.SubjectsItem si = SIR.SelectByID(_subjectID, _eventID, _crfID, ci.CRF_ItemID, rowIndex);
            if (si == null)
                return;
            else
                _si = si;
            List<Models.Note> Notes = NR
                .GetManyByFilter(x => x.SubjectID == _subjectID && x.EventID == _eventID && x.CRFID == _crfID && x.ItemID == si.ItemID && x.IndexID == si.IndexID).ToList();
            gvNotes.DataSource = Notes;
            gvNotes.DataBind();

            _mpe.Show();
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
            lbNoteText.Text = "Текст ответа";
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
            int closingRowIndex = (_btn.Parent.Parent as GridViewRow).RowIndex;

            List<Models.Note> Notes = NR
                .GetManyByFilter(x => x.SubjectID == _subjectID && x.EventID == _eventID && x.CRFID == _crfID && x.ItemID == _si.ItemID && x.IndexID == _si.IndexID).ToList();
            Models.Note closingNote = NR.SelectByID(Notes[closingRowIndex].NoteID);
            CloseNote(closingNote);
            LoadMPE(_mpe.ID);
        }

        void CloseNote(Models.Note note)
        {
            if (note == null)
                throw new ArgumentNullException();
            note.Status = Core.QueryStatus.Closed;
            NR.Update(note);
            NR.Save();
        }

        protected void btnCreateNote_Click(object sender, EventArgs e)
        {
            if(_mpe!=null)
                LoadMPE(_mpe.ID);
            divCreate.Visible = true;
            btnSaveWindow.Visible = true;
            btnCreateNote.Visible = false;
            lbNoteText.Text = "Текст заметки";
        }
        protected void btnSaveWindow_Click(object sender, EventArgs e)
        {
            Models.Note note = new Models.Note();
            note.Status = Core.QueryStatus.New;

            if (answerRowIndex == -1)
            {
                note.ForUser = _si.CreatedBy;
                note.Header = tbHeader.Text;
            }
            else
            {
                List<Models.Note> Notes = NR
                    .GetManyByFilter(x => x.SubjectID == _subjectID && x.EventID == _eventID && x.CRFID == _crfID && x.ItemID == _si.ItemID && x.IndexID == _si.IndexID).ToList();
                Models.Note answeringNote = NR.SelectByID(Notes[answerRowIndex].NoteID);
                answeringNote.Status = Core.QueryStatus.Updated;
                note.ForUser = answeringNote.FromUser;
                note.PreviousNoteID = answeringNote.PreviousNoteID == null ? answeringNote.NoteID : answeringNote.PreviousNoteID;
                NR.Update(answeringNote);
            }

            note.Type = Core.NoteType.Note;
            note.FromUser = User.Identity.Name;
            note.Text = tbNoteText.Text;
            note.CreationDate = DateTime.Now;

            note.SubjectID = _subjectID;
            note.EventID = _eventID;
            note.CRFID = _crfID;
            note.ItemID = _si.ItemID;
            note.IndexID = _si.IndexID;
            note.MedicalCenterID = SR.SelectByID(_subjectID).MedicalCenterID;

            NR.Create(note);
            NR.Save();

            tbHeader.Text = "";
            tbNoteText.Text = "";

            LoadMPE(_mpe.ID);
        }

        protected void gvNotes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                bool answer = true;
                bool close = true;
                Models.Note _note = (gvNotes.DataSource as List<Models.Note>)[e.Row.RowIndex];
                if(_note.Status == Core.QueryStatus.Closed || _note.Status == Core.QueryStatus.Note)
                {
                    //ответить
                    answer = false;

                    //закрыть
                    close = false;
                }
                if(_note.PreviousNoteID !=null)
                {
                    //закрыть
                    close = false;

                    if(_note.PreviousNote.Status == Core.QueryStatus.Closed)
                        answer = false;
                }

                //ответить
                (e.Row.Cells[6].Controls[1] as Button).Visible = answer;

                //закрыть
                (e.Row.Cells[7].Controls[1] as Button).Visible = close;
            }
        }

        protected void btnApproved_Click(object sender, EventArgs e)
        {
            _sCRF = SCR.SelectByID(_subjectID, _eventID, _crfID);
            
            _sCRF.IsApprove = true;
            _sCRF.IsApprovedBy = User.Identity.Name;
            SCR.Update(_sCRF);
            SCR.Save();

            _sCRF = SCR.SelectByID(_subjectID, _eventID, _crfID);
        }

        protected void btnCheckAll_Click(object sender, EventArgs e)
        {
            _sCRF = SCR.SelectByID(_subjectID, _eventID, _crfID);

            _sCRF.IsCheckAll = true;
            _sCRF.IsCheckAllBy = User.Identity.Name;
            SCR.Update(_sCRF);
            SCR.Save();

            _sCRF = SCR.SelectByID(_subjectID, _eventID, _crfID);
        }

        protected void btnEnd_Click(object sender, EventArgs e)
        {
            _sCRF = SCR.SelectByID(_subjectID, _eventID, _crfID);
            _sCRF.IsEnd = true;
            _sCRF.IsEndBy = User.Identity.Name;
            SCR.Update(_sCRF);
            SCR.Save();
        }

        protected void tcCRF_ActiveTabChanged(object sender, EventArgs e)
        {
            ViewState["ActiveTabIndex"] = tcCRF.ActiveTabIndex;
        }

    }
}