using Newtonsoft.Json;
using owner.WebService;
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
	public partial class QuizPage : ContentPage
	{
		public QuizPage ()
		{
			InitializeComponent ();

            ent_name.Text = App.owner_name;
            ent_email.Text = App.owner_email;

		}

        private async void Btn_send_Clicked(System.Object sender, System.EventArgs e)
        {
            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("user_name", ent_name.Text),
                        new KeyValuePair<string, string>("user_email", ent_email.Text),
                        new KeyValuePair<string, string>("question_contents", ent_question.Text)
                });

                try
                {
                    var request = await cl.PostAsync(Constants.SERVER_Quiz_URL, formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        await Navigation.PushAsync(new QuizConfPage());
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

        private async void Back_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}