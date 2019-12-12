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
	public partial class NewsDetailPage : ContentPage
	{
        private string news_url;
        private int index;
        public IList<News_Comments> comments { get; set; }
        private string news_id;
        private int tapped_index;

        public NewsDetailPage (string selected_news_id, string title, string favorite_image_source)
		{
			InitializeComponent ();
            label_title.Text = title;
            img_favorite.Source = favorite_image_source;
            news_id = selected_news_id;
            
            comments = new List<News_Comments>();

            int news_count = Global.news.Count;
            if (news_count != 0)
            {
                for (int i = 0; i < news_count; i++)
                {
                    if (string.Equals(selected_news_id, Global.news[i].news_id))
                    {
                        index = i;
                        break;
                    }
                }

                news_url = Global.news[index].news_url;
                img_thumbnail.Source = Global.news[index].news_image_url;
                lbl_news_title.Text = Global.news[index].news_title;
                lbl_news_time.Text = Global.news[index].news_date.ToString();
                lbl_news_source.Text = Global.news[index].news_source;
                lbl_news_category.Text = Global.news[index].news_category;
                lbl_news_contents.Text = Global.news[index].news_contents;

                int comments_count = Global.news[index].news_comment_data.Count;
                if (comments_count != 0)
                {
                    for (int i = 0; i < comments_count; i++)
                    {
                        News_Comments tempComments = new News_Comments();

                        tempComments.news_comment_id = Global.news[index].news_comment_data[i].news_comment_id;
                        tempComments.news_comment_writer_name = Global.news[index].news_comment_data[i].news_comment_writer_name;
                        tempComments.news_comment_writer_image = Global.news[index].news_comment_data[i].news_comment_writer_image;
                        tempComments.news_comment_writer_profile = Global.news[index].news_comment_data[i].news_comment_writer_profile;
                        tempComments.news_comment_contents = Global.news[index].news_comment_data[i].news_comment_contents;
                        tempComments.news_comment_likes = Global.news[index].news_comment_data[i].news_comment_likes;
                        tempComments.c_date = Global.news[index].news_comment_data[i].c_date;

                        if (string.IsNullOrEmpty(tempComments.news_comment_writer_image))
                        {
                            tempComments.news_comment_writer_image = "imgUser.png";
                        }
                        else
                        {
                            if (tempComments.news_comment_writer_image.Substring(0, 1) == "o")
                            {
                                tempComments.news_comment_writer_image = Constants.IMAGE_UPLOAD_URL_PREFIX + tempComments.news_comment_writer_image;
                            }
                            else if (tempComments.news_comment_writer_image.Substring(0, 1) == "t")
                            {
                                tempComments.news_comment_writer_image = Constants.IMAGE_UPLOAD_URL_PREFIX_TENANT + tempComments.news_comment_writer_image;
                            }
                        }

                        comments.Add(tempComments);
                    }

                    listview.ItemsSource = comments;
                }
            }

            listview.ItemSelected += DeselectItem;
        }

        protected async override void OnAppearing()
        {
            var selected_news = await App.Favorite_News_Data.GetSelectedNewsAsync(news_id);
            if (selected_news != null)
            {
                imgFavorite.Source = "favorite.png";
            }
            else
            {
                imgFavorite.Source = "infavorite.png";
            }

            base.OnAppearing();
        }

        public void DeselectItem(object sender, EventArgs e)
        {
            ((Xamarin.Forms.ListView)sender).SelectedItem = null;
        }

        private async void Imgback_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private async void ImgFavorite_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewsFavoritePage());
        }


        private async void ImgBtn_news_detail_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewsWebPage(news_url)); 
        }

        private void likes_tap(object sender, EventArgs e)
        {
            StackLayout stk = sender as StackLayout;
            var item = (TapGestureRecognizer)stk.GestureRecognizers[0];
            string selected_comment_id = item.CommandParameter.ToString();

            Image img_likes = stk.Children[0] as Image;
            string source = img_likes.Source as FileImageSource;
            if (String.Equals(source, "thumbs_up_yellow.png"))
            {
                img_likes.Source = "thumbs_up_black.png";
                get_likes_count(Constants.SERVER_DISLIKE_COMMENT_URL, selected_comment_id);
            }
            else
            {
                img_likes.Source = "thumbs_up_yellow.png";
                get_likes_count(Constants.SERVER_LIKE_COMMENT_URL, selected_comment_id);
            }
            
        }

        private async void get_likes_count(string url, string comment_id)
        {
            loadingbar.IsRunning = true;

            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("news_comment_id", comment_id)
                });
                try
                {
                    var request = await cl.PostAsync(url, formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        loadingbar.IsRunning = false;

                        int likes = resultMsg.comment_likes;
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

        private void comment_tap(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new NewsCommentPage(news_id));
        }

        private void favorite_tap(object sender, EventArgs e)
        {
            favorite_result(news_id);
        }

        private async void favorite_result(string news_id)
        {
            for (int i = 0; i < Global.news.Count; i++)
            {
                if (news_id == Global.news[i].news_id)
                {
                    tapped_index = i; break;
                }
            }

            var favorite_tapped_news = await App.Favorite_News_Data.GetSelectedNewsAsync(news_id);
            if (favorite_tapped_news == null)
            {
                FavoriteNews favorite_news = new FavoriteNews();

                favorite_news.news_id = news_id;
                favorite_news.news_image_url = Global.news[tapped_index].news_image_url;
                favorite_news.news_category = Global.news[tapped_index].news_category;
                favorite_news.news_title = Global.news[tapped_index].news_title;
                favorite_news.news_date = Global.news[tapped_index].news_date;
                favorite_news.news_source = Global.news[tapped_index].news_source;
                favorite_news.writer_name = Global.news[tapped_index].writer_name;
                favorite_news.writer_image = Global.news[tapped_index].writer_image;
                favorite_news.writer_profile = Global.news[tapped_index].writer_profile;
                favorite_news.news_url = Global.news[tapped_index].news_url;
                favorite_news.news_contents = Global.news[tapped_index].news_contents;

                favorite_news.comment_count = Global.news[tapped_index].comment_count;
                favorite_news.brief_comment = Global.news[tapped_index].brief_comment;
                favorite_news.first_commenter_image = Global.news[tapped_index].first_commenter_image;
                favorite_news.first_commenter_name = Global.news[tapped_index].first_commenter_name;
                favorite_news.second_commenter_image = Global.news[tapped_index].second_commenter_image;
                favorite_news.second_commenter_name = Global.news[tapped_index].second_commenter_name;
                favorite_news.third_commenter_image = Global.news[tapped_index].third_commenter_image;
                favorite_news.third_commenter_name = Global.news[tapped_index].third_commenter_name;

                await App.Favorite_News_Data.SaveNewsAsync(favorite_news);

                imgFavorite.Source = "favorite.png";
            }
            else
            {
                await App.Favorite_News_Data.DeleteNewsAsync(favorite_tapped_news);

                imgFavorite.Source = "infavorite.png";
            }
        }
    }
}