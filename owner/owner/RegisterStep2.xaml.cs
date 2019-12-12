using owner.WebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace owner
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterStep2 : ContentPage
    {
        public RegisterStep2()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.White;
            ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.Black;

        }

        private async void Step2_next_btn_Clicked(object sender, EventArgs e)
        {
            if (admin_expenses.Text.Equals(""))
            {
                await DisplayAlert("", "管理費を入力してください。", "はい");
            }
            else if (repair_reserve.Text.Equals(""))
            {
                await DisplayAlert("", "修繕積立金を入力してください。", "はい");
            }
            else
            {
                Preferences.Set(Constants.ADMIN_EXPENSES, admin_expenses.Text);
                Preferences.Set(Constants.REPAIR_RESERVE, repair_reserve.Text);
                await Navigation.PushAsync(new RegisterStep3
                {

                });
            }
        }

        private async void Back_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}