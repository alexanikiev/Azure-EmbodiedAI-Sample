using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

namespace AzureEmbodiedAISamples
{
    [Inspectable]
    public class SampleConversation
    {
        public string ConversationId { get; set; }
        public SampleMessage[] Messages { get; set; }
        public string Context { get; set; }
        public string CurrentMessageId { get; set; }

        public SampleConversation()
        {
            ConversationId = new Guid().ToString();
            Messages = new SampleMessage[] { };
            Context = "Clippy is an endearing and helpful digital assistant, designed to make using Microsoft Office Suite of products more efficient and user-friendly. With his iconic paperclip shape and friendly personality, Clippy is always ready and willing to assist users with any task or question they may have. His ability to anticipate and address potential issues before they even arise has made him a beloved and iconic figure in the world of technology, widely recognized as an invaluable tool for productivity.\n\n";
            CurrentMessageId = string.Empty;
        }

        public void AppendMessage(string text)
        {
            List<SampleMessage> messages = new List<SampleMessage>(Messages);
            SampleMessage message = new SampleMessage { Id = messages.Count.ToString(), ParentMessageId = CurrentMessageId, Text = text };
            messages.Add(message);
            Messages = messages.ToArray();
            CurrentMessageId = message.Id;
        }

        public string CreatePrompt(bool debug)
        {
            string promptString = String.Join("\n", Messages.Select((m, i) => (i % 2 == 0) ? "Clippy: " + m.Text : "Human: " + m.Text));

            return Context + promptString + (debug ? string.Empty : "\nClippy:");
        }
    }
}