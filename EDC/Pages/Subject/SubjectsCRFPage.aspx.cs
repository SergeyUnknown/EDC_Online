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

        static Models.CRF _crf;
        static long _subjectID;
        static long _eventID;
        static long _crfID;
        static int RowCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetInfoFromRequest();
                _crf = CRFR.SelectByID(_crfID);
                ConfigButtonVisible();
                RowCount = 0;
            }
                LoadForm(_crf);
            
        }

        void ConfigButtonVisible()
        {
            Models.Subject currentSubject = SR.SelectByID(_subjectID);
            List<Models.Subject> subjectsInCenter = SR.GetManyByFilter(x=>x.MedicalCenterID == currentSubject.MedicalCenterID).OrderBy(x => x.SubjectID).ToList();
            if (subjectsInCenter.Count > 0)
            {
                int currentSubjectIndex = subjectsInCenter.FindIndex(x => x.SubjectID == _subjectID);
                if (currentSubjectIndex == 0)
                    btnPrevSubject.Visible = false;
                else
                    btnPrevSubject.PostBackUrl = string.Format("~/Subjects/{0}/{1}/{2}", subjectsInCenter[currentSubjectIndex-1].SubjectID, _eventID, _crfID);
                
                if (currentSubjectIndex == subjectsInCenter.Count - 1)
                    btnNextSubject.Visible = false;
                else
                    btnPrevSubject.PostBackUrl = string.Format("~/Subjects/{0}/{1}/{2}", subjectsInCenter[currentSubjectIndex +1].SubjectID, _eventID, _crfID);
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
                    btnPrevCRFInEvent.PostBackUrl = string.Format("~/Subjects/{0}/{1}/{2}", _subjectID, _eventID, CIEs[currentCRFIndex - 1].CRFID);

                if (currentCRFIndex == CIEs.Count - 1)
                    btnNextCRFInEvent.Visible = false;
                else
                    btnNextCRFInEvent.PostBackUrl = string.Format("~/Subjects/{0}/{1}/{2}", _subjectID, _eventID, CIEs[currentCRFIndex + 1].CRFID);

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
            foreach (Models.CRF_Section section in _crf.Sections)
            {
                AjaxControlToolkit.TabPanel tp = new AjaxControlToolkit.TabPanel();
                tp.HeaderText = section.Title;
                tp.ID = "tp" + section.Label;

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

                    GetAddedUngroupedControl(item, ref tc); //запись в tc нужных контролов

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

                        #region readCurrentValues

                        List<SubjectsItem> SIs = SIR
                            .GetManyByFilter(x => x.SubjectID == _subjectID && x.EventID == _eventID && x.CRFID == _crfID && x.ItemID == item.CRF_ItemID)
                            .OrderBy(x => x.IndexID).ToList(); //значения в этом столбце

                        if (SIs.Count > 0)
                        {
                            for (int k = 0; k < SIs.Count; k++)
                            {
                                tc = new TableCell();
                                GetAddedGroupedControl(item, ref tc, SIs[k].IndexID, SIs[k].Value);
                                addedRows[k].Cells.Add(tc);
                            }
                        }

                        #endregion

                        ////////////////////Поле ввода данных/////////////////
                        tc = new TableCell();
                        GetAddedGroupedControl(item, ref tc, groupedIndex, null);
                        trsGroupedValues.Cells.Add(tc);
                        //////////////////////////////////////////////////
                    }

                    tableGrouped.Rows.Add(trGroupedHeaders); //добавление шапки
                    if(addedRows.Length>0)
                        tableGrouped.Rows.AddRange(addedRows); //введенные данные
                    tableGrouped.Rows.Add(trsGroupedValues); //добавление пустых полей

                    //////////////////////отриовка добавленных строк/////////////////////////
                    TableRow addingTR = new TableRow();
                    for (int j = 0; j < RowCount; j++)
                    {
                        for (int i = 0; i < groupedItems.Count; i++)
                        {
                            TableCell tc = new TableCell();
                            GetAddedGroupedControl(groupedItems[i], ref tc, j+2, null);
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

        void GetAddedUngroupedControl(CRF_Item item, ref TableCell tc)
        {
            Control addedControl = new Control();

            #region readCurrentValue
            SubjectsItem si = SIR.SelectByID(_subjectID, _eventID, _crfID, item.CRF_ItemID,-1);
            List<SubjectsItem> temp = SIR.SelectAll().ToList();
            //CRF_Item temp1 = temp[0].Item;
            string itemValue = "";
            bool isNullValue = true;
            if (si != null && !string.IsNullOrWhiteSpace(si.Value))
            {
                isNullValue = false;
                itemValue = si.Value;
            }
            #endregion

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
                                    FTBE.TargetControlID = item.Identifier;
                                    FTBE.ValidChars = ".";
                                    FTBE.FilterType = AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.Custom;
                                    tc.Controls.Add(FTBE);
                                    break;
                                }
                            case Core.DataType.DATE:
                                {
                                    AjaxControlToolkit.CalendarExtender CE = new AjaxControlToolkit.CalendarExtender();
                                    CE.TargetControlID = item.Identifier;
                                    CE.Format = "dd.MM.yyyy";
                                    tc.Controls.Add(CE);

                                    AjaxControlToolkit.FilteredTextBoxExtender FTBE = new AjaxControlToolkit.FilteredTextBoxExtender();
                                    FTBE.TargetControlID = item.Identifier;
                                    FTBE.ValidChars = ".";
                                    FTBE.FilterType = AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.Custom;
                                    tc.Controls.Add(FTBE);
                                    break;
                                }
                        }
                        #endregion
                        if (!isNullValue)
                            tb.Text = itemValue;
                        addedControl = tb;
                        break;
                    }
                case Core.ResponseType.Textarea:
                    {
                        TextBox tb = new TextBox();
                        tb.TextMode = TextBoxMode.MultiLine;
                        if (!isNullValue)
                            tb.Text = itemValue;
                        tb.CssClass = "response-Item ";
                        addedControl = tb;
                        break;
                    }
                case Core.ResponseType.MultiSelect:
                case Core.ResponseType.Checkbox:
                    {
                        CheckBoxList cb = new CheckBoxList();
                        cb.Items.AddRange(GetListItems(item).ToArray());
                        cb.CssClass = "response-Item " + item.ResponseLayout;
                        if (!isNullValue)
                            cb.SelectedValue = itemValue;
                        addedControl = cb;
                        break;
                    }
                case Core.ResponseType.Radio:
                    {
                        RadioButtonList rb = new RadioButtonList();
                        rb.Items.AddRange(GetListItems(item).ToArray());
                        rb.CssClass = "response-Item " + item.ResponseLayout;
                        if (!isNullValue)
                            rb.SelectedValue = itemValue;
                        addedControl = rb;
                        break;
                    }
                case Core.ResponseType.SingleSelect:
                    {
                        DropDownList ddl = new DropDownList();
                        ddl.Items.AddRange(GetListItems(item).ToArray());
                        if (!isNullValue)
                            ddl.SelectedValue = itemValue;
                        ddl.CssClass = "response-Item ";
                        addedControl = ddl;
                        break;
                    }

            }
            addedControl.ID = item.Identifier; //ID параметра
            tc.Controls.Add(addedControl);

            if (item.Required) //если обязательное
            {
                Label lbl = new Label();
                lbl.Text = "*";
                tc.Controls.Add(lbl);
            }



        }

        void GetAddedGroupedControl(CRF_Item item, ref TableCell tc, int index, string value)
        {
            Control addedControl = new Control();

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
                                    FTBE.TargetControlID = item.Identifier+"_"+index;
                                    FTBE.ValidChars = ".";
                                    FTBE.FilterType = AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.Custom;
                                    tc.Controls.Add(FTBE);
                                    break;
                                }
                            case Core.DataType.DATE:
                                {
                                    AjaxControlToolkit.CalendarExtender CE = new AjaxControlToolkit.CalendarExtender();
                                    CE.TargetControlID = item.Identifier + "_" + index;
                                    CE.Format = "dd.MM.yyyy";
                                    tc.Controls.Add(CE);

                                    AjaxControlToolkit.FilteredTextBoxExtender FTBE = new AjaxControlToolkit.FilteredTextBoxExtender();
                                    FTBE.TargetControlID = item.Identifier + "_" + index;
                                    FTBE.ValidChars = ".";
                                    FTBE.FilterType = AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.Custom;
                                    tc.Controls.Add(FTBE);
                                    break;
                                }
                        }
                        #endregion
                        if (value != null)
                            tb.Text = value;
                        addedControl = tb;
                        break;
                    }
                case Core.ResponseType.Textarea:
                    {
                        TextBox tb = new TextBox();
                        tb.TextMode = TextBoxMode.MultiLine;
                        if (value != null)
                            tb.Text = value;
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
                            cb.SelectedValue = value;
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
                        addedControl = rb;
                        break;
                    }
                case Core.ResponseType.SingleSelect:
                    {
                        DropDownList ddl = new DropDownList();
                        ddl.Items.AddRange(GetListItems(item).ToArray());
                        if (value != null)
                            ddl.SelectedValue = value;
                        addedControl = ddl;
                        break;
                    }
            }

            if (item.Required) //если обязательное
            {
                Label lbl = new Label();
                lbl.Text = "*";
                tc.Controls.Add(lbl);
            }

            addedControl.ID = item.Identifier + "_" + index;
            tc.Controls.Add(addedControl);
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



            foreach(var item in groupedItems)
            {
                Control _control;
                _control = form.FindControl(item.Identifier + "_1");
                Table groupedTable = _control.Parent.Parent.Parent as Table;
                int valuesRowCount = groupedTable.Rows.Count - 2;
                for(int i =0;i<valuesRowCount-1;i++)
                {
                    _control = form.FindControl(item.Identifier +"_" + (i+1).ToString());
                    string value = GetValueFromControl(_control);
                    SubjectsItem si = SIR.SelectByID(_subjectID, _eventID, _crfID, item.CRF_ItemID, i+1);
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
                        value = cbl.SelectedValue;
                        break;
                    }
            }
            return value;
        }

        void AddGroupedRow(Control form, Models.CRF_Section section)
        {
            Table tInfo = form as Table;
            int rIndex = tInfo.Rows.Count-2;
            if(rIndex<1)
                return;
            TableRow addingTR = new TableRow();

            List<CRF_Item> groupedItems = section.Items
                    .Where(x => !x.Ungrouped)
                    .OrderBy(x => x.CRF_ItemID).ToList(); //итемы в группе

            for (int i = 0; i < tInfo.Rows[rIndex].Cells.Count; i++)
            {
                TableCell tc = new TableCell();
                GetAddedGroupedControl(groupedItems[i], ref tc, rIndex + 1, null);
                addingTR.Cells.Add(tc);
            }

            RowCount++;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Control tempControl = ((Button)sender).Parent;
            string panelName = (tempControl as AjaxControlToolkit.TabPanel).HeaderText;
            CRF_Section section = _crf.Sections.First(x => x.Title == panelName);
            GetInfo(tempControl, section);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Control tempControl = ((Button)sender).Parent.Parent.Parent;
            string panelName = (tempControl.Parent as AjaxControlToolkit.TabPanel).HeaderText;
            CRF_Section section = _crf.Sections.First(x => x.Title == panelName);
            AddGroupedRow(tempControl, section);
        }

        protected void prevCRFInEvent_Click(object sender, EventArgs e)
        {

            Models.CRFInEvent CIE = CIER.SelectByID(_crfID, _eventID);
            List<Models.CRFInEvent> CIEs = CIER.GetManyByFilter(x => x.EventID == _eventID && x.Position < CIE.Position).OrderBy(x => x.Position).ToList();
            if (CIEs.Count > 0)
                Response.Redirect(string.Format("~/Subjects/{0}/{1}/{2}", _subjectID, _eventID, CIEs.Last().CRFID));
        }

        protected void nextCRFInEvent_Click(object sender, EventArgs e)
        {
            Models.CRFInEvent CIE = CIER.SelectByID(_crfID, _eventID);
            List<Models.CRFInEvent> CIEs = CIER.GetManyByFilter(x => x.EventID == _eventID && x.Position > CIE.Position).OrderBy(x => x.Position).ToList();
            if (CIEs.Count > 0)
                Response.Redirect(string.Format("~/Subjects/{0}/{1}/{2}", _subjectID, _eventID, CIEs[0].CRFID));
        }

    }
}