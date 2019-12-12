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
    public class ColumnData
    {
        readonly SQLiteAsyncConnection _database;

        public ColumnData(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Columns>().Wait();
            //var connectionFunc = new Func<SQLiteConnectionWithLock>(() =>
            //    new SQLiteConnectionWithLock
            //    (
            //        sqlitePlatform,
            //        new SQLiteConnectionString(dbPath, storeDateTimeAsTicks: false)
            //    ));
            //_database = new SQLiteAsyncConnection(connectionFunc);
            //_database.CreateTableAsync<Columns>();
        }

        public Task<List<Columns>> GetColumnAsync()
        {
            return _database.Table<Columns>().ToListAsync();
        }

        public Task<Columns> GetDelColumnAsync(string column_id)
        {
            return _database.Table<Columns>()
                            .Where(i => i.column_id == column_id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveColumnAsync(Columns column)
        {
            if (column.ID != 0)
            {
                return _database.UpdateAsync(column);
            }
            else
            {
                return _database.InsertAsync(column);
            }
        }

        public Task<int> DeletecolumnAsync(Columns column)
        {
            return _database.DeleteAsync(column);
        }
    }
}
