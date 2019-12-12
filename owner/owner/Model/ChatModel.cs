using System;
using System.Collections.Generic;
using System.Text;

namespace owner.Model
{
    public class ChatModel
    {
        public double XValue { get; set; }

        public double YValue { get; set; }

        public ChatModel(double xValue, double yValue)
        {
            XValue = xValue;
            YValue = yValue;
        }
    }
}
