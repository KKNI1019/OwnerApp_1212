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
    public class ThreadData
    {
        readonly SQLiteAsyncConnection _database;

        public ThreadData(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Threads>().Wait();
            //var connectionFunc = new Func<SQLiteConnectionWithLock>(() =>
            //    new SQLiteConnectionWithLock
            //    (
            //        sqlitePlatform,
            //        new SQLiteConnectionString(dbPath, storeDateTimeAsTicks: false)
            //    ));
            //_database = new SQLiteAsyncConnection(connectionFunc);
            //_database.CreateTableAsync<Threads>();
        }

        public Task<List<Threads>> GetThreadAsync()
        {
            return _database.Table<Threads>().ToListAsync();
        }

        public Task<Threads> GetSelectedThreadAsync(string th_id)
        {
            return _database.Table<Threads>()
                            .Where(i => i.Th_id == th_id)
                            .FirstOrDefaultAsync();
        }
        
        public Task<int> SaveThreadAsync(Threads thread)
        {
            if (thread.ID != 0)
            {
                return _database.UpdateAsync(thread);
            }
            else
            {
                return _database.InsertAsync(thread);
            }
        }

        public Task<int> DeleteThreadAsync(Threads thread)
        {
            return _database.DeleteAsync(thread);
        }

    }
}
