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
	public partial class ZeroRegisterPage1 : ContentPage
	{
		public ZeroRegisterPage1 ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void Back_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Start_btn_Clicked(object sender, EventArgs e)
        {
            if (zero_company.Text == "")
            {
                await DisplayAlert("", "管理会社名を入力してください。", "はい");
            }
            else if (agency_name.Text == "")
            {
                await DisplayAlert("", "担当者名を入力してください。", "はい");
            }
            else if (agency_phone.Text == "")
            {
                await DisplayAlert("", "担当者連絡先を入力してください。", "はい");
            }
            else if (end_date_year.Text == "" || end_date_month.Text == "" || end_date_day.Text == "")
            {
                await DisplayAlert("", "解除予定日を正確に入力してください。", "はい");
            }
            else
            {
                Global.zero_company = zero_company.Text;
                Global.zero_agency_name = agency_name.Text;
                Global.zero_agency_phone = agency_phone.Text;
                Global.zero_end_date = end_date_year.Text + "/" + end_date_month.Text + "/" + end_date_day.Text;

                SendZeroInfo();
            }
        }

        private async void SendZeroInfo()
        {
            using (var cl = new HttpClient())
            {
                loadingbar.IsRunning = true;

                var formcontent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>(Constants.OWNER_ID, App.owner_ID),
                    new KeyValuePair<string, string>(Constants.ESTATE_NAME, Global.zero_estate_name),
                    new KeyValuePair<string, string>(Constants.ESTATE_ADDRESS, Global.zero_estate_address),
                    new KeyValuePair<string, string>(Constants.ESTATE_ROOM_NUMBER, Global.zero_estate_address),
                    new KeyValuePair<string, string>(Constants.ZERO_COMPANY, Global.zero_company),
                    new KeyValuePair<string, string>(Constants.ZERO_END_DATE, Global.zero_end_date),
                    new KeyValuePair<string, string>(Constants.ZERO_TYPE, Global.zero_type),
                    new KeyValuePair<string, string>(Constants.ZERO_AGENCY_NAME, Global.zero_agency_name),
                    new KeyValuePair<string, string>(Constants.ZERO_AGENCY_PHONE, Global.zero_agency_phone),
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

                        App.owner_estate_id += resultMsg.estate_id + ",";
                        
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