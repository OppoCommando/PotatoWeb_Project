using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Net;
using System.IO;

namespace PBALBS
{
    public partial class AddEditMember : System.Web.UI.Page
    {
        string API_INDEX;
        public int MemberId
        {
            get { return Convert.ToInt32(ViewState["MemberId"]); }
            set { ViewState["MemberId"] = value; }
        }
        public int SubscriptionId
        {
            get { return Convert.ToInt32(ViewState["SubscriptionId"]); }
            set { ViewState["SubscriptionId"] = value; }
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
                if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString().Trim().Length > 0)
                {
                    MemberId = Convert.ToInt32(Request.QueryString["id"].ToString().Trim());
                    LoadMemberDetails();
                    if (Request.QueryString["SubId"] != null && Request.QueryString["SubId"].ToString().Trim().Length > 0)
                        SubscriptionId = Convert.ToInt32(Request.QueryString["SubId"].ToString().Trim());

                }
                else
                {
                    ClearControls();
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
                dr["DistrictName"] = "--Select--";
                dt.Rows.InsertAt(dr, 0);
                dt.AcceptChanges();
                ddlDistrict.DataSource = dt;
                ddlDistrict.DataBind();
            }
        }

        protected void LoadBlock()
        {
            BusinessLayer.Block ObjBlock = new BusinessLayer.Block();
            string Statename = "West Bengal";
            DataTable dt = ObjBlock.GetAll(Statename);
            if (dt != null)
            {
                DataRow dr = dt.NewRow();
                dr["BlockId"] = "0";
                dr["BlockName"] = "--Select--";
                dt.Rows.InsertAt(dr, 0);
                dt.AcceptChanges();
            }
        }

        protected void LoadMemberDetails()
        {
            BusinessLayer.Member ObjMember = new BusinessLayer.Member();
            Entity.Member Member = new Entity.Member();
            Member = ObjMember.GetAllById(MemberId);
            if (Member != null)
            {
                txtMemberName.Text = Member.MemberName;
                ddlDistrict.SelectedValue = Member.DistrictId.ToString();
                txtBlockName.Text = Member.BlockName;
                txtMobileNo.Text = Member.MobileNo;
                txtStartDate.Text = Member.StartDate.ToString("dd/MM/yyyy");
                txtEndDate.Text = Member.EndDate.ToString("dd/MM/yyyy");
                txtFormNo.Text = Member.FormNo;
                txtFirmName.Text = Member.FirmName;
                txtAddress.Text = Member.Address;
                txtPin.Text = Member.Pin;
                txtLandLine.Text = Member.LandLine;
                ChkLifeMemberShip.Checked = Member.IsLifeMembership;
                ChkSMSSubscriber.Checked = Member.IsYearlySMSSubscriber;
                txtLifeMembershipAmt.Text = String.Format("{0:0.##}", Member.LifeMembershipAmt);
                txtSMSSubscriberAmt.Text = String.Format("{0:0.##}", Member.SMSSubscriberAmt);
                chkPortalMember.Checked = Member.IsPortalMember;
                txtPortalAmount.Text = String.Format("{0:0.##}", Member.PortalMemberAmt);

                lblMessage.Text = "";
            }
        }

        protected void ClearControls()
        {
            MemberId = 0;
            lblMessage.Text = "";
            txtMemberName.Text = "";
            ddlDistrict.SelectedIndex = 0;
            txtBlockName.Text = "";
            txtMobileNo.Text = "";
            txtStartDate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            txtEndDate.Text = DateTime.Now.Date.AddYears(1).ToString("dd/MM/yyyy");
            txtFormNo.Text = "";
            txtFirmName.Text = "";
            txtAddress.Text = "";
            txtPin.Text = "";
            txtLandLine.Text = "";
            ChkLifeMemberShip.Checked = false;
            ChkSMSSubscriber.Checked = true;
            chkPortalMember.Checked = false;
            txtLifeMembershipAmt.Text = "0";
            txtSMSSubscriberAmt.Text = "0";
            txtPortalAmount.Text = "0";

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            BusinessLayer.Member ObjMember = new BusinessLayer.Member();
            Entity.Member Member = new Entity.Member();
            Member.MemberId = MemberId;
            Member.MemberName = txtMemberName.Text.Trim();
            Member.FormNo = txtFormNo.Text.Trim();
            Member.FirmName = txtFirmName.Text.Trim();
            Member.Address = txtAddress.Text.Trim();
            Member.BlockName = txtBlockName.Text;
            Member.DistrictId = int.Parse(ddlDistrict.SelectedValue.Trim());
            Member.Pin = txtPin.Text.Trim();
            Member.MobileNo = txtMobileNo.Text.Trim();
            Member.LandLine = txtLandLine.Text.Trim();

            string[] SDate = txtStartDate.Text.Trim().Split('/');
            Member.StartDate = Convert.ToDateTime(SDate[1].Trim() + "/" + SDate[0].Trim() + "/" + SDate[2].Trim() + " 00:00:00");

            string[] EDate = txtEndDate.Text.Trim().Split('/');
            Member.EndDate = Convert.ToDateTime(EDate[1].Trim() + "/" + EDate[0].Trim() + "/" + EDate[2].Trim() + " 00:00:00");

            Member.IsLifeMembership = ChkLifeMemberShip.Checked;
            Member.IsYearlySMSSubscriber = ChkSMSSubscriber.Checked;
            Member.LifeMembershipAmt = (txtLifeMembershipAmt.Text.Trim().Length == 0) ? 0 : decimal.Parse(txtLifeMembershipAmt.Text.Trim());
            Member.SMSSubscriberAmt = (txtSMSSubscriberAmt.Text.Trim().Length == 0) ? 0 : decimal.Parse(txtSMSSubscriberAmt.Text.Trim());
            Member.IsPortalMember = chkPortalMember.Checked;
            Member.PortalMemberAmt = (txtPortalAmount.Text.Trim().Length == 0) ? 0 : decimal.Parse(txtPortalAmount.Text.Trim());
            if (chkApprovePayment.Checked == true)
                Member.SubscriptionId = SubscriptionId;
            else
                Member.SubscriptionId = 0;

            int rowsaffected = ObjMember.Save(Member);
            if (rowsaffected > 0)
            {
                if (MemberId == 0)
                    NewMemberJoiningAlert();
                if (chkApprovePayment.Checked == true)
                    SendNewSubscriptionAlert(txtMemberName.Text.ToString(), txtSMSSubscriberAmt.Text.Trim());
                ClearControls();
                lblMessage.Text = "Member Information Saved/Updated Successfully";
                lblMessage.BackColor = System.Drawing.Color.Green;
            }
            else
            {
                lblMessage.Text = "Mobile Number Already Exist";
                lblMessage.BackColor = System.Drawing.Color.Red;
            }
        }

        protected void SendNewSubscriptionAlert(string Member, string amount)
        {
            string AssigToNo = txtMobileNo.Text.Trim();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"Dear " + Member + "," + "%0a");
            sb.Append(@"Your Payment of Rs. " + amount + " for Potato Market Rate SMS Service, Successfully Updated." + "%0a");
            sb.Append(@"Thank You" + "%0a");

            string message = (sb.ToString().Length > 158) ? sb.ToString().Substring(0, 158) : sb.ToString();
            string strUrl = GetHTTPAPI(AssigToNo, message);

            WebRequest request = HttpWebRequest.Create(strUrl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream s = (Stream)response.GetResponseStream();
            StreamReader readStream = new StreamReader(s);
            string dataString = readStream.ReadToEnd();
            response.Close();
            s.Close();
            readStream.Close();
        }

        protected void NewMemberJoiningAlert()
        {
            string AssigToNo = txtMobileNo.Text.Trim();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"Welcome to India's Largest Potato Market Rate (Approx.) SMS Service Network. Your Service will Start Soon." + "%0a");
            sb.Append(@"HelpDesk: 9563153631" + "%0a");
            sb.Append(@"Thank You for Joining Us.");

            string message = (sb.ToString().Length > 158) ? sb.ToString().Substring(0, 158) : sb.ToString();
            string strUrl = GetHTTPAPI(AssigToNo, message);

            WebRequest request = HttpWebRequest.Create(strUrl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream s = (Stream)response.GetResponseStream();
            StreamReader readStream = new StreamReader(s);
            string dataString = readStream.ReadToEnd();
            response.Close();
            s.Close();
            readStream.Close();
        }

        private string GetHTTPAPI(string mobiles, string message)
        {
            string API = string.Empty;
            string ROUTE_1 = System.Configuration.ConfigurationSettings.AppSettings["ROUTE_1"];
            API_INDEX = ROUTE_1;

            if (API_INDEX == "1")
                API = string.Format("http://api.mVaayoo.com/mvaayooapi/MessageCompose?user=admin@fourfusionsolutions.com:solution2012&senderID=PBALBS&receipientno={0}&msgtxt={1}&state=1", mobiles, message);
            else if (API_INDEX == "2")
                API = string.Format("http://sms.fourfusiontechnologies.com/new/api/api_http.php?username=FOURFTECH&password=four8956&senderid=PBALBS&to={0}&text={1}&route=Informative&type=text", mobiles, message);

            return API;
        }

        protected void btnSaveWithOutSms_Click(object sender, EventArgs e)
        {
            BusinessLayer.Member ObjMember = new BusinessLayer.Member();
            Entity.Member Member = new Entity.Member();
            Member.MemberId = MemberId;
            Member.MemberName = txtMemberName.Text.Trim();
            Member.FormNo = txtFormNo.Text.Trim();
            Member.FirmName = txtFirmName.Text.Trim();
            Member.Address = txtAddress.Text.Trim();
            Member.BlockName = txtBlockName.Text;
            Member.DistrictId = int.Parse(ddlDistrict.SelectedValue.Trim());
            Member.Pin = txtPin.Text.Trim();
            Member.MobileNo = txtMobileNo.Text.Trim();
            Member.LandLine = txtLandLine.Text.Trim();

            string[] SDate = txtStartDate.Text.Trim().Split('/');
            Member.StartDate = Convert.ToDateTime(SDate[1].Trim() + "/" + SDate[0].Trim() + "/" + SDate[2].Trim() + " 00:00:00");

            string[] EDate = txtEndDate.Text.Trim().Split('/');
            Member.EndDate = Convert.ToDateTime(EDate[1].Trim() + "/" + EDate[0].Trim() + "/" + EDate[2].Trim() + " 00:00:00");

            Member.IsLifeMembership = ChkLifeMemberShip.Checked;
            Member.IsYearlySMSSubscriber = ChkSMSSubscriber.Checked;
            Member.LifeMembershipAmt = (txtLifeMembershipAmt.Text.Trim().Length == 0) ? 0 : decimal.Parse(txtLifeMembershipAmt.Text.Trim());
            Member.SMSSubscriberAmt = (txtSMSSubscriberAmt.Text.Trim().Length == 0) ? 0 : decimal.Parse(txtSMSSubscriberAmt.Text.Trim());
            Member.IsPortalMember = chkPortalMember.Checked;
            Member.PortalMemberAmt = (txtPortalAmount.Text.Trim().Length == 0) ? 0 : decimal.Parse(txtPortalAmount.Text.Trim());
            if (chkApprovePayment.Checked == true)
                Member.SubscriptionId = SubscriptionId;
            else
                Member.SubscriptionId = 0;
            Member.IsMobileAppActivated = ChkMobileAppActivated.Checked;
            Member.DeviceId = txtDeviceId.Text.Trim();
            Member.Password = txtPassword.Text.Trim();

            int rowsaffected = ObjMember.Save(Member);
            if (rowsaffected > 0)
            {
                ClearControls();
                lblMessage.Text = "Member Information Saved/Updated Successfully";
                lblMessage.BackColor = System.Drawing.Color.Green;
            }
            else
            {
                lblMessage.Text = "Mobile Number Already Exist";
                lblMessage.BackColor = System.Drawing.Color.Red;
            }
        }

    }
}
