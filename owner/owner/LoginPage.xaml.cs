using Newtonsoft.Json;
using owner.Model;
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
	public partial class LoginPage : ContentPage
	{
        private string building_img_url;

        public LoginPage ()
		{
			InitializeComponent ();

            if (string.IsNullOrEmpty(Preferences.Get(Constants.OWNER_REGIST_FROM, "")))
            {
                email.Text = Preferences.Get(Constants.OWNER_EMAIL, "");
            }
        }

        private async void ImgBtn_Login_Clicked(object sender, EventArgs e)
        {
            if (email.Text.Trim() == "")
            {
                await DisplayAlert("", "メールアドレスを入力してください。", "はい");
            } 
            else if (password.Text == "")
            {
                await DisplayAlert("", "パスワードを入力してください。", "はい");
            }
            else
            {
                LoginInfoCheck();
                //await Navigation.PushAsync(new TabPage());
            }
        }

        private async void LoginInfoCheck()
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
                    var request = await cl.PostAsync(Constants.SERVER_OWNER_LOGIN_URL, formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        Preferences.Set(Constants.OWNER_EMAIL, email.Text);
                        Preferences.Set(Constants.OWNER_PASSWORD, password.Text);

                        ResponseData resultData = JsonConvert.DeserializeObject<ResponseData>(response);

                        App.owner_bonus = resultData.owner_data.owner_bonus;
                        App.owner_ID = resultData.owner_data.owner_id;
                        App.owner_name = resultData.owner_data.owner_name;
                        App.owner_kana = resultData.owner_data.owner_kana;
                        App.owner_nickname = resultData.owner_data.owner_nickname;
                        App.owner_phone1 = resultData.owner_data.owner_phone1;
                        App.owner_phone2 = resultData.owner_data.owner_phone2;
                        App.owner_email = resultData.owner_data.owner_email;
                        App.owner_password = resultData.owner_data.owner_password;
                        App.owner_address = resultData.owner_data.owner_address;
                        App.owner_postal_address = resultData.owner_data.owner_postal_address;
                        App.owner_memo = resultData.owner_data.owner_memo;
                        App.owner_estate_id = resultData.owner_data.owner_estate_id;
                        App.owner_type = resultData.owner_data.owner_type;
                        App.owner_profile = resultData.owner_data.owner_profile;
                        App.programm_fee = resultData.program_fee;
                        App.income_rate = resultData.income_rate;
                        App.minimum_rental_income = resultData.agency_fee;
                        App.owner_license = resultData.owner_data.owner_license;

                        var estate_num = resultData.owner_data.owner_estate_data.Length;
                        App.estate_num = estate_num;
                        if (estate_num != 0)
                        {
                            App.repair_reserve = 0;
                            App.rental_income = 0;
                            App.admin_expense = 0;
                            App.agency_fee = 0;

                            Global.Buildings = new List<BuildingInfo>();
                            for (int i = 0; i < estate_num; i++)
                            {
                                
                                if (string.IsNullOrEmpty(resultData.owner_data.owner_estate_data[i].estate_image_url))
                                {
                                    building_img_url = "noimage.png";
                                }
                                else
                                {
                                    building_img_url = Constants.ESTATE_IMAGE_URL_PREFIX + resultData.owner_data.owner_estate_data[i].estate_image_url;
                                }
                                Global.Buildings.Add(new BuildingInfo
                                {
                                    building_id = resultData.owner_data.owner_estate_data[i].estate_id,
                                    building_name = resultData.owner_data.owner_estate_data[i].estate_name,
                                    rental_income = Convert.ToInt32(resultData.owner_data.owner_estate_data[i].estate_rent),
                                    admin_expense = Convert.ToInt32(resultData.owner_data.owner_estate_data[i].estate_admin_expense),
                                    agency_fee = Convert.ToInt32(resultData.owner_data.owner_estate_data[i].estate_agency_fee),
                                    repair_reserve = Convert.ToInt32(resultData.owner_data.owner_estate_data[i].estate_repair_reserve),
                                    estate_image_url = building_img_url,
                                    estate_loan_repay = resultData.owner_data.owner_estate_data[i].estate_loan_repay,
                                    estate_loan_amount = resultData.owner_data.owner_estate_data[i].estate_loan_amount,
                                    estate_yearly_profit = resultData.owner_data.owner_estate_data[i].estate_yearly_profit,
                                    estate_repay_period = resultData.owner_data.owner_estate_data[i].estate_repay_period,
                                    estate_property_tax = resultData.owner_data.owner_estate_data[i].estate_property_tax,
                                    program_fee = App.programm_fee,
                                    zero_status = resultData.owner_data.owner_estate_data[i].estate_zero_status

                                });
                     
                                App.rental_income += Convert.ToInt32(resultData.owner_data.owner_estate_data[i].estate_rent);
                                App.admin_expense += Convert.ToInt32(resultData.owner_data.owner_estate_data[i].estate_admin_expense);
                                App.repair_reserve += Convert.ToInt32(resultData.owner_data.owner_estate_data[i].estate_repair_reserve);
                                App.agency_fee += Convert.ToInt32(resultData.owner_data.owner_estate_data[i].estate_agency_fee);
                                
                            }
                        }

                        var prefecture_num = resultData.prefecture_data.Length;
                        if (prefecture_num > 0)
                        {
                            Global.prefectures = new List<Prefectures>();
                            for (int i = 0; i < prefecture_num; i++)
                            {
                                Prefectures temp_prefecture = new Prefectures();
                                temp_prefecture.prefecture_name = resultData.prefecture_data[i].prefecture_name;
                                temp_prefecture.prefecture_id = resultData.prefecture_data[i].prefecture_id;

                                temp_prefecture.city = new List<City>();
                                var city_num = resultData.city_data.Length;
                                if (city_num > 0)
                                {
                                    for (int j = 0; j < city_num; j++)
                                    {
                                        if (temp_prefecture.prefecture_id == resultData.city_data[j].prefecture_id)
                                        {
                                            City temp_city = new City();
                                            temp_city.city_name = resultData.city_data[j].city_name;
                                            temp_prefecture.city.Add(temp_city);
                                        }
                                    }
                                }
                                Global.prefectures.Add(temp_prefecture);
                            }
                        }
                        loadingbar.IsRunning = false;
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
                        Preferences.Set(Constants.OWNER_EMAIL, email);
                        Preferences.Set(Constants.OWNER_NAME, name);
                        Preferences.Set(Constants.OWNER_REGIST_FROM, method);

                        loadingbar.IsRunning = false;

                        ResponseData resultData = JsonConvert.DeserializeObject<ResponseData>(response);

                        App.owner_bonus = resultData.owner_data.owner_bonus;
                        App.owner_ID = resultData.owner_data.owner_id;
                        App.owner_name = resultData.owner_data.owner_name;
                        App.owner_kana = resultData.owner_data.owner_kana;
                        App.owner_nickname = resultData.owner_data.owner_nickname;
                        App.owner_phone1 = resultData.owner_data.owner_phone1;
                        App.owner_phone2 = resultData.owner_data.owner_phone2;
                        App.owner_email = resultData.owner_data.owner_email;
                        App.owner_password = resultData.owner_data.owner_password;
                        App.owner_address = resultData.owner_data.owner_address;
                        App.owner_postal_address = resultData.owner_data.owner_postal_address;
                        App.owner_memo = resultData.owner_data.owner_memo;
                        App.owner_estate_id = resultData.owner_data.owner_estate_id;
                        App.owner_type = resultData.owner_data.owner_type;
                        App.owner_profile = resultData.owner_data.owner_profile;
                        App.programm_fee = resultData.program_fee;
                        App.income_rate = resultData.income_rate;
                        App.minimum_rental_income = resultData.agency_fee;
                        App.owner_license = resultData.owner_data.owner_license;

                        var estate_num = resultData.owner_data.owner_estate_data.Length;
                        App.estate_num = estate_num;
                        if (estate_num != 0)
                        {
                            App.repair_reserve = 0;
                            App.rental_income = 0;
                            App.admin_expense = 0;
                            App.agency_fee = 0;

                            Global.Buildings = new List<BuildingInfo>();
                            for (int i = 0; i < estate_num; i++)
                            {

                                if (string.IsNullOrEmpty(resultData.owner_data.owner_estate_data[i].estate_image_url))
                                {
                                    building_img_url = "noimage.png";
                                }
                                else
                                {
                                    building_img_url = Constants.ESTATE_IMAGE_URL_PREFIX + resultData.owner_data.owner_estate_data[i].estate_image_url;
                                }
                                Global.Buildings.Add(new BuildingInfo
                                {
                                    building_id = resultData.owner_data.owner_estate_data[i].estate_id,
                                    building_name = resultData.owner_data.owner_estate_data[i].estate_name,
                                    rental_income = Convert.ToInt32(resultData.owner_data.owner_estate_data[i].estate_rent),
                                    admin_expense = Convert.ToInt32(resultData.owner_data.owner_estate_data[i].estate_admin_expense),
                                    agency_fee = Convert.ToInt32(resultData.owner_data.owner_estate_data[i].estate_agency_fee),
                                    repair_reserve = Convert.ToInt32(resultData.owner_data.owner_estate_data[i].estate_repair_reserve),
                                    estate_image_url = building_img_url,
                                    estate_loan_repay = resultData.owner_data.owner_estate_data[i].estate_loan_repay,
                                    estate_loan_amount = resultData.owner_data.owner_estate_data[i].estate_loan_amount,
                                    estate_yearly_profit = resultData.owner_data.owner_estate_data[i].estate_yearly_profit,
                                    estate_repay_period = resultData.owner_data.owner_estate_data[i].estate_repay_period,
                                    estate_property_tax = resultData.owner_data.owner_estate_data[i].estate_property_tax,
                                    program_fee = App.programm_fee,
                                    zero_status = resultData.owner_data.owner_estate_data[i].estate_zero_status
                                });

                                App.rental_income += Convert.ToInt32(resultData.owner_data.owner_estate_data[i].estate_rent);
                                App.admin_expense += Convert.ToInt32(resultData.owner_data.owner_estate_data[i].estate_admin_expense);
                                App.repair_reserve += Convert.ToInt32(resultData.owner_data.owner_estate_data[i].estate_repair_reserve);
                                App.agency_fee += Convert.ToInt32(resultData.owner_data.owner_estate_data[i].estate_agency_fee);

                            }
                        }

                        var prefecture_num = resultData.prefecture_data.Length;
                        if (prefecture_num > 0)
                        {
                            Global.prefectures = new List<Prefectures>();
                            for (int i = 0; i < prefecture_num; i++)
                            {
                                Prefectures temp_prefecture = new Prefectures();
                                temp_prefecture.prefecture_name = resultData.prefecture_data[i].prefecture_name;
                                temp_prefecture.prefecture_id = resultData.prefecture_data[i].prefecture_id;

                                temp_prefecture.city = new List<City>();
                                var city_num = resultData.city_data.Length;
                                if (city_num > 0)
                                {
                                    for (int j = 0; j < city_num; j++)
                                    {
                                        if (temp_prefecture.prefecture_id == resultData.city_data[j].prefecture_id)
                                        {
                                            City temp_city = new City();
                                            temp_city.city_name = resultData.city_data[j].city_name;
                                            temp_prefecture.city.Add(temp_city);
                                        }
                                    }
                                }
                                Global.prefectures.Add(temp_prefecture);
                            }
                        }

                        loadingbar.IsRunning = false;
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

        protected  override bool OnBackButtonPressed()
        {
            closeApp_check();

            //return base.OnBackButtonPressed();
            return true;
        }

        private async void closeApp_check()
        {
            var action = await DisplayAlert("", "アプリを終了しますか？", "はい", "キャンセル");
            if (action)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
        
    }
}