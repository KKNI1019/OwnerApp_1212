using Newtonsoft.Json;
using owner.WebService;
using Plugin.Media;
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
	public partial class MyPage1 : ContentPage
	{
		public MyPage1 ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);

            txt_name.Text = App.owner_name;
            txt_nickname.Text = App.owner_nickname;
            txt_email.Text = App.owner_email;
            txt_address.Text = App.owner_address;
            txt_phone.Text = App.owner_phone1 + " " + App.owner_phone2;

            if(App.owner_type.Equals("0"))
            {
                if (App.owner_profile != null)
                {
                    ImgUser.Source = Constants.IMAGE_UPLOAD_URL_PREFIX + App.owner_profile;
                }
                img_owner_type.Source = "free_member.png";
            } 
            else
            {
                if (App.owner_profile != null)
                {
                    ImgUser.Source = Constants.IMAGE_UPLOAD_URL_PREFIX + App.owner_profile;
                }
                img_owner_type.Source = "paid_member.png";
            }

            lbl_editFinish.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => EditFinish_Clicked()),
            });

            var onImgTap = new TapGestureRecognizer();
            onImgTap.Tapped += (s, e) =>
            {
                imgMember_clicked();
            };
            ImgUser.GestureRecognizers.Add(onImgTap);
        }

        private async void imgMember_clicked()
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Not supported", "Your device does not support this functionality.", "OK");
                return;
            }
            var mediaOptions = new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Medium
            };
            var selectedImgFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);
            if (selectedImgFile == null)
            {
                await DisplayAlert("Error", "Could not get the image, please try again.", "OK");
                return;
            }

            try
            {
                loadingbar.IsRunning = true;

                StreamContent scontent = new StreamContent(selectedImgFile.GetStream());
                scontent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    FileName = App.owner_ID,
                    Name = "image"
                };
                scontent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                var client = new HttpClient();
                var multi = new MultipartFormDataContent();
                multi.Add(scontent);
                client.BaseAddress = new Uri(Constants.ENDPOINT_URL);
                var result = client.PostAsync("api/owner/upload_profile", multi).Result;
                result.EnsureSuccessStatusCode();
                var response = await result.Content.ReadAsStringAsync();
                ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);
                if (resultMsg.resp == "success")
                {
                    loadingbar.IsRunning = false;

                    ImgUser.Source = ImageSource.FromStream(() => selectedImgFile.GetStream());
                    App.owner_profile = resultMsg.owner_profile;
                    
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
        

        private async void EditFinish_Clicked()
        {
            App.owner_name = txt_name.Text;
            App.owner_nickname = txt_nickname.Text;
            App.owner_email = txt_email.Text;
            App.owner_address = txt_address.Text;
            App.owner_phone1 = txt_phone.Text;

            if (string.IsNullOrWhiteSpace(txt_name.Text) || string.IsNullOrWhiteSpace(txt_nickname.Text) || string.IsNullOrWhiteSpace(txt_address.Text) || string.IsNullOrWhiteSpace(txt_email.Text) || string.IsNullOrWhiteSpace(txt_phone.Text))
            {
                await DisplayAlert("", "詳細情報を正確に入力してください。", "はい");
            }
            else
            {
                loadingbar.IsRunning = true;

                using (var cl = new HttpClient())
                {
                    var formcontent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>(Constants.OWNER_ID, App.owner_ID),
                        new KeyValuePair<string, string>(Constants.OWNER_NAME, App.owner_name),
                        new KeyValuePair<string, string>(Constants.OWNER_NICKNAME, App.owner_nickname),
                        new KeyValuePair<string, string>(Constants.OWNER_ADRESS, App.owner_address),
                        new KeyValuePair<string, string>(Constants.OWNER_EMAIL, App.owner_email),
                        new KeyValuePair<string, string>(Constants.OWNER_PHONE1, App.owner_phone1)
                    });

                    try
                    {
                        var request = await cl.PostAsync(Constants.SERVER_OWNER_PROFILE_UPDATE_URL, formcontent);
                        request.EnsureSuccessStatusCode();
                        var response = await request.Content.ReadAsStringAsync();
                        ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                        if (resultMsg.resp.Equals("success"))
                        {
                            loadingbar.IsRunning = false;

                            await Navigation.PopAsync();

                            MessagingCenter.Send<App>((App)Application.Current, "OnProfileInfoUpdated");
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

                        await DisplayAlert("", "サーバー接続でエラーが発生しました。", "はい");
                    }
                }
            }
        }
    }
}