using owner.DB;
using owner.Model;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace owner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SaleEstimatePage : ContentPage
	{
		public SaleEstimatePage ()
		{
			InitializeComponent ();

            List<string> prefecture_names = new List<string>();
            for (int i = 0; i < Global.prefectures.Count; i++)
            {
                prefecture_names.Add(Global.prefectures[i].prefecture_name);
            }
            picker_state.ItemsSource = prefecture_names.ToArray();
        }

        private async void imgBack_Clicked(object sender, EventArgs e)
        {
           await Navigation.PopAsync();
        }
        
        private async void Sale_btn_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lbl_state.Text))
            {
                await DisplayAlert("", "都道府県を選択してください。", "はい");
            }
            else if (string.IsNullOrEmpty(lbl_city.Text))
            {
                await DisplayAlert("", "市区町村を選択してください。", "はい");
            }
            else if (around_station.Text == "")
            {
                await DisplayAlert("", "最寄り駅*を入力してください。", "はい");
            }
            else if (walking_time.Text == "")
            {
                await DisplayAlert("", "駅徒歩*を入力してください。", "はい");
            }
            else if (rental_income.Text == "")
            {
                await DisplayAlert("", "家賃*を入力してください。", "はい");
            }
            else if (admin_expenses.Text == "")
            {
                await DisplayAlert("", "管理費・終戦積立金等*を入力してください。", "はい");
            }
            else
            {
                int int_rental_income = Convert.ToInt32(rental_income.Text);
                int int_admin_expenses = Convert.ToInt32(admin_expenses.Text);
                await Navigation.PushAsync(new SaleEstimateCompletePagexaml(int_rental_income, int_admin_expenses));
            }
        }

        private void lbl_state_tap(object sender, EventArgs e)
        {
            picker_state.Focus();
        }

        private async void Picker_state_SelectedIndexChanged(object sender, EventArgs e)
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
    }
}