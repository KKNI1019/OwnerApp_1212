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
	public partial class ColumnPopup : PopupPage
    {
        private TaskCompletionSource<bool> taskCompletionSource;
        private ImageSource imgsource;
        private string writterName;

        public ColumnPopup(ImageSource img, string name)
        {
            InitializeComponent();
            imgsource = img;
            writterName = name;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            taskCompletionSource = new TaskCompletionSource<bool>();

            imgWritter.Source = imgsource;
            lbl_writterName.Text = writterName;
        }

        private async void BtnClose_Clicked(object sender, EventArgs e)
        {

            await PopupNavigation.Instance.PopAsync();
        }

        protected override bool OnBackgroundClicked()
        {
            CloseAllPopup();
            return false;
        }
        protected override bool OnBackButtonPressed()
        {
            CloseAllPopup();
            return false;
        }


        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAsync();
        }
        
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            taskCompletionSource.SetResult(true);
        }
    }
}