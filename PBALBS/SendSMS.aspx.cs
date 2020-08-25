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
using System.Text.RegularExpressions;

namespace PBALBS
{
    public  partial class SendSMS : System.Web.UI.Page
    {
        string API_INDEX;
        public string CurrentProvider
        {
            get { return ViewState["CurrentProvider"].ToString(); }
            set { ViewState["CurrentProvider"] = value; }
        }
        public decimal count
        {
            get { return Convert.ToInt32(ViewState["count"]); }
            set { ViewState["count"] = value; }
        }
        int num = 0;
        public int Balance
        {
            get { return Convert.ToInt32(ViewState["Balance"]); }
            set { ViewState["Balance"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
                Response.Redirect("Login.aspx");

            if (!IsPostBack)
            {
                lblCharacter.Attributes.Add("readonly", "readonly");
                txtCredit.Attributes.Add("readonly", "readonly");
                BusinessLayer.SMSTrigger ObjSMSTrigger = new BusinessLayer.SMSTrigger();
                Hidden1.Value = (ObjSMSTrigger.IsMessageSentToday() == true) ? "1" : "0";
                if (Hidden1.Value == "1")
                    btnSendSMS.Style.Add("display", "none");
                else
                    btnSendSMS.Style.Add("display", "block");

                txtMobNo.Text = "";
                txtMsg.Text = "";
                txtMsgPreview.Text = "";

                ddlDay.SelectedValue = DateTime.Now.Day.ToString();
                ddlMonth.SelectedValue = DateTime.Now.Month.ToString();
                ddlYear.SelectedValue = DateTime.Now.Year.ToString();

                if (Request.QueryString["day"] != null && Request.QueryString["day"].ToString().Trim().Length > 0)
                {
                    ddlDay.SelectedValue = Request.QueryString["day"].ToString().Trim();

                }
                if (Request.QueryString["month"] != null && Request.QueryString["month"].ToString().Trim().Length > 0)
                {
                    ddlMonth.SelectedValue = Request.QueryString["month"].ToString().Trim();

                }
                if (Request.QueryString["year"] != null && Request.QueryString["year"].ToString().Trim().Length > 0)
                {
                    ddlYear.SelectedValue = Request.QueryString["year"].ToString().Trim();

                }
                GetBalance();
                LoadMsg();
                counts();
                AppDownloadCount();
            }

        }


        private void GetBalance()
        {
            BusinessLayer.SMSTrigger objSMSTrigger = new BusinessLayer.SMSTrigger();
            DataTable DT = objSMSTrigger.GetBalance();

            Balance = Convert.ToInt32(DT.Rows[0]["Balance"]) - Convert.ToInt32(DT.Rows[0]["ExpanceBalance"]);
            lblBalance.Text = Balance.ToString();
        }

        private void AppDownloadCount()
        {
            BusinessLayer.SMSTrigger objSMSTrigger = new BusinessLayer.SMSTrigger();
            ltrDownloadCount.Text = objSMSTrigger.GetAppDownloadCount().ToString();

        }

        protected void counts()
        {
            count = txtMsg.Text.Length + Regex.Matches(txtMsg.Text, @"\n").Count;
            lblCharacter.Text = count.ToString("00");
            if (count < 159)
            {
                txtCredit.Text = "1";
            }
            else if (count < 304)
            {
                txtCredit.Text = "2";
            }
            else if (count < 450)
            {
                txtCredit.Text = "3";
            }
            else if (count < 608)
            {
                txtCredit.Text = "4";
            }
        }

        protected void LoadMsg()
        {
            BusinessLayer.PotatoRate ObjRate = new BusinessLayer.PotatoRate();
            string Date = ddlDay.SelectedValue.Trim()  + "/" + ddlMonth.SelectedValue.Trim() + "/" + ddlYear.SelectedValue.Trim() + " 00:00:00";
            DataTable dt = ObjRate.GetRateByDate(Date);

            string day = ddlDay.SelectedValue.Trim();
            day = (day.Length == 1) ? "0" + day : day;

            string month = ddlMonth.SelectedValue.Trim();
            month = (month.Length == 1) ? "0" + month : month;

            string year = ddlYear.SelectedValue.Trim();
            //year = year.Substring(2);

            StringBuilder sb = new StringBuilder();
            sb.Append("Date ");
            sb.Append(day + "/" + month + "/" + year + " : Low/High\n");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append(dt.Rows[i][1].ToString().Trim() + " ");
                sb.Append(FitRate(dt.Rows[i][2].ToString().Trim()) + "/");
                sb.Append(FitRate(dt.Rows[i][3].ToString().Trim()));
                //sb.Append(FitRate(dt.Rows[i][4].ToString().Trim()) + ",");
                if (i != dt.Rows.Count - 1)
                    sb.Append("\n");
            }

            txtMsg.Text = sb.ToString();
            txtMsgPreview.Text = txtMsg.Text;
        }

        protected string FitRate(string Rate)
        {
            if (Rate.Trim().Length > 0)
                return Rate;
            else
                return "0";
        }

        protected void btnGetMsg_Click(object sender, EventArgs e)
        {
            LoadMsg();
        }

        protected void btnSendSMS_Click(object sender, EventArgs e)
        {
            try
            {
                BusinessLayer.Provider ObjProvider = new BusinessLayer.Provider();
                CurrentProvider = ObjProvider.GetCurrentProvider();
                //--------------------

                string mobiles = "";
                string message = "";
                string smsPerTrans = ddlNoOfMember.SelectedValue.Trim();

                string strUrl;
                string dataString;
                int CharCount = Convert.ToInt32(lblCharacter.Text);


                message = txtMsg.Text.Trim();
                //int lgnth = txtMsg.Text.Length;
                mobiles = txtMobNo.Text.Trim();

                //Put 91 before all mobile nos when not sending from database

                int MobNoCount = 0;
                if (mobiles.Length > 0)
                {
                    if (message.Length <= 445)
                    {
                        string[] Arrmob = mobiles.Trim().Split(',');
                        MobNoCount = Arrmob.Length;//How many nos are sending for SMS
                        mobiles = "";
                        for (int index = 0; index < Arrmob.Length; index++)
                        {
                            if (Arrmob[index].Length == 10)
                            {
                                mobiles += "91" + Arrmob[index].Trim() + ",";
                            }
                            else if (Arrmob[index].Length == 12)
                            {
                                mobiles += Arrmob[index].Trim() + ",";
                            }
                        }
                        mobiles = mobiles.Trim().Substring(0, mobiles.Length - 1).Trim();
                    }
                    else
                    {
                        ltrMsg.Text = "<h2  style='color:red'>Message Content More than 445 Character Please Contact Service Provider</h2>";
                    }

                }
                //------------------------------------

                if (message.Length <= 445)
                {
                    if (message.Length > 0)
                    {

                        if (mobiles.Length == 0)//Then fetch mobile numbers from Database
                        {
                            DataTable dt = getMobileNumbers();
                            MobNoCount = dt.Rows.Count;//How many nos are sending for SMS
                            int counter = 0;
                            foreach (DataRow dr in dt.Rows)
                            {
                                mobiles += dr["MobileNo"].ToString() + ",";
                                counter++;
                                if (counter == int.Parse(smsPerTrans))
                                {
                                    mobiles = mobiles.Trim().Substring(0, mobiles.Length - 1);

                                    strUrl = GetHTTPAPI(mobiles, message);

                                    WebRequest request = HttpWebRequest.Create(strUrl);
                                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                    Stream s = (Stream)response.GetResponseStream();
                                    StreamReader readStream = new StreamReader(s);
                                    dataString = readStream.ReadToEnd();
                                    response.Close();
                                    s.Close();
                                    readStream.Close();

                                    counter = 0;
                                    mobiles = "";
                                }
                            }
                        }


                        if (mobiles.LastIndexOf(',') == mobiles.Length - 1)
                        {
                            mobiles = mobiles.Trim().Substring(0, mobiles.Length - 1);
                        }
                        //strUrl = string.Format("http://api.mVaayoo.com/mvaayooapi/MessageCompose?user=admin@fourfusionsolutions.com:solution2012&senderID=PBALBS&receipientno={0}&msgtxt={1}&state=1", mobiles, message);

                        int Credit = Convert.ToInt32(txtCredit.Text);
                        int TotalCredit = Credit * MobNoCount;
                        strUrl = GetHTTPAPI(mobiles, message);

                        WebRequest request1 = HttpWebRequest.Create(strUrl);
                        HttpWebResponse response1 = (HttpWebResponse)request1.GetResponse();
                        Stream s1 = (Stream)response1.GetResponseStream();
                        StreamReader readStream1 = new StreamReader(s1);
                        dataString = readStream1.ReadToEnd();
                        response1.Close();
                        s1.Close();
                        readStream1.Close();

                        BusinessLayer.SMSTrigger ObjSMSTrigger = new BusinessLayer.SMSTrigger();
                        if (MobNoCount > 0)
                        {
                            ObjSMSTrigger.Save(HttpContext.Current.User.Identity.Name, MobNoCount, CharCount, TotalCredit);
                            ltrMsg.Text = "<h2 style='color:green'>Message send successfully</h2>";
                            Hidden1.Value = "1";
                            btnSendSMS.Style.Add("display", "none");
                            GetBalance();
                        }
                        else
                        {
                            ltrMsg.Text = "<h2  style='color:red'>Please Contact Service Provider</h2>";
                        }
                    }
                }
                else
                {
                    ltrMsg.Text = "<h2  style='color:red'>Message Content More than 445 Character Please Contact Service Provider</h2>";
                }
            }
            catch (Exception ex)
            {
                ltrMsg.Text = "<h2>Error: " + ex.Message + "</h2>";
            }
        }

        private string GetHTTPAPI(string mobiles, string message)
        {
            BusinessLayer.ApiConfiguration ObjApi = new BusinessLayer.ApiConfiguration();
            DataTable dt = ObjApi.GetAll();
            DataView DV = new DataView(dt);
            DV.RowFilter = "IsActive = 1";
            string API = string.Empty;
            //string ROUTE_1 = System.Configuration.ConfigurationSettings.AppSettings["ROUTE_1"];
            API_INDEX = Convert.ToString(DV[0]["SMSAPIId"]);

            if (API_INDEX == "1")
                API = string.Format("http://api.mVaayoo.com/mvaayooapi/MessageCompose?user=admin@fourfusionsolutions.com:solution2012&senderID=PBALBS&receipientno={0}&msgtxt={1}&state=1", mobiles, message);
            else if (API_INDEX == "2")
                //API = string.Format("http://sms.fourfusiontechnologies.com/new/api/api_http.php?username=FOURFTECH&password=FourFM@2020&senderid=SAHYOG&to={0}&text={1}&route=Informative&type=text", mobiles, message);
            API = string.Format("http://sms.fourfusiontechnologies.com/new/api/api_http.php?username=FOURFTECH&password=FourFM@2020&senderid=PBALBS&to={0}&text={1}&route=Informative&type=text", mobiles, message);

            return API;
        }

        private DataTable getMobileNumbers()
        {
            BusinessLayer.Member ObjMember = new BusinessLayer.Member();
            DataTable dtMobileNos = ObjMember.getMobileNumbers();
            return dtMobileNos;
        }

        protected void ChckedChanged(object sender, EventArgs e)
        {
            BusinessLayer.SMSTrigger objTrigger = new BusinessLayer.SMSTrigger();
            objTrigger.Unlock();
            {
                BusinessLayer.SMSTrigger ObjSMSTrigger = new BusinessLayer.SMSTrigger();
                Hidden1.Value = (ObjSMSTrigger.IsMessageSentToday() == true) ? "1" : "0";
                if (Hidden1.Value == "1")
                    btnSendSMS.Style.Add("display", "none");
                else
                    btnSendSMS.Style.Add("display", "block");
            }
        }

        protected void btnSendNotification_Click_Individual(object sender, EventArgs e)
        {
            BusinessLayer.Member ObjMember = new BusinessLayer.Member();
            DataTable dtMembers = ObjMember.getMembersForNotification();
            //int maxNoOfNotification = Convert.ToInt32(BusinessLayer.Common.GetScalarValue("KeyValue", "KeyValueConfig", "KeyCode", "NOOFNOTIFICATION"));

            foreach (DataRow member in dtMembers.Rows)
            {
                if (member["DeviceId"] != null)
                {
                    var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

                    request.KeepAlive = true;
                    request.Method = "POST";
                    request.ContentType = "application/json; charset=utf-8";

                    request.Headers.Add("authorization", "Basic ZGYyY2VlNGYtZDQ5MC00YTIwLWE3YzEtNDdlYTRlZmI3MDBk");

                    byte[] byteArray = Encoding.UTF8.GetBytes("{"
                                                            + "\"app_id\": \"03b78b0e-44da-4221-ae8c-31003fbc8a6e\","
                                                            + "\"contents\": {\"en\": \"PBPABS Potato rate published for " + member["Today"].ToString() + ". Please Login to APP and Check!!\"},"
                                                            + "\"include_player_ids\": [\"" + member["DeviceId"].ToString() + "\"]}");//PlayerId is DeviceID
                    string responseContent = null;
                    try
                    {
                        using (var writer = request.GetRequestStream())
                        {
                            writer.Write(byteArray, 0, byteArray.Length);
                        }

                        using (var response = request.GetResponse() as HttpWebResponse)
                        {
                            using (var reader = new StreamReader(response.GetResponseStream()))
                            {
                                responseContent = reader.ReadToEnd();
                            }
                        }

                    }
                    catch (WebException ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
                    }
                }
            }
        }

        protected void btnSendNotification_Click(object sender, EventArgs e)
        {
            BusinessLayer.Member ObjMember = new BusinessLayer.Member();
            DataTable dtMembers = ObjMember.getMembersForNotification();
            //int maxNoOfNotification = Convert.ToInt32(BusinessLayer.Common.GetScalarValue("KeyValue", "KeyValueConfig", "KeyCode", "NOOFNOTIFICATION"));

            
                    var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;
                    string ImageUrl = "http://api.wbpoultryfederation.org/Images/PBALBS_LargeIcon.jpg";
                    request.KeepAlive = true;
                    request.Method = "POST";
                    request.ContentType = "application/json; charset=utf-8";


                    request.Headers.Add("authorization", "Basic ZGYyY2VlNGYtZDQ5MC00YTIwLWE3YzEtNDdlYTRlZmI3MDBk");

                    byte[] byteArray = Encoding.UTF8.GetBytes("{"
                                                            + "\"app_id\": \"03b78b0e-44da-4221-ae8c-31003fbc8a6e\","
                                                            + "\"large_icon\" : \" " + ImageUrl + " \", "
                                                            + "\"data\": {\"NotificationType\": \"Today\"},"
                                                            + "\"contents\": {\"en\": \"PBPABS Potato rate published for today.Please Login to APP and Check!!\"},"
                                                            + "\"included_segments\": [\"All\"]}");


                    //request.Headers.Add("authorization", "Basic ZGYyY2VlNGYtZDQ5MC00YTIwLWE3YzEtNDdlYTRlZmI3MDBk");

                    //byte[] byteArray = Encoding.UTF8.GetBytes("{"
                    //                                        + "\"app_id\": \"03b78b0e-44da-4221-ae8c-31003fbc8a6e\","
                    //                                        + "\"contents\": {\"en\": \"PBPABS Potato rate published for " + member["Today"].ToString() + ". Please Login to APP and Check!!\"},"
                    //                                        + "\"include_player_ids\": [\"" + member["DeviceId"].ToString() + "\"]}");//PlayerId is DeviceID


            string responseContent = null;
                    try
                    {
                        using (var writer = request.GetRequestStream())
                        {
                            writer.Write(byteArray, 0, byteArray.Length);
                        }

                        using (var response = request.GetResponse() as HttpWebResponse)
                        {
                            using (var reader = new StreamReader(response.GetResponseStream()))
                            {
                                responseContent = reader.ReadToEnd();
                            }
                        }

                    }
                    catch (WebException ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
                    }
                }
            




    }
}
