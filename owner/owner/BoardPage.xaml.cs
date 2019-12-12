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
	public partial class BoardPage : ContentPage
	{
        public IList<Threads> Threads { get; set; }

        public BoardPage ()
		{
			InitializeComponent ();

            get_thread_list(App.owner_ID, Preferences.Get("last_thread_id", ""));

            MessagingCenter.Subscribe<App>((App)Application.Current, "OnBoardPageRefresh", (sender) => {
                RefreshPage();
            });
            
        }

        private async void RefreshPage()
        {
            listview.ItemsSource = await App.Thread_data.GetThreadAsync();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            listview.ItemsSource = await App.Thread_data.GetThreadAsync();
        }

        private void imgAdd_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new BoardPopup());
        }

        private async void get_thread_list(string tenantId, string threadId)
        {
            loadingbar.IsRunning = true;

            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>(Constants.OWNER_ID, tenantId),
                    new KeyValuePair<string, string>(Constants.LAST_THREAD_ID, threadId)
                });

                try
                {
                    var request = await cl.PostAsync(Constants.SERVER_GET_THREAD_LIST_URL, formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        loadingbar.IsRunning = false;

                        ResponseData resultData = JsonConvert.DeserializeObject<ResponseData>(response);

                        var thread_num = resultData.thread_list_data.Length;
                        if (thread_num != 0)
                        {
                            for (int i = 0; i < thread_num; i++)
                            {
                                await App.Thread_data.SaveThreadAsync(new Threads
                                {
                                    img_url = "img_building.png",
                                    Th_id = resultData.thread_list_data[i].thread_id,
                                    Th_category = resultData.thread_list_data[i].thread_category,
                                    Th_note = resultData.thread_list_data[i].thread_note,
                                    Date = resultData.thread_list_data[i].u_date
                                });
                            }

                            listview.ItemsSource = await App.Thread_data.GetThreadAsync();

                            Preferences.Set(Constants.LAST_THREAD_ID, resultData.thread_list_data[thread_num - 1].thread_id);
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

        private async void imgBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void Listview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var obj = (Threads)e.Item;
            var _id = Convert.ToString(obj.Th_id);
            var seleted_thread_title = Convert.ToString(obj.Th_category);
            string selected_thread_id = Convert.ToString(_id);

            //await Navigation.PushAsync(new ThreadPage(selected_thread_id, seleted_thread_title));

            var masterPage = this.Parent.Parent as TabbedPage;
            if (masterPage != null)
            {
                masterPage.CurrentPage = masterPage.Children[1];
            }

            ((ListView)sender).SelectedItem = null;
        }

        private void btnMore_Clicked(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            string del_thread_id = button.CommandParameter.ToString();

            PopupNavigation.Instance.PushAsync(new BoardDelPopup(del_thread_id, "del_thread"));
        }
    }
}