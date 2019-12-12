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
    public class ThreadCommentData
    {
        readonly SQLiteAsyncConnection _database;

        public ThreadCommentData(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Thread_Comments>().Wait();
            //var connectionFunc = new Func<SQLiteConnectionWithLock>(() =>
            //    new SQLiteConnectionWithLock
            //    (
            //        sqlitePlatform,
            //        new SQLiteConnectionString(dbPath, storeDateTimeAsTicks: false)
            //    ));
            //_database = new SQLiteAsyncConnection(connectionFunc);
            //_database.CreateTableAsync<Thread_Comments>();
        }

        public Task<List<Thread_Comments>> GetTh_commentAsync(string selected_th_id)
        {
            return _database.Table<Thread_Comments>()
                .Where(i => i.Th_id == selected_th_id)
                .ToListAsync();
        }

        public Task<Thread_Comments> GetSelectedTh_commentAsync(string th_comment_id)
        {
            return _database.Table<Thread_Comments>()
                            .Where(i => i.Th_comment_id == th_comment_id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveTh_CommentAsync(Thread_Comments th_comment)
        {
            if (th_comment.ID != 0)
            {
                return _database.UpdateAsync(th_comment);
            }
            else
            {
                return _database.InsertAsync(th_comment);
            }
        }

        public Task<int> DeleteTh_commentAsync(Thread_Comments th_comment)
        {
            return _database.DeleteAsync(th_comment);
        }
    }
}
