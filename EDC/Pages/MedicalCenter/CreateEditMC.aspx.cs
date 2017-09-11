using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EDC.Pages.MedicalCenter
{
    public partial class CreateEditMC : BasePage
    {
        Models.Repository.MedicalCenterRepository mcr;

         Models.MedicalCenter _mc
         {
             get
             {
                 if(Session["pageMC"] == null)
                     Session["pageMC"] = new Models.MedicalCenter();
                 return Session["pageMC"] as Models.MedicalCenter;
             }
             set
             {
                 Session["pageMC"] = value;
             }
         }
        bool Editing
        {
            get
            {
                if (Session["pageMCEditing"] == null)
                    Session["pageMCEditing"] = true;
                return (bool)Session["pageMCEditing"];
            }
            set
            {
                Session["pageMCEditing"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            mcr = new Models.Repository.MedicalCenterRepository();
            if (!IsPostBack)
            {
                if (Request.Url.ToString().IndexOf("Edit") == -1)
                {
                    btnOk.Text = Resources.LocalizedText.Add;
                    Title = "Добавление медицинского центра";
                    Editing = false;
                }
                else
                {
                    _mc = mcr.SelectByID(GetIDFromRequest());
                    if (_mc == null)
                        throw new NullReferenceException();
                    btnOk.Text = Resources.LocalizedText.Edit;
                    Title = "Редактирование медицинского центра " + _mc.Name;
                    Editing = true;

                    tbCity.Text = _mc.City;
                    tbCountry.Text = _mc.Country;
                    tbHouse.Text = _mc.House;
                    tbName.Text = _mc.Name;
                    tbNumber.Text = _mc.Number;
                    tbPhone.Text = _mc.Phone;
                    tbPI.Text = _mc.PrincipalInvestigator;
                    tbRegion.Text = _mc.Region;
                    tbStreet.Text = _mc.Street;
                }
            }

        }

        long GetIDFromRequest()
        {
            long id;
            string strID = (string)RouteData.Values["id"] ?? Request.QueryString["id"];
            if(string.IsNullOrWhiteSpace(strID))
            {
                throw new ArgumentException("Необходимо указать ID редактируемого Мед. Центра");
            }
            if(!long.TryParse(strID,out id))
            {
                throw new ArgumentException("ID указан в неверном формате");
            }
            return id;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MedicalCenters/");
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {

            if (!Editing)
                _mc = new Models.MedicalCenter();
            else
                _mc = mcr.SelectByID(_mc.MedicalCenterID);
            _mc.City = tbCity.Text;
            _mc.Country = tbCountry.Text;
            _mc.House = tbHouse.Text;
            _mc.Name = tbName.Text;
            _mc.Number = tbNumber.Text;
            _mc.Phone = tbPhone.Text;
            _mc.PrincipalInvestigator = tbPI.Text;
            _mc.Region = tbRegion.Text;
            _mc.Street = tbStreet.Text;

            if(Editing)
            {
                mcr.Update(_mc);
            }
            else
            {
                mcr.Create(_mc);
            }
            mcr.Save();
            Response.Redirect("~/MedicalCenters");
        }
    }
}