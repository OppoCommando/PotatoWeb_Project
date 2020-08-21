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
    public partial class SubscriptionList : System.Web.UI.Page
    {
        public int CurrentPageIndex
        {
            get { return Convert.ToInt32(ViewState["CurrentPageIndex"]); }
            set { ViewState["CurrentPageIndex"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetNoStore();
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadDistrict();
                LoadMemberList();

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

                //((Literal)e.Row.FindControl("ltrSl")).Text = (e.Row.RowIndex + 1).ToString();
            }
        }
    }
}