using owner.Model;
using SQLite;
//using SQLite.Net;
//using SQLite.Net.Async;
//using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace owner.DB
{
    public class NoticeData
    {
        readonly SQLiteAsyncConnection _database;

        public NoticeData(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Notifications>().Wait();
            //var connectionFunc = new Func<SQLiteConnectionWithLock>(() =>
            //        new SQLiteConnectionWithLock
            //        (
            //            sqlitePlatform,
            //            new SQLiteConnectionString(dbPath, storeDateTimeAsTicks: false)
            //        ));
            //_database = new SQLiteAsyncConnection(connectionFunc);
            //_database.CreateTableAsync<Notifications>();
        }

        public Task<List<Notifications>> GetNotiAsync()
        {
            return _database.Table<Notifications>().OrderByDescending(a => a.ID).ToListAsync();
        }

        public Task<int> SaveNotiAsync(Notifications notifcation)
        {
            if (notifcation.ID != 0)
            {
                return _database.UpdateAsync(notifcation);
            }
            else
            {
                return _database.InsertAsync(notifcation);
            }
        }

        public Task<int> DeleteNotiAsync(Notifications notifcation)
        {
            return _database.DeleteAsync(notifcation);
        }
    }
}
