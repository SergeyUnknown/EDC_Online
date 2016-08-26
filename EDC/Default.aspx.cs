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
                if(columnCount <=1)
                {
                   foreach(var item in section.Items)
                   {
                       if (!string.IsNullOrWhiteSpace(item.Header)) //Header
                       {
                           Label header = new Label();
                           header.Text = item.Header;
                           tp.Controls.Add(header);
                           tp.Controls.Add(new LiteralControl("<br />"));
                       }
                       if(!string.IsNullOrWhiteSpace(item.LeftItemText)) //LeftItemText
                       {
                           Label LIT = new Label();
                           LIT.Text = item.LeftItemText;
                           tp.Controls.Add(LIT);
                       }
                       Control addedControl = new Control();
                       switch (item.ResponseType)
                       {
                           case Core.ResponseType.Text:
                               {
                                   TextBox tb = new TextBox();
                                   if (item.DataType == Core.DataType.INT)
                                       tb.TextMode = TextBoxMode.Number;
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
                                   addedControl = cb;
                                   break;
                               }
                           case Core.ResponseType.Radio:
                               {
                                   RadioButtonList rb = new RadioButtonList();
                                   rb.Items.AddRange(GetListItems(item).ToArray());
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
                       addedControl.ID = item.Identifier;
                       tp.Controls.Add(addedControl);
                       if (!string.IsNullOrWhiteSpace(item.Units)) //Units
                       {
                           Label Units = new Label();
                           Units.Text = item.Units;
                           tp.Controls.Add(Units);
                       }
                       if (!string.IsNullOrWhiteSpace(item.RightItemText)) //RightItemText
                       {
                           Label RIT = new Label();
                           RIT.Text = item.RightItemText;
                           tp.Controls.Add(RIT);
                       }

                       tp.Controls.Add(new LiteralControl("<br />"));
                   }
                }

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