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

        static Models.Subject _subject = new Models.Subject();
        static bool Editing = true;

        static List<Models.MedicalCenter> _MCs;
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
                    Editing = true;

                }
            }

        }

        void LoadMC()
        {
            _MCs = MCR.SelectAll().ToList();
            ddlCenters.Items.Add(new ListItem("Выберите.."));
            for (int i = 0; i < _MCs.Count; i++)
            {
                ddlCenters.Items.Add(new ListItem(_MCs[i].Name));
            }
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

            if (!Editing)
                _subject = new Models.Subject();

            if(ddlCenters.SelectedIndex==0)
            {
                labelStatus.Visible = true;
                labelStatus.ForeColor = System.Drawing.Color.Red;
                labelStatus.Text = "Не выбран Медицинский центр";
                return;
            }

            _subject.MedicalCenterID = _MCs[ddlCenters.SelectedIndex - 1].MedialCenterID;
            _subject.CreatedBy = User.Identity.Name;
            _subject.CreationDate = DateTime.Now;
            _subject.Number = tbNumber.Text;
            _subject.InclusionDate = Convert.ToDateTime(tbDate.Text);

            if (Editing)
            {
                SR.Update(_subject);
            }
            else
            {
                SR.Create(_subject);
            }
            SR.Save();
            Response.Redirect("~/Subjects");
        }
    }
}