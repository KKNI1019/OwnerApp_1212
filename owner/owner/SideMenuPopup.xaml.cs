using owner.WebService;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
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
	public partial class SideMenuPopup : PopupPage
    {
		public SideMenuPopup ()
		{
			InitializeComponent ();

            myPage.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => MyPage_clicked()),
            });
            botPage.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => BotPage_clicked()),
            });
            faqPage.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => FaqPage_clicked()),
            });
            privacyPage.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => PrivacyPage_clicked()),
            });
            licensePage.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => LicensePage_clicked()),
            });
            lbl_logout.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => lblLogout_clicked()),
            });
        }

        private async void MyPage_clicked()
        {
            var masterPage = Application.Current.MainPage.Navigation.NavigationStack[Navigation.NavigationStack.Count-1] as TabbedPage;
            if (masterPage != null)
            {
                masterPage.CurrentPage = masterPage.Children[3];
            }
            else
            {
                masterPage = Application.Current.MainPage.Navigation.NavigationStack[Navigation.NavigationStack.Count - 2] as TabbedPage;
                if (masterPage != null)
                {
                    masterPage.CurrentPage = masterPage.Children[3];
                }
                else
                {
                    masterPage = Application.Current.MainPage.Navigation.NavigationStack[Navigation.NavigationStack.Count - 3] as TabbedPage;
                    if (masterPage != null)
                    {
                        masterPage.CurrentPage = masterPage.Children[3];
                    }
                }
            }
            await PopupNavigation.Instance.PopAsync();
        }

        private async void BotPage_clicked()
        {
            var masterPage = Navigation.NavigationStack[Navigation.NavigationStack.Count - 1] as TabbedPage;
            if (masterPage != null)
            {
                masterPage.CurrentPage = masterPage.Children[1];
            }
            else
            {
                masterPage = Application.Current.MainPage.Navigation.NavigationStack[Navigation.NavigationStack.Count - 2] as TabbedPage;
                if (masterPage != null)
                {
                    masterPage.CurrentPage = masterPage.Children[1];
                }
                else
                {
                    masterPage = Application.Current.MainPage.Navigation.NavigationStack[Navigation.NavigationStack.Count - 3] as TabbedPage;
                    if (masterPage != null)
                    {
                        masterPage.CurrentPage = masterPage.Children[1];
                    }
                }
            }
            await PopupNavigation.Instance.PopAsync();
        }

        private async void FaqPage_clicked()
        {
            await Navigation.PushAsync(new QuizPage());
            await PopupNavigation.Instance.PopAsync();
        }

        private async void PrivacyPage_clicked()
        {
            await Navigation.PushAsync(new PripacyPage());
            await PopupNavigation.Instance.PopAsync();
        }

        private async void LicensePage_clicked()
        {
            await Navigation.PushAsync(new RulePage());
            await PopupNavigation.Instance.PopAsync();
        }

        private async void lblLogout_clicked()
        {
            Preferences.Set(Constants.REGISTERED, false);
            await Navigation.PushAsync(new LoginPage());
            await PopupNavigation.Instance.PopAsync();
        }

        private async void imgDel_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}