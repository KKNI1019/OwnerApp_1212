using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace owner.Model
{
    public class News
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        
        public string news_id { get; set; }
        public string news_image_url { get; set; }
        public string news_image_url_header { get; set; }
        public string news_image_url_contents { get; set; }
        public string news_category { get; set; }
        public string news_title { get; set; }
        public DateTime news_date { get; set; }
        public string news_source { get; set; }
        public string writer_image { get; set; }
        public string writer_name { get; set; }
        public string writer_profile { get; set; }
        public int comment_count { get; set; }
        public string news_url { get; set; }
        public string news_contents { get; set; }
        public string imgfavorite { get; set; }
        public int img_header_height { get; set; }

        public string brief_comment { get; set; }
        public string first_commenter_image { get; set; }
        public string second_commenter_image { get; set; }
        public string third_commenter_image { get; set; }
        public string first_commenter_name { get; set; }
        public string second_commenter_name { get; set; }
        public string third_commenter_name { get; set; }
        
        //public IList<News_Comments> news_comment_data { get; set; }
        public List<News_Comments> news_comment_data { get; set; }

        public string favoriteImg_source { get; set; }
    }
    
}
