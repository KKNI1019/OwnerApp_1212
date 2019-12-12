using owner.Model;
using owner.WebService;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace owner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewsFavoritePage : ContentPage
	{
        public ObservableCollection<FavoriteNews> newsInfo { get; set; }

        public NewsFavoritePage ()
		{
			InitializeComponent ();

            newsInfo = new ObservableCollection<FavoriteNews>();

            show_favorite_news();

            listview.ItemSelected += DeselectItem;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public void DeselectItem(object sender, EventArgs e)
        {
            ((Xamarin.Forms.ListView)sender).SelectedItem = null;
        }

        private async void show_favorite_news()
        {
            
                bool first_favorite = true;
                var favorite_news = await App.Favorite_News_Data.GetNewsAsync();
                

                for (int i = 0; i < favorite_news.Count; i++)
                {
                    FavoriteNews tempNews = new FavoriteNews();
                    tempNews.news_id = favorite_news[i].news_id;

                    if (first_favorite)
                    {
                        tempNews.news_image_url_header = favorite_news[i].news_image_url;
                        tempNews.img_header_height = 200;
                        first_favorite = false;
                    }
                    else
                    {
                        tempNews.news_image_url_contents = favorite_news[i].news_image_url;
                    }

                    tempNews.news_category = favorite_news[i].news_category;
                    tempNews.news_title = favorite_news[i].news_title;
                    tempNews.news_date = favorite_news[i].news_date;
                    tempNews.news_source = favorite_news[i].news_source;
                    tempNews.writer_name = favorite_news[i].writer_name;
                    tempNews.writer_profile = favorite_news[i].writer_profile;
                    tempNews.news_url = favorite_news[i].news_url;
                    tempNews.news_contents = favorite_news[i].news_contents;

                    tempNews.comment_count = favorite_news[i].comment_count;
                    tempNews.brief_comment = favorite_news[i].brief_comment;
                    tempNews.first_commenter_image = favorite_news[i].first_commenter_image;
                    tempNews.first_commenter_name = favorite_news[i].first_commenter_name;
                    tempNews.second_commenter_image = favorite_news[i].second_commenter_image;
                    tempNews.second_commenter_name = favorite_news[i].second_commenter_name;
                    tempNews.third_commenter_image = favorite_news[i].third_commenter_image;
                    tempNews.third_commenter_name = favorite_news[i].third_commenter_name;

                    newsInfo.Add(tempNews);
                }

                listview.ItemsSource = newsInfo;

        }

        private async void Imgback_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send<App>((App)Application.Current, "NewsPageRefresh");

            await Navigation.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            MessagingCenter.Send<App>((App)Application.Current, "NewsPageRefresh");

            return false;
        }

        private async void favorite_tap(object sender, EventArgs e)
        {
            StackLayout stk = sender as StackLayout;
            var item = (TapGestureRecognizer)stk.GestureRecognizers[0];
            string selected_news_id = item.CommandParameter.ToString();

            var favorite_tapped_news = await App.Favorite_News_Data.GetSelectedNewsAsync(selected_news_id);
            await App.Favorite_News_Data.DeleteNewsAsync(favorite_tapped_news);

            for (int i = 0; i < newsInfo.Count; i++)
            {
                if (selected_news_id == newsInfo[i].news_id)
                {
                    newsInfo.RemoveAt(i);
                    listview.ItemsSource = newsInfo;

                    break;
                }
            }
        }

        private void comment_tap(object sender, EventArgs e)
        {
            StackLayout stk = sender as StackLayout;
            var item = (TapGestureRecognizer)stk.GestureRecognizers[0];
            string selected_news_id = item.CommandParameter.ToString();

            PopupNavigation.Instance.PushAsync(new NewsCommentPage(selected_news_id));
        }

        private async void news_title_tap(object sender, EventArgs e)
        {
            Label lbl_news_title = (Label)sender;
            var item = (TapGestureRecognizer)lbl_news_title.GestureRecognizers[0];
            string selected_news_id = item.CommandParameter.ToString();

            await Navigation.PushAsync(new NewsDetailPage(selected_news_id, "お気に入り", null));
        }
    }
}