using Newtonsoft.Json;
using owner.WebService;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Stormlion.PhotoBrowser;
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
	public partial class MyPage : ContentPage
	{
        public MyPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            var buildingTapFinance = new TapGestureRecognizer();
            buildingTapFinance.Tapped += (s, e) =>
            {
                Goto_BuildingListPage(s, e);
            };
            lay_buildingNumber.GestureRecognizers.Add(buildingTapFinance);

            var editProfileTapFinance = new TapGestureRecognizer();
            editProfileTapFinance.Tapped += (s, e) =>
            {
                EditProfile_Clicked();
            };
            lbl_editProfile.GestureRecognizers.Add(editProfileTapFinance);

            var faqTapFinance = new TapGestureRecognizer();
            faqTapFinance.Tapped += (s, e) =>
            {
                Goto_FAQPage(s, e);
            };
            lbl_faq.GestureRecognizers.Add(faqTapFinance);

        }

        protected override void OnAppearing()
        {
            lbl_email.Text = App.owner_email;
            lbl_name.Text = App.owner_name;
            lbl_address.Text = App.owner_address;
            lbl_building_num.Text = App.estate_num.ToString();
            if (App.owner_profile != null)
            {
                ImgUser.Source = Constants.IMAGE_UPLOAD_URL_PREFIX + App.owner_profile;
            }

            if (!string.IsNullOrEmpty(App.owner_postal_address))
            {
                lbl_postal_address.Text = App.owner_postal_address;
            }
            else
            {
                lbl_postal_address.Text = "住所と同一";
            }

            if (App.owner_type.Equals("0"))
            {
                if (App.owner_profile != null)
                {
                    ImgUser.Source = Constants.IMAGE_UPLOAD_URL_PREFIX + App.owner_profile;
                }
                img_owner_type.Source = "free_member.png";
                stk_contracts.IsVisible = false;
            }
            else
            {
                if (App.owner_profile != null)
                {
                    ImgUser.Source = Constants.IMAGE_UPLOAD_URL_PREFIX + App.owner_profile;
                }
                img_owner_type.Source = "paid_member.png";

                update_btn.IsVisible = false;
                stk_contracts.IsVisible = true;
            }

            base.OnAppearing();
        }

        private async void Goto_FAQPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new QuizPage());
        }

        private async void EditProfile_Clicked()
        {
            await Navigation.PushAsync(new MyPage1(){ });
        }

        private async void Goto_BuildingListPage(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SaleRequestPage("登録物件"));
        }

        private async void Update_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyInfoUpdatePage());
        }

        private async void stk_contracts_tap(object sender, EventArgs e)
        {
            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("owner_id", App.owner_ID)
                });

                try
                {
                    var request = await cl.PostAsync(Constants.SERVER_BASE_URL + "get_agreement_all", formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        var photoBrowser = new PhotoBrowser();
                        var photoList = new List<Photo>();

                        if (App.owner_license != null)
                        {
                            var licence_photo = new Photo();
                            licence_photo.URL = Constants.LICENCE_IMAGE_URL_PREFIX + App.owner_license;
                            photoList.Add(licence_photo);
                        }

                        int image_num = resultMsg.agreement_data.Length;
                        if (image_num > 0)
                        {
                            for (int i = 0; i < image_num; i++)
                            {
                                var photo = new Photo();
                                photo.URL = Constants.AGREEMENT_IMAGE_URL_PREFIX + resultMsg.agreement_data[i].agreement_image;
                                photoList.Add(photo);
                            }
                            photoBrowser.Photos = photoList;
                            photoBrowser.Show();
                        }
                        else
                        {
                            await DisplayAlert("", "管理委託契約した建物が存在しません。", "はい");
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
    }
}