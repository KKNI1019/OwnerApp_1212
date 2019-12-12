using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using owner.WebService;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace owner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FeeCalculateAddPage : ContentPage
	{
		public FeeCalculateAddPage ()
		{
			InitializeComponent ();

            var lbl_calc_only_tap = new TapGestureRecognizer();
            lbl_calc_only_tap.Tapped += (s, e) =>
            {
                Calc_Only();
            };
            lbl_calculate_only.GestureRecognizers.Add(lbl_calc_only_tap);
        }

        private void menuBtn_clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new SideMenuPopup());
        }

        private async void imgBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Add_btn_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(building_name.Text))
            {
                await DisplayAlert("", "家賃名を入力してください。", "はい");
            }
            else if (string.IsNullOrEmpty(rental_income.Text))
            {
                await DisplayAlert("", "家賃収入を入力してください。", "はい");
            }
            else if (string.IsNullOrEmpty(admin_expenses.Text))
            {
                await DisplayAlert("", "管理費を入力してください。", "はい");
            }
            else if (string.IsNullOrEmpty(repair_reserve.Text))
            {
                await DisplayAlert("", "修繕積立金を入力してください。", "はい");
            }
            else if (string.IsNullOrEmpty(agency_fee.Text))
            {
                await DisplayAlert("", "代行手数料を入力してください。", "はい");
            }
            else
            {
                checkRegisterInfo();
            }
        }

        private async void checkRegisterInfo()
        {
            loadingbar.IsRunning = true;

            using (var cl = new HttpClient())
            {
                
                var formcontent = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>(Constants.OWNER_ID, App.owner_ID),
                        new KeyValuePair<string, string>(Constants.ESTATE_NAME, building_name.Text),
                        new KeyValuePair<string, string>(Constants.RENTAL_INCOME, rental_income.Text),
                        new KeyValuePair<string, string>(Constants.ADMIN_EXPENSES, admin_expenses.Text),
                        new KeyValuePair<string, string>(Constants.REPAIR_RESERVE, repair_reserve.Text),
                        new KeyValuePair<string, string>(Constants.AGENCY_FEE, agency_fee.Text),
                        new KeyValuePair<string, string>(Constants.estate_property_tax, property_tax.Text)
                });

                try
                {
                    var request = await cl.PostAsync(Constants.SERVER_ZERO_REGIST_URL, formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        loadingbar.IsRunning = false;

                        ResponseData resultData = JsonConvert.DeserializeObject<ResponseData>(response);
                        //App.estate_num += 1;
                        
                        await Navigation.PushAsync(new ZeroRegisterAlert("管理物件追加"));
                    }
                    else
                    {
                        loadingbar.IsRunning = false;

                        await DisplayAlert("", resultMsg.resp, "はい");
                    }
                }
                catch
                {
                    loadingbar.IsRunning = false;

                    await DisplayAlert("", Constants.NETWORK_ERROR, "はい");
                }

            }
        }

        private async void Calc_Only()
        {
            if (string.IsNullOrEmpty(building_name.Text))
            {
                await DisplayAlert("", "家賃名を入力してください。", "はい");
            }
            else if (string.IsNullOrEmpty(rental_income.Text))
            {
                await DisplayAlert("", "家賃収入を入力してください。", "はい");
            }
            else if (string.IsNullOrEmpty(admin_expenses.Text))
            {
                await DisplayAlert("", "管理費を入力してください。", "はい");
            }
            else if (string.IsNullOrEmpty(repair_reserve.Text))
            {
                await DisplayAlert("", "修繕積立金を入力してください。", "はい");
            }
            else if (string.IsNullOrEmpty(agency_fee.Text))
            {
                await DisplayAlert("", "代行手数料を入力してください。", "はい");
            }
            else
            {
                App.new_agency_fee = agency_fee.Text;

                await Navigation.PushAsync(new FeeCalculatePage1(building_name.Text, rental_income.Text, admin_expenses.Text, repair_reserve.Text, agency_fee.Text));

                building_name.Text = "";
                admin_expenses.Text = "";
                rental_income.Text = "";
                repair_reserve.Text = "";
                agency_fee.Text = "";
                property_tax.Text = "";
            }

           
        }
    }
}