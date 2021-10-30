using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Notification
{
    public class NotoficationDictionary
    {
        public static IDictionary<string, WebSocket> Sockets = new Dictionary<string, WebSocket>();
    }
}
