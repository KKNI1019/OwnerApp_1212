using Newtonsoft.Json;
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
    public partial class ConfirmEmailPage : ContentPage
    {
        public ConfirmEmailPage()
        {
            InitializeComponent();

            lbl_email.Text = App.owner_email;
        }

        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Btn_confirm_Clicked(object sender, EventArgs e)
        {
            loadingbar.IsRunning = true;

            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("owner_code", ent_confCode.Text),
                        new KeyValuePair<string, string>("owner_email",App.owner_email)
                });

                try
                {
                    var request = await cl.PostAsync(Constants.SERVER_BASE_URL + "email_confirm", formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        loadingbar.IsRunning = false;
                        await Navigation.PushAsync(new TabPage());
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

        private async void Lbl_sendAgain_tap(object sender, EventArgs e)
        {
            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("owner_email",App.owner_email)
                });

                try
                {
                    var request = await cl.PostAsync(Constants.SERVER_BASE_URL + "email_confirm", formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        loadingbar.IsRunning = false;
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