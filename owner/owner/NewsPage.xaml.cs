using Newtonsoft.Json;
using owner.Model;
using owner.WebService;
using Rg.Plugins.Popup.Services;
using Syncfusion.GridCommon.ScrollAxis;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace owner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewsPage : ContentPage
	{
        public ObservableCollection<News> newsInfo { get; set; }
        private bool isLoading;
        private int tapped_index;
        private string temp_news_id;

        public NewsPage ()
		{
			InitializeComponent ();

            newsInfo = new ObservableCollection<News>();
            Global.news = new List<News>();

            App.last_news_id = null;
            getNewsInfo(App.last_news_id);
            
            listview.ItemSelected += DeselectItem;
        }

        protected override void OnAppearing()
        {
            MessagingCenter.Subscribe<App>((App)Application.Current, "NewsPageRefresh", (sender) => {
                
            });

            base.OnAppearing();
        }

        public void DeselectItem(object sender, EventArgs e)
        {
            ((Xamarin.Forms.ListView)sender).SelectedItem = null;
        }

        private async Task getNewsInfo(string last_news_id)
        {
            isLoading = true;
            loadingbar.IsRunning = true;
            temp_news_id = last_news_id;

            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>(Constants.LAST_NEWS_ID, last_news_id)
                });

                try
                {
                    var request = await cl.PostAsync(Constants.SERVER_GET_NEWS_URL, formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        isLoading = false;
                        loadingbar.IsRunning = false;

                        ResponseData resultData = JsonConvert.DeserializeObject<ResponseData>(response);
                        var news_num = resultData.news_data.Length;

                        if (news_num != 0)
                        {
                            App.last_news_id = resultData.news_data[news_num - 1].news_id;

                            for (int i = 0; i < news_num; i++)
                            {
                                News tmpnews = new News();

                                string thumbnail_img_header = null;
                                string thumbnail_img = null;
                                string first_comment_contents = null;
                                string first_img = null;
                                string first_name = null;
                                string second_img = null;
                                string second_name = null;
                                string third_img = null;
                                string third_name = null;
                                int thumbnail_img_header_height = 0;
                                string main_imgNews_url = null;

                                string news_img = resultData.news_data[i].news_image_url;
                                if (news_img.Length >= 4 && (string.Equals(news_img.Substring(news_img.Length - 4), ".jpg") || string.Equals(news_img.Substring(news_img.Length - 4), ".png") || string.Equals(news_img.Substring(news_img.Length - 5), ".jpeg")))
                                {
                                    main_imgNews_url = resultData.news_data[i].news_image_url;

                                    if (i == 0 && string.IsNullOrEmpty(last_news_id))
                                    {
                                        thumbnail_img_header = resultData.news_data[i].news_image_url;
                                        thumbnail_img_header_height = 200;
                                    }
                                    else
                                    {
                                        thumbnail_img = resultData.news_data[i].news_image_url;
                                    }
                                }
                                else
                                {
                                    main_imgNews_url = "noimage.png";
                                    if (i == 0)
                                    {
                                        thumbnail_img_header = "noimage.png";
                                        thumbnail_img_header_height = 200;
                                    }
                                    else
                                    {
                                        thumbnail_img = "noimage.png";
                                    }
                                }
                                
                                if (resultData.news_data[i].news_comment_data.Length != 0)
                                {

                                    first_comment_contents = resultData.news_data[i].news_comment_data[0].news_comment_contents;

                                    if (resultData.news_data[i].news_comment_data.Length == 1)
                                    {
                                        first_img = resultData.news_data[i].news_comment_data[0].news_comment_writer_image;
                                        first_name = resultData.news_data[i].news_comment_data[0].news_comment_writer_name;
                                        
                                        if (string.IsNullOrEmpty(first_img))
                                        {
                                            first_img = "imgUser.png";
                                        }
                                        else
                                        {
                                            if (first_img.Substring(0, 1) == "o")
                                            {
                                                first_img = Constants.IMAGE_UPLOAD_URL_PREFIX + first_img;
                                            }
                                            else if (first_img.Substring(0, 1) == "t")
                                            {
                                                first_img = Constants.IMAGE_UPLOAD_URL_PREFIX_TENANT + first_img;
                                            }
                                        }
                                    }
                                    else if (resultData.news_data[i].news_comment_data.Length == 2)
                                    {
                                        first_img = resultData.news_data[i].news_comment_data[0].news_comment_writer_image;
                                        first_name = resultData.news_data[i].news_comment_data[0].news_comment_writer_name;
                                        second_img = resultData.news_data[i].news_comment_data[1].news_comment_writer_image;
                                        second_name = resultData.news_data[i].news_comment_data[1].news_comment_writer_name;
                                        if (string.IsNullOrEmpty(first_img))
                                        {
                                            first_img = "imgUser.png";
                                        }
                                        else
                                        {
                                            if (first_img.Substring(0, 1) == "o")
                                            {
                                                first_img = Constants.IMAGE_UPLOAD_URL_PREFIX + first_img;
                                            }
                                            else if (first_img.Substring(0, 1) == "t")
                                            {
                                                first_img = Constants.IMAGE_UPLOAD_URL_PREFIX_TENANT + first_img;
                                            }
                                        }
                                        if (string.IsNullOrEmpty(second_img))
                                        {
                                            second_img = "imgUser.png";
                                        }
                                        else
                                        {
                                            if (second_img.Substring(0, 1) == "o")
                                            {
                                                second_img = Constants.IMAGE_UPLOAD_URL_PREFIX + second_img;
                                            }
                                            else if (first_img.Substring(0, 1) == "t")
                                            {
                                                second_img = Constants.IMAGE_UPLOAD_URL_PREFIX_TENANT + second_img;
                                            }
                                        }
                                        
                                    }
                                    else if (resultData.news_data[i].news_comment_data.Length >= 3)
                                    {
                                        first_img = resultData.news_data[i].news_comment_data[0].news_comment_writer_image;
                                        first_name = resultData.news_data[i].news_comment_data[0].news_comment_writer_name;
                                        second_img = resultData.news_data[i].news_comment_data[1].news_comment_writer_image;
                                        second_name = resultData.news_data[i].news_comment_data[1].news_comment_writer_name;
                                        third_img = resultData.news_data[i].news_comment_data[2].news_comment_writer_image;
                                        third_name = resultData.news_data[i].news_comment_data[2].news_comment_writer_name;
                                        if (string.IsNullOrEmpty(first_img))
                                        {
                                            first_img = "imgUser.png";
                                        }
                                        else
                                        {
                                            if (first_img.Substring(0, 1) == "o")
                                            {
                                                first_img = Constants.IMAGE_UPLOAD_URL_PREFIX + first_img;
                                            }
                                            else if (first_img.Substring(0, 1) == "t")
                                            {
                                                first_img = Constants.IMAGE_UPLOAD_URL_PREFIX_TENANT + first_img;
                                            }
                                        }
                                        if (string.IsNullOrEmpty(second_img))
                                        {
                                            second_img = "imgUser.png";
                                        }
                                        else
                                        {
                                            if (second_img.Substring(0, 1) == "o")
                                            {
                                                second_img = Constants.IMAGE_UPLOAD_URL_PREFIX + second_img;
                                            }
                                            else if (first_img.Substring(0, 1) == "t")
                                            {
                                                second_img = Constants.IMAGE_UPLOAD_URL_PREFIX_TENANT + second_img;
                                            }
                                        }
                                        if (string.IsNullOrEmpty(third_img))
                                        {
                                            third_img = "imgUser.png";
                                        }
                                        else
                                        {
                                            if (third_img.Substring(0, 1) == "o")
                                            {
                                                third_img = Constants.IMAGE_UPLOAD_URL_PREFIX + third_img;
                                            }
                                            else if (first_img.Substring(0, 1) == "t")
                                            {
                                                third_img = Constants.IMAGE_UPLOAD_URL_PREFIX_TENANT + third_img;
                                            }
                                        }
                                    }
                                }

                                tmpnews.news_id = resultData.news_data[i].news_id;
                                tmpnews.news_image_url_header = thumbnail_img_header;
                                tmpnews.img_header_height = thumbnail_img_header_height;
                                tmpnews.news_image_url_contents = thumbnail_img;
                                tmpnews.news_image_url = main_imgNews_url;
                                tmpnews.news_category = resultData.news_data[i].news_category;
                                tmpnews.news_title = resultData.news_data[i].news_title;
                                tmpnews.news_date = resultData.news_data[i].news_date;
                                tmpnews.news_source = resultData.news_data[i].news_source;
                                tmpnews.writer_name = resultData.news_data[i].news_writer_name;
                                tmpnews.writer_profile = resultData.news_data[i].news_writer_profile;
                                tmpnews.news_url = resultData.news_data[i].news_url;
                                tmpnews.news_contents = resultData.news_data[i].news_contents;
                                
                                var favorite_tapped_news = await App.Favorite_News_Data.GetSelectedNewsAsync(resultData.news_data[i].news_id);
                                if (favorite_tapped_news != null)
                                {
                                    tmpnews.imgfavorite = "favorite.png";
                                }
                                else
                                {
                                    tmpnews.imgfavorite = "infavorite.png";
                                }
                                    
                                tmpnews.comment_count = resultData.news_data[i].news_comment_data.Count();
                                tmpnews.brief_comment = first_comment_contents;
                                tmpnews.first_commenter_image = first_img;
                                tmpnews.first_commenter_name = first_name;
                                tmpnews.second_commenter_image = second_img;
                                tmpnews.second_commenter_name = second_name;
                                tmpnews.third_commenter_image = third_img;
                                tmpnews.third_commenter_name = third_name;

                                tmpnews.news_comment_data = new List<News_Comments>();
                                if (tmpnews.comment_count != 0)
                                {
                                    for (int j = 0; j < tmpnews.comment_count; j++)
                                    {
                                        News_Comments tmp_news_comments = new News_Comments();

                                        tmp_news_comments.news_comment_id = resultData.news_data[i].news_comment_data[j].news_comment_id;
                                        tmp_news_comments.news_comment_writer_image = resultData.news_data[i].news_comment_data[j].news_comment_writer_image;
                                        tmp_news_comments.news_comment_writer_name = resultData.news_data[i].news_comment_data[j].news_comment_writer_name;
                                        tmp_news_comments.news_comment_writer_profile = resultData.news_data[i].news_comment_data[j].news_comment_writer_profile;
                                        tmp_news_comments.news_comment_contents = resultData.news_data[i].news_comment_data[j].news_comment_contents;
                                        tmp_news_comments.news_comment_likes = resultData.news_data[i].news_comment_data[j].news_comment_likes;
                                        tmp_news_comments.c_date = resultData.news_data[i].news_comment_data[j].c_date;
                                        
                                        try
                                        {
                                            tmpnews.news_comment_data.Add(tmp_news_comments);
                                        } catch(Exception e)
                                        {
                                            
                                        }
                                    }
                                }

                                Global.news.Add(tmpnews);
                                newsInfo.Add(tmpnews);

                            }

                            listview.ItemsSource = newsInfo;
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

        private async void Imgback_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private async void ImgFavorite_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewsFavoritePage());
        }

        private async void news_title_tap(object sender, EventArgs e)
        {
            Label lbl_news_title = (Label)sender;
            var item = (TapGestureRecognizer)lbl_news_title.GestureRecognizers[0];
            string selected_news_id = item.CommandParameter.ToString();
            
            await Navigation.PushAsync(new NewsDetailPage(selected_news_id, "ニュース", "favorite"));
        }

        private void favorite_tap(object sender, EventArgs e)
        {
            StackLayout stk = sender as StackLayout;
            var item = (TapGestureRecognizer)stk.GestureRecognizers[0];
            string selected_news_id = item.CommandParameter.ToString();
            
            Image image = stk.Children[0] as Image;
            string source = image.Source as FileImageSource; 
            if (String.Equals(source, "favorite.png"))
            {
                image.Source = "infavorite.png";
            }
            else
            {
                image.Source = "favorite.png";
            }

            favorite_result(selected_news_id);
        }

        private async void favorite_result(string news_id)
        {
            for (int i = 0; i < newsInfo.Count; i++)
            {
                if (news_id == newsInfo[i].news_id)
                {
                    tapped_index = i; break;
                }
            }

            var favorite_tapped_news = await App.Favorite_News_Data.GetSelectedNewsAsync(news_id);
            if (favorite_tapped_news == null)
            {
                FavoriteNews favorite_news = new FavoriteNews();

                favorite_news.news_id = news_id;
                favorite_news.news_image_url = newsInfo[tapped_index].news_image_url;
                favorite_news.news_category = newsInfo[tapped_index].news_category;
                favorite_news.news_title = newsInfo[tapped_index].news_title;
                favorite_news.news_date = newsInfo[tapped_index].news_date;
                favorite_news.news_source = newsInfo[tapped_index].news_source;
                favorite_news.writer_name = newsInfo[tapped_index].writer_name;
                favorite_news.writer_image = newsInfo[tapped_index].writer_image;
                favorite_news.writer_profile = newsInfo[tapped_index].writer_profile;
                favorite_news.news_url = newsInfo[tapped_index].news_url;
                favorite_news.news_contents = newsInfo[tapped_index].news_contents;

                favorite_news.comment_count = newsInfo[tapped_index].comment_count;
                favorite_news.brief_comment = newsInfo[tapped_index].brief_comment;
                favorite_news.first_commenter_image = newsInfo[tapped_index].first_commenter_image;
                favorite_news.first_commenter_name = newsInfo[tapped_index].first_commenter_name;
                favorite_news.second_commenter_image = newsInfo[tapped_index].second_commenter_image;
                favorite_news.second_commenter_name = newsInfo[tapped_index].second_commenter_name;
                favorite_news.third_commenter_image = newsInfo[tapped_index].third_commenter_image;
                favorite_news.third_commenter_name = newsInfo[tapped_index].third_commenter_name;

                await App.Favorite_News_Data.SaveNewsAsync(favorite_news);

            }
            else
            {
                await App.Favorite_News_Data.DeleteNewsAsync(favorite_tapped_news);
                
            }
        }
                
        private void comment_tap(object sender, EventArgs e)
        {
            StackLayout stk = sender as StackLayout;
            var item = (TapGestureRecognizer)stk.GestureRecognizers[0];
            string selected_news_id = item.CommandParameter.ToString();

            PopupNavigation.Instance.PushAsync(new NewsCommentPage(selected_news_id));
        }

        private async void Listview_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (isLoading || newsInfo.Count == 0) { return; }

            if (e.Item == newsInfo[newsInfo.Count - 1] && newsInfo.Count == 5)
            {
                await getNewsInfo(App.last_news_id); 
            }
            
        }
        
    }
}