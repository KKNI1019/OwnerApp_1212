using System;
using System.Collections.Generic;
using System.Text;

namespace owner.Model
{
    public class News_Comments
    {
        public string news_comment_id { get; set; }
        public string news_comment_writer_image { get; set; }
        public string news_comment_writer_name { get; set; }
        public string news_comment_writer_profile { get; set; }
        public string news_comment_contents { get; set; }
        public string news_comment_likes { get; set; }
        public DateTime c_date { get; set; }
    }
}
