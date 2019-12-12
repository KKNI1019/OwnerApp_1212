using System;
using System.Collections.Generic;
using System.Text;

namespace owner.Model
{
    public class Global
    {
        public static string zero_estate_address { get; set; }
        public static string zero_estate_room_number { get; set; }
        public static string zero_estate_name { get; set; }
        public static string zero_type { get; set; }
        public static string zero_company { get; set; }
        public static string zero_company_address { get; set; }
        public static string zero_agency_name { get; set; }
        public static string zero_agency_phone { get; set; }
        public static string zero_end_date { get; set; }
        public static string zero_rent_income { get; set; }

        public static string sale_estate_address { get; set; }
        public static string sale_around_station { get; set; }
        public static string sale_working_time { get; set; }
        public static string sale_rental_income { get; set; }
        public static string sale_admin_expenses { get; set; }
        public static string sale_owner_name { get; set; }
        public static string sale_owner_address { get; set; }
        public static string sale_owner_postal_address { get; set; }
        public static string sale_owner_phone { get; set; }
        public static string sale_estate_location { get; set; }
        public static string sale_estate_name { get; set; }
        public static string sale_estate_room_number { get; set; }
        public static string sale_price_zone { get; set; }
        public static string sale_certify_type { get; set; }

        public static IList<BuildingInfo> Buildings { get; set; }
        public static IList<ReportItems> reportItems { get; set; }
        public static IList<News> news { get; set; }
        public static IList<string> update_months { get; set; }
        public static IList<Prefectures> prefectures { get; set; }

        public static string[] current_year_prices = new string[13];

        public static Dictionary<string, ReportItems> dic { get; set; }

        public static int items_num { get; set; }
        public static int month { get; set; }

        public static string MakeZero(string tt)
        {
            if (string.IsNullOrEmpty(tt))
            {
                return "0";
            }
            else
            {
                return tt;
            }
        }
    }
}
