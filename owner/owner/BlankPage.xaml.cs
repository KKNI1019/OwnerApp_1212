using Newtonsoft.Json;
using owner.Model;
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
	public partial class BlankPage : ContentPage
    {
        private string building_img_url;

        public BlankPage ()
		{
			InitializeComponent ();

            loadingbar.IsRunning = true;
            loadingbar.IsEnabled = true;

            bool video_checked = Preferences.Get("video_checked", false);
            if (!video_checked)
            {
                ToVideoPage();
            }
            else
            {
                if (!Preferences.Get(Constants.REGISTERED, false))
                {
                    if (string.IsNullOrEmpty(Preferences.Get(Constants.OWNER_EMAIL, "")))
                    {
                        ToLoginStart();
                    }
                    else
                    {
                        ToLogin();
                    }

                }
                else
                {
                    if (string.IsNullOrEmpty(Preferences.Get(Constants.OWNER_REGIST_FROM, "")))
                    {
                        login();
                    }
                    else
                    {
                        social_login();
                    }
                }
            }
        }

        private async void ToVideoPage()
        {
            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("", "")
                });

                try
                {
                    var request = await cl.PostAsync(Constants.SERVER_VIDEO_URL, formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        loadingbar.IsRunning = false;
                        Preferences.Set("owner_video", resultMsg.owner_video);
                        Preferences.Set("owner_zero_video", resultMsg.owner_zero_video);

                        if (string.IsNullOrEmpty(resultMsg.owner_video))
                        {
                            if (Preferences.Get(Constants.REGISTERED, false))
                            {
                                if (string.IsNullOrEmpty(Preferences.Get(Constants.OWNER_REGIST_FROM, "")))
                                {
                                    login();
                                }
                                else
                                {
                                    social_login();
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(Preferences.Get(Constants.OWNER_EMAIL, "")))
                                {
                                    ToLoginStart();
                                }
                                else
                                {
                                    ToLogin();
                                }
                            }
                        }
                        else
                        {
                            await Navigation.PushAsync(new VideoPage());
                            Navigation.RemovePage(this);
                        }
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

        private async void ToLoginStart()
        {
            await Navigation.PushAsync(new LoginStartPage());
            Navigation.RemovePage(this);
        }

        private async void ToLogin()
        {
            await Navigation.PushAsync(new LoginPage());
            Navigation.RemovePage(this);
        }
        private async void login()
        {
            loadingbar.IsRunning = true;
            loadingbar.IsEnabled = true;
            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>(Constants.OWNER_EMAIL, Preferences.Get(Constants.OWNER_EMAIL,"")),
                        new KeyValuePair<string, string>(Constants.OWNER_PASSWORD, Preferences.Get(Constants.OWNER_PASSWORD,"")),
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
                        loadingbar.IsRunning = false;
                        loadingbar.IsEnabled = false;
                        ResponseData resultData = JsonConvert.DeserializeObject<ResponseData>(response);

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
                        App.owner_license = resultData.owner_data.owner_license;
                        App.programm_fee = resultData.program_fee;
                        App.income_rate = resultData.income_rate;
                        App.minimum_rental_income = resultData.agency_fee;

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
                                    program_fee = App.programm_fee

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
                        loadingbar.IsEnabled = false;
                        await Navigation.PushAsync(new TabPage());
                        Navigation.RemovePage(this);
                    }
                    else
                    {
                        loadingbar.IsRunning = false;
                        loadingbar.IsEnabled = false;
                        await DisplayAlert("", resultMsg.resp, "はい");
                    }
                }
                catch
                {
                    loadingbar.IsRunning = false;
                    loadingbar.IsEnabled = false;
                    await DisplayAlert("", Constants.NETWORK_ERROR, "はい");
                }
            }
        }

        private async void social_login()
        {
            loadingbar.IsRunning = true;
            loadingbar.IsEnabled = true;
            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>(Constants.OWNER_EMAIL, Preferences.Get(Constants.OWNER_EMAIL,"")),
                        new KeyValuePair<string, string>(Constants.OWNER_NAME, Preferences.Get(Constants.OWNER_NAME,"")),
                        new KeyValuePair<string, string>(Constants.OWNER_REGIST_FROM, Preferences.Get(Constants.OWNER_REGIST_FROM,"")),
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
                        ResponseData resultData = JsonConvert.DeserializeObject<ResponseData>(response);

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
                        App.owner_license = resultData.owner_data.owner_license;
                        App.programm_fee = resultData.program_fee;
                        App.income_rate = resultData.income_rate;
                        App.minimum_rental_income = resultData.agency_fee;

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
                                    program_fee = App.programm_fee

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
                        loadingbar.IsEnabled = false;
                        await Navigation.PushAsync(new TabPage());
                        Navigation.RemovePage(this);
                    }
                    else
                    {
                        loadingbar.IsRunning = false;
                        loadingbar.IsEnabled = false;
                        await DisplayAlert("", resultMsg.resp, "はい");
                        Navigation.RemovePage(this);
                    }
                }
                catch
                {
                    loadingbar.IsRunning = false;
                    loadingbar.IsEnabled = false;
                    await DisplayAlert("", Constants.NETWORK_ERROR, "はい");
                    Navigation.RemovePage(this);
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}