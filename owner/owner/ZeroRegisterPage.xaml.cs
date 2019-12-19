using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using owner.DB;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using owner.Model;
using System.Net.Http;
using owner.WebService;
using Xamarin.Essentials;
using Newtonsoft.Json;

namespace owner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ZeroRegisterPage : ContentPage
	{
        private string title;

		public ZeroRegisterPage (string txt_title)
		{
			InitializeComponent ();

            title = txt_title;
            lbl_title.Text = title;

            List<string> prefecture_names = new List<string>();
            for (int i = 0; i < Global.prefectures.Count; i++)
            {
                prefecture_names.Add(Global.prefectures[i].prefecture_name);
            }
            picker_state.ItemsSource = prefecture_names.ToArray();
            
		}

        private async void Start_btn_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lbl_state.Text))
            {
                await DisplayAlert("", "都道府県を選択してください。", "はい");
            }
            else if (string.IsNullOrEmpty(lbl_city.Text))
            {
                await DisplayAlert("", "市区町村を選択してください。", "はい");
            }
            else if (company_name.Text == "")
            {
                await DisplayAlert("", "管理会社を入力してください。", "はい");
            }
            else if (company_phone.Text == "")
            {
                await DisplayAlert("", "管理会社連絡先を入力してください。", "はい");
            }
            else if (estate_name.Text == "")
            {
                await DisplayAlert("", "物件名を入力してください。", "はい");
            }
            else if (room_number.Text == "")
            {
                await DisplayAlert("", "号室を入力してください。", "はい");
            }
            else if (ent_rent.Text == "")
            {
                await DisplayAlert("", "家賃を入力してください。", "はい");
            }
            else if (zero_type.SelectedItem == null)
            {
                await DisplayAlert("", "管理種別を選択してください。", "はい");
            }
            else
            {
                Global.zero_estate_address = lbl_state.Text + lbl_city.Text;
                Global.zero_estate_name = estate_name.Text;
                Global.zero_estate_room_number = room_number.Text;
                Global.zero_type = zero_type.SelectedItem.ToString();
                Global.zero_company = company_name.Text;
                Global.zero_company_address = company_phone.Text;
                Global.zero_rent_income = ent_rent.Text;

                if (Convert.ToInt32(ent_rent.Text) < Convert.ToInt32(App.minimum_rental_income))
                {
                    await Navigation.PushAsync(new ZeroNoResponsePage(title));
                }
                else
                {
                    RegisterZeroInfo();
                }
            }
        }

        private async void RegisterZeroInfo()
        {
            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>(Constants.OWNER_ID, App.owner_ID),
                        new KeyValuePair<string, string>(Constants.ESTATE_NAME, Global.zero_estate_name),
                        new KeyValuePair<string, string>(Constants.ESTATE_ADDRESS, Global.zero_estate_address),
                        new KeyValuePair<string, string>(Constants.ESTATE_ROOM_NUMBER, Global.zero_estate_room_number),
                        new KeyValuePair<string, string>(Constants.ZERO_TYPE, Global.zero_type),
                        new KeyValuePair<string, string>(Constants.ZERO_AGENCY_NAME, Global.zero_company),
                        new KeyValuePair<string, string>(Constants.ZERO_AGENCY_PHONE, Global.zero_company_address),
                        new KeyValuePair<string, string>(Constants.RENTAL_INCOME, Global.zero_rent_income),
                        new KeyValuePair<string, string>(Constants.ADMIN_EXPENSES, "0"),
                        new KeyValuePair<string, string>(Constants.REPAIR_RESERVE, "0"),
                        new KeyValuePair<string, string>(Constants.AGENCY_FEE, "0"),
                        new KeyValuePair<string, string>(Constants.estate_property_tax, "0"),
                });

                try
                {
                    var request = await cl.PostAsync(Constants.SERVER_ZERO_REGIST_URL, formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        App.zero_id = resultMsg.zero_id;
                       
                        await Navigation.PushAsync(new ZeroUnregisterAlert(title));
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

        private void PickerSelection1(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            //put your code here
        }

        private void lbl_state_tap(object sender, EventArgs e)
        {
            picker_state.Focus();
        }

        private void Picker_state_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                lbl_state.Text = (string)picker.ItemsSource[selectedIndex];
            }

            int index = 0;
            List<string> city_names = new List<string>();
            for (int i = 0; i < Global.prefectures.Count; i++)
            {
                if (string.Equals(lbl_state.Text, Global.prefectures[i].prefecture_name))
                {
                    index = i;
                    break;
                }
            }

            if (Global.prefectures[index].city.Count > 0)
            {
                for (int j = 0; j < Global.prefectures[index].city.Count; j++)
                {
                    city_names.Add(Global.prefectures[index].city[j].city_name);
                }
            }

            picker_city.ItemsSource = city_names.ToArray();
        }

        private void lbl_city_tap(object sender, EventArgs e)
        {
            picker_city.Focus();
        }

        private void Picker_city_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                lbl_city.Text = (string)picker.ItemsSource[selectedIndex];
            }
        }

        private void lbl_cancel_tap(object sender, EventArgs e)
        {
            Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 2]);
            Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 1]);
        }
    }
}