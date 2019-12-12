using owner.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace owner.DB
{
    public class FavoriteNewsDB
    {
        readonly SQLiteAsyncConnection _database;

        public FavoriteNewsDB(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            try
            {
                _database.CreateTableAsync<FavoriteNews>().Wait();
            }
            catch (AggregateException ex)
            {
                string ss = ex.Message.ToString();
            }
            
        }

        public Task<List<FavoriteNews>> GetNewsAsync()
        {
            return _database.Table<FavoriteNews>().ToListAsync();
        }

        public Task<FavoriteNews> GetSelectedNewsAsync(string favorite_news_id)
        {
            return _database.Table<FavoriteNews>()
                            .Where(i => i.news_id == favorite_news_id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveNewsAsync(FavoriteNews news)
        {
            if (news.ID != 0)
            {
                return _database.UpdateAsync(news);
            }
            else
            {
                return _database.InsertAsync(news);
            }
        }

        public Task<int> DeleteNewsAsync(FavoriteNews news)
        {
            return _database.DeleteAsync(news);
        }
    }
}
