using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace PBALBS
{
    public partial class SubscriptionApproval : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
                Response.Redirect("Login.aspx");

            if (!IsPostBack)
            {
                LoadFinancialYear();
                LoadMember();
                LoadBlock();

                if (HttpContext.Current.User.IsInRole("Agent"))
                {
                    dgvSubscriptionList.Columns[9].Visible = false;
                }
            }
        }

        protected void LoadFinancialYear()
        {
            BusinessLayer.Subscription objSubscription = new BusinessLayer.Subscription();
            DataTable dt = new DataTable();

            dt = objSubscription.FinancialYear_GetAll();

            if (dt != null)
            {
                ddlYear.DataSource = dt;
                ddlYear.DataTextField = "FinancialYear";
                ddlYear.DataValueField = "FinancialYearId";
                ddlYear.DataBind();
            }
            ListItem li = new ListItem("--SELECT--", "0");
            ddlYear.Items.Insert(0, li);
        }

        protected void LoadBlock()
        {
            BusinessLayer.Block objBlock = new BusinessLayer.Block();
            DataTable dt = new DataTable();
            string Statename = "West Bengal";
            dt = objBlock.GetAll(Statename);

            if (dt != null)
            {
                ddlBlock.DataSource = dt;
                ddlBlock.DataTextField = "BlockName";
                ddlBlock.DataBind();
            }
            ListItem li = new ListItem("--SELECT--", "0");
            ddlBlock.Items.Insert(0, li);
        }

        protected void LoadMember()
        {
            BusinessLayer.Member ObjMember = new BusinessLayer.Member();
            DataTable dt = ObjMember.GetAll("", 0, "", "", "", "", 1, "");

            if (dt != null)
            {
                ddlMember.DataSource = dt;
                ddlMember.DataTextField = "MemberNameFull";
                ddlMember.DataValueField = "MemberId";
                ddlMember.DataBind();

            }

            ListItem li = new ListItem("--SELECT--", "0");
            ddlMember.Items.Insert(0, li);
            ddlBlock.Items.Insert(0, li);
        }

        protected void LoadSubscriptionList()
        {
            BusinessLayer.Subscription objSubscription = new BusinessLayer.Subscription();
            Entity.Subscription subscription = new Entity.Subscription();
            DataTable dt = new DataTable();

            subscription.MemberId = int.Parse(ddlMember.SelectedValue);
            subscription.FinancialYearId = int.Parse(ddlYear.SelectedValue);
            subscription.EntryDate = DateTime.MinValue;
            subscription.EntryDateTo = DateTime.MinValue;
            subscription.Status = Convert.ToInt32(ddlStatus.SelectedValue);
            subscription.MobileNo = txtMobNo.Text;
            subscription.BlockName = ddlBlock.SelectedItem.Text;
            subscription.Amount = (txtAmount.Text == "") ? 0 : Convert.ToDecimal(txtAmount.Text);
               
            dt = objSubscription.Subscription_GetAll(subscription);
            if (dt != null)
            {
                dgvSubscriptionList.DataSource = dt;
                dgvSubscriptionList.DataBind();
            }
        }

        protected void dgvSubscriptionList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvSubscriptionList.PageIndex = e.NewPageIndex;
            LoadSubscriptionList();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadSubscriptionList();
        }

        protected void dgvSubscriptionList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int SubscriptionId = Convert.ToInt32(dgvSubscriptionList.DataKeys[e.NewEditIndex].Values[0]);
            int MemberId = Convert.ToInt32(dgvSubscriptionList.DataKeys[e.NewEditIndex].Values[1]);
            Response.Redirect("AddEditMember.aspx?SubId=" + SubscriptionId + "&id=" + MemberId);
        }
    }
}