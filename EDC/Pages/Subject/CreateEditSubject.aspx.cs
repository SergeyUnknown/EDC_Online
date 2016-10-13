using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EDC.Pages.Subject
{
    public partial class CreateEditSubject : System.Web.UI.Page
    {
        Models.Repository.SubjectRepository SR = new Models.Repository.SubjectRepository();
        Models.Repository.MedicalCenterRepository MCR = new Models.Repository.MedicalCenterRepository();
        Models.Repository.AuditsRepository AR = new Models.Repository.AuditsRepository();
        Models.Repository.UserProfileRepository UPR = new Models.Repository.UserProfileRepository();

        Models.Subject _subject
        {
            get 
            {
                if (Session["ces_subject"] == null)
                    Session["ces_subject"] = new Models.Subject();
                return (Models.Subject)Session["ces_subject"];
            }
            set { Session["ces_subject"] = value; }
        }
        bool Editing
        {
            get 
            {
                if(Session["ces_Editing"] == null)
                    Session["ces_Editing"] = true;
                return (bool)Session["ces_Editing"];
            }
            set { Session["ces_Editing"] = value; }
        }

        List<Models.MedicalCenter> _MCs
        {
            get 
            {
                if (Session["ces_MCs"] == null)
                    Session["ces_MCs"] = new List<Models.MedicalCenter>();
                return (List<Models.MedicalCenter>)Session["ces_MCs"];
            }
            set 
            {
                Session["ces_MCs"] = value;

            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMC();

                if (Request.Url.ToString().IndexOf("Edit") == -1)
                {
                    btnOk.Text = "Добавить";
                    Title = "Добавление Субъекта";
                    Editing = false;
                }
                else
                {
                    _subject = SR.SelectByID(GetIDFromRequest());
                    if (_subject == null)
                        throw new NullReferenceException();
                    btnOk.Text = "Изменить";
                    Title = "Редактирование Субъекта ";
                    tbDate.Text = _subject.InclusionDate.ToShortDateString();
                    tbNumber.Text = _subject.Number;

                    ddlCenters.SelectedIndex = _MCs.FindIndex(x=>x.MedialCenterID == _subject.MedicalCenterID) + 1;
                    Editing = true;

                }
            }

        }

        void LoadMC()
        {
            Models.UserProfile up = UPR.SelectByID(System.Web.Security.Membership.GetUser(User.Identity.Name).ProviderUserKey);
            if (up != null)
                _MCs = up.MedicalCenters.Select(x => x.MedicalCenter).ToList();
            if (_MCs.Count != 0)
            {
                ddlCenters.Items.Add(new ListItem("Выберите.."));
                for (int i = 0; i < _MCs.Count; i++)
                {
                    ddlCenters.Items.Add(new ListItem(_MCs[i].Name));
                }
            }
            else
                ddlCenters.Items.Add(new ListItem("Вы не состоите не в одном центре"));
        }
        long GetIDFromRequest()
        {
            long id;
            string strID = (string)RouteData.Values["id"] ?? Request.QueryString["id"];
            if (string.IsNullOrWhiteSpace(strID))
            {
                throw new ArgumentException("Необходимо указать ID Субъекта");
            }
            if (!long.TryParse(strID, out id))
            {
                throw new ArgumentException("ID указан в неверном формате");
            }
            return id;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Subjects/");
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            if (ddlCenters.SelectedIndex == 0)
            {
                labelStatus.Visible = true;
                labelStatus.ForeColor = System.Drawing.Color.Red;
                labelStatus.Text = "Не выбран Медицинский центр";
                return;
            }
            if (!Editing)
                _subject = new Models.Subject();
            else
            {
                _subject = SR.SelectByID(_subject.SubjectID);
                if (_subject.Number != tbNumber.Text)
                {
                    Models.Audit audit = new Models.Audit();
                    audit.UserName = User.Identity.Name;
                    audit.UserID = (Guid)System.Web.Security.Membership.GetUser(User.Identity.Name).ProviderUserKey;
                    audit.SubjectID = _subject.SubjectID;
                    audit.OldValue = _subject.Number;
                    audit.NewValue = tbNumber.Text;
                    audit.ActionDate = DateTime.Now;
                    audit.FieldName = "Номер субъекта";
                    audit.ActionType = Core.AuditActionType.SubjectParam;
                    audit.ChangesType = Core.AuditChangesType.Update;
                    AR.Create(audit);
                }
                if (_subject.MedicalCenterID != _MCs[ddlCenters.SelectedIndex - 1].MedialCenterID)
                {
                    Models.Audit audit = new Models.Audit();
                    audit.UserName = User.Identity.Name;
                    audit.UserID = (Guid)System.Web.Security.Membership.GetUser(User.Identity.Name).ProviderUserKey;
                    audit.SubjectID = _subject.SubjectID;
                    audit.OldValue = _subject.MedicalCenter.Name;
                    audit.NewValue = _MCs[ddlCenters.SelectedIndex - 1].Name;
                    audit.ActionDate = DateTime.Now;
                    audit.FieldName = "Мед. Центр";
                    audit.ActionType = Core.AuditActionType.SubjectParam;
                    audit.ChangesType = Core.AuditChangesType.Update;
                    AR.Create(audit);
                }
                if (_subject.InclusionDate != Convert.ToDateTime(tbDate.Text))
                {
                    Models.Audit audit = new Models.Audit();
                    audit.UserName = User.Identity.Name;
                    audit.UserID = (Guid)System.Web.Security.Membership.GetUser(User.Identity.Name).ProviderUserKey;
                    audit.SubjectID = _subject.SubjectID;
                    audit.OldValue = _subject.InclusionDate.ToShortDateString();
                    audit.NewValue = tbDate.Text;
                    audit.ActionDate = DateTime.Now;
                    audit.FieldName = "Дата включения";
                    audit.ActionType = Core.AuditActionType.SubjectParam;
                    audit.ChangesType = Core.AuditChangesType.Update;
                    AR.Create(audit);
                }
            }

            _subject.MedicalCenterID = _MCs[ddlCenters.SelectedIndex - 1].MedialCenterID;
            _subject.CreatedBy = User.Identity.Name;
            _subject.CreationDate = DateTime.Now;
            _subject.Number = tbNumber.Text;
            _subject.InclusionDate = Convert.ToDateTime(tbDate.Text);

            if (Editing)
            {
                SR.Update(_subject);
                SR.Save();
            }
            else
            {
                _subject = SR.Create(_subject);
                SR.Save();

                Models.Audit audit = new Models.Audit();
                audit.UserName = User.Identity.Name;
                audit.UserID = (Guid)System.Web.Security.Membership.GetUser(User.Identity.Name).ProviderUserKey;
                audit.SubjectID = _subject.SubjectID;
                audit.NewValue = tbNumber.Text;
                audit.ActionDate = DateTime.Now;
                audit.FieldName = "Номер субъекта";
                audit.ActionType = Core.AuditActionType.SubjectParam;
                audit.ChangesType = Core.AuditChangesType.Create;
                AR.Create(audit);

                audit = new Models.Audit();
                audit.UserName = User.Identity.Name;
                audit.UserID = (Guid)System.Web.Security.Membership.GetUser(User.Identity.Name).ProviderUserKey;
                audit.SubjectID = _subject.SubjectID;
                audit.NewValue = _MCs[ddlCenters.SelectedIndex - 1].Name;
                audit.ActionDate = DateTime.Now;
                audit.FieldName = "Мед. Центр";
                audit.ActionType = Core.AuditActionType.SubjectParam;
                audit.ChangesType = Core.AuditChangesType.Create;
                AR.Create(audit);

                audit = new Models.Audit();
                audit.UserName = User.Identity.Name;
                audit.UserID = (Guid)System.Web.Security.Membership.GetUser(User.Identity.Name).ProviderUserKey;
                audit.SubjectID = _subject.SubjectID;
                audit.NewValue = tbDate.Text;
                audit.ActionDate = DateTime.Now;
                audit.FieldName = "Дата включения";
                audit.ActionType = Core.AuditActionType.SubjectParam;
                audit.ChangesType = Core.AuditChangesType.Create;
                AR.Create(audit);

            }
            AR.Save();
            Response.Redirect("~/Subjects");
        }
    }
}