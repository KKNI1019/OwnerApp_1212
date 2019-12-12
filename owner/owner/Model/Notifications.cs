//using SQLite.Net.Attributes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace owner.Model
{
    public class Notifications
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public DateTime date { get; set; }
        public bool IsVisible { get; set; }
        public string noti_id { get; set; }
        public string noti_title { get; set; }
        public string noti_content { get; set; }
        public string noti_kind { get; set; }
        public string noti_destination { get; set; }
        public string img_source { get; set; }
        public string other_id { get; set; }
    }
}
