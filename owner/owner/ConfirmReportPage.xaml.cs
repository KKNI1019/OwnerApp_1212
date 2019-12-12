using owner.Model;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using System.IO;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System.Reflection;
using System.Net.Http;
using owner.WebService;
using Newtonsoft.Json;
using System.Data;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace owner
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmReportPage : ContentPage
    {
        private int current_year;
        private int building_index;

        public ConfirmReportPage()
        {
            InitializeComponent();

            DateTime date = DateTime.Now;
            current_year = date.Year;
            lbl_year.Text = $"{current_year}";
        }

        protected override async void OnAppearing()
        {
            if (Global.Buildings != null)
            {
                Create_ui(current_year, building_index);
            }
            else
            {
                stack_content.Children.Clear();
                await DisplayAlert("", "登録された不動産がありません。", "はい");
            }
            try
            {
                if (Global.Buildings.Count > 1)
                {
                    btn_next_building.IsVisible = true;
                }
            }
            catch { }

            base.OnAppearing();
        }

        private async void ImgBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void Select_year_SelectedItemChanged(object sender, Plugin.InputKit.Shared.Utils.SelectedItemChangedArgs e)
        {

        }

        private async void PDF_btn_Clicked(object sender, EventArgs e)
        {
            if (Global.Buildings[building_index].zero_status != "5")
            {
                await DisplayAlert("", "決済して管理していない物件です。", "はい");
            }
            else
            {
                //// Create a new PDF document
                PdfDocument document = new PdfDocument();
                document.PageSettings.Orientation = PdfPageOrientation.Landscape;

                PdfSection section1 = document.Sections.Add();
                section1.PageSettings.Size = PdfPageSize.A4;
                PdfPage page = section1.Pages.Add();

                //Add a page to the document
                //PdfPage page = document.Pages.Add();
                //page.Size = PdfPageSize.A4;

                //Create PDF graphics for the page
                PdfGraphics graphics = page.Graphics;
                Stream fontStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("owner.arialuni.TTF");
                //Stream fontStream = this.GetType().Assembly.GetManifestResourceStream("owner.Resources.arialuni.ttf");
                string[] resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();
                //Set the standard font
                //PdfFont font = new PdfStandardFont(PdfFontFamily.ZapfDingbats, 20);
                PdfTrueTypeFont title_font = new PdfTrueTypeFont(fontStream, 25);
                PdfTrueTypeFont normal_font = new PdfTrueTypeFont(fontStream, 16);
                PdfTrueTypeFont big_font = new PdfTrueTypeFont(fontStream, 20);
                PdfTrueTypeFont small_font = new PdfTrueTypeFont(fontStream, 10);

                //Draw the title_text
                graphics.DrawString("■ 確 定 申 告 資 料（ 収 支 明 細 書 ）■", title_font, PdfBrushes.Black, new PointF(150, 10));

                //Draw the owner_info_text
                graphics.DrawString("家主名", normal_font, PdfBrushes.Black, new PointF(10, 40));
                string owner_name = "金井";
                graphics.DrawString(owner_name, normal_font, PdfBrushes.Black, new PointF(70, 40));

                graphics.DrawString("物件名", normal_font, PdfBrushes.Black, new PointF(10, 60));
                string building_name = "レジデンシャル六本木";
                graphics.DrawString(building_name, normal_font, PdfBrushes.Black, new PointF(70, 60));

                string position = "北海道札幌市中央区南1条西1丁目";
                graphics.DrawString("所在地", normal_font, PdfBrushes.Black, new PointF(10, 80));
                graphics.DrawString(position, normal_font, PdfBrushes.Black, new PointF(70, 80));

                //Draw the income_info_text
                graphics.DrawString("【収入】", big_font, PdfBrushes.Blue, new PointF(8, 100));

                //Create a PdfGrid.
                PdfGrid income_Grid = new PdfGrid();

                //Add the columns

                income_Grid.Columns.Add(17);

                //Specify the style for the PdfGridCell.

                PdfGridCellStyle pdfGridCellStyle = new PdfGridCellStyle();

                pdfGridCellStyle.TextPen = PdfPens.White;
                pdfGridCellStyle.TextBrush = PdfBrushes.White;
                pdfGridCellStyle.BackgroundBrush = PdfBrushes.Blue;
                pdfGridCellStyle.Borders.All = PdfPens.Black;
                pdfGridCellStyle.Font = small_font;

                for (int i = 0; i < 8; i++)
                {
                    PdfGridRow row = income_Grid.Rows.Add();
                    row.Height = 15;
                    row.Cells[0].ColumnSpan = 2;
                    row.Cells[2].ColumnSpan = 2;

                    if (i == 0)
                    {
                        for (int j = 4; j < 17; j++)
                        {
                            if (j == 4)
                            {
                                row.Cells[j].Value = String.Format("{0}年{1}月", current_year - 2000, j - 3);
                            }
                            else if (j == 16)
                            {
                                row.Cells[j].Value = "合計";
                            }
                            else
                            {
                                row.Cells[j].Value = String.Format("{0}月", j - 3);
                            }

                            row.Cells[j].Style = pdfGridCellStyle;
                        }
                    }

                }

                income_Grid.Rows[1].Cells[0].RowSpan = 6;

                //Set the value to the specific cell.

                income_Grid.Rows[7].Cells[0].Value = "収入計";
                income_Grid.Rows[7].Cells[0].Style = pdfGridCellStyle;
                income_Grid.Rows[1].Cells[2].Value = "家賃";
                income_Grid.Rows[1].Cells[2].Style = pdfGridCellStyle;
                income_Grid.Rows[2].Cells[2].Value = "管理費";
                income_Grid.Rows[2].Cells[2].Style = pdfGridCellStyle;
                income_Grid.Rows[3].Cells[2].Value = "礼金";
                income_Grid.Rows[3].Cells[2].Style = pdfGridCellStyle;
                income_Grid.Rows[4].Cells[2].Value = "更新料";
                income_Grid.Rows[4].Cells[2].Style = pdfGridCellStyle;
                income_Grid.Rows[5].Cells[2].Value = "その他";
                income_Grid.Rows[5].Cells[2].Style = pdfGridCellStyle;
                income_Grid.Rows[6].Cells[2].Value = "小計";
                income_Grid.Rows[6].Cells[2].Style = pdfGridCellStyle;


                income_Grid.Draw(page, new PointF(10, 130));

                //Draw the outcome_info_text
                graphics.DrawString("【支出】", big_font, PdfBrushes.Blue, new PointF(8, 255));

                //Create a PdfGrid.
                PdfGrid outcome_Grid = new PdfGrid();

                //Add the columns

                outcome_Grid.Columns.Add(17);

                for (int i = 0; i < 13; i++)
                {
                    PdfGridRow row = outcome_Grid.Rows.Add();
                    row.Height = 15;
                    row.Cells[0].ColumnSpan = 2;
                    row.Cells[2].ColumnSpan = 2;

                    if (i == 0)
                    {
                        for (int j = 4; j < 17; j++)
                        {
                            if (j == 4)
                            {
                                row.Cells[j].Value = String.Format("{0}年{1}{2}", current_year - 2000, j - 3, "月");
                            }
                            else if (j == 16)
                            {
                                row.Cells[j].Value = "合計";
                            }
                            else
                            {
                                row.Cells[j].Value = String.Format("{0}月", j - 3);
                            }

                            row.Cells[j].Style = pdfGridCellStyle;
                        }
                    }

                }

                outcome_Grid.Rows[1].Cells[0].RowSpan = 11;

                //Set the value to the specific cell.

                outcome_Grid.Rows[12].Cells[0].Value = "支出計";
                outcome_Grid.Rows[12].Cells[0].Style = pdfGridCellStyle;
                outcome_Grid.Rows[1].Cells[2].Value = "建物管理費";
                outcome_Grid.Rows[1].Cells[2].Style = pdfGridCellStyle;
                outcome_Grid.Rows[2].Cells[2].Value = "管理料";
                outcome_Grid.Rows[2].Cells[2].Style = pdfGridCellStyle;
                outcome_Grid.Rows[3].Cells[2].Value = "送金料";
                outcome_Grid.Rows[3].Cells[2].Style = pdfGridCellStyle;
                outcome_Grid.Rows[4].Cells[2].Value = "新 管理料";
                outcome_Grid.Rows[4].Cells[2].Style = pdfGridCellStyle;
                outcome_Grid.Rows[5].Cells[2].Value = "印 代";
                outcome_Grid.Rows[5].Cells[2].Style = pdfGridCellStyle;
                outcome_Grid.Rows[6].Cells[2].Value = "更新手数料";
                outcome_Grid.Rows[6].Cells[2].Style = pdfGridCellStyle;
                outcome_Grid.Rows[7].Cells[2].Value = "健交損代";
                outcome_Grid.Rows[7].Cells[2].Style = pdfGridCellStyle;
                outcome_Grid.Rows[8].Cells[2].Value = "内装代";
                outcome_Grid.Rows[8].Cells[2].Style = pdfGridCellStyle;
                outcome_Grid.Rows[9].Cells[2].Value = "立替代";
                outcome_Grid.Rows[9].Cells[2].Style = pdfGridCellStyle;
                outcome_Grid.Rows[10].Cells[2].Value = "都理費";
                outcome_Grid.Rows[10].Cells[2].Style = pdfGridCellStyle;
                outcome_Grid.Rows[11].Cells[2].Value = "その他";
                outcome_Grid.Rows[11].Cells[2].Style = pdfGridCellStyle;
                outcome_Grid.Rows[12].Cells[2].Value = "小計";
                outcome_Grid.Rows[12].Cells[2].Style = pdfGridCellStyle;

                outcome_Grid.Draw(page, new PointF(10, 285));

                //Draw the balance_info_text
                graphics.DrawString("【収支】", big_font, PdfBrushes.Blue, new PointF(8, 485));

                //Create a PdfGrid.
                PdfGrid balance_Grid = new PdfGrid();

                //Add the columns

                balance_Grid.Columns.Add(17);

                for (int i = 0; i < 2; i++)
                {
                    PdfGridRow row = balance_Grid.Rows.Add();
                    row.Height = 15;
                    row.Cells[0].ColumnSpan = 2;
                    row.Cells[2].ColumnSpan = 2;

                    if (i == 0)
                    {
                        for (int j = 4; j < 17; j++)
                        {
                            if (j == 4)
                            {
                                row.Cells[j].Value = String.Format("{0}年{1}月", current_year - 2000, j - 3);
                            }
                            else if (j == 16)
                            {
                                row.Cells[j].Value = "合計";
                            }
                            else
                            {
                                row.Cells[j].Value = String.Format("{0}月", j - 3);
                            }

                            row.Cells[j].Style = pdfGridCellStyle;
                        }
                    }

                }

                //Set the value to the specific cell.

                balance_Grid.Rows[1].Cells[0].Value = "収支計";
                balance_Grid.Rows[1].Cells[0].Style = pdfGridCellStyle;


                balance_Grid.Draw(page, new PointF(10, 505));

                //Save the document to the stream
                MemoryStream stream = new MemoryStream();
                document.Save(stream);

                //Close the document
                document.Close(true);

                //Save the stream as a file in the device and invoke it for viewing
                DependencyService.Get<ISave>().SaveAndView("Output.pdf", "application/pdf", stream);
                //Process.Start("Output.pdf");
            }
        }

        private async void Declare_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ConfirmReportAddPage(current_year, building_index));
        }

        private async void Previous_btn_Clicked(object sender, EventArgs e)
        {
            if (Global.Buildings[building_index].zero_status != "5")
            {
                await DisplayAlert("", "決済して管理していない物件です。", "はい");
            }
            else
            {
                if (Convert.ToInt32(lbl_year.Text) >= current_year)
                {
                    current_year--;
                }
                lbl_year.Text = $"{current_year}";

                Create_ui(current_year, building_index);
            }
        }

        private async void Next_btn_Clicked(object sender, EventArgs e)
        {
            if (Global.Buildings[building_index].zero_status != "5")
            {
                await DisplayAlert("", "決済して管理していない物件です。", "はい");
            }
            else
            {
                if (Convert.ToInt32(lbl_year.Text) <= current_year)
                {
                    current_year++;
                }
                lbl_year.Text = $"{current_year}";

                Create_ui(current_year, building_index);
            }
        }

        private async void Create_ui(int selected_year, int index)
        {
            if (index == 0)
            {
                btn_previous_building.IsVisible = false;
            }
            else if (index == Global.Buildings.Count - 1)
            {
                btn_next_building.IsVisible = false;
            }

            lbl_building_name.Text = Global.Buildings[index].building_name;

            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("owner_id", App.owner_ID),
                    new KeyValuePair<string, string>("estate_id", Global.Buildings[index].building_id),
                    new KeyValuePair<string, string>("year", current_year.ToString())
                });

                try
                {
                    var request = await cl.PostAsync(Constants.SERVER_BASE_URL + "get_calculation_info", formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        ResponseData resultData = JsonConvert.DeserializeObject<ResponseData>(response);

                        var stack_header = new StackLayout()
                        {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            BackgroundColor = Xamarin.Forms.Color.Transparent,
                        };

                        var lbl_title = new Label { TextColor = Xamarin.Forms.Color.Black, FontSize = 12, WidthRequest = 90 };
                        stack_header.Children.Add(lbl_title);

                        for (int j = 0; j < 12; j++)
                        {
                            var lbl_header = new Label { HorizontalTextAlignment = TextAlignment.Center, TextColor = Xamarin.Forms.Color.Black, FontSize = 12, WidthRequest = 60, Text = (j + 1).ToString() + "月", HorizontalOptions = LayoutOptions.CenterAndExpand };
                            stack_header.Children.Add(lbl_header);
                        }

                        var lbl_sum = new Label { HorizontalTextAlignment = TextAlignment.Center, TextColor = Xamarin.Forms.Color.Black, FontSize = 12, WidthRequest = 80, Text = "合計", HorizontalOptions = LayoutOptions.CenterAndExpand };
                        stack_header.Children.Add(lbl_sum);

                        var stk_contents = new StackLayout() { Orientation = StackOrientation.Vertical };
                        stk_contents.Children.Add(stack_header);

                        Global.reportItems = new List<ReportItems>();
                        int data_num = resultData.calculation_data.Length;
                        if (data_num > 4)
                        {
                            Preferences.Set("newitem_id", Convert.ToInt32(resultData.calculation_data[data_num - 1].dynamic_index));
                        }

                        for (int i = 0; i < data_num; i++)
                        {
                            Global.reportItems.Add(new ReportItems
                            {
                                dynamic_index = resultData.calculation_data[i].dynamic_index,
                                fee_name = resultData.calculation_data[i].fee_name,
                                fee_value = resultData.calculation_data[i].fee_value,
                                calculate_type = resultData.calculation_data[i].calculation_type
                            });

                            var frame = new Frame() { BorderColor = Xamarin.Forms.Color.DarkGray, HasShadow = false, Padding = new Thickness(2, 5) };

                            var substack = new StackLayout() { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center };


                            var grid = new Grid() { WidthRequest = 90, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center };
                            var img_month = new Xamarin.Forms.Image() { WidthRequest = 90, Aspect = Aspect.Fill, Source = "img_month_back.png", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
                            var lbl_fee = new Label() { Text = resultData.calculation_data[i].fee_name, HorizontalTextAlignment = TextAlignment.Start, TextColor = Xamarin.Forms.Color.Black, FontSize = 12, HorizontalOptions = LayoutOptions.Start };
                            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0, GridUnitType.Star) });
                            grid.Children.Add(img_month, 0, 0);
                            grid.Children.Add(lbl_fee, 0, 0);

                            substack.Children.Add(grid);

                            int total = 0;
                            for (int k = 0; k < 12; k++)
                            {
                                var lbl_values = new Label { HorizontalTextAlignment = TextAlignment.Center, TextColor = getColor(resultData.calculation_data[i].calculation_type), FontSize = 12, WidthRequest = 60, Text = resultData.calculation_data[i].fee_value[k], HorizontalOptions = LayoutOptions.CenterAndExpand };
                                var lbl_slash = new Label() { Text = "/", Margin = new Thickness(-6, 0), FontSize = 12 };
                                substack.Children.Add(lbl_values);
                                substack.Children.Add(lbl_slash);
                                total += Convert.ToInt32(resultData.calculation_data[i].fee_value[k]);
                            }

                            var lbl_total = new Label { HorizontalTextAlignment = TextAlignment.Center, TextColor = Xamarin.Forms.Color.Black, FontSize = 12, WidthRequest = 80, Text = total.ToString(), HorizontalOptions = LayoutOptions.CenterAndExpand };
                            substack.Children.Add(lbl_total);

                            frame.Content = substack;
                            stk_contents.Children.Add(frame);
                        }

                        var scrview = new ScrollView()
                        {
                            Orientation = ScrollOrientation.Both,
                            Content = stk_contents,
                            Padding = new Thickness(0, 0, 0, 10),
                            VerticalOptions = LayoutOptions.Center
                        };

                        stack_show_list.Children.Clear();
                        stack_show_list.Children.Add(scrview);

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

        private void Btn_previous_building_Clicked(object sender, EventArgs e)
        {
            btn_next_building.IsVisible = true;
            building_index--;
            if (building_index < 1)
            {
                btn_previous_building.IsVisible = false;
            }
            Create_ui(current_year, building_index);
        }

        private void Btn_next_building_Clicked(object sender, EventArgs e)
        {
            btn_previous_building.IsVisible = true;
            building_index++;
            if (building_index == Global.Buildings.Count - 1)
            {
                btn_next_building.IsVisible = false;
            }
            Create_ui(current_year, building_index);
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
    }
}