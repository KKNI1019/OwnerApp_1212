using Newtonsoft.Json;
using owner.Model;
using owner.WebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace owner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EstateSaleStep4Page : ContentPage
	{
		public EstateSaleStep4Page ()
		{
			InitializeComponent ();
		}

        private async void Sale_request_btn_Clicked(object sender, EventArgs e)
        {
            if (sale_price_zone.Text == "")
            {
                await DisplayAlert("", "売却希望金額を入力してください。", "はい");
            }
            else
            {
                Global.sale_price_zone = sale_price_zone.Text;

                sendSaleInfo();
            }
        }

        private async void imgBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Lbl_back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void sendSaleInfo()
        {
            loadingbar.IsRunning = true;

            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>(Constants.OWNER_ID, App.owner_ID),
                        new KeyValuePair<string, string>(Constants.OWNER_NAME, Global.sale_owner_name),
                        new KeyValuePair<string, string>(Constants.OWNER_ADRESS, Global.sale_owner_address),
                        new KeyValuePair<string, string>(Constants.OWNER_PHONE1, Global.sale_owner_phone),
                        new KeyValuePair<string, string>(Constants.ESTATE_NAME, Global.sale_estate_name),
                        new KeyValuePair<string, string>(Constants.ESTATE_ADDRESS, Global.sale_estate_address + Global.sale_estate_location),
                        new KeyValuePair<string, string>(Constants.ESTATE_ROOM_NUMBER, Global.sale_estate_room_number),
                        new KeyValuePair<string, string>(Constants.ESTATE_NEAR_STATTION, Global.sale_around_station),
                        new KeyValuePair<string, string>(Constants.ESTATE_WALKING_TIME, Global.sale_working_time),
                        new KeyValuePair<string, string>(Constants.RENTAL_INCOME, Global.sale_rental_income),
                        new KeyValuePair<string, string>(Constants.ADMIN_EXPENSES, Global.sale_admin_expenses),
                        new KeyValuePair<string, string>(Constants.SALE_PRICE_ZONE, Global.sale_price_zone)
                });

                try
                {
                    var request = await cl.PostAsync(Constants.SERVER_REGIST_SALE_URL, formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        loadingbar.IsRunning = false;

                        ResponseData resultData = JsonConvert.DeserializeObject<ResponseData>(response);
                        //App.estate_num += 1;

                        await Navigation.PushAsync(new EstateSaleStep5Page("pay"));
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