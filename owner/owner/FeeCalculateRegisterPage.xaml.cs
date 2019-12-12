using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using owner.Model;
using owner.WebService;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace owner
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeeCalculateRegisterPage : ContentPage
    {
        private string rental_income_value;
        private string admin_expenses_value;
        private string repair_reserve_value;
        private string agency_fee_value;
        private string estate_name;
        private string estate_rental_income;
        private string estate_admin_expense;
        private string estate_repair_reserve;
        private string estate_agency_fee;
        private string estate_property_tax;

        public FeeCalculateRegisterPage()
        {
            InitializeComponent();

            rental_income_value = Preferences.Get(Constants.RENTAL_INCOME, "");
            rental_income.Text = rental_income_value;
            admin_expenses_value = Preferences.Get(Constants.ADMIN_EXPENSES, "");
            admin_expenses.Text = admin_expenses_value;
            repair_reserve_value = Preferences.Get(Constants.REPAIR_RESERVE, "");
            repair_reserve.Text = repair_reserve_value;
            agency_fee_value = Preferences.Get(Constants.AGENCY_FEE, "");
            agency_fee.Text = agency_fee_value;
            
        }

        private void menuBtn_clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new SideMenuPopup());
        }

        private async void imgBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Update_btn_Clicked(object sender, EventArgs e)
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
                 estate_name = building_name.Text;
                 estate_rental_income = rental_income.Text;
                 estate_admin_expense = admin_expenses.Text;
                 estate_repair_reserve = repair_reserve.Text;
                 estate_agency_fee = agency_fee.Text;
                 estate_property_tax = property_tax.Text;

                var formcontent = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>(Constants.OWNER_ID, App.owner_ID),
                        new KeyValuePair<string, string>(Constants.ESTATE_NAME, Global.MakeZero(estate_name)),
                        new KeyValuePair<string, string>(Constants.RENTAL_INCOME, Global.MakeZero(estate_rental_income)),
                        new KeyValuePair<string, string>(Constants.ADMIN_EXPENSES, Global.MakeZero(estate_admin_expense)),
                        new KeyValuePair<string, string>(Constants.REPAIR_RESERVE, Global.MakeZero(estate_repair_reserve)),
                        new KeyValuePair<string, string>(Constants.AGENCY_FEE, Global.MakeZero(estate_agency_fee)),
                        new KeyValuePair<string, string>(Constants.estate_property_tax, Global.MakeZero(estate_property_tax))
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

                        Preferences.Set(Constants.RENTAL_INCOME, "");
                        Preferences.Set(Constants.ADMIN_EXPENSES, "");
                        Preferences.Set(Constants.AGENCY_FEE, "");
                        Preferences.Set(Constants.REPAIR_RESERVE, "");

                        App.new_agency_fee = agency_fee.Text;
                                                
                        await Navigation.PushAsync(new FeeCalculatePage1(estate_name, estate_rental_income, estate_admin_expense, estate_repair_reserve, estate_agency_fee));
                        
                        lbl_intro.Text = "追加する物件情報を入力してください。";
                        building_name.Text = "";
                        rental_income.Text = "";
                        admin_expenses.Text = "";
                        repair_reserve.Text = "";
                        agency_fee.Text = "";
                        property_tax.Text = "";
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
    }

    
}