using Newtonsoft.Json;
using owner.Model;
using owner.WebService;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace owner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FAQPage : ContentPage
	{
        private ObservableCollection<MessageItem> msgItem { get; set; }

        private string conversationID;
        private string token;
        private HttpClient _httpClient;
        private string user_image;
        public FAQPage ()
		{
			InitializeComponent ();
            msgItem = new ObservableCollection<MessageItem>();

            listview.ItemSelected += DeselectItem;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            conversationID = await getConversationId();
        }

        public void DeselectItem(object sender, EventArgs e)
        {
            ((Xamarin.Forms.ListView)sender).SelectedItem = null;
        }

        private void btnMenu_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new SideMenuPopup());
        }

        private async void Btn_post_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ent_question.Text))
            {
                if (string.IsNullOrEmpty(App.owner_profile))
                {
                    user_image = "imgUser.png";
                }
                else
                {
                    user_image = Constants.IMAGE_UPLOAD_URL_PREFIX + App.owner_profile;
                }
                msgItem.Add(new MessageItem
                {
                    imgUser = user_image,
                    UserQuestion = ent_question.Text,
                    botFrameVisibility = false,
                    userFrameVisibility = true,
                    imgUserVisibility = true,
                    imgBotVisibility = false
                });

                listview.ItemsSource = msgItem;

                List<MessageItem> msgList = ((IEnumerable<MessageItem>)this.listview.ItemsSource).ToList();
                listview.ScrollTo(msgList[msgList.Count - 1], ScrollToPosition.End, true);

                var messageToSend = new BotMessage() { From = App.owner_nickname, Text = ent_question.Text };
                ent_question.Text = string.Empty;
                var contentPost = new StringContent(JsonConvert.SerializeObject(messageToSend), Encoding.UTF8, "application/json");
                var coid = conversationID;
                var conversationUrl = "https://directline.botframework.com/api/conversations/" + conversationID + "/messages/";

                var response = await _httpClient.PostAsync(conversationUrl, contentPost);

                var messagesReceived = await _httpClient.GetAsync(conversationUrl);
                var messagesReceivedData = await messagesReceived.Content.ReadAsStringAsync();
                var messagesRoot = JsonConvert.DeserializeObject<BotMessageRoot>(messagesReceivedData);
                var messages = messagesRoot.Messages;

                var renewUrl = "https://directline.botframework.com/api/tokens/" + conversationID + "/renew/";
                response = await _httpClient.GetAsync(renewUrl);
                try
                {
                    var botMessage = messages.Last().Text;
                    if (botMessage == "No QnA Maker answers were found.")
                    {
                        botMessage = "※専用アプリにご加入いただきます。アプリ利用料金（物件一戸数月額500円）。\n更新料がある物件の場合は更新料の半月分を更新代行手数料としてお支払いいただきます。";
                    }

                    msgItem.Add(new MessageItem
                    {
                        imgBot = "imgRobot.png",
                        BotAnswer = botMessage,
                        userFrameVisibility = false,
                        botFrameVisibility = true,
                        imgBotVisibility = true,
                        imgUserVisibility = false
                    });

                    listview.ItemsSource = msgItem;

                    List<MessageItem> mmlist = ((IEnumerable<MessageItem>)this.listview.ItemsSource).ToList();
                    listview.ScrollTo(mmlist[mmlist.Count - 1], ScrollToPosition.End, true);
                }
                catch { }
            }
        }
        private async Task<string> getConversationId()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://directline.botframework.com/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "UGjvpZZtc1Q.DBxWuqC9JFaJblY59mjvtOc3cZj4RVPms98k5lUNacQ");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "DiXEYpVouk8.vEnWFaAmnxqrErqsewOePI73TnlphY5MAuTYFKJkz6c");
            var response = await _httpClient.PostAsync("/api/tokens/conversation", null);
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync();
                token = JsonConvert.DeserializeObject<string>(result.Result);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await _httpClient.PostAsync("/api/conversations", null);
                if (response.IsSuccessStatusCode)
                {
                    var conversationInfo = await response.Content.ReadAsStringAsync();
                    var conversationId = JsonConvert.DeserializeObject<Conversation>(conversationInfo).ConversationId;

                    return conversationId;
                }

            }

            return null;
        }
    }

   
}