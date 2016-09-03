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
        Models.Repository.SubjectsItemRepoitory SIR = new Models.Repository.SubjectsItemRepoitory();
        Models.Repository.SubjectsCRFRepository SCR = new Models.Repository.SubjectsCRFRepository();

        static Models.CRF _crf;
        static long _eventID;
        static long _subjectID;
        static long _crfID;
        protected void Page_Load(object sender, EventArgs e)
        {
            GetInfoFromRequest();
            _crf = CRFR.SelectByID(_crfID);
            LoadForm(_crf);
        }

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

                Table tableGrouped = new Table();
                TableRow trGrouped = new TableRow();
                TableRow trGroupedValues = new TableRow();


                Button btnSave = new Button(); //Кнопка сохранить
                btnSave.ID = "btnSave";
                btnSave.Click += btnSave_Click;
                btnSave.Text = "Сохранить";

                Button btnAdd = new Button();
                btnAdd.ID = "btnAdd";
                btnAdd.Click +=btnAdd_Click;
                btnAdd.Text = "Добавить";
                int groupedIndex=0; //индекс строки в группе
                for (int i = 0; i < section.Items.Count; i++)
                {
                    var item = section.Items[i];

                    if (item.Group.Identifier.IndexOf("UNGROUPED") == 0)
                    {
                        #region NoGroup

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
                        Control addedControl = new Control(); //добавлемый контрол

                        addedControl = GetAddedControl(item, ref tc);
                        addedControl.ID = item.Identifier; //ID параметра
                        tc.Controls.Add(addedControl);

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
                            trUngrouped = new TableRow();
                            tc = new TableCell();

                            tc.Controls.Add(btnSave);
                            trUngrouped.Cells.Add(tc);
                            tableUngrouped.Rows.Add(trUngrouped);
                        }
                        #endregion
                    }
                    else
                    {
                        TableCell tc = new TableCell();
                        if (!string.IsNullOrWhiteSpace(item.LeftItemText)) //LeftItemText
                        {
                            Label LIT = new Label();
                            LIT.Text = item.LeftItemText;
                            LIT.CssClass = "CRFLeftItem";
                            tc.CssClass = "CRFCellLeftItem";
                            tc.Controls.Add(LIT);
                        }
                        trGrouped.Cells.Add(tc);
                        tc = new TableCell();
                        Control addedControl = GetAddedControl(item, ref tc,groupedIndex);
                        addedControl.ID = item.Identifier + "_" + groupedIndex;
                        tc.Controls.Add(addedControl);

                        if (item.Required) //если обязательное
                        {
                            RequiredFieldValidator RFV = new RequiredFieldValidator();
                            RFV.ControlToValidate = item.Identifier+"_"+groupedIndex;
                            RFV.Display = ValidatorDisplay.Dynamic;
                            RFV.ErrorMessage = "Данное поле обязательно для заполнения";
                            RFV.CssClass = "field-validation-error";
                            tc.Controls.Add(RFV);
                        }

                        trGroupedValues.Cells.Add(tc);
                    }
                }

                if (tableUngrouped.Rows.Count > 0)
                {
                    tp.Controls.Add(tableUngrouped);
                    //tcCRF.Tabs.Add(tp);
                    ScriptManager.GetCurrent(this).RegisterPostBackControl(btnSave);
                }
                
                {
                    tableGrouped.Rows.Add(trGrouped);
                    tableGrouped.Rows.Add(trGroupedValues);
                    TableCell tc = new TableCell();
                    trGrouped = new TableRow();
                    tc.Controls.Add(btnAdd);
                    trGrouped.Cells.Add(tc);
                    tableGrouped.Rows.Add(trGrouped);
                    tp.ScrollBars = ScrollBars.Auto; //скролбар
                    tp.Controls.Add(tableGrouped);
                    tcCRF.ScrollBars = ScrollBars.Auto; //скролбар
                    tcCRF.Tabs.Add(tp);
                    ScriptManager.GetCurrent(this).RegisterPostBackControl(btnAdd);
                }
            }
        }

        Control GetAddedControl(CRF_Item item, ref TableCell tc)
        {
            Control addedControl = new Control();

            #region readCurrentValue
            SubjectsItem si = SIR.SelectByID(_subjectID, _eventID, _crfID, item.CRF_ItemID);
            string itemValue = "";
            bool isNullValue = true;
            if (si != null && !string.IsNullOrWhiteSpace(si.Value))
            {
                isNullValue = false;
                itemValue = si.Value;
            }

            itemValue = (si != null && !string.IsNullOrWhiteSpace(si.Value)) ? si.Value : ""; //считываем текущее значение
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
            return addedControl;
        }

        Control GetAddedControl(CRF_Item item, ref TableCell tc, int index)
        {
            Control addedControl = new Control();

            #region readCurrentValue
            SubjectsItem si = SIR.SelectByID(_subjectID, _eventID, _crfID, item.CRF_ItemID);
            string itemValue = "";
            bool isNullValue = true;
            if (si != null && !string.IsNullOrWhiteSpace(si.Value))
            {
                isNullValue = false;
                itemValue = si.Value;
            }

            itemValue = (si != null && !string.IsNullOrWhiteSpace(si.Value)) ? si.Value : ""; //считываем текущее значение
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
                        addedControl = ddl;
                        break;
                    }
            }

            return addedControl;
        }

        void GetInfo(Control form, Models.CRF crf)
        {
            List<Models.CRF_Item> items = crf.Items;
            foreach (var item in items)
            {
                Control _control = form.FindControl(item.Identifier);
                Type type = _control.GetType();
                string value = "";
                switch (type.Name)
                {
                    case "TextBox":
                        {
                            TextBox tb = _control as TextBox;
                            value = tb.Text;
                            break;
                        }
                    case "RadioButtonList":
                        {
                            RadioButtonList rbl = _control as RadioButtonList;
                            value = rbl.SelectedValue;
                            break;
                        }
                    case "CheckBoxList":
                        {
                            CheckBoxList cbl = _control as CheckBoxList;
                            value = cbl.SelectedValue;
                            break;
                        }
                }
                SubjectsItem si = SIR.SelectByID(_subjectID, _eventID, _crfID, item.CRF_ItemID);
                if (si == null)
                {
                    si = new SubjectsItem();
                    si.SubjectID = _subjectID;
                    si.EventID = _eventID;
                    si.CRFID = _crfID;
                    si.ItemID = item.CRF_ItemID;
                    si.Value = value;
                    SIR.Create(si);
                }
                else
                {
                    si.Value = value;
                    SIR.Update(si);
                }

                SIR.Save();

                List<SubjectsItem> sis = SIR.SelectAll().ToList();

            }
        }

        List<ListItem> GetListItems(Models.CRF_Item item)
        {
            List<ListItem> listItems = new List<ListItem>();
            string[] responseOptionText = item.ResponseOptionText.Replace("\\,", "^^").Split(',');
            string[] responseValues = item.ResponseValuesOrCalculation.Split(',');
            for (int i = 0; i < responseOptionText.Length; i++)
            {
                ListItem li = new ListItem(responseOptionText[i].Replace("^^", ","), responseValues[i]);
                listItems.Add(li);
            }
            return listItems;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            GetInfo(((Button)sender).Parent.Parent, _crf);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

        }
    }
}