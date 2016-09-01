using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EDC.Pages.Subject
{
    public partial class SubjectsCRFPage : System.Web.UI.Page
    {
        Models.Repository.CRFRepository CRFR = new Models.Repository.CRFRepository();
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

                Table table = new Table(); //таблица итемов
                TableRow tr = new TableRow();

                Button btnSave = new Button();
                btnSave.ID = "btnSave";
                btnSave.Click += btnSave_Click;
                btnSave.Text = "Сохранить";

                for (int i = 0; i < section.Items.Count; i++)
                {
                    var item = section.Items[i];
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
                        table.Rows.Add(_tr);
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
                        table.Rows.Add(_tr);
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
                    Control addedControl = new Control();
                    switch (item.ResponseType)
                    {
                        case Core.ResponseType.Text:
                            {
                                TextBox tb = new TextBox();
                                if (item.DataType == Core.DataType.INT)
                                    tb.TextMode = TextBoxMode.Number;
                                if (item.DataType == Core.DataType.REAL)
                                {
                                    AjaxControlToolkit.FilteredTextBoxExtender FTBE = new AjaxControlToolkit.FilteredTextBoxExtender();
                                    FTBE.TargetControlID = item.Identifier;
                                    FTBE.ValidChars = ".";
                                    FTBE.FilterType = AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.Custom;
                                    tc.Controls.Add(FTBE);
                                }
                                if (item.DataType == Core.DataType.DATE)
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
                                }
                                addedControl = tb;
                                break;
                            }
                        case Core.ResponseType.Textarea:
                            {
                                TextBox tb = new TextBox();
                                tb.TextMode = TextBoxMode.MultiLine;
                                addedControl = tb;
                                break;
                            }
                        case Core.ResponseType.Checkbox | Core.ResponseType.MultiSelect:
                            {
                                CheckBoxList cb = new CheckBoxList();
                                cb.Items.AddRange(GetListItems(item).ToArray());
                                cb.CssClass = item.ResponseLayout;
                                addedControl = cb;
                                break;
                            }
                        case Core.ResponseType.Radio:
                            {
                                RadioButtonList rb = new RadioButtonList();
                                rb.Items.AddRange(GetListItems(item).ToArray());
                                rb.CssClass = "radibuttonTable " + item.ResponseLayout;
                                addedControl = rb;
                                break;
                            }
                        case Core.ResponseType.SingleSelect:
                            {
                                DropDownList ddl = new DropDownList();
                                ddl.Items.AddRange(GetListItems(item).ToArray());
                                addedControl = ddl;
                                break;
                            }
                    }

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
                        RFV.ErrorMessage = "Данное поле обязательно для заполнения";
                        RFV.CssClass = "field-validation-error";
                        tc.Controls.Add(RFV);
                    }

                    tr.Cells.Add(tc);

                    if (i < section.Items.Count - 1)
                    {
                        if (section.Items[i + 1].ColumnNumber <= section.Items[i].ColumnNumber)
                        {
                            table.Rows.Add(tr);
                            tr = new TableRow();
                        }

                    }
                    else
                    {
                        table.Rows.Add(tr);
                        tc = new TableCell();
                        tc.Controls.Add(btnSave);
                        tr.Cells.Add(tc);
                        table.Rows.Add(tr);
                    }
                }
                tp.Controls.Add(table);
                tcCRF.Tabs.Add(tp);
                ScriptManager.GetCurrent(this).RegisterPostBackControl(btnSave);
            }
        }

        void GetInfo(Control form, Models.CRF crf)
        {
            List<Models.CRF_Item> items = crf.Items;
            foreach (var item in items)
            {
                Control _control = form.FindControl(item.Identifier);
                Type type = _control.GetType();
                string value;
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
    }
}