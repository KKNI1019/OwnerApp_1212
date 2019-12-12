//using SQLite.Net.Attributes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace owner.Model
{
    public class Columns
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string column_id { get; set; }
        public DateTime date { get; set; }
        public bool IsVisible { get; set; }
        public string col_title { get; set; }
        public string col_content { get; set; }
        public string user_name { get; set; }
        public string img_url { get; set; }
    }
}
