using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TurboPotato.ServerSideBlazor.Data
{
    public class ChatMessage
    {
        public string From { get; set; }
        public DateTime SentAtUtc { get; set; }

        public string Message { get; set; }
    }
}
