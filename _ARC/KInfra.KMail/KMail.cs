using System;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using KInfra.Logger;
using KInfra.OldMW.Web;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using KInfra.InterfaceBridge;


namespace KInfra.OldMW
{
    /// <summary>
    /// Summary description for KMail.
    /// </summary>

    public class KMail
    {
        public static bool Send(string xsToEmail, string xsHTMLBody, string xsSubject)
        {
            return SendMessage(
              CreateMessage("postmaster@hetk.ee", xsToEmail, "", xsHTMLBody, xsSubject, "html")
              );
        }

        public static bool Send(string xsFromEmail, string xsToEmail, string xsHTMLBody, string xsSubject)
        {
            return SendMessage(
              CreateMessage(xsFromEmail, xsToEmail, "", xsHTMLBody, xsSubject, "html")
              );
        }
        public static bool Send(string xsFromEmail, string xsToEmail, string xsCC, string xsHTMLBody, string xsSubject)
        {
            return SendMessage(
                        CreateMessage(xsFromEmail, xsToEmail, xsCC, xsHTMLBody, xsSubject, "html")
                        );
        }

        public static bool Send(string xsFrom, string xsTo, string xsCC, string xsHTMLBody, string xsSubject, string xFormat)
        {
            return SendMessage(
                CreateMessage(xsFrom, xsTo, xsCC, xsHTMLBody, xsSubject, xFormat));
        }

        public static bool SendMessage(string xsFromName, string xsFromEmail, string xsToName, string xsToEmail, string xsCC, string xsHTMLBody, string xsSubject, string xsFormat)
        {
            return SendMessage(
                CreateMessage(xsFromName, xsFromEmail, xsToName, xsToEmail, xsCC, xsHTMLBody, xsSubject, xsFormat));
        }

        public static MailMessage CreateMessage(string xsFromEmail, string xsToEmail, string xsCC, string xsHTMLBody, string xsSubject, string xsFormat)
        {
            return CreateMessage(null, xsFromEmail, null, xsToEmail, xsCC, xsHTMLBody, xsSubject, xsFormat);
        }
        public static MailMessage CreateMessage(string xsFromName, string xsFromEmail, string xsToName, string xsToEmail, string xsCC, string xsHTMLBody, string xsSubject, string xsFormat)
        {
            if (Cnv.IsEmail(xsFromEmail) && Cnv.IsEmail(xsToEmail))
            {

                MailMessage xMessage = new MailMessage();

                if (xsFromName != null)
                    xMessage.From = new MailAddress(xsFromEmail, xsFromName);
                else
                    xMessage.From = new MailAddress(xsFromEmail);
                xMessage.Headers.Add("Sender", xsFromEmail);
                xMessage.Headers.Add("Reply-To", xsFromEmail);
                if (xsToName != null)
                    xMessage.To.Add(new MailAddress(xsToEmail, xsToName));
                else
                    xMessage.To.Add(new MailAddress(xsToEmail));
                if (Cnv.CStr(xsCC).Length > 0) xMessage.CC.Add(new MailAddress(xsCC));

                xMessage.IsBodyHtml = (xsFormat != "txt");
                xMessage.Body = xsHTMLBody;
                xMessage.Subject = xsSubject;
                return xMessage;
            }
            else
                return null;
        }

        public static SmtpClient smtpClient;

        public static bool SendMessage(System.Net.Mail.MailMessage xMessage)
        {
            NetworkCredential creds = null;
            if (KSettings.TryGet("SMTP", "sSmtpLogin").Length > 0 &&
                KSettings.TryGet("SMTP", "sSmtpLoginPwd").Length > 0)
                creds = new NetworkCredential(KSettings.Get("SMTP", "sSmtpLogin"),
                    KSettings.Get("SMTP", "sSmtpLoginPwd").ToString());

            return SendMessage(xMessage, creds);

        }

        public static object Logger;

        public static bool SendMessage(System.Net.Mail.MailMessage xMessage, NetworkCredential creds)
        {
            if (smtpClient == null)
                smtpClient = new SmtpClient(KSettings.sSmtpServer);

            if (KSettings.TryGet("SMTP", "bMailSending_Enabled") == "false")
                return false;

            if (creds != null)
            {
                smtpClient.Credentials = creds;
                smtpClient.UseDefaultCredentials = false;
            }
            else
                smtpClient.UseDefaultCredentials = true;

            xMessage.BodyEncoding = System.Text.Encoding.GetEncoding("iso-8859-1");

            //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(
            //    (Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors ssl) => { return true; });
            //smtpClient.EnableSsl = true;
            //smtpClient.Port = 587;
            //smtpClient.Host = "smtp.gmail.com";
            //smtpClient.Port = 587;
            //smtpClient.UseDefaultCredentials = false;
            //smtpClient.Credentials = new System.Net.NetworkCredential("kalevrebane@gmail.com", "Bingo123");
            //xMessage.From = new MailAddress("kalevrebane@gmail.com");

            try
            {
                smtpClient.Send(xMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                var logger = (ILogger)Logger;
                if (logger != null)
                {
                    if ((logger.CurrentRequest ?? "") == "")
                        logger.CurrentRequest = "KMail.SendMessage";
                    logger.Add(xMessage.Subject, xMessage.To[0].Address + ex.Message, creds.UserName);
                    logger.Save();
                }
                return false;
            }

        }

    }

}