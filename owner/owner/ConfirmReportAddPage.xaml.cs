using System;
using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using owner.Model;
using System.Net.Http;
using owner.WebService;
using Newtonsoft.Json;
using System.Json;

namespace owner
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmReportAddPage : ContentPage
    {
        private StackLayout _previousLayout;
        private StackLayout _previousHighlightLayout;
        private List<Month> month_list;
        private int current_month = 1;
        private int building_index;
        private int selected_year;
        private int temp_month;

        public ConfirmReportAddPage(int year, int index)
        {
            InitializeComponent();

            building_index = index;
            selected_year = year;
            month_list = new List<Month>();

            //show_original_values(1);

        }

        protected override void OnAppearing()
        {

            base.OnAppearing();

            DateTime date = DateTime.Now;

            for (int j = 1; j < 13; j++)
            {
                Month tmp = new Month();
                tmp.year = $"{selected_year.ToString()} {"年"}";
                tmp.month = $"{j} {"月"}";
                month_list.Add(tmp);
            }

            date_list.ItemsSource = month_list;

            MessagingCenter.Subscribe<App>((App)Application.Current, "AddNewItem", (sender) =>
            {
                Create_UI(Global.month);
            });

            Create_UI(1);
        }  

        private StackLayout addNewItem(string fee_name, string fee_value, string calculate_type)
        {
            var stack_new_item = new StackLayout()
            {
                Padding = new Thickness(20, 10),
                HorizontalOptions = LayoutOptions.Center
            };
            var grid = new Grid();
            var frame_lay = new Frame() { BorderColor = Color.FromHex("#F8F8F8"), BackgroundColor = Color.White, Padding = new Thickness(5) };

            var stack = new StackLayout();
            var lbl_item_name = new Label()
            {
                FontSize = 10,
                TextColor = Color.FromHex("#A0A0A0"),
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Text = fee_name
            };
            var editor_new_item = new BorderlessEditor()
            {
                HorizontalOptions = LayoutOptions.Fill,
                TextColor = getColor(calculate_type),
                MaxLength = 10,
                AutoSize = EditorAutoSizeOption.TextChanges,
                Keyboard = Keyboard.Numeric,
                FlowDirection = FlowDirection.RightToLeft,
                Text = fee_value
            };
            stack.Children.Add(lbl_item_name);
            stack.Children.Add(editor_new_item);
            frame_lay.Content = stack;

            var lbl_unit = new Label()
            {
                Text = "円",
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.End
            };

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(9, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.Children.Add(frame_lay, 0, 0);
            grid.Children.Add(lbl_unit, 1, 0);

            stack_new_item.Children.Add(grid);
            return stack_new_item;
        }

        private void Create_UI(int month)
        {
            stack_items.Children.Clear();

            for (int i = 0; i < Global.reportItems.Count; i++)
            {
                stack_items.Children.Add(addNewItem(Global.reportItems[i].fee_name, Global.reportItems[i].fee_value[month - 1], Global.reportItems[i].calculate_type));
            }
            
        }

        private async void imgAdd_Clicked(object sender, EventArgs e)
        {
            //if (App.owner_type == "0")
            //{
            //    await DisplayAlert("", "決済して管理していない物件です。", "はい");
            //}
            //else
            //{
                PopupNavigation.Instance.PushAsync(new ConfirmReportAddPopup(current_month));
            //}

        }

        private async void imgBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void OnChildItemLayout_Tapped(object sender, EventArgs e)
        {
            Label child = (Label)sender;
            OnItemLayout_Tapped(child.Parent, e);
        }

        private async void OnItemLayout_Tapped(object sender, EventArgs e)
        {
            DateTime current_date = new DateTime(selected_year, current_month, 1);

            for (int i = 0; i < Global.reportItems.Count; i++)
            {
                
                if (Global.reportItems[i].fee_value[current_month-1] == get_value(i))
                {

                }
                else
                {
                    var update = await DisplayAlert("", "変更された値を保存しますか？", "確認", "キャンセル");
                    if (update)
                    {
                        using (var cl = new HttpClient())
                        {
                            List<dynamic> list = new List<dynamic>();
                            
                            string dynamic_data;
                            if (Global.reportItems.Count > 4)
                            {
                                for (int k = 4; k < Global.reportItems.Count; k++)
                                {
                                    dynamic item = new dynamic();

                                    item.dynamic_index = Global.reportItems[k].dynamic_index.ToString();
                                    item.dynamic_title = get_name(k);
                                    item.dynamic_value = get_value(k);
                                    item.calculation_type = Global.reportItems[k].calculate_type;

                                    list.Add(item);
                                }
                                dynamic_data = JsonConvert.SerializeObject(list);
                            }
                            else
                            {
                                dynamic_data = string.Empty;
                            }

                            var formcontent = new FormUrlEncodedContent(new[]
                            {
                                new KeyValuePair<string, string>("owner_id", App.owner_ID),
                                new KeyValuePair<string, string>("estate_id", Global.Buildings[building_index].building_id),
                                new KeyValuePair<string, string>("c_date", current_date.ToString("yyyy/M/dd")),
                                new KeyValuePair<string, string>("rent", get_value(0)),
                                new KeyValuePair<string, string>("admin_expense", get_value(1)),
                                new KeyValuePair<string, string>("agency_fee", get_value(2)),
                                new KeyValuePair<string, string>("repair_reserve", get_value(3)),
                                new KeyValuePair<string, string>("dynamic_data", dynamic_data)
                            });

                            try
                            {
                                var request = await cl.PostAsync(Constants.SERVER_BASE_URL + "set_calculation_info", formcontent);
                                request.EnsureSuccessStatusCode();
                                var response = await request.Content.ReadAsStringAsync();
                                ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                                if (resultMsg.resp.Equals("success"))
                                {

                                }
                                else
                                {
                                    await DisplayAlert("", resultMsg.resp, "はい");
                                }
                            }
                            catch (NullReferenceException ex)
                            {
                                var err = ex;
                                await DisplayAlert("", Constants.NETWORK_ERROR, "はい");
                            }
                        }

                        if (current_month < 12)
                        {
                            if (Global.reportItems[i].fee_value[current_month - 1] == Global.reportItems[i].fee_value[current_month])
                            {
                                using (var cl = new HttpClient())
                                {
                                    List<dynamic> list = new List<dynamic>();

                                    string dynamic_data;
                                    if (Global.reportItems.Count > 4)
                                    {
                                        for (int k = 4; k < Global.reportItems.Count; k++)
                                        {
                                            dynamic item = new dynamic();

                                            item.dynamic_index = Global.reportItems[k].dynamic_index.ToString();
                                            item.dynamic_title = get_name(k);
                                            item.dynamic_value = Global.MakeZero(Global.reportItems[k].fee_value[current_month - 1]);
                                            item.calculation_type = Global.reportItems[k].calculate_type;

                                            list.Add(item);
                                        }
                                        dynamic_data = JsonConvert.SerializeObject(list);
                                    }
                                    else
                                    {
                                        dynamic_data = string.Empty;
                                    }

                                    var formcontent = new FormUrlEncodedContent(new[]
                                    {
                                        new KeyValuePair<string, string>("owner_id", App.owner_ID),
                                        new KeyValuePair<string, string>("estate_id", Global.Buildings[building_index].building_id),
                                        new KeyValuePair<string, string>("c_date", current_date.AddMonths(1).ToString("yyyy/M/dd")),
                                        new KeyValuePair<string, string>("rent", Global.reportItems[0].fee_value[current_month - 1]),
                                        new KeyValuePair<string, string>("admin_expense", Global.reportItems[1].fee_value[current_month - 1]),
                                        new KeyValuePair<string, string>("agency_fee", Global.reportItems[2].fee_value[current_month - 1]),
                                        new KeyValuePair<string, string>("repair_reserve", Global.reportItems[3].fee_value[current_month - 1]),
                                        new KeyValuePair<string, string>("dynamic_data", dynamic_data)
                                    });

                                    try
                                    {
                                        var request = await cl.PostAsync(Constants.SERVER_BASE_URL + "set_calculation_info", formcontent);
                                        request.EnsureSuccessStatusCode();
                                        var response = await request.Content.ReadAsStringAsync();
                                        ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                                        if (resultMsg.resp.Equals("success"))
                                        {

                                        }
                                        else
                                        {
                                            await DisplayAlert("", resultMsg.resp, "はい");
                                        }
                                    }
                                    catch (NullReferenceException ex)
                                    {
                                        var err = ex;
                                        await DisplayAlert("", Constants.NETWORK_ERROR, "はい");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (_previousLayout != null)
            {
                _previousLayout.BackgroundColor = Color.Transparent;
            }

            if (_previousHighlightLayout != null)
            {
                _previousHighlightLayout.BackgroundColor = Color.Transparent;
            }

            var selectLayout = (StackLayout)sender;
            StackLayout selected_highlight = (StackLayout)selectLayout.Children[2];
            selected_highlight.BackgroundColor = Color.FromHex("F8B500");

            _previousLayout = selectLayout;
            _previousHighlightLayout = selected_highlight;

            Label selected_month = (Label)selectLayout.Children[1];
            current_month = Convert.ToInt32(selected_month.Text.Replace("月", string.Empty));
            
            Create_UI(current_month);
        }

        private async void ImgBtnSave_Clicked(object sender, EventArgs e)
        {
            DateTime current_date = new DateTime(selected_year, current_month, 1);

            for (int i = 0; i < Global.reportItems.Count; i++)
            {

                if (Global.reportItems[i].fee_value[current_month - 1] == get_value(i))
                {

                }
                else
                {
                    using (var cl = new HttpClient())
                    {
                        List<dynamic> list = new List<dynamic>();

                        string dynamic_data;
                        if (Global.reportItems.Count > 4)
                        {
                            for (int k = 4; k < Global.reportItems.Count; k++)
                            {
                                dynamic item = new dynamic();

                                item.dynamic_index = Global.reportItems[k].dynamic_index.ToString();
                                item.dynamic_title = get_name(k);
                                item.dynamic_value = get_value(k);
                                item.calculation_type = Global.reportItems[k].calculate_type;

                                list.Add(item);
                            }
                            dynamic_data = JsonConvert.SerializeObject(list);
                        }
                        else
                        {
                            dynamic_data = string.Empty;
                        }

                        var formcontent = new FormUrlEncodedContent(new[]
                        {
                            new KeyValuePair<string, string>("owner_id", App.owner_ID),
                            new KeyValuePair<string, string>("estate_id", Global.Buildings[building_index].building_id),
                            new KeyValuePair<string, string>("c_date", current_date.ToString("yyyy/M/dd")),
                            new KeyValuePair<string, string>("rent", get_value(0)),
                            new KeyValuePair<string, string>("admin_expense", get_value(1)),
                            new KeyValuePair<string, string>("agency_fee", get_value(2)),
                            new KeyValuePair<string, string>("repair_reserve", get_value(3)),
                            new KeyValuePair<string, string>("dynamic_data", dynamic_data)
                        });

                        try
                        {
                            var request = await cl.PostAsync(Constants.SERVER_BASE_URL + "set_calculation_info", formcontent);
                            request.EnsureSuccessStatusCode();
                            var response = await request.Content.ReadAsStringAsync();
                            ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                            if (resultMsg.resp.Equals("success"))
                            {

                            }
                            else
                            {
                                await DisplayAlert("", resultMsg.resp, "はい");
                            }
                        }
                        catch (NullReferenceException ex)
                        {
                            var err = ex;
                            await DisplayAlert("", Constants.NETWORK_ERROR, "はい");
                        }
                    }

                    if (current_month < 12)
                    {
                        if (Global.reportItems[i].fee_value[current_month - 1] == Global.reportItems[i].fee_value[current_month])
                        {
                            using (var cl = new HttpClient())
                            {
                                List<dynamic> list = new List<dynamic>();

                                string dynamic_data;
                                if (Global.reportItems.Count > 4)
                                {
                                    for (int k = 4; k < Global.reportItems.Count; k++)
                                    {
                                        dynamic item = new dynamic();

                                        item.dynamic_index = Global.reportItems[k].dynamic_index.ToString();
                                        item.dynamic_title = get_name(k);
                                        item.dynamic_value = Global.MakeZero(Global.reportItems[k].fee_value[current_month - 1]);
                                        item.calculation_type = Global.reportItems[k].calculate_type;

                                        list.Add(item);
                                    }
                                    dynamic_data = JsonConvert.SerializeObject(list);
                                }
                                else
                                {
                                    dynamic_data = string.Empty;
                                }

                                var formcontent = new FormUrlEncodedContent(new[]
                                {
                                    new KeyValuePair<string, string>("owner_id", App.owner_ID),
                                    new KeyValuePair<string, string>("estate_id", Global.Buildings[building_index].building_id),
                                    new KeyValuePair<string, string>("c_date", current_date.AddMonths(1).ToString("yyyy/M/dd")),
                                    new KeyValuePair<string, string>("rent", Global.reportItems[0].fee_value[current_month - 1]),
                                    new KeyValuePair<string, string>("admin_expense", Global.reportItems[1].fee_value[current_month - 1]),
                                    new KeyValuePair<string, string>("agency_fee", Global.reportItems[2].fee_value[current_month - 1]),
                                    new KeyValuePair<string, string>("repair_reserve", Global.reportItems[3].fee_value[current_month - 1]),
                                    new KeyValuePair<string, string>("dynamic_data", dynamic_data)
                                });

                                try
                                {
                                    var request = await cl.PostAsync(Constants.SERVER_BASE_URL + "set_calculation_info", formcontent);
                                    request.EnsureSuccessStatusCode();
                                    var response = await request.Content.ReadAsStringAsync();
                                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                                    if (resultMsg.resp.Equals("success"))
                                    {

                                    }
                                    else
                                    {
                                        await DisplayAlert("", resultMsg.resp, "はい");
                                    }
                                }
                                catch (NullReferenceException ex)
                                {
                                    var err = ex;
                                    await DisplayAlert("", Constants.NETWORK_ERROR, "はい");
                                }
                            }
                        }
                    }
                }
            }

            await Navigation.PopAsync();
        }
        
        private string get_value(int i)
        {
            var stack = stack_items.Children[i] as StackLayout;
            var grid = stack.Children[0] as Grid;
            var frame = grid.Children[0] as Frame;
            var sub_stack = frame.Content as StackLayout;

            var editor_new_item = sub_stack.Children[1] as BorderlessEditor;
            var item_value = editor_new_item.Text;

            return item_value;
        }

        private string get_name(int i)
        {
            var stack = stack_items.Children[i] as StackLayout;
            var grid = stack.Children[0] as Grid;
            var frame = grid.Children[0] as Frame;
            var sub_stack = frame.Content as StackLayout;

            var lbl_item_name = sub_stack.Children[0] as Label;
            var item_name = lbl_item_name.Text;

            return item_name;
        }

        public Xamarin.Forms.Color getColor(string type)
        {
            if (type == "0")
            {
                return Xamarin.Forms.Color.FromHex("#327BC4");
            }
            else
            {
                return Xamarin.Forms.Color.FromHex("#EA4343");
            }
        }

        public class dynamic
        {
            public string dynamic_index { get; set; }
            public string dynamic_title { get; set; }
            public string dynamic_value { get; set; }
            public string calculation_type { get; set;} 
        }
    }
}