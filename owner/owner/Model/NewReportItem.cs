using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace owner.Model
{
    public class NewReportItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string item_index { get; set; }
        public string item_name { get; set; }
        public string item_value { get; set; }
        public string item_type { get; set; }
    }
}
