using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Utils;

namespace Ereader.Knowledge{
    public class Wiki{

        public void QueryPageInfo(string query){
            
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors) => { return true; });
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                "https://en.wikipedia.org/w/api.php?action=query&prop=categories|info|description|images|info|links&format=xmlfm&titles=" + query);//
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string myResponse = "";
            using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream())){
                myResponse = sr.ReadToEnd();
            }
            Console.Write("");
        }
        

        private static bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                //Return true if the server certificate is ok
                if (sslPolicyErrors == SslPolicyErrors.None)
                    return true;
        
                bool acceptCertificate = true;
                string msg = "The server could not be validated for the following reason(s):\r\n";
        
                //The server did not present a certificate
                if ((sslPolicyErrors &
                     SslPolicyErrors.RemoteCertificateNotAvailable) == SslPolicyErrors.RemoteCertificateNotAvailable)
                {
                    msg = msg + "\r\n    -The server did not present a certificate.\r\n";
                    acceptCertificate = false;
                }
                else
                {
                    //The certificate does not match the server name
                    if ((sslPolicyErrors &
                         SslPolicyErrors.RemoteCertificateNameMismatch) == SslPolicyErrors.RemoteCertificateNameMismatch)
                    {
                        msg = msg + "\r\n    -The certificate name does not match the authenticated name.\r\n";
                        acceptCertificate = false;
                    }
        
                    //There is some other problem with the certificate
                    if ((sslPolicyErrors &
                         SslPolicyErrors.RemoteCertificateChainErrors) == SslPolicyErrors.RemoteCertificateChainErrors)
                    {
                        foreach (X509ChainStatus item in chain.ChainStatus)
                        {
                            if (item.Status != X509ChainStatusFlags.RevocationStatusUnknown &&
                                item.Status != X509ChainStatusFlags.OfflineRevocation)
                                break;
        
                            if (item.Status != X509ChainStatusFlags.NoError)
                            {
                                msg = msg + "\r\n    -" + item.StatusInformation;
                                acceptCertificate = false;
                            }
                        }
                    }
                }
        
                //If Validation failed, present message box
                if (acceptCertificate == false)
                {
                    msg = msg + "\r\nDo you wish to override the security check?";
        //          if (MessageBox.Show(msg, "Security Alert: Server could not be validated",
        //                       MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        acceptCertificate = true;
                }
        
                return acceptCertificate;
            }
    }
    
}


// WIKIPEDIA API

//Useful page info (prop aka properties):
//categories
//description
//extracts (returns html or plain text)
//images
//info
//Links
//e.g. https://en.wikipedia.org/w/api.php?action=query&prop=categories|info|description|images|info|links&format=xmlfm&titles=snow
//NOTE: If page doesn’t exist it will have a negative id (_idx)
//Get page content
//https://en.wikipedia.org/w/api.php?action=query&format=xmlfm&prop=revisions&titles=snow&rvprop=content&rvslots=*&rvlimit=1
//