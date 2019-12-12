using Newtonsoft.Json;
using owner.WebService;
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
	public partial class MyInfoUpdateCompletePage : ContentPage
	{
		public MyInfoUpdateCompletePage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);

            payment_result();

            //Preferences.Set(Constants.PAID_MEMBER, true);
        }

        private async void Home_btn_Clicked(object sender, EventArgs e)
        {

            //Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 5]);
            //Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 4]);
            //Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 3]);
            //Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 2]);
            //Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 1]);

            //var masterPage = this.Parent.Parent as TabbedPage;
            //if (masterPage != null)
            //{
            //    masterPage.CurrentPage = masterPage.Children[0];
            //}

            await Navigation.PushAsync(new HomePage());
            
        }

        private async void payment_result()
        {
            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>(Constants.OWNER_ID, App.owner_ID)
                });

                try
                {
                    var request = await cl.PostAsync(Constants.SERVER_PAY_CHECK_URL, formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        ResponseData resultData = JsonConvert.DeserializeObject<ResponseData>(response);
                        App.owner_type = "1";
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
}