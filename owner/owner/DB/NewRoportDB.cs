using owner.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace owner.DB
{
    public class NewRoportDB
    {
        readonly SQLiteAsyncConnection _database;

        public NewRoportDB(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            try
            {
                _database.CreateTableAsync<NewReportItem>().Wait();
            }
            catch (AggregateException ex)
            {
                string ss = ex.Message.ToString();
            }

        }

        public Task<List<NewReportItem>> GetReportAsync()
        {
            return _database.Table<NewReportItem>().ToListAsync();
        }

        public Task<NewReportItem> GetSelectedReportAsync(string item_index)
        {
            return _database.Table<NewReportItem>()
                            .Where(i => i.item_index == item_index)
                            .FirstOrDefaultAsync();
        }

        public Task<NewReportItem> GetshownReportAsync(string item_name)
        {
            return _database.Table<NewReportItem>()
                            .Where(i => i.item_name == item_name)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveReportAsync(NewReportItem newItem)
        {
            if (newItem.ID != 0)
            {
                return _database.UpdateAsync(newItem);
            }
            else
            {
                return _database.InsertAsync(newItem);
            }
        }

        public Task<int> DeleteReportAsync(NewReportItem newItem)
        {
            
            return _database.DeleteAsync(newItem);
        }
    }
}
