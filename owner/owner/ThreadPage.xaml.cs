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

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace owner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ThreadPage : ContentPage
	{
        public IList<Threads> Threads { get; set; }
        private string selected_thread_id;
        private string profile_url;

        public ThreadPage(string selected_th_id, string selected_th_title)
        {
            InitializeComponent();
            lbl_thread_title.Text = selected_th_title;
            selected_thread_id = selected_th_id;

            listview.ItemSelected += DeselectItem;
        }

        public void DeselectItem(object sender, EventArgs e)
        {
            ((Xamarin.Forms.ListView)sender).SelectedItem = null;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            listview.ItemsSource = await App.Thread_Comment_data.GetTh_commentAsync(selected_thread_id);

            var thread = await App.Thread_data.GetSelectedThreadAsync(selected_thread_id);
            get_th_comment_list(selected_thread_id, thread.Last_comment_Id, App.owner_ID);

            MessagingCenter.Subscribe<App>((App)Application.Current, "OnThreadPageRefresh", (sender) => {
                RefreshPage();
            });
        }

        private async void RefreshPage()
        {
            listview.ItemsSource = await App.Thread_Comment_data.GetTh_commentAsync(selected_thread_id);
        }

        private async void get_th_comment_list(string threadId, string th_comment_id, string owner_Id)
        {
            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>(Constants.THREAD_ID, threadId),
                    new KeyValuePair<string, string>(Constants.LAST_THREAD_COMMENT_ID,th_comment_id),
                    new KeyValuePair<string, string>(Constants.OWNER_ID, owner_Id)
                });
                try
                {
                    var request = await cl.PostAsync(Constants.SERVER_GET_THREAD_DETAIL_URL, formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        ResponseData resultData = JsonConvert.DeserializeObject<ResponseData>(response);

                        var th_comment_num = resultData.thread_comment_data.Length;
                        if (th_comment_num != 0)
                        {

                            for (int i = 0; i < th_comment_num; i++)
                            {
                                if (resultData.thread_comment_data[i].user_profile != null)
                                {
                                    if (resultData.thread_comment_data[i].user_profile.Substring(0,1) == "o")
                                    {
                                        profile_url = Constants.IMAGE_UPLOAD_URL_PREFIX + resultData.thread_comment_data[i].user_profile;
                                    }
                                    else if (resultData.thread_comment_data[i].user_profile.Substring(0, 1) == "t")
                                    {
                                        profile_url = Constants.IMAGE_UPLOAD_URL_PREFIX_TENANT + resultData.thread_comment_data[i].user_profile;
                                    }
                                }
                                else
                                {
                                    profile_url = "imgUser.png";
                                }
                                await App.Thread_Comment_data.SaveTh_CommentAsync(new Thread_Comments
                                {
                                    img_url = profile_url,
                                    Th_comment_id = resultData.thread_comment_data[i].thread_comment_id,
                                    Th_id = resultData.thread_comment_data[i].thread_comment_category,
                                    Th_comment_content = System.Net.WebUtility.UrlDecode(resultData.thread_comment_data[i].thread_comment_contents),
                                    c_date = resultData.thread_comment_data[i].c_date,
                                    Th_comment_writer_nickname = resultData.thread_comment_data[i].thread_comment_writer_nickname
                                });
                            }

                            var source = await App.Thread_Comment_data.GetTh_commentAsync(selected_thread_id);
                            listview.ItemsSource = source;

                            var thread = await App.Thread_data.GetSelectedThreadAsync(resultData.thread_comment_data[th_comment_num - 1].thread_comment_category);
                            thread.Last_comment_Id = resultData.thread_comment_data[th_comment_num - 1].thread_comment_id;
                            await App.Thread_data.SaveThreadAsync(thread);
                            //Preferences.Set("last_thread_id", resultData.thread_list_data[th_comment_num - 1].thread_id);
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

        private async void Btn_post_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ent_myComment.Text))
            {
                using (var cl = new HttpClient())
                {
                    var formcontent = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>(Constants.THREAD_COMMENT_CONTENTS, System.Net.WebUtility.UrlEncode(ent_myComment.Text)),
                    new KeyValuePair<string, string>(Constants.THREAD_COMMENT_WRITER_ID, "o_"+App.owner_ID),
                    new KeyValuePair<string, string>(Constants.THREAD_COMMENT_WRITER_NICKNAME, App.owner_nickname),
                    new KeyValuePair<string, string>(Constants.THREAD_COMMENT_CATEGORY, selected_thread_id)
                });
                    try
                    {
                        var request = await cl.PostAsync(Constants.SERVER_SEND_THREAD_COMMENT_URL, formcontent);
                        request.EnsureSuccessStatusCode();
                        var response = await request.Content.ReadAsStringAsync();
                        ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                        if (resultMsg.resp.Equals("success"))
                        {
                            ent_myComment.Text = string.Empty;

                            var thread = await App.Thread_data.GetSelectedThreadAsync(selected_thread_id);
                            get_th_comment_list(selected_thread_id, thread.Last_comment_Id, App.owner_ID);
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

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private async void ImgBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void btnMore_Clicked(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            string del_comment_id = button.CommandParameter.ToString();

            PopupNavigation.Instance.PushAsync(new BoardDelPopup(del_comment_id, "del_comment"));
        }
    }
}