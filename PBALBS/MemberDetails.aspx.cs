using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace PBALBS
{
    public partial class MemberDetails : System.Web.UI.Page
    {
        public int CurrentPageIndex
        {
            get { return Convert.ToInt32(ViewState["CurrentPageIndex"]); }
            set { ViewState["CurrentPageIndex"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            if (!IsPostBack)
            {
                LoadDistrict();
                LoadMemberList();
                if (HttpContext.Current.User.IsInRole("Admin"))
                {
                    hidden_role.Value = "Admin";
                }
                else { hidden_role.Value = "Bibhas"; }

                if (HttpContext.Current.User.IsInRole("Agent"))
                {
                    dgvMember.Columns[14].Visible = false;
                    dgvMember.Columns[15].Visible = false;
                }
            }
        }

        protected void LoadDistrict()
        {
            BusinessLayer.District ObjDistrict = new BusinessLayer.District();
            string Statename = "West Bengal";
            DataTable dt = ObjDistrict.GetAll(Statename);
            if (dt != null)
            {
                DataRow dr = dt.NewRow();
                dr["DistrictId"] = "0";
                dr["DistrictName"] = "All";
                dt.Rows.InsertAt(dr, 0);
                dt.AcceptChanges();
                ddlDistrict.DataSource = dt;
                ddlDistrict.DataBind();
            }
        }

        protected void LoadMemberList()
        {
            BusinessLayer.Member ObjMember = new BusinessLayer.Member();
            string FormNo = txtFormNo.Text.Trim();
            int DistrictId = int.Parse(ddlDistrict.SelectedValue.Trim());
            string BlockName = txtBlockName.Text;
            string MemberName = txtMemberName.Text.Trim();
            string MobileNo = txtMobileNo.Text.Trim();
            string ExpirationDate = txtExpirationDate.Text.Trim();
            string SMSAmt = txtSMSSubscriberAmt.Text.Trim();
            if (ExpirationDate.Length != 0)
            {
                string[] Arr = ExpirationDate.Split('/');
                ExpirationDate = Arr[2].Trim() + "/" + Arr[1].Trim() + "/" + Arr[0].Trim();
            }

            int SearchType = Convert.ToInt32(ddlExpiration.SelectedValue.Trim());
            DataTable dt = ObjMember.GetAll(FormNo, DistrictId, BlockName, MemberName, MobileNo, ExpirationDate, SearchType, SMSAmt);

            if (Session["PageIndex"] == null)
            {
                Session["PageIndex"] = 0;
            }

            if (dt != null)
            {
                dgvMember.PageIndex = int.Parse(Session["PageIndex"].ToString());
                dgvMember.DataSource = dt;
                dgvMember.DataBind();

                Session["MemberList"] = dt;
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Session["PageIndex"] = 0;
            LoadMemberList();
        }

        protected void dgvMember_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Session["PageIndex"] = e.NewPageIndex;
            LoadMemberList();
        }

        protected void dgvMember_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int Id = Convert.ToInt32(dgvMember.DataKeys[e.NewEditIndex].Value);
            Response.Redirect("AddEditMember.aspx?id=" + Id);
        }

        protected void dgvMember_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int Id = Convert.ToInt32(dgvMember.DataKeys[e.RowIndex].Value);
            BusinessLayer.Member ObjMember = new BusinessLayer.Member();
            ObjMember.Delete(Id);
            LoadMemberList();
        }

        protected void dgvMember_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkbtn = (LinkButton)e.Row.FindControl("lnkMemberName");
                lnkbtn.CommandArgument = dgvMember.DataKeys[e.Row.RowIndex].Value.ToString();
                string IsExpired = ((HiddenField)e.Row.FindControl("HidIsExpired")).Value;

                if (IsExpired == "YES")
                {
                    e.Row.CssClass = "ExpiredRowStyle";
                    lnkbtn.Style.Add("text-decoration", "underline");
                    lnkbtn.ToolTip = "Activate Now";
                    lnkbtn.ForeColor = System.Drawing.Color.White;
                    lnkbtn.Font.Bold = true;

                }
                else
                {
                    e.Row.CssClass = "ActiveRowStyle";
                    lnkbtn.Style.Add("text-decoration", "none");
                    lnkbtn.ToolTip = "Activated";
                    lnkbtn.ForeColor = System.Drawing.Color.White;
                    lnkbtn.Font.Bold = false;

                }

                ((LinkButton)e.Row.FindControl("lnkEdit")).ForeColor = System.Drawing.Color.White;
                ((LinkButton)e.Row.FindControl("lnkDelete")).ForeColor = System.Drawing.Color.White;
            }
        }

        protected void dgvMember_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Activate"))
            {
                int Id = Convert.ToInt32(e.CommandArgument.ToString());
                BusinessLayer.Member ObjMember = new BusinessLayer.Member();
                ObjMember.QuickUpdate(Id);
                LoadMemberList();
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            if (dgvMember.Rows.Count > 0)
            {
                dgvMember.Columns[0].Visible = false;
                dgvMember.Columns[12].Visible = false;
                dgvMember.Columns[13].Visible = false;

                Save(dgvMember, "Total Report.xls");
            }
        }


        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int rowsEff = 0;

            foreach (GridViewRow gvr in dgvMember.Rows)
            {
                if (((CheckBox)gvr.FindControl("chk")).Checked)
                {
                    int memberId = Convert.ToInt32(dgvMember.DataKeys[gvr.RowIndex].Values[0].ToString());
                    DateTime endDate;
                    if (txtNewEndDate.Text.Length != 0)
                    {
                        string[] Arr = txtNewEndDate.Text.Split('/');
                        endDate = Convert.ToDateTime(Arr[2].Trim() + "-" + Arr[1].Trim() + "-" + Arr[0].Trim());
                        rowsEff += BusinessLayer.Member.EndDate_Update(memberId, endDate);
                    }
                }
            }

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "script", "<script>alert('" + rowsEff.ToString() + " rows updated successfully...');</script>", false);
            LoadMemberList();
        }

        public void Save(GridView oGrid, string exportFile)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/vnd.ms-word";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename = MemberList_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xls");
            HttpContext.Current.Response.AddHeader("Content-Type", "application/Excel");

            HttpContext.Current.Response.Charset = "";

            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            oGrid.GridLines = GridLines.Both;
            oGrid.RenderControl(oHtmlTextWriter);
            HttpContext.Current.Response.Write(oStringWriter.ToString());
            HttpContext.Current.Response.End();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
    }
}
