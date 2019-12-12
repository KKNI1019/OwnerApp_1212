using System;
using System.Collections.Generic;
using System.Text;

namespace owner.Model
{
    public class MessageItem
    {
        public string imgUser { get; set; }
        public string imgBot { get; set; }
        public string UserQuestion { get; set; }
        public string BotAnswer { get; set; }
        public bool userFrameVisibility { get; set; }
        public bool botFrameVisibility { get; set; }
        public bool imgBotVisibility { get; set; }
        public bool imgUserVisibility { get; set; }
    }
}
