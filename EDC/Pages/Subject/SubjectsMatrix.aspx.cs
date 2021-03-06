﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EDC.Pages.Subject
{
    public partial class SubjectsMatrix : System.Web.UI.Page
    {
        Models.Repository.SubjectRepository SR = new Models.Repository.SubjectRepository();
        Models.Repository.SubjectsCRFRepository SCR = new Models.Repository.SubjectsCRFRepository();
        Models.Repository.EventRepository ER = new Models.Repository.EventRepository();
        Models.Repository.CRFInEventRepository CIER = new Models.Repository.CRFInEventRepository();

        int pageSize = 50;
        static int recordCount = 0;
        static List<Models.Subject> _subjects;
        static List<Models.Event> _events;
        List<Models.CRFInEvent> _eventCRFs = new List<Models.CRFInEvent>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.IsInRole(Core.Roles.Administrator.ToString()) || User.IsInRole(Core.Roles.Investigator.ToString()) || User.IsInRole(Core.Roles.Principal_Investigator.ToString()))
            {
                dtInfo.ViewButton = true;
            }
            _subjects = SR.SelectAllForUser(User).Skip((CurrentPage - 1) * pageSize).Take(pageSize).ToList();
            _events = ER.SelectAll().OrderBy(x => x.Position).ToList();
            LoadMatrix();
            LoadDTDataItem();

        }

        private int GetPageFromRequest()
        {
            int page;
            string reqValue = (string)RouteData.Values["page"] ?? Request.QueryString["page"];
            return reqValue != null && int.TryParse(reqValue, out page) && page > 0 ? page : 1;
        }

        //номер текущей страницы
        protected int CurrentPage
        {
            get
            {
                int page;
                page = GetPageFromRequest();
                return page > MaxPageCount ? MaxPageCount : page;
            }
        }

        //максимальный номер страницы
        protected int MaxPageCount
        {
            get
            {
                return (int)Math.Ceiling((decimal)SR.SelectAllForUser(User).Count() / pageSize);
            }
        }

        void LoadDTDataItem()
        {
            string pageInfo = String.Format(Localization.Page, CurrentPage, MaxPageCount);
            DownTableDataItem dtDataItem = new DownTableDataItem(
                "Добавить субъекта",
                "~/Subjects/Add",
                CurrentPage,
                MaxPageCount,
                "~/SubjectsMatrix",
                Core.DropDownListItems25,
                pageSize, pageInfo);

            dtInfo.DataItem = dtDataItem;
            dtInfo.DataBind();
        }

        protected void dtInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            pageSize = dtInfo.DropDownSelectedValue;
            int maxRecordsOnPage = (CurrentPage * pageSize) > recordCount ? recordCount : (CurrentPage * pageSize);

            LoadDTDataItem();
        }
        
        bool ShowDeleted()
        {
            return User.IsInRole(Core.Roles.Data_Manager.ToString()) || User.IsInRole(Core.Roles.Administrator.ToString());
        }
        void LoadMatrix()
        {
            tMatrix.Rows.Clear();
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            //////////////////////////Head///////////////////////
            Label lb = new Label();
            lb.Text = "Номер субъекта";
            tc.Controls.Add(lb);
            tc.RowSpan = 2;
            tr.Cells.Add(tc);
            TableRow eventsNameRow = new TableRow();
            TableRow crfNameRow = new TableRow();
            for(int i =0;i< _events.Count;i++)
            {
                
                tc = new TableCell();
                lb = new Label();
                lb.Text = _events[i].Name;
                tc.Controls.Add(lb);
                long eventId = _events[i].EventID;
                List<Models.CRFInEvent> eventCRFs = CIER.GetManyByFilter(x=>x.EventID == eventId).OrderBy(x=>x.Position).ToList();
                _eventCRFs.AddRange(eventCRFs);
                tc.ColumnSpan = eventCRFs.Count;
                tr.Cells.Add(tc);
                for(int y=0;y<eventCRFs.Count;y++)
                {
                    tc = new TableCell();
                    lb = new Label();
                    lb.Text = string.IsNullOrWhiteSpace(eventCRFs[y].CRF.RussianName) ? eventCRFs[y].CRF.Name : eventCRFs[y].CRF.RussianName;
                    tc.Controls.Add(lb);
                    crfNameRow.Cells.Add(tc);
                }
            }
            tMatrix.Rows.Add(tr);
            tMatrix.Rows.Add(crfNameRow);
            ///////////////////////////////////////////////////

            for(int i =0;i< _subjects.Count;i++)
            {
                if (!ShowDeleted() && _subjects[i].IsDeleted)
                    continue;
                tr = new TableRow();
                tc = new TableCell();
                lb = new Label();

                lb.Text = _subjects[i].Number;
                tc.Controls.Add(lb);
                tr.Cells.Add(tc);

                //////////////////Кнопки/////////////////////////////
                for (int y = 0; y < _eventCRFs.Count; y++)
                {
                    tc = new TableCell();
                    Button btn = new Button();
                    Models.SubjectsCRF _sc = SCR.SelectByID(_subjects[i].SubjectID, _eventCRFs[y].EventID, _eventCRFs[y].CRFID);
                    btn.PostBackUrl = string.Format("~/Subjects/{0}/{1}/{2}", _subjects[i].SubjectID, _eventCRFs[y].EventID, _eventCRFs[y].CRFID);
                    if (_sc != null)
                    {
                        if (_sc.IsDeleted)
                            btn.CssClass = "ActionIc Delete";
                        else if (_sc.IsStopped)
                            btn.CssClass = "ActionIc Lock";
                        else if (_sc.IsStopped)
                            btn.CssClass = "ActionIc Stopped";
                        else if (_sc.IsCheckAll)
                            btn.CssClass = "ActionIc CheckAll";
                        else if (_sc.IsApprove)
                            btn.CssClass = "ActionIc Approve";
                        else if (_sc.IsEnd)
                            btn.CssClass = "ActionIc End";
                        else if (_sc.IsStart)
                            btn.CssClass = "ActionIc Start";

                    }
                    else
                    {
                        if (_subjects[i].IsDeleted)
                            btn.CssClass = "ActionIc Delete";
                        else
                            btn.CssClass = "ActionIc Unplaned";
                    }

                    tc.Controls.Add(btn);
                    tr.Cells.Add(tc);
                }
                ////////////////////////////////////////////////
                tMatrix.Rows.Add(tr);
            }

        }
    }
}