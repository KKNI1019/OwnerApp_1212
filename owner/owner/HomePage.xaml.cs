using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using owner.Model;
using owner.WebService;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace owner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
        private StackLayout _previousLayout;
        private Label _previousYearLabel;
        private Label _previousMonthLabel;
        private StackLayout _previousHighlightLayout;

        private List<Month> month_list;
        private Color balance_color;


        public HomePage ()
		{
			InitializeComponent ();

            NavigationPage.SetHasNavigationBar(this, false);
            month_list = new List<Month>();

            DateTime date = DateTime.Now;

            getBalance(date.Year,date.Month);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            DateTime date = DateTime.Now;
            

            for (int i = date.Year-1; i <= date.Year+1; i++)
            {
                for(int j = 1; j < 13; j++)
                {
                    Month tmp = new Month();
                    tmp.year = $"{i} {"年"}";
                    tmp.month = $"{j} {"月"}";
                    month_list.Add(tmp);
                }
            }

            date_list.ItemsSource = month_list;

            if (App.owner_type == "0")
            {
                imgZero.IsVisible = true;
                imgAdmin.IsVisible = false;
            }
            else
            {
                imgZero.IsVisible = false;
                imgAdmin.IsVisible = true;
            }
        }

        private async void getBalance(int year,int month)
        {
            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("owner_id", App.owner_ID),
                    new KeyValuePair<string, string>("month", year + "/" + month +"/1")
                });

                try
                {
                    var request = await cl.PostAsync(Constants.SERVER_BASE_URL + "get_month_calculation", formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        ResponseData resultData = JsonConvert.DeserializeObject<ResponseData>(response);
                        
                        string balance_month = $"{year}{"年"}{month}{"月の収支"}";

                        lbl_balance.Text = balance_month;
                        lbl_income.Text = resultData.income;
                        lbl_spend.Text = resultData.outcome;
                        var current_balance = Convert.ToInt32(resultData.income) - Convert.ToInt32(resultData.outcome);
                        if (current_balance >= 0)
                        {
                            balance_color = Color.Black;
                        }
                        else
                        {
                            balance_color = Color.DarkRed;
                        }
                        lbl_current_balance.Text = current_balance.ToString();
                        lbl_current_balance.TextColor = balance_color;
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

        private void menuBtn_clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new SideMenuPopup());
        }

        private void OnChildItemLayout_Tapped(object sender, EventArgs e)
        {
            Label child = (Label)sender;
            OnItemLayout_Tapped(child.Parent, e);
        }

        private void OnItemLayout_Tapped(object sender, EventArgs e)
        {
            if (_previousLayout != null)
            {
                _previousLayout.BackgroundColor = Color.Transparent;
            }

            if (_previousYearLabel != null)
            {
                _previousYearLabel.BackgroundColor = Color.Transparent;
            }

            if (_previousMonthLabel != null)
            {
                _previousMonthLabel.BackgroundColor = Color.Transparent;
            }

            if (_previousHighlightLayout != null)
            {
                _previousHighlightLayout.BackgroundColor = Color.Transparent;
            }

            var selectLayout = (StackLayout)sender;
            Label selected_year = (Label) selectLayout.Children[0];
            var year = Convert.ToInt32(selected_year.Text.Replace("年", string.Empty));

            Label selected_month = (Label)selectLayout.Children[1];
            var month = Convert.ToInt32(selected_month.Text.Replace("月", string.Empty));

            StackLayout selected_highlight = (StackLayout)selectLayout.Children[2];
            selected_highlight.BackgroundColor = Color.FromHex("F8B500");

            _previousLayout = selectLayout;
            _previousMonthLabel = selected_month;
            _previousYearLabel = selected_year;
            _previousHighlightLayout = selected_highlight;

            getBalance(year, month);
        }

        private async void imgZero_Clicked(object sender, EventArgs e)
        {
            bool video_checked = Preferences.Get("zero_video_checked", false);
            if (string.IsNullOrEmpty(Preferences.Get("owner_zero_video", "")))
            {
                await Navigation.PushAsync(new ZeroRegisterAlert("Zero管理"));
            }
            else
            {
                if (!video_checked)
                {
                    await Navigation.PushAsync(new ZeroVideoPage());
                }
                else
                {
                    await Navigation.PushAsync(new ZeroRegisterAlert("Zero管理"));
                }
            }
        }

        private async void imgAdmin_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SaleRequestPage("物件売却依頼"));
        }
        private void imgQA_Clicked(object sender, EventArgs e)
        {
            var masterPage = this.Parent.Parent as TabbedPage;
            if (masterPage != null)
            {
                masterPage.CurrentPage = masterPage.Children[1];
            }
        }

        private async void ImgBtn_board_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BoardPage());
        }

        private async void ImgBtn_column_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewsPage());
        }

        private async void imgEstimate_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SaleEstimatePage());
        }
        
        private async void imgSaleList_clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EstateSaleStep1Page());
            //await Navigation.PushAsync(new SaleRequestPage("物件売却依頼"));
        }
        
        private async void imgConfirm_clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ConfirmReportPage());
        }

        private async void Fee_btn_Clicked(object sender, EventArgs e)
        {
            if (App.owner_type == "0")
            {
                await Navigation.PushAsync(new FeeCalculateRegisterPage());
            }
            else
            {
                await Navigation.PushAsync(new FeeCalculatePage());
            }
            
        }
    }
}