using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TurboPotato.ServerSideBlazor.Data
{
    public class InMemoryChatMessageRepository
    {
        private IList<ChatMessage> chatMessages;

        public InMemoryChatMessageRepository()
        {
            chatMessages = new List<ChatMessage>();
        }

        public IReadOnlyList<ChatMessage> GetChatMessages()
        {
            return chatMessages.ToArray();
        }

        public void AddChatMessage(ChatMessage message)
        {
            chatMessages.Add(message);
        }
    }
}
