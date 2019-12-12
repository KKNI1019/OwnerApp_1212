using Newtonsoft.Json;
using owner.Model;
using owner.WebService;
using Rg.Plugins.Popup.Services;
using Stormlion.PhotoBrowser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace owner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotificationDetailPage : ContentPage
	{
        private Notifications mNotification;
        private string selected_notice_id;
        private string selected_other_id;
        
		public NotificationDetailPage ()
		{
			InitializeComponent ();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            mNotification = this.BindingContext as Notifications;
            selected_notice_id = mNotification.noti_id;
            selected_other_id = mNotification.other_id;
            
            if (mNotification.noti_kind == "3")
            {
                sale_confirm_btn.IsVisible = false;
                btn_cancel_confirm.IsVisible = true;
                lbl_confirm_image.IsVisible = true;
                lbl_wrong_cost.IsVisible = true;
            }
            else if (mNotification.noti_kind == "2")
            {
                sale_confirm_btn.IsVisible = true;
                btn_cancel_confirm.IsVisible = false;
                lbl_confirm_image.IsVisible = false;
                lbl_wrong_cost.IsVisible = false;
            }
            else
            {
                sale_confirm_btn.IsVisible = false;
                btn_cancel_confirm.IsVisible = false;
                lbl_confirm_image.IsVisible = false;
                lbl_wrong_cost.IsVisible = false;
            }

        }

        protected override bool OnBackButtonPressed()
        {
            removeNew();

            return base.OnBackButtonPressed();
        }

        private async void imgBack_Clicked(object sender, EventArgs e)
        {
            removeNew();

            await Navigation.PopAsync();
        }

        private void ImgMenu_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new SideMenuPopup());
        }

        private async void Sale_confirm_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NoticeSaleDetail(selected_notice_id, selected_other_id));
        }

        private async void removeNew()
        {
            Notifications notification = this.BindingContext as Notifications;
            if (notification.IsVisible)
            {
                int badgeNum = Preferences.Get("badgeNum", 0);
                badgeNum--;
                Preferences.Set("badgeNum", badgeNum);

                MessagingCenter.Send<App>((App)Application.Current, "BadgeCountRefresh");

                notification.IsVisible = false;
                await App.Notice_data.SaveNotiAsync(notification);
            }
        }

        private void Btn_cancel_confirm_Clicked(object sender, EventArgs e)
        {
            confirm_zero_info("5");
        }

        private async void lbl_confirm_image_tap(object sender, EventArgs e)
        {
            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("zero_id", selected_other_id)
                });

                try
                {
                    var request = await cl.PostAsync(Constants.SERVER_BASE_URL + "get_agreement", formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        var photoBrowser = new PhotoBrowser();
                        var photoList = new List<Photo>();
                        
                        if (App.owner_license != null)
                        {
                            var licence_photo = new Photo();
                            licence_photo.URL = Constants.LICENCE_IMAGE_URL_PREFIX + App.owner_license;
                            photoList.Add(licence_photo);
                        }

                        int image_num = resultMsg.agreement_data.Length;
                        if (image_num > 0)
                        {
                            for (int i = 0; i < image_num; i++)
                            {
                                var photo = new Photo();
                                photo.URL = Constants.AGREEMENT_IMAGE_URL_PREFIX + resultMsg.agreement_data[i].agreement_image;
                                photoList.Add(photo);
                            }

                            photoBrowser.Photos = photoList;
                            photoBrowser.Show();
                        }
                        
                    }
                    else
                    {
                        await DisplayAlert("", resultMsg.resp, "はい");
                    }
                }
                catch
                {
                    await DisplayAlert("", Constants.NETWORK_ERROR, "はい");
                }

            }
        }

        private void lbl_wrong_cost_tap(object sender, EventArgs e)
        {
            confirm_zero_info("6");
        }

        private async void confirm_zero_info(string zero_status)
        {
            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("zero_id", selected_other_id),
                    new KeyValuePair<string, string>("zero_status", "5")
                });

                try
                {
                    var request = await cl.PostAsync(Constants.SERVER_BASE_URL + "confirm_zero_info", formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        if (zero_status == "5")
                        {
                            await Navigation.PushAsync(new ZeroCompletionPage());
                        }
                        else if(zero_status == "6")
                        {
                            await Navigation.PushAsync(new ZeroWrongPage());
                        }
                    }
                    else
                    {
                        await DisplayAlert("", resultMsg.resp, "はい");
                    }
                }
                catch
                {
                    await DisplayAlert("", Constants.NETWORK_ERROR, "はい");
                }

            }
        }
    }
}