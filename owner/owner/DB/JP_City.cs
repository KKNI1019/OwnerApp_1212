using System;
using System.Collections.Generic;
using System.Text;
//using SQLite.Net.Attributes;
using SQLite;

namespace owner.DB
{
    [Table("owner_city")]
    public class JP_City
    {
        
        public string field1 { get; set; }
        public string field2 { get; set; }
    }
}
