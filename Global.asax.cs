using System;
using System.Net.Mail;
using System.Threading;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.WebPages;

namespace Ptpn8
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //public static System.Timers.Timer timer = new System.Timers.Timer(60000); // setiap 60 detik
        static System.Threading.Timer TTimer = null;
        static System.Threading.Timer TAssesment= null;

        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            //timer.Stop();
            System.Data.DataTable tap = new System.Data.DataTable();
            string scriptSQL = "SELECT [NotifikasiSudinModinId],[NamaUser],[MenuId],[Notifikasi],[StatusNotifikasi],[DocumentId] FROM [SPDK_KanpusEF].[dbo].[eOffice_NotifikasiSudinModin] WHERE [StatusNotifikasi]='BARU'";
            string scriptSQL1 = "";
            tap = ExecSQL(scriptSQL);
            if (tap.Rows.Count > 0)
            {
                for (int i = 0; i < tap.Rows.Count; i++)
                {
                    if (KirimEmail(tap.Rows[i][1].ToString(), "Memo Dinas Online", tap.Rows[i][3].ToString()))
                    {
                        scriptSQL1 = "UPDATE [SPDK_KanpusEF].[dbo].[eOffice_NotifikasiSudinModin] SET [StatusNotifikasi]='EMAIL' WHERE [NotifikasiSudinModinId]='" + tap.Rows[i][0].ToString() + "'";
                        ExecSQL(scriptSQL1);
                    }
                }
            }
            //timer.Start();
        }

        private void TickTimer(object state)
        {
            TTimer = null;
           
            System.Data.DataTable tap = new System.Data.DataTable();
            string scriptSQL = "SELECT [NotifikasiSudinModinId],[NamaUser],[MenuId],[Notifikasi],[StatusNotifikasi],[DocumentId],[Perihal] FROM [SPDK_KanpusEF].[dbo].[eOffice_NotifikasiSudinModin] WHERE [StatusNotifikasi]='BARU'";
            string scriptSQL1 = "";
            tap = ExecSQL(scriptSQL);
            if (tap.Rows.Count > 0)
            {
                for (int i = 0; i < tap.Rows.Count; i++)
                {
                    if (KirimEmail(tap.Rows[i][1].ToString(), tap.Rows[i][6].ToString(), tap.Rows[i][3].ToString()))
                    {
                        scriptSQL1 = "UPDATE [SPDK_KanpusEF].[dbo].[eOffice_NotifikasiSudinModin] SET [StatusNotifikasi]='EMAIL' WHERE [NotifikasiSudinModinId]='" + tap.Rows[i][0].ToString() + "'";
                        ExecSQL(scriptSQL1);
                    }
                }
            }

            StartTimer(10000); 
            
        }

        public void StartTimer(int dueTime)
        {
            TTimer = new Timer(new TimerCallback(TickTimer));
            TTimer.Change(dueTime, 0);
        }

        private void TickTimerAssesment(object state)
        {
            TAssesment = null;

            StartAssesment(1000);
        }

        public void StartAssesment(int dueTime)
        {
            TAssesment = new Timer(new TimerCallback(TickTimerAssesment));
            TAssesment.Change(dueTime, 0);

        }

        protected void Application_Start()
        {
            //TTimer = new System.Threading.Timer(new System.Threading.TimerCallback(TickTimer),null,1000,60000);
            StartTimer(10000);
            //StartAssesment(1000);
            //timer.Elapsed += OnTimedEvent;
            //timer.AutoReset = true;
            //timer.Enabled = true;

            Telerik.Reporting.Services.WebApi.ReportsControllerConfiguration.RegisterRoutes(System.Web.Http.GlobalConfiguration.Configuration);
            DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("iPhone")
            {
                ContextCondition = (context => context.GetOverriddenUserAgent().IndexOf
                    ("iPhone", StringComparison.OrdinalIgnoreCase) >= 0)
            });

            
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RegisterRoutes(RouteTable.Routes);

            BundleMobileConfig.RegisterBundles(BundleTable.Bundles);

            //Dictionary<string, HttpSessionState> sessionData =
            //   new Dictionary<string, HttpSessionState>();
            //Application["Ptpn8"] = sessionData;

        }

        //Sesssion Start Event
        protected void Session_Start(object sender, EventArgs e)
        {
            //Dictionary<string, HttpSessionState> sessionData =
            //  (Dictionary<string, HttpSessionState>)Application["Ptpn8"];

            //if (sessionData.Keys.Contains(HttpContext.Current.Session.SessionID))
            //{
            //    sessionData.Remove(HttpContext.Current.Session.SessionID);
            //    sessionData.Add(HttpContext.Current.Session.SessionID,
            //                    HttpContext.Current.Session);
            //}
            //else
            //{
            //    sessionData.Add(HttpContext.Current.Session.SessionID,
            //                    HttpContext.Current.Session);
            //}
            //Application["Ptpn8"] = sessionData;
        }

        //Session End Event
        protected void Session_End(object sender, EventArgs e)
        {
            //Dictionary<string, HttpSessionState> sessionData =
            //   (Dictionary<string, HttpSessionState>)Application["Ptpn8"];
            //sessionData.Remove(HttpContext.Current.Session.SessionID);
            //Application["Ptpn8"] = sessionData;
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                 "Default",
                 "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "Ptpn8.Controllers" }
            );

        }

        public static System.Data.DataTable ExecSQL(string scriptSQL)
        {
            string strcs = System.Configuration.ConfigurationManager.ConnectionStrings["csSPDKKanpus"].ConnectionString;
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(strcs);
            System.Data.DataTable tap = new System.Data.DataTable();
            try
            {
                con.Open();
                new System.Data.SqlClient.SqlDataAdapter(scriptSQL, con).Fill(tap);
            }
            catch (Exception)
            { }
            finally
            {
                if (con != null) con.Close();
            }
            return tap;
        }



        public static bool KirimEmail(string _to, string _subject, string _body)
        {
            try
            {

                if(_to.IndexOf("@")==-1 || _to.IndexOf(",")>-1)
                {
                    return true;
                }

                //_to = "bdhendra2012@gmail.com";
                SmtpClient mySmtpClient = new SmtpClient();
                mySmtpClient.Credentials = new System.Net.NetworkCredential("memoonline@ptpn8.co.id", "Sttosp9039");
                mySmtpClient.Port = 587;
                //mySmtpClient.Timeout= 10000;
                mySmtpClient.Host = "mail.ptpn8.co.id";
                mySmtpClient.EnableSsl = true;
                mySmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                MailMessage mailMessage = new MailMessage();
                String[] addr = _to.Split(';');
                mailMessage.From = new MailAddress("memoonline@ptpn8.co.id");
                for (byte i = 0; i < addr.Length; i++)
                {
                    if(addr[i].Trim()!="")
                    {
                        mailMessage.To.Add(addr[i]);
                        mailMessage.ReplyToList.Add(addr[i]);
                    }
                }
                mailMessage.Subject = _subject;
                mailMessage.Body = _body;
                mailMessage.IsBodyHtml = true;
                mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                

                mySmtpClient.Send(mailMessage);


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool KirimEmailX(string _to, string _subject, string _body)
        {
            try
            {
                //_to = "ujicoba.portalpn8@gmail.com";
                SmtpClient mySmtpClient = new SmtpClient();
                mySmtpClient.Credentials = new System.Net.NetworkCredential("memoonline.pn8@gmail.com", "Abcd_1234");
                mySmtpClient.Port = 465; 
                mySmtpClient.Host = "smtp.gmail.com";
                mySmtpClient.EnableSsl = true;
                mySmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                MailMessage mailMessage = new MailMessage();
                String[] addr = _to.Split(';');
                mailMessage.From = new MailAddress("memoonline.pn8@gmail.com");
                for(byte i = 0; i < addr.Length; i++)
                {
                    mailMessage.To.Add(addr[i]);
                }
                mailMessage.Subject = _subject;
                mailMessage.Body = _body;
                mailMessage.IsBodyHtml = true;
                mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                mailMessage.ReplyToList.Add(_to);

                mySmtpClient.Send(mailMessage);

                
                return true;
            }
            catch(Exception ex) 
            {
                return false;
            }
        }
    }
}
