using owner.WebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace owner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ZeroVideoPage : ContentPage
	{
        
        public ZeroVideoPage()
        {
            InitializeComponent();

            imgBtn_play.IsVisible = false;
            imgBtn_pause.IsVisible = true;

            videoplay();
        }

        private async void videoplay()
        {
            loadingbar.IsRunning = true;

            var videoInfoUrl = "https://www.youtube.com/get_video_info?video_id=" + Preferences.Get("owner_zero_video","");
            using (var client = new HttpClient())
            {
                try
                {
                    var videoPageContent = client.GetStringAsync(videoInfoUrl).Result;
                    var videoParameters = ParseQueryString(videoPageContent);
                    var encodedStreamsDelimited = WebUtility.HtmlDecode(videoParameters["url_encoded_fmt_stream_map"]);
                    var encodedStreams = encodedStreamsDelimited.Split(',');
                    var streams = encodedStreams.Select(ParseQueryString);

                    var stream = streams
                        .OrderBy(s =>
                        {
                            var type = s["type"];
                            if (type.Contains("video/mp4")) return 10;
                            if (type.Contains("video/3gpp")) return 20;
                            if (type.Contains("video/x-flv")) return 30;
                            if (type.Contains("video/webm")) return 40;
                            return int.MaxValue;
                        })
                        .ThenBy(s =>
                        {
                            var quality = s["quality"];

                            switch (Device.Idiom)
                            {
                                case TargetIdiom.Phone:
                                    return Array.IndexOf(new[] { "medium", "high", "small" }, quality);
                                default:
                                    return Array.IndexOf(new[] { "high", "medium", "small" }, quality);
                            }
                        })
                        .FirstOrDefault();

                    loadingbar.IsRunning = false;
                    myVideoView.Source = stream["url"];
                }
                catch
                {
                    await DisplayAlert("", "動画を再生することができません。", "はい");

                    myVideoView.IsVisible = false;
                    myVideoView.Opacity = 0;
                }

            }
        }

        private Dictionary<string, string> ParseQueryString(string query)
        {
            return ParseQueryString(query, Encoding.UTF8);
        }

        private Dictionary<string, string> ParseQueryString(string query, Encoding encoding)
        {
            if (query == null)
                throw new ArgumentNullException("query");
            if (encoding == null)
                throw new ArgumentNullException("encoding");
            if (query.Length == 0 || (query.Length == 1 && query[0] == '?'))
                return new Dictionary<string, string>();
            if (query[0] == '?')
                query = query.Substring(1);

            var result = new Dictionary<string, string>();
            ParseQueryString(query, encoding, result);
            return result;
        }

        private void ParseQueryString(string query, Encoding encoding, Dictionary<string, string> result)
        {
            if (query.Length == 0)
                return;

            string decoded = System.Net.WebUtility.HtmlDecode(query);
            int decodedLength = decoded.Length;
            int namePos = 0;
            bool first = true;
            while (namePos <= decodedLength)
            {
                int valuePos = -1, valueEnd = -1;
                for (int q = namePos; q < decodedLength; q++)
                {
                    if (valuePos == -1 && decoded[q] == '=')
                    {
                        valuePos = q + 1;
                    }
                    else if (decoded[q] == '&')
                    {
                        valueEnd = q;
                        break;
                    }
                }

                if (first)
                {
                    first = false;
                    if (decoded[namePos] == '?')
                        namePos++;
                }

                string name, value;
                if (valuePos == -1)
                {
                    name = null;
                    valuePos = namePos;
                }
                else
                {
                    name = System.Net.WebUtility.UrlDecode(decoded.Substring(namePos, valuePos - namePos - 1));
                }
                if (valueEnd < 0)
                {
                    namePos = -1;
                    valueEnd = decoded.Length;
                }
                else
                {
                    namePos = valueEnd + 1;
                }
                value = System.Net.WebUtility.UrlDecode(decoded.Substring(valuePos, valueEnd - valuePos));

                try
                {
                    result.Add(name, value);
                }
                catch { }

                if (namePos == -1)
                    break;
            }
        }

        private void ImgBtn_mute_Clicked(object sender, EventArgs e)
        {
            imgBtn_mute.IsVisible = false;
            imgBtn_muteOff.IsVisible = true;

            myVideoView.Volume = 0;
        }

        private void ImgBtn_muteOff_Clicked(object sender, EventArgs e)
        {
            imgBtn_mute.IsVisible = true;
            imgBtn_muteOff.IsVisible = false;

            myVideoView.Volume = 100;
        }

        private void ImgBtn_skip_Clicked(object sender, EventArgs e)
        {
            Goto_Admin_Register_page();

            myVideoView.IsVisible = false;
            myVideoView.Opacity = 0;
            myVideoView.Pause();
        }

        private void ImgBtn_play_Clicked(object sender, EventArgs e)
        {
            imgBtn_play.IsVisible = false;
            imgBtn_pause.IsVisible = true;

            myVideoView.Play();
        }

        private void ImgBtn_pause_Clicked(object sender, EventArgs e)
        {
            myVideoView.Pause();

            imgBtn_pause.IsVisible = false;
            imgBtn_play.IsVisible = true;
        }

        private void MyVideoView_Completed(object sender, Octane.Xamarin.Forms.VideoPlayer.Events.VideoPlayerEventArgs e)
        {
            Goto_Admin_Register_page();

            myVideoView.IsVisible = false;
            myVideoView.Opacity = 0;
            myVideoView.Pause();
        }

        private async void Goto_Admin_Register_page()
        {
           
            await Navigation.PushAsync(new ZeroRegisterAlert("Zero管理"));

            //Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 1]);
        }

        protected override void OnDisappearing()
        {
            Preferences.Set("zero_video_checked", true);
            base.OnDisappearing();
            Navigation.RemovePage(this);
        }
    }
}