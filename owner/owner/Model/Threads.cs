//using SQLite.Net.Attributes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace owner.Model
{
    public class Threads
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Th_id { get; set; }
        public string Th_category { get; set; }
        public string Th_content { get; set; }
        public string Th_note { get; set; }
        public DateTime Date { get; set; }
        public string img_url { get; set; }
        public string user_name { get; set; }
        public string Last_comment_Id { get; set; }
    }
}
