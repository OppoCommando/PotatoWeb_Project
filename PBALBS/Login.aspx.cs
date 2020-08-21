using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace PBALBS
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoginStatus1.Focus();
            }
        }

        protected void LoginStatus1_Authenticate(object sender, AuthenticateEventArgs e)
        {
            string u = LoginStatus1.UserName;
            string p = LoginStatus1.Password;

            

            if (u.ToUpper() == "FOURFUSION" && p.ToUpper() == "FOURFUSION123456789")
            {

                FormsAuthenticationTicket Authticket = new FormsAuthenticationTicket(
                                                                1,
                                                                "Admin",
                                                                DateTime.Now,
                                                                DateTime.Now.AddMinutes(30),
                                                                false,
                                                                "admin",
                                                                FormsAuthentication.FormsCookiePath);
                string hash = FormsAuthentication.Encrypt(Authticket);
                HttpCookie Authcookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
                if (Authticket.IsPersistent)
                    Authcookie.Expires = Authticket.Expiration;
                Response.Cookies.Add(Authcookie);
                Session["UserId"] = "Admin";
                Response.Redirect("AddEditUPBlock.aspx");

            }
            else if (u.ToUpper() == "BIBHAS" && p.ToUpper() == "NOACCESS@123")
            {

                FormsAuthenticationTicket Authticket = new FormsAuthenticationTicket(
                                                                1,
                                                                "Bibhas",
                                                                DateTime.Now,
                                                                DateTime.Now.AddMinutes(30),
                                                                false,
                                                                "Customer",
                                                                FormsAuthentication.FormsCookiePath);
                string hash = FormsAuthentication.Encrypt(Authticket);
                HttpCookie Authcookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
                if (Authticket.IsPersistent)
                    Authcookie.Expires = Authticket.Expiration;
                Response.Cookies.Add(Authcookie);
                Session["UserId"] = "Bibhas";
                Response.Redirect("AddEditPotatoRate.aspx");

            }
            else
            {
                BusinessLayer.AgentMaster objAgentMaster = new BusinessLayer.AgentMaster();
                Entity.AgentMaster agentMaster = new Entity.AgentMaster();
                agentMaster = objAgentMaster.AuthenticateUser(u);

                if (agentMaster != null)
                {
                    if ((agentMaster.Password) == p)
                    {
                        string UserId = agentMaster.AgentId.ToString();
                        FormsAuthenticationTicket Authticket = new FormsAuthenticationTicket(
                                                                   1,
                                                                   UserId,
                                                                   DateTime.Now,
                                                                   DateTime.Now.AddMinutes(240),
                                                                   false,
                                                                   "Agent",
                                                                   FormsAuthentication.FormsCookiePath);
                        string hash = FormsAuthentication.Encrypt(Authticket);
                        HttpCookie Authcookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
                        if (Authticket.IsPersistent)
                            Authcookie.Expires = Authticket.Expiration;
                        Response.Cookies.Add(Authcookie);

                        System.Web.Caching.Cache cacheObject = System.Web.HttpContext.Current.Cache;
                        //cacheObject.Insert(UserId, Employee.Roles);

                        Session["UserId"] = agentMaster.Name;
                        Session.Timeout = 240;
                        Response.Redirect("AddSubscription.aspx");
                    }
                }
                //txtPassword.Text = "";
                //txtPassword.Focus();
                //FailureText.Text = "Invalid Username or Password !";
            }
        }
    }
}
