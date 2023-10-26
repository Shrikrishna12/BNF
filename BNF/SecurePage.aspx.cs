using System;
using log4net;
using BNAF.DecryptResponse;
using System.Configuration;
using System.Xml;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.X509Certificates;

namespace BNF
{
    public partial class SecurePage : System.Web.UI.Page
    {
        ILog logger;
        DecryptResponse dr = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            string ticket = Convert.ToString(Request.Params["ticket"]);
            string clasSessionId = Convert.ToString(Request.Params["clasSessionId"]);
            bool isCAuth = true;

            if (string.IsNullOrEmpty(ticket))
            {
                logger.Warn("Ticket is empty. Unable to get authentication result.");
                isCAuth = false;
            }

            if (isCAuth && clasSessionId == null)
            {
                logger.Warn("NAF session ID is empty. Unable to get authentication result.");
                isCAuth = false;
            }

            if (isCAuth)
            {
                reqAuthnStatus(ticket, clasSessionId);
            }
            else
            {
                logger.Warn("Invalid Response from server Or Session got expired");
            }
        }

        public void reqAuthnStatus(string ticket, string clasSessionId)
        {
            dr = new DecryptResponse();
            AppSettingsReader appConf = new AppSettingsReader();
            string respType = appConf.GetValue("respType", typeof(string)).ToString();
            string SignerCertificateSubject = appConf.GetValue("SignerCertificateSubject", typeof(string)).ToString();
            dr.hostUrl = appConf.GetValue("hostUrl", typeof(string)).ToString();
            ltInfo.Text = dr.Decrypt(ticket, clasSessionId, respType, SignerCertificateSubject);
        }
    }
}


