using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;

namespace PRone
{
    public class FtpOperation
    {
        public bool FtpDirectoryExists(string directoryPath, string ftpUser, string ftpPassword)
        {
            bool IsExists = true;
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(directoryPath);
                request.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                request.Method = WebRequestMethods.Ftp.PrintWorkingDirectory;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                IsExists = false;
            }
            return IsExists;
        }
        public string DisplayFileFromServer(Uri serverUri,string user,string password)
        {
            string fileString = "NA";
            if (serverUri.Scheme != Uri.UriSchemeFtp)
                return fileString;

            WebClient request = new WebClient();
            request.Credentials = new NetworkCredential(user, password);
            try
            {
                byte[] newFileData = request.DownloadData(serverUri.ToString());
                fileString = System.Text.Encoding.UTF8.GetString(newFileData);
            }
            catch (WebException e)
            {
                fileString = "ERROR *** " + e.ToString();
            }
            return fileString;
        }
    }
}
