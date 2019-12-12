using System;
using System.Collections.Generic;
using System.Text;

namespace owner.WebService
{
    public class ResponseData
    {
        public owner_data owner_data { get; set; }
        public estate_data estate_data { get; set; }
        public thread_list_data[] thread_list_data { get; set; }
        public comment_list_data[] comment_list_data { get; set; }
        public notice_list_data[] notice_list_data { get; set; }
        public thread_comment_data[] thread_comment_data { get; set; }
        public string pay_info_id { get; set; }
        public int program_fee { get; set; }
        public string agency_fee { get; set; }
        public float income_rate { get; set; }
        public news_data[] news_data { get; set; }
        public string income { get; set; }
        public string outcome { get; set; }
        public calculation_fixed_data[] calculation_fixed_data { get; set; }
        public calculation_dynamic_data[] calculation_dynamic_data { get; set; }
        public calculation_data[] calculation_data { get; set; }
        public prefecture_data[] prefecture_data { get; set; }
        public city_data[] city_data { get; set; }
        public sale_request_data[] sale_request_data { get; set; }
        public string zero_id { get; set; }
    }

    public class calculation_data
    {
        public string fee_name;
        public string calculation_type;
        public string[] fee_value;
        public int dynamic_index;
    }
    

    public class estate_data
    {
        public string estate_id;
        public string estate_name;
        public string estate_address;
        public string estate_room_number;
        public string estate_rent;
        public string estate_owner_id;
        public string estate_sale_status;
        public string estate_zero_status;
        public string estate_memo;
    }

    public class owner_estate_data
    {
        public string estate_id;
        public string estate_name;
        public string estate_address;
        public string estate_room_number;
        public string estate_rent;
        public string estate_owner_id;
        public string estate_sale_status;
        public string estate_zero_status;
        public string estate_memo;
        public string estate_agency_fee;
        public string estate_repair_reserve;
        public string estate_admin_expense;
        public string estate_image_url;
        public int estate_loan_repay;
        public int estate_loan_amount;
        public int estate_yearly_profit;
        public string estate_repay_period;
        public int estate_property_tax;

    }

    public class owner_data
    {
        public string owner_id;
        public string owner_name;
        public string owner_kana;
        public string owner_nickname;
        public string owner_email;
        public string owner_password;
        public string owner_phone1;
        public string owner_phone2;
        public string owner_estate_id;
        public string owner_address;
        public string owner_postal_address;
        public string owner_memo;
        public string owner_type;
        public string owner_profile;
        public owner_estate_data[] owner_estate_data;
        public string pay_info_id;
        public string owner_license;
    }

    public class thread_list_data
    {
        public string thread_id;
        public string thread_category;
        public string thread_note;
        public DateTime u_date;
    }

    public class comment_list_data
    {
        public string comment_id;
        public string comment_title;
        public string comment_writer_user_name;
        public string comment_contents;
        public DateTime u_date;
        public string user_profile;
    }

    public class notice_list_data
    {
        public string notice_id;
        public string notice_title;
        public string notice_contents;
        public string notice_kind;
        public string notice_destination;
        public DateTime u_date;
        public string other_id;
    }

    public class thread_comment_data
    {
        public string thread_comment_id;
        public string thread_comment_contents;
        public string thread_comment_writer_nickname;
        public string thread_comment_category;
        public DateTime c_date;
        public string user_profile;
    }

    public class news_data
    {
        public string news_id { get; set; }
        public string news_image_url { get; set; }
        public string news_category { get; set; }
        public string news_title { get; set; }
        public DateTime news_date { get; set; }
        public string news_source { get; set; }
        public string news_writer_image { get; set; }
        public string news_writer_name { get; set; }
        public string news_writer_profile { get; set; }
        public int comment_count { get; set; }
        public string news_url { get; set; }
        public string news_contents { get; set; }
        public news_comment_data[] news_comment_data { get; set; }
    }

    public class news_comment_data
    {
        public string news_comment_id;
        public string news_comment_writer_image;
        public string news_comment_writer_name;
        public string news_comment_writer_profile;
        public string news_comment_contents;
        public string news_comment_likes;
        public DateTime c_date;
    }

    public class calculation_fixed_data
    {
        public string rent;
        public string admin_expense;
        public string repair_reserve;
        public string property_tax;
        public string agency_fee;
        public DateTime c_date;
    }
    public class calculation_dynamic_data
    {
        public string dynamic_title;
        public string dynamic_value;
        public DateTime c_date;
    }
    public class prefecture_data
    {
        public string prefecture_name;
        public string prefecture_id;
    }
    public class city_data{
        public string city_name;
        public string prefecture_id;
    }

    public class sale_request_data
    {
        public string sale_request_id;
        public string request_user_name;
        public string request_price;
        public string sale_id;
    }
}
