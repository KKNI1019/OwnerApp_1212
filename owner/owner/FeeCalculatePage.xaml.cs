using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Syncfusion.SfChart.XForms;
using owner.Model;
using System.Collections.ObjectModel;
using System.Globalization;

namespace owner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FeeCalculatePage : ContentPage
	{
        public IList<BuildingInfo> Estates { get; set; }

        public FeeCalculatePage ()
		{
			InitializeComponent ();
            
            Estates = new List<BuildingInfo>();

            var estates_num = Global.Buildings.Count;
            if (estates_num != 0)
            {
                for (int i = 0; i < estates_num; i++)
                {
                    BuildingInfo tempbuilding = new BuildingInfo();

                    tempbuilding.building_name = Global.Buildings[i].building_name;
                    tempbuilding.yearly_rental_income = Global.Buildings[i].rental_income;
                    tempbuilding.yearly_admin_expense = Global.Buildings[i].admin_expense;
                    tempbuilding.yearly_repair_reserve = Global.Buildings[i].repair_reserve;
                    tempbuilding.yearly_agency_fee = Global.Buildings[i].agency_fee;
                    tempbuilding.program_fee = Global.Buildings[i].program_fee;
                    tempbuilding.yearly_property_tax = Global.Buildings[i].estate_property_tax;
                    tempbuilding.yearly_other_fee = Global.Buildings[i].yearly_other_fee;
                    tempbuilding.monthly_balance = Global.Buildings[i].rental_income - Global.Buildings[i].admin_expense - Global.Buildings[i].repair_reserve - Global.Buildings[i].program_fee - Global.Buildings[i].estate_property_tax - Global.Buildings[i].yearly_other_fee;
                    tempbuilding.yearly_balance = (tempbuilding.monthly_balance * 12).ToString();
                    tempbuilding.estate_loan_repay = Global.Buildings[i].estate_loan_repay;
                    tempbuilding.estate_loan_amount = Global.Buildings[i].estate_loan_amount;
                    tempbuilding.estate_yearly_profit = Global.Buildings[i].estate_yearly_profit;
                    tempbuilding.estate_repay_period = Global.Buildings[i].estate_repay_period;
                    tempbuilding.Width = App.ScreenWidth;

                    tempbuilding.Data2 = new ObservableCollection<ChatModel>();
                    tempbuilding.Data3 = new ObservableCollection<ChatModel>();
                    tempbuilding.Data4 = new ObservableCollection<ChatModel>();                    
                    
                    double rent = Global.Buildings[i].rental_income;
                    double repair = Global.Buildings[i].repair_reserve;
                    double admin = Global.Buildings[i].admin_expense;
                    double agency = Global.Buildings[i].agency_fee;
                    double remaining = Global.Buildings[i].estate_loan_amount;
                    double tax = Global.Buildings[i].yearly_property_tax;

                    for (int j = 2019; j <= 2025; j++)
                    {
                        var sale_amount = (rent - admin - repair) * 12 / App.income_rate;
                        tempbuilding.Data2.Add(new ChatModel(j, sale_amount));

                        double interest = remaining * Global.Buildings[i].estate_yearly_profit * 30 / 365;
                        double origin_amount = Global.Buildings[i].estate_loan_repay - interest;
                        remaining -= origin_amount;
                        tempbuilding.Data3.Add(new ChatModel(j, remaining));

                        double balance = (rent - repair - admin - tax - App.programm_fee)*12;
                        tempbuilding.Data4.Add(new ChatModel(j, balance));

                        tempbuilding.sale_amount = String.Format("{0:0.##}", (rent - admin - repair) * 12 / App.income_rate); 
                        tempbuilding.remaining_amount = String.Format("{0:0.##}", Global.Buildings[i].estate_loan_repay - interest - origin_amount) ;
                        tempbuilding.yearly_balance = String.Format("{0:0.##}", rent - repair - admin - tax - App.programm_fee); 
                        tempbuilding.sale_loss = String.Format("{0:0.##}", Convert.ToDouble(tempbuilding.sale_amount) - Convert.ToDouble(tempbuilding.remaining_amount) - Convert.ToDouble(tempbuilding.yearly_balance));

                        rent *= 0.99;
                    }

                    Estates.Add(tempbuilding);
                }

                pager.ItemsSource = Estates;
            }
		}

        protected override void OnAppearing()
        {
            MessagingCenter.Subscribe<App>((App)Application.Current, "ShowLoanRepay", (sender) => {
                ShowLoanRepay();
            });

            base.OnAppearing();
        }

        private void ShowLoanRepay()
        {
            string loan_repay_type = App.loan_reapy_type;
            string loan_repay_value = App.loan_repay_value;
        }

        private void menuBtn_clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new SideMenuPopup());
        }

        private async void imgBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        
        private async void AddBuilding_Btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FeeCalculateAddPage());
        }

        private void estate_loan_repay_tap(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new FeeCalculatePopup());
        }
    }
}