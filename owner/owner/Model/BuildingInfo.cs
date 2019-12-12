using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace owner.Model
{
    public class BuildingInfo
    {
        public string building_id { get; set; }
        public string building_name { get; set; }
        public string building_img { get; set; }
        public int rental_income { get; set; }
        public int admin_expense { get; set; }
        public int agency_fee { get; set; }
        public int repair_reserve { get; set; }
        public string estate_image_url { get; set; }
        public int estate_loan_repay { get; set; }
        public int estate_loan_amount { get; set; }
        public int estate_yearly_profit { get; set; }
        public string estate_repay_period { get; set; }
        public int estate_property_tax { get; set; }
        public string zero_status { get; set; }


        public int monthly_balance { get; set; }
        public double yearly_balance { get; set; }
        public int yearly_rental_income { get; set; }
        public int yearly_admin_expense { get; set; }
        public int yearly_repair_reserve { get; set; }
        public int yearly_agency_fee { get; set; }
        public int yearly_property_tax { get; set; }
        public int program_fee { get; set; }
        public int yearly_other_fee { get; set; }

        public double Width { get; set; }

        public ObservableCollection<ChatModel> Data2 { get; set; }
        public ObservableCollection<ChatModel> Data3 { get; set; }
        public ObservableCollection<ChatModel> Data4 { get; set; }

        public double sale_loss { get; set; }
        public double sale_amount { get; set; }
        public double remaining_amount { get; set; }

    }
}
