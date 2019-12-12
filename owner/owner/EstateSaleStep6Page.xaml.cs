using Newtonsoft.Json;
using owner.WebService;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace owner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EstateSaleStep6Page : ContentPage
	{
        private MediaFile mediafile;
        public EstateSaleStep6Page (MediaFile file)
		{
			InitializeComponent ();

            img_certificationPhoto.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

            mediafile = file;
        }

        private void ImgBtn_sendCerti_Clicked(object sender, EventArgs e)
        {
            sendFile();
        }

        private async void sendFile()
        {
            loadingbar.IsRunning = true;

            try
            {
                StreamContent scontent = new StreamContent(mediafile.GetStream());
                scontent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    FileName = App.owner_ID,
                    Name = "image"
                };
                scontent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                var client = new HttpClient();
                var multi = new MultipartFormDataContent();
                multi.Add(scontent);
                client.BaseAddress = new Uri("http://192.168.0.129:5000/real_estate_management/");
                var result = client.PostAsync("api/user/upload_license", multi).Result;
                result.EnsureSuccessStatusCode();
                var response = await result.Content.ReadAsStringAsync();
                ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);
                if (resultMsg.resp == "success")
                {
                    loadingbar.IsRunning = false;

                    DependencyService.Get<IMessage>().LongAlert(resultMsg.message);
                    //await Navigation.PushAsync(new LoginPage());
                }
                else
                {
                    loadingbar.IsRunning = false;

                    await DisplayAlert("", resultMsg.message, "はい");
                }

            }
            catch
            {
                loadingbar.IsRunning = false;

                await DisplayAlert("", "サーバー接続でエラーが発生しました。", "はい");
            }
        }

        private async void BtnCaptureAgain_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void stkBack_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void imgBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Send_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EstateSaleStep7Page());
        }
    }
}