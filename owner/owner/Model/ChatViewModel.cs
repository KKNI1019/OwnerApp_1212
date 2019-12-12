using owner.WebService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Essentials;

namespace owner.Model
{
    public class ChatViewModel
    {
        public ObservableCollection<ChatModel> Data { get; set; }
        public ObservableCollection<ChatModel> Data1 { get; set; }

        public ObservableCollection<ChatModel> Data2 { get; set; }
        public ObservableCollection<ChatModel> Data3 { get; set; }
        public ObservableCollection<ChatModel> Data4 { get; set; }

        public ChatViewModel()
        {
            
            Data = new ObservableCollection<ChatModel>();
            Data1 = new ObservableCollection<ChatModel>();

            int agency_fee = Convert.ToInt32(App.new_agency_fee);
            int budi_fee = App.programm_fee;
            for (int i = 2019; i <= 2025; i++)
            {
                Data.Add(new ChatModel(i, agency_fee));
                Data1.Add(new ChatModel(i, budi_fee));
            }
        }
    }
}
