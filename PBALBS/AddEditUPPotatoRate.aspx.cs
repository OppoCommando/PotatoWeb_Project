using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PBALBS
{
   public partial class AddEditUPPotatoRate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            if (!IsPostBack)
            {
                ClearControls();
            }
        }

        protected void ClearControls()
        {
            LoadPotatoType();
            trMsg.Visible = false;
            ddlDay.SelectedValue = DateTime.Now.Day.ToString();
            ddlMonth.SelectedValue = DateTime.Now.Month.ToString();
            ddlYear.SelectedValue = DateTime.Now.Year.ToString();
            LoadDailyRate();
           
        }
        protected void LoadPotatoType()
        {
            BusinessLayer.PotatoType ObjPotato = new BusinessLayer.PotatoType();
            string Statename = "Uttar Pradesh";
            DataTable dt = ObjPotato.GetAll(Statename);
            ViewState["PotatoType"] = dt;
            dd2PotatoType.DataSource = dt;
            dd2PotatoType.DataBind();

        }




        protected void LoadDailyRate()
        {
            BusinessLayer.PotatoRate ObjRate = new BusinessLayer.PotatoRate();

            string Date = ddlDay.SelectedValue.Trim() + "/" + ddlMonth.SelectedValue.Trim() + "/" + ddlYear.SelectedValue.Trim() + " 00:00:00.000";
            string PotatoType =dd2PotatoType.SelectedValue.Trim();
            int PotatoTypeId = Convert.ToInt32(PotatoType);
            string d = Date;

            DataTable dt = ObjRate.GetUpRateByDate(Date, PotatoTypeId);
            if (dt != null)
            {
                dgvBlock.DataSource = dt;
                dgvBlock.DataBind();
            }
        }

        protected string FitRate(string Rate)
        {
            string FinalRate = "";
            switch (Rate.Trim().Length)
            {
                case 0:
                    FinalRate = "000";
                    break;
                case 1:
                    FinalRate = "00" + Rate;
                    break;
                case 2:
                    FinalRate = "0" + Rate;
                    break;
                case 3:
                    FinalRate = Rate;
                    break;
                case 4:
                    FinalRate = Rate;
                    break;
            }

            return FinalRate;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        protected void btnGetPrice_Click(object sender, EventArgs e)
        {
            trMsg.Visible = false;
            LoadDailyRate();
        }

        protected void Save()
        {
            BusinessLayer.PotatoRate ObjRate = new BusinessLayer.PotatoRate();
            string Date = ddlDay.SelectedValue.Trim() + "/" + ddlMonth.SelectedValue.Trim() + "/" + ddlYear.SelectedValue.Trim() + " 00:00:00.000";
            string PotatoType = dd2PotatoType.SelectedValue.Trim();
            int PotatoTypeId = Convert.ToInt32(PotatoType);

            DataTable dt = new DataTable();
            dt.Columns.Add("BlockId");
            dt.Columns.Add("Bond");
            dt.Columns.Add("Avg");
            dt.Columns.Add("Dala");
            DataRow dr;

            foreach (GridViewRow row in dgvBlock.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    string BlockId = dgvBlock.DataKeys[row.RowIndex].Value.ToString();
                    string Bond = FitRate(((TextBox)row.FindControl("txtBond")).Text.Trim());
                    string Avg = FitRate(((TextBox)row.FindControl("txtAvg")).Text.Trim());
                    string Dala = FitRate(((TextBox)row.FindControl("txtDala")).Text.Trim());

                    dr = dt.NewRow();
                    dr["BlockId"] = BlockId;
                    dr["Bond"] = Bond;
                    dr["Avg"] = Avg;
                    dr["Dala"] = Dala;
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();

                }
            }

            ObjRate.UPSave(Date, dt, PotatoTypeId);
            trMsg.Visible = true;

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        protected void btnSaveAndSend_Click(object sender, EventArgs e)
        {
            Save();
            Response.Redirect("SendSMS.aspx?day=" + ddlDay.SelectedValue.Trim() + "&month=" + ddlMonth.SelectedValue.Trim() + "&year=" + ddlYear.SelectedValue.Trim());

        }

        protected void btnPartialSave_Click(object sender, EventArgs e)
        {
            Save();
            //Page.ClientScript.RegisterStartupScript(GetType(), "javascript", "alert('Saved Successfully');", true);
        }
    }


}