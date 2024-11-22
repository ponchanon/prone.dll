using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRone
{
    public class Messenger
    {
        /// <summary>
        /// Summary description for Class1.
        /// </summary>
        /// 
        [Serializable]
        public class LetterReceive
        {
            string message;
            string from;

            public LetterReceive(string message, string from)
            {
                this.message = message;
                this.from = from;
            }

            public string Message
            {
                get
                {
                    return message;
                }
            }

            public string From
            {
                get
                {
                    return from;
                }
            }
        }

        public interface IServer
        {
            bool SignUp(string username, string password);
            bool ChangePassword(string username, string curPassword, string newPassword);
            bool SignIn(string username, string password, bool visible);
            bool SignOut(string username);

            bool IsVisible(string username);
            bool AddContact(string username, string contact);
            bool RemoveContact(string username, string contact);
            ArrayList GetContacts(string username);

            bool Send(string from, string to, string message);
            LetterReceive Receive(string to);
            ArrayList ReceiveOffline(string to);
        }
    }
}
