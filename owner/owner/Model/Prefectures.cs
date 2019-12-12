using System;
using System.Collections.Generic;
using System.Text;

namespace owner.Model
{
    public class Prefectures
    {
        public string prefecture_id { get; set; }
        public string prefecture_name { get; set; }
        public IList<City> city { get; set; }
    }
}
