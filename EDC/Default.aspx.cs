using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EDC
{
    public partial class _Default : Page
    {
        Models.Repository.CRFRepository CRFR = new Models.Repository.CRFRepository();
        static List<Models.CRF> _crfs;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _crfs = CRFR.SelectAll().ToList();
                ddlCRF.Items.Add("Выберите...");
                foreach (var item in _crfs.Select(x => x.Name))
                {
                    ddlCRF.Items.Add(item);
                }
            }
        }

        protected void ddlCRF_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if(ddlCRF.SelectedIndex==0)
            {
                return;
            }
            Models.CRF _crf = _crfs[ddlCRF.SelectedIndex-1];
            foreach(Models.CRF_Section section in _crf.Sections)
            {
                AjaxControlToolkit.TabPanel tp = new AjaxControlToolkit.TabPanel();
                tp.HeaderText = section.Title;
                tp.ID = "tp" + section.Label;

                int columnCount = section.Items.Max(x => x.ColumnNumber);

                Table table = new Table(); //таблица итемов
                TableRow tr = new TableRow();
                for(int i=0;i<section.Items.Count;i++)
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
<<<<<<< HEAD
                                rb.CssClass = "radibuttonTable";
=======
                                rb.CssClass = item.ResponseLayout;
>>>>>>> refs/remotes/origin/master
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
                    tr.Cells.Add(tc);

                    if(i<section.Items.Count-1)
                    {
                        if(section.Items[i+1].ColumnNumber<=section.Items[i].ColumnNumber)
                        {
                            table.Rows.Add(tr);
                            tr = new TableRow();
                        }
                        
                    }
                    else
                        table.Rows.Add(tr);
                }
                tp.Controls.Add(table);
                tcCRF.Tabs.Add(tp);

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
    }
}