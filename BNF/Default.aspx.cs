/* ---------------------------------------------------------
 * Code Written  by : Seshagiri Deshpande 
 * Code Reviewed by : Sam Kong.  
 * 
 * --------------------------------------------------------
 * */
using System;
using System.Security;
using System.IO;
using System.Xml;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using log4net.Util;
using log4net;
using BNF.Constants;
using BNAF.DecryptResponse;

using System.Configuration;

public partial class _Default : System.Web.UI.Page
{
    ILog logger;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        log4net.Config.XmlConfigurator.Configure();
        logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }

    /// <summary>
    /// Login Begins here.. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmdPost_Click(object sender, EventArgs e)
    {
        try
        {
            DecryptResponse dr = new DecryptResponse();
            AppSettingsReader appConf = new AppSettingsReader();
            // Forming the required XML 
            string agencyID = appConf.GetValue("eServiceId", typeof(string)).ToString();
            string returnUrl = appConf.GetValue("returnUrl", typeof(string)).ToString();
            string authnLevel = appConf.GetValue("authLevel", typeof(string)).ToString();
            string locale = appConf.GetValue("locale", typeof(string)).ToString();
            string respType = appConf.GetValue("respType", typeof(string)).ToString();
            string isCorppassResponse = appConf.GetValue("isCorppassResponse", typeof(string)).ToString();
            string SignerCertificateSubject = appConf.GetValue("SignerCertificateSubject", typeof(string)).ToString();
            string EncryptionCertificateSubject = appConf.GetValue("EncryptionCertificateSubject", typeof(string)).ToString();
            string isAllowGCCUserLogin = appConf.GetValue("isAllowGCCUserLogin", typeof(string)).ToString();
            string sessionId = "";

            //Authentication URL 
            string authUrl = appConf.GetValue("authUrl", typeof(string)).ToString();

            bool validateParams = true;

            // check that required parameters are set

            if (string.IsNullOrEmpty(agencyID) || string.IsNullOrEmpty(returnUrl) || string.IsNullOrEmpty(authUrl) || Convert.ToInt64(authnLevel) <= 0 || string.IsNullOrEmpty(locale) || string.IsNullOrEmpty(SignerCertificateSubject) || string.IsNullOrEmpty(EncryptionCertificateSubject))
            {
                logger.Info("locale, eserviceId, returnUrl, authUrl, authLevel cannot be empty." + agencyID + ", " + returnUrl + ", " + authUrl + ", " + authnLevel.ToString() + ", " + locale);
                validateParams = false;
            }

            if (string.IsNullOrEmpty(authUrl))
            {
                validateParams = false;
                logger.Info("Authentication URL: " + authUrl);
            }

            if (validateParams)
            {
                try
                {
                    //It will encrypt and sign the xml string
                    string data = String.Empty;
                    if(isAllowGCCUserLogin.Equals("true") && isCorppassResponse.Equals("true")){
                     data = dr.EncryptAndSignDoc(agencyID, returnUrl, authnLevel, locale, respType, SignerCertificateSubject, EncryptionCertificateSubject, isCorppassResponse,System.Convert.ToBoolean(isAllowGCCUserLogin));
                    }else if(isCorppassResponse.Equals("true")){
                     data = dr.EncryptAndSignDoc(agencyID, returnUrl, authnLevel, locale, respType, SignerCertificateSubject, EncryptionCertificateSubject, isCorppassResponse);
                    }else if (isAllowGCCUserLogin.Equals("true")){
                        data = dr.EncryptAndSignDoc(agencyID, returnUrl, authnLevel, locale, respType, SignerCertificateSubject, EncryptionCertificateSubject, Convert.ToBoolean(isAllowGCCUserLogin));
                    }else{
                        data = dr.EncryptAndSignDoc(agencyID, returnUrl, authnLevel, locale, respType, SignerCertificateSubject, EncryptionCertificateSubject);
                    }
                    string url = appConf.GetValue("SessionUrl", typeof(string)).ToString();
                    sessionId = dr.Post2Server(url, "encryptedMessage=" + HttpUtility.UrlEncode(data));

                    if (string.IsNullOrEmpty(sessionId))
                    {
                        string redirectURL = ResolveUrl("~/ErrorPage.aspx");
                        Session["ErrorMessage"] = dr.ErrorMessage;
                        Response.Redirect(redirectURL);
                    }
                    else
                    {
                        sessionId = sessionId.Replace("\r\n", ""); // Remove new line characters 
                        sessionId = sessionId.Replace("\n", ""); // Remove new line characters
                        authUrl = authUrl + sessionId;
                    }
                }
                catch (System.Web.HttpException ex)
                {
                    logger.Error("Error Occured: " + ex.Message);
                }
                catch (Exception ex)
                {
                    logger.Error("Error Occured: " + ex.Message);
                }

                //Redirected to Login Page.. sucessfully.. 
                Response.Redirect(authUrl);
            }
            else
            {
                lblError.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error("Error Occured: " + ex.Message);
        }
    }


}