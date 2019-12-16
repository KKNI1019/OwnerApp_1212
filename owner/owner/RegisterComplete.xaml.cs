using Newtonsoft.Json;
using owner.Social;
using owner.WebService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace owner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterComplete : ContentPage
	{
		public RegisterComplete ()
		{
			InitializeComponent ();

            NavigationPage.SetHasNavigationBar(this, false);
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.White;
            ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.Black;

            NavigationPage.SetTitleIcon(this, "logo_full_land.png");


            label_rule.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => OnLabelClicked()),
            });

        }

        private async void OnLabelClicked()
        {
            await Navigation.PushAsync(new RulePage { });
        }

        private async void ImgBtn_Login_Clicked(object sender, EventArgs e)
        {
            if (imgBtn_checked.IsVisible == true)
            {
                if (email.Text.Trim() == "")
                {
                    await DisplayAlert("", "メールアドレスを入力してください。", "はい");
                }
                else if(password.Text == "")
                {
                    await DisplayAlert("", "パスワードを入力してください。", "はい");
                }
                else if (password_confrim.Text == "")
                {
                    await DisplayAlert("", "パスワード再入力を入力してください。", "はい");
                }
                else if (password.Text != password_confrim.Text)
                {
                    await DisplayAlert("", "パスワードとパスワード再入力が一致しません。", "はい");
                }
                else
                {
                    RegistUserInfo();
                }
            }
            else
            {
                await DisplayAlert("", "利用規約を確認してください。", "はい");
            }
            
        }

        private void UncheckedBtn_Clicked(object sender, EventArgs e)
        {
            imgBtn_unchecked.IsVisible = false;
            imgBtn_checked.IsVisible = true;
        }

        private void CheckedBtn_Clicked(object sender, EventArgs e)
        {
            imgBtn_checked.IsVisible = false;
            imgBtn_unchecked.IsVisible = true;
        }

        private void ImgBtn_facebookLogin_Clicked(object sender, EventArgs e)
        {
            OAuth2Base oAuth2 = null;
            oAuth2 = FacebookOAuth2.Instance;

            var authenticator = new OAuth2Authenticator(
                    oAuth2.ClientId,
                    oAuth2.ClientSecret,
                    oAuth2.Scope,
                    oAuth2.AuthorizationUri,
                    oAuth2.RedirectUri,
                    oAuth2.RequestTokenUri,
                    null,
                    oAuth2.IsUsingNativeUI);

            authenticator.Completed += async (s, ee) =>
            {
                if (ee.IsAuthenticated)
                {
                    var user = await oAuth2.GetUserInfoAsync(ee.Account);
                    string name = user.Name;
                    string email = user.email;
                    string Id = user.Id;

                    RegisterResultCheck(name, email, "facebook_" + Id);

                    //await Navigation.PushAsync(new TabPage());

                    Debug.WriteLine("Authentication Success");
                }
            };
            authenticator.Error += (s, ee) =>
            {
                Debug.WriteLine("Authentication error: " + ee.Message);
            };

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
        }

        private void ImgBtn_twitterLogin_Clicked(object sender, EventArgs e)
        {
            var Twitterauth = new OAuth1Authenticator(
                           consumerKey: Constants.TW_CONSUMER_KEY,
                           consumerSecret: Constants.TW_CONSUMER_SECRET,
                           requestTokenUrl: new Uri(Constants.TW_REQUEST_TOKEN_URI),
                           authorizeUrl: new Uri(Constants.TW_AUTHORIZATION_URI),
                           accessTokenUrl: new Uri(Constants.TW_ACCESS_TOKEN_URI),
                           callbackUrl: new Uri(Constants.TW_CALLBACK_URI)
             );

            Twitterauth.Completed += TwitterAuth_Completed;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(Twitterauth);
        }

        private async void TwitterAuth_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("include_email", "true");
                var request = new OAuth1Request("GET",
                    new Uri(Constants.TW_OAUTH_URI),
                    param,
                    e.Account);
                try
                {
                    var response = await request.GetResponseAsync();
                    var json = response.GetResponseText();
                    var twitterUser = JsonConvert.DeserializeObject<TwitterUser>(json);

                    string name = twitterUser.name;
                    string email = twitterUser.email;
                    string id = twitterUser.Id;

                    RegisterResultCheck(name, email, "twitter_" + id);
                }
                catch { }
            }
        }

        private void ImgBtn_lineLogin_Clicked(object sender, EventArgs e)
        {
            OAuth2Base oAuth2 = null;
            oAuth2 = LineOAuth2.Instance;

            var authenticator = new OAuth2Authenticator(
                    oAuth2.ClientId,
                    oAuth2.ClientSecret,
                    oAuth2.Scope,
                    oAuth2.AuthorizationUri,
                    oAuth2.RedirectUri,
                    oAuth2.RequestTokenUri,
                    null,
                    oAuth2.IsUsingNativeUI);

            authenticator.Completed += async (s, ee) =>
            {
                if (ee.IsAuthenticated)
                {
                    //await Navigation.PushAsync(new TabPage());

                    var user = await oAuth2.GetUserInfoAsync(ee.Account);
                    string name = user.Name;
                    string email = user.email;
                    string Id = user.Id;

                    RegisterResultCheck(name, email, "line_" + Id);

                    Debug.WriteLine("Authentication Success");
                }
            };
            authenticator.Error += (s, ee) =>
            {
                Debug.WriteLine("Authentication error: " + ee.Message);
            };

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
        }

        private async void Back_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void RegisterResultCheck(string name, string email, string method)
        {
            loadingbar.IsRunning = true;

            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>(Constants.OWNER_EMAIL, email),
                        new KeyValuePair<string, string>(Constants.OWNER_NAME, name),
                        new KeyValuePair<string, string>(Constants.OWNER_REGIST_FROM, method),
                        new KeyValuePair<string, string>(Constants.DEVICE_TOKEN, Preferences.Get(Constants.DEVICE_TOKEN, ""))
                });

                try
                {
                    var request = await cl.PostAsync(Constants.SERVER_SOCIAL_LOGIN_URL, formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        loadingbar.IsRunning = false;
                        Preferences.Set(Constants.OWNER_EMAIL, email);
                        Preferences.Set(Constants.OWNER_NAME, name);
                        Preferences.Set(Constants.OWNER_REGIST_FROM, method);

                        ResponseData resultData = JsonConvert.DeserializeObject<ResponseData>(response);

                        App.owner_ID = resultMsg.owner_id;
                        App.owner_name = null;
                        App.owner_kana = null;
                        App.owner_nickname = null;
                        App.owner_phone1 = null;
                        App.owner_phone2 = null;
                        App.owner_email = email;
                        App.owner_password = null;
                        App.owner_address = null;
                        App.owner_postal_address = null;
                        App.owner_memo = null;
                        App.owner_estate_id = null;
                        App.owner_type = "0";
                        App.owner_profile = null;
                        App.estate_num = 0;
                        App.programm_fee = resultMsg.program_fee;
                        App.income_rate = resultMsg.income_rate;
                        App.minimum_rental_income = resultData.agency_fee;


                        await Navigation.PushAsync(new TabPage());
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

        private async void RegistUserInfo()
        {
            loadingbar.IsRunning = true;

            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>(Constants.OWNER_EMAIL, email.Text),
                        new KeyValuePair<string, string>(Constants.OWNER_PASSWORD, password.Text),
                        new KeyValuePair<string, string>(Constants.DEVICE_TOKEN, Preferences.Get(Constants.DEVICE_TOKEN, ""))
                });

                try
                {
                    var request = await cl.PostAsync(Constants.SERVER_OWNER_REGIST_URL, formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success") || resultMsg.resp.Equals("half"))
                    {
                        loadingbar.IsRunning = false;
                        Preferences.Set(Constants.OWNER_EMAIL, email.Text);
                        Preferences.Set(Constants.OWNER_PASSWORD, password.Text);

                        ResponseData resultData = JsonConvert.DeserializeObject<ResponseData>(response);

                        App.owner_ID = resultMsg.owner_id;
                        App.owner_name = null;
                        App.owner_kana = null;
                        App.owner_nickname = null;
                        App.owner_phone1 = null;
                        App.owner_phone2 = null;
                        App.owner_email = email.Text;
                        App.owner_password = null;
                        App.owner_address = null;
                        App.owner_postal_address = null;
                        App.owner_memo = null;
                        App.owner_estate_id = null;
                        App.owner_type = "0";
                        App.owner_profile = null;
                        App.estate_num = 0;
                        App.programm_fee = resultMsg.program_fee;
                        App.income_rate = resultMsg.income_rate;
                        App.minimum_rental_income = resultData.agency_fee;

                        await Navigation.PushAsync(new ConfirmEmailPage());
                    }
                    else
                    {
                        loadingbar.IsRunning = false;

                        await DisplayAlert("", resultMsg.message, "はい");
                    }
                }
                catch
                {
                    loadingbar.IsRunning = false;

                    await DisplayAlert("", Constants.NETWORK_ERROR, "はい");
                }
            }
        }
    }
}