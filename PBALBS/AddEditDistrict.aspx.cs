using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace PBALBS
{
    public partial class AddEditDistrict : System.Web.UI.Page
    {
        public int DistrictId
        {
            get { return Convert.ToInt32(ViewState["DistrictId"].ToString()); }
            set { ViewState["DistrictId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            if (!IsPostBack)
            {
                DistrictId = 0;
                LoadDistrictList();
            }
        }

        protected void LoadDistrictList()
        {
            BusinessLayer.District ObjDistrict = new BusinessLayer.District();
            string Statename = "West Bengal";
            DataTable dt = ObjDistrict.GetAll(Statename);
            DataRow dr = dt.NewRow();
            dr["DistrictId"] = "0";
            dr["DistrictName"] = "";
            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();
            
            dgvDistrict.DataSource = dt;
            dgvDistrict.DataBind();
        }

        protected void dgvDistrict_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((LinkButton)e.Row.FindControl("lnkUpdate")).CommandArgument = e.Row.RowIndex.ToString();


                string ID = dgvDistrict.DataKeys[e.Row.RowIndex].Value.ToString();
                if (int.Parse(ID) == 0)
                {
                    ((LinkButton)e.Row.FindControl("lnkUpdate")).Text = "Add";

                }
                else
                {
                    ((LinkButton)e.Row.FindControl("lnkUpdate")).Text = "Update";

                }

            }
        }

        protected void dgvDistrict_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DistrictId = Convert.ToInt32(dgvDistrict.DataKeys[e.RowIndex].Value.ToString());
            BusinessLayer.District ObjDistrict = new BusinessLayer.District();
            ObjDistrict.Delete(DistrictId);
            LoadDistrictList();
        }

        protected void dgvDistrict_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddUpdate"))
            {
                int RowIndex = Convert.ToInt32(e.CommandArgument.ToString());
                DistrictId = Convert.ToInt32(dgvDistrict.DataKeys[RowIndex].Value.ToString());
                BusinessLayer.District ObjDistrict = new BusinessLayer.District();
                Entity.District District = new Entity.District();
                District.DistrictId = DistrictId;
                District.DistrictName = ((TextBox)dgvDistrict.Rows[RowIndex].FindControl("txtDistrict")).Text;
                ObjDistrict.Save(District);
                LoadDistrictList();

            }
        }
    }
}
