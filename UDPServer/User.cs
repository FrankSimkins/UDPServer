using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace UDPServer
{
    class User
    {
        #region Fields

        private string _userName;
        private IPAddress _userIP;
        private int _port;
        private IPEndPoint _userEndPoint;

        #endregion

        #region Properties

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public IPAddress UserIP
        {
            get { return _userIP; }
            set { _userIP = value; }
        }

        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        public IPEndPoint UserEndPoint
        {
            get { return _userEndPoint; }
            set { _userEndPoint = value; }
        }

        #endregion

        #region Constructors

        public User()
        {

        }

        #endregion

        #region Methods

        #endregion

    }
}
