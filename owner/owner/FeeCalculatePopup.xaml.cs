using Rg.Plugins.Popup.Pages;
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
	public partial class FeeCalculatePopup : PopupPage
    {
		public FeeCalculatePopup ()
		{
			InitializeComponent ();
		}

        private async void Btn_input_Clicked(object sender, EventArgs e)
        {
            if (radio_loan_repay_type.SelectedItem == null)
            {
                await DisplayAlert("", "選択内容を確認してください。", "はい");
            }
            else if (string.IsNullOrEmpty(edit_loan_repay_value.Text))
            {
                await DisplayAlert("", "金額を入力してください。", "はい");
            }
            else
            {
                App.loan_reapy_type = lbl_loan_repay_type.Text;
                App.loan_repay_value = edit_loan_repay_value.Text;

                MessagingCenter.Send<App>((App)Application.Current, "ShowLoanRepay");

                await PopupNavigation.Instance.PopAsync();
            }
        }

        private void Radio_loan_repay_type_SelectedItemChanged(object sender, EventArgs e)
        {
            lbl_loan_repay_type.Text = radio_loan_repay_type.SelectedItem.ToString();
        }
    }
}