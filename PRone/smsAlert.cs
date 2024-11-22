using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace PRone
{
    public class SMSAlert
    {
        public static string sendSmsAlert(string senderNo, string[] receiverNo, string smsHost, string user, string pass, string msgValue)
        {
            string msgBody = msgValue;
            string smsResponse = "";
            //DbOperation dbop = new DbOperation();

            try
            {
                for (int smscount = 0; smscount < receiverNo.Length;smscount++ )
                {
                    string url = "http://" + smsHost + "/cgi-bin/sendsms?username="+user+"&password="+pass+"&from="+senderNo+"&to=" + receiverNo[smscount] + "&text=" + msgBody;
                    //string url = "http://" + smsHost + "/cgi-bin/sendsms?username=tester&password=foobar&from=test&to=" + receiverNo[smscount] + "&text=" + msgBody;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "GET";
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    //smsResponse = response.GetResponseStream.CharacterSet.ToString();
                    //StreamReader reader = new StreamReader(response.GetResponseStream());
                    smsResponse = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                }                
            }
            catch (Exception ex)
            {
                smsResponse = "Error:SMS";
            }
            return smsResponse;
        }
    
    }
}
