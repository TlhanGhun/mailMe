using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using libSnarlStyles;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace mailMe
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]

    public class StyleInstance : IStyleInstance
    {
        private string snarlTitle = "";
        private string snarlText = "";

        public StyleInstance()
        {
            // this is a workaround for mail server with an invalid (self signed...) certificate

            ServicePointManager.ServerCertificateValidationCallback += delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };


        }

         

        #region IStyleInstance Members

        [ComVisible(true)]
        void IStyleInstance.AdjustPosition(ref int x, ref int y, ref short Alpha, ref bool Done)
        {
            return;
        }

        [ComVisible(true)]
        melon.MImage IStyleInstance.GetContent()
        {
            
            throw new NotImplementedException();
        }

        [ComVisible(true)]
        bool IStyleInstance.Pulse()
        {
            return false;
            throw new NotImplementedException();
        }

        [ComVisible(true)]
        void IStyleInstance.Show(bool Visible)
        {
            return;
            throw new NotImplementedException();
        }

        [ComVisible(true)]
        void IStyleInstance.UpdateContent(ref notification_info NotificationInfo)
        {
            snarlText = NotificationInfo.Text;
            snarlTitle = NotificationInfo.Title;

            if (Properties.Settings.Default.mailServerHost == string.Empty || Properties.Settings.Default.username == string.Empty)
            {
                MessageBox.Show("Please set at least a mail server host and a username");
                return;
            }

            try
            {

                SmtpClient client = new SmtpClient(Properties.Settings.Default.mailServerHost, Properties.Settings.Default.mailServerPort);

                client.Credentials = new NetworkCredential(Properties.Settings.Default.username, Crypto.ToInsecureString(Crypto.DecryptString(Properties.Settings.Default.password)));
                client.EnableSsl = Properties.Settings.Default.mailServerUseSSL;

                using (MailMessage msg = new MailMessage())
                {
                    msg.From = new MailAddress(replacePlaceholder(Properties.Settings.Default.from));
                    msg.Subject = replacePlaceholder(Properties.Settings.Default.subject);
                    msg.Body = replacePlaceholder(Properties.Settings.Default.body);
                    msg.To.Add(new MailAddress(replacePlaceholder(Properties.Settings.Default.to)));

                    client.Send(msg);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        private string replacePlaceholder(string toBeWorkedOnText)
        {
            toBeWorkedOnText = toBeWorkedOnText.Replace("%title%", snarlTitle);
            return toBeWorkedOnText.Replace("%text%", snarlText);
        }


        #endregion


    }
}