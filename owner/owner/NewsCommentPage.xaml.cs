using Newtonsoft.Json;
using owner.Model;
using owner.WebService;
using Rg.Plugins.Popup.Pages;
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
	public partial class NewsCommentPage : PopupPage
    {
        private string comment_news_id;

        public NewsCommentPage (string news_id)
		{
			InitializeComponent ();

            comment_news_id = news_id;

            for (int i = 0; i < Global.news.Count; i++)
            {
                if (string.Equals(news_id, Global.news[i].news_id))
                {
                    lbl_writerName.Text = Global.news[i].writer_name;
                    lbl_writerProfile.Text = Global.news[i].writer_profile;
                    if (string.IsNullOrEmpty(Global.news[i].writer_image))
                    {
                        imgWriter.Source = "imgUser.png";
                    }
                    else
                    {
                        imgWriter.Source = Global.news[i].writer_image;
                    }
                    
                    break;
                }
            }
		}

        private void lbl_writeComment_tap(object sender, EventArgs e)
        {
            frame_wirteComment.IsVisible = true;
        }

        private async void Btn_send_comment_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(editor_comment.Text))
            {
                using (var cl = new HttpClient())
                {
                    loadingbar.IsRunning = true;

                    var formcontent = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("comment_contents", System.Net.WebUtility.UrlEncode(editor_comment.Text)),
                    new KeyValuePair<string, string>("comment_writer_id", "o_"+App.owner_ID),
                    new KeyValuePair<string, string>("comment_news_id", comment_news_id)
                    });
                    try
                    {
                        var request = await cl.PostAsync(Constants.SERVER_REGIST_COMMENT_URL, formcontent);
                        request.EnsureSuccessStatusCode();
                        var response = await request.Content.ReadAsStringAsync();
                        ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                        if (resultMsg.resp.Equals("success"))
                        {
                            loadingbar.IsRunning = false;

                            editor_comment.Text = string.Empty;

                            await PopupNavigation.Instance.PopAsync();
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
                        await PopupNavigation.Instance.PopAsync();
                    }

                }
            }
        }

    }
}