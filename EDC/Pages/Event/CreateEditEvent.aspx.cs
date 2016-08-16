using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EDC.Pages.Event
{
    public partial class CreateEditEvent : System.Web.UI.Page
    {
        Models.Repository.EventRepository ER = new Models.Repository.EventRepository();

        static Models.Event _event;
        static bool Editing = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Url.ToString().IndexOf("Edit") == -1)
                {
                    btnOk.Text = "Добавить";
                    Title = "Добавление События";
                    Editing = false;
                }
                else
                {
                    _event = ER.SelectByID(GetIDFromRequest());
                    if (_event == null)
                        throw new NullReferenceException();
                    btnOk.Text = "Изменить";
                    Title = "Редактирование События " + _event.Name;
                    Editing = true;

                    tbName.Text = _event.Name;
                    tbIdentifier.Text = _event.Identifier;
                }
            }
        }

        long GetIDFromRequest()
        {
            long id;
            string strID = (string)RouteData.Values["id"] ?? Request.QueryString["id"];
            if (string.IsNullOrWhiteSpace(strID))
            {
                throw new ArgumentException("Необходимо указать ID редактируемого события");
            }
            if (!long.TryParse(strID, out id))
            {
                throw new ArgumentException("ID указан в неверном формате");
            }
            return id;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Events");
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {

            if (!Editing)
            {
                _event = new Models.Event();
                _event.CreatedBy = User.Identity.Name;
                _event.DateCreation = DateTime.Now;
                _event.Position = ER.SelectAll().Count() + 1;
            }

            _event.Name = tbName.Text;
            _event.Identifier = tbIdentifier.Text;


            if (Editing)
            {
                ER.Update(_event);
            }
            else
            {
                ER.Create(_event);
            }
            ER.Save();
            Response.Redirect("~/Events");
        }
    }
}