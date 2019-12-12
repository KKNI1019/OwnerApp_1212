using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace owner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FeeCalculatePage1 : ContentPage
	{
        private string estate_name;
        private string estate_rental_income;
        private string estate_admin_expense;
        private string estate_repair_reserve;
        private string estate_agency_fee;

		public FeeCalculatePage1 (string building_name, string rental_income, string admin_expense, string repair_reserve, string agency_fee)
		{
			InitializeComponent ();

            estate_name = building_name;
            estate_rental_income = rental_income;
            estate_admin_expense = admin_expense;
            estate_repair_reserve = repair_reserve;
            estate_agency_fee = agency_fee;
            
            span_total_agency.Text = (Convert.ToInt32(estate_agency_fee) * 12).ToString();
            span_total_budifee.Text = (App.programm_fee*12).ToString();
            span_total_diff.Text = (Convert.ToInt32(estate_agency_fee) * 12 - (App.programm_fee * 12)).ToString();

            lbl_buildingName.Text = estate_name;
            year_rental_income.Text = (Convert.ToInt32(estate_rental_income) *12).ToString();
            year_adminFee.Text = (Convert.ToInt32(estate_admin_expense) * 12).ToString();
            year_repairFee.Text = (Convert.ToInt32(estate_repair_reserve) * 12).ToString();
            year_agencyFee.Text = (Convert.ToInt32(estate_agency_fee) * 12).ToString();
            year_balance.Text = (Convert.ToInt32(year_rental_income.Text) - Convert.ToInt32(year_adminFee.Text) - Convert.ToInt32(year_repairFee.Text) - Convert.ToInt32(year_agencyFee.Text)).ToString();
            month_balance.Text = (Convert.ToInt32(estate_rental_income) - Convert.ToInt32(estate_admin_expense) - Convert.ToInt32(estate_repair_reserve) - Convert.ToInt32(estate_agency_fee)).ToString();

            var black_frames_click = new TapGestureRecognizer();
            black_frames_click.Tapped += (s, e) =>
            {
                Alert_show();
            };
            frame_budi_fee.GestureRecognizers.Add(black_frames_click);
            frame_repair_reserve.GestureRecognizers.Add(black_frames_click);
            frame_other_fee.GestureRecognizers.Add(black_frames_click);
            frame_loan_repay.GestureRecognizers.Add(black_frames_click);
            frame_loan_amount.GestureRecognizers.Add(black_frames_click);
            frame_yearly_profit.GestureRecognizers.Add(black_frames_click);
            frame_repay_period.GestureRecognizers.Add(black_frames_click);
        }

        private async void Alert_show()
        {
            if(App.owner_type == "0"){
                var update = await DisplayAlert("", "有料版にアップデートしますか？", "アップデート", "キャンセル");
                if (update)
                {
                    await Navigation.PushAsync(new MyInfoUpdatePage());
                }
            }
            else
            {
                var request = await DisplayAlert("", "物件の管理依頼をしますか？", "依頼する", "キャンセル");
                if (request)
                {
                    await Navigation.PushAsync(new ZeroRegisterAlert("管理物件追加"));
                }
            }
            
        }

        private void menuBtn_clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new SideMenuPopup());
        }

        private async void imgBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void AddBuilding_Btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}