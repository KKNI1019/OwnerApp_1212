using owner.Model;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Net.Http;
using owner.WebService;
using Newtonsoft.Json;

namespace owner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ColumnPage : ContentPage
	{
        public IList<Columns> Columns { get; set; }
        int itemIndex = -1;
        public string profile_url;

        public ColumnPage()
        {
            InitializeComponent();
            get_column_list(App.owner_ID, Preferences.Get(Constants.LAST_COMMENT_ID, ""));

            sflist.ItemTapped += ListView_ItemTapped;

            MessagingCenter.Subscribe<App>((App)Application.Current, "OnCategoryCreated", (sender) => {
                GetMerchantCategory();
            });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            sflist.ItemsSource = await App.Column_data.GetColumnAsync();
        }

        private async void GetMerchantCategory()
        {
            sflist.ItemsSource = await App.Column_data.GetColumnAsync();
        }

        private async void get_column_list(string ownerId, string columnId)
        {
            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>(Constants.OWNER_ID, ownerId),
                    new KeyValuePair<string, string>(Constants.LAST_COMMENT_ID, columnId)
                });

                try
                {
                    var request = await cl.PostAsync(Constants.SERVER_GET_COMMENT_LIST_URL, formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        ResponseData resultData = JsonConvert.DeserializeObject<ResponseData>(response);

                        var column_num = resultData.comment_list_data.Length;
                        if (column_num != 0)
                        {

                            for (int i = 0; i < column_num; i++)
                            {
                                if (resultData.comment_list_data[i].user_profile != null)
                                {
                                    if (resultData.comment_list_data[i].user_profile.Substring(0, 1) == "o")
                                    {
                                        profile_url = Constants.IMAGE_UPLOAD_URL_PREFIX + resultData.comment_list_data[i].user_profile;
                                    }
                                    else if (resultData.comment_list_data[i].user_profile.Substring(0, 1) == "t")
                                    {
                                        profile_url = Constants.IMAGE_UPLOAD_URL_PREFIX_TENANT + resultData.comment_list_data[i].user_profile;
                                    }
                                }
                                else
                                {
                                    profile_url = "imgUser.png";
                                }
                                await App.Column_data.SaveColumnAsync(new Columns
                                {
                                    img_url = profile_url,
                                    column_id = resultData.comment_list_data[i].comment_id,
                                    col_title = resultData.comment_list_data[i].comment_title,
                                    col_content = resultData.comment_list_data[i].comment_contents,
                                    user_name = resultData.comment_list_data[i].comment_writer_user_name,
                                    date = resultData.comment_list_data[i].u_date,
                                    IsVisible = true,
                                });
                            }

                            sflist.ItemsSource = await App.Column_data.GetColumnAsync();

                            Preferences.Set(Constants.LAST_COMMENT_ID, resultData.comment_list_data[column_num - 1].comment_id);
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

        private async void ListView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new ColumnDetailPage
            {
                BindingContext = e.ItemData as Columns
            });

            ((SfListView)sender).SelectedItem = null;
        }

        private async void imgBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void imgMenu_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new SideMenuPopup());
        }

        private void ListView_SwipeStarted(object sender, SwipeStartedEventArgs e)
        {
            itemIndex = -1;
        }

        private void ListView_Swiping(object sender, SwipingEventArgs e)
        {
            if (e.ItemIndex == 1 && e.SwipeOffSet > 70)
                e.Handled = true;
        }

        private void ListView_SwipeEnded(object sender, SwipeEndedEventArgs e)
        {
            itemIndex = e.ItemIndex;
        }

        private async void BtnDel_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayAlert("", "コラムを削除します。", "削除する", "キャンセル");
            if (action)
            {
                Button button = (Button)sender;
                string del_column_id = button.CommandParameter.ToString();

                var column = await App.Column_data.GetDelColumnAsync(del_column_id);
                await App.Column_data.DeletecolumnAsync(column);

                MessagingCenter.Send<App>((App)Application.Current, "OnCategoryCreated");
            }
        }
    }
}