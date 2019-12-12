using System;
using System.Collections.Generic;
using System.Text;

namespace owner.Model
{
    public class ReportItems
    {
        public string fee_name { get; set; }
        public string calculate_type { get; set; }
        public string[] fee_value { get; set; }
        public int dynamic_index { get; set; }
    }
}
