using Newtonsoft.Json;
using owner.Model;
using owner.WebService;
using Rg.Plugins.Popup.Services;
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
	public partial class NotificationPage : ContentPage
	{
        public IList<Notifications> Notifications { get; set; }

        public NotificationPage ()
		{
			InitializeComponent ();

            NavigationPage.SetHasNavigationBar(this, false);

            Notifications = new List<Notifications>();
            get_noti_list(App.owner_ID, Preferences.Get(Constants.LAST_NOTICE_ID, ""));

            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            listview.ItemsSource = await App.Notice_data.GetNotiAsync();
        }

        private async void get_noti_list(string tenantId, string noticeId)
        {
            loadingbar.IsRunning = true;

            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>(Constants.OWNER_ID, tenantId),
                    new KeyValuePair<string, string>(Constants.LAST_NOTICE_ID, noticeId)
                });

                try
                {
                    var request = await cl.PostAsync(Constants.SERVER_GET_NOTICE_URL, formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        loadingbar.IsRunning = false;

                        ResponseData resultData = JsonConvert.DeserializeObject<ResponseData>(response);

                        var notice_num = resultData.notice_list_data.Length;
                        if (notice_num != 0)
                        {
                            int badgeNum = Preferences.Get("badgeNum", 0);

                            for (int i = 0; i < notice_num; i++)
                            {
                                badgeNum++;

                                Notifications noti_temp = new Notifications();
                                noti_temp.noti_id = resultData.notice_list_data[i].notice_id;
                                noti_temp.noti_title = resultData.notice_list_data[i].notice_title;
                                noti_temp.noti_content = resultData.notice_list_data[i].notice_contents;
                                noti_temp.date = resultData.notice_list_data[i].u_date;
                                noti_temp.noti_kind = resultData.notice_list_data[i].notice_kind;
                                noti_temp.noti_destination = resultData.notice_list_data[i].notice_destination;
                                noti_temp.IsVisible = true;
                                noti_temp.other_id = resultData.notice_list_data[i].other_id;

                                if (resultData.notice_list_data[i].notice_kind == "0" || resultData.notice_list_data[i].notice_kind == "3")
                                {
                                    noti_temp.img_source = "img_new.png";
                                }
                                else
                                {
                                    noti_temp.img_source = "img_sale_notice.png";
                                }

                                await App.Notice_data.SaveNotiAsync(noti_temp);
                            }

                            Preferences.Set("badgeNum", badgeNum);
                            MessagingCenter.Send<App>((App)Application.Current, "BadgeCountRefresh");

                            listview.ItemsSource = await App.Notice_data.GetNotiAsync();
                          
                            Preferences.Set(Constants.LAST_NOTICE_ID, resultData.notice_list_data[notice_num - 1].notice_id);
                        }
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

        private void ImgMenu_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new SideMenuPopup());
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {

                await Navigation.PushAsync(new NotificationDetailPage
                {
                    BindingContext = e.SelectedItem as Notifications
                });

                ((ListView)sender).SelectedItem = null;
            }

        }
        
    }
}