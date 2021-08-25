using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketUtil;

namespace KChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            new Server(1234).StartSocket();
        }
    }
}
