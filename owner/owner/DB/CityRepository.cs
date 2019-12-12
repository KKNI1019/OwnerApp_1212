using System;
using System.Collections.Generic;
using System.Linq;
using owner.DB;
//using SQLite.Net;
//using SQLite.Net.Interop;
//using SQLite.Net.Async;
using System.Threading.Tasks;
using SQLite;

namespace owner.DB
{
    public class CityRepository
    {
        private SQLiteAsyncConnection dbConn;

        public string StatusMessage { get; set; }

        public CityRepository(string dbPath)
        {
            dbConn = new SQLiteAsyncConnection(dbPath);
            dbConn.CreateTableAsync<JP_City>().Wait();
            //initialize a new SQLiteConnection 
            //if (dbConn == null)
            //{
            //    var connectionFunc = new Func<SQLiteConnectionWithLock>(() =>
            //        new SQLiteConnectionWithLock
            //        (
            //            sqlitePlatform,
            //            new SQLiteConnectionString(dbPath, storeDateTimeAsTicks: false)
            //        ));

            //    dbConn = new SQLiteAsyncConnection(connectionFunc);
            //    dbConn.CreateTableAsync<JP_City>();
            //}
        }

        public async Task AddNewCityAsync(string name)
        {
            int result = 0;
            try
            {
                //basic validation to ensure a name was entered
                if (string.IsNullOrEmpty(name))
                    throw new Exception("Valid name required");

                //insert a new person into the Person table
                result = await dbConn.InsertAsync(new JP_City { field2 = name });
                StatusMessage = string.Format("{0} record(s) added [field2: {1})", result, name);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", name, ex.Message);
            }

        }

        public async Task<List<JP_City>> GetAllPeopleAsync()
        {
            //return a list of people saved to the Person table in the database
            List<JP_City> cities = await dbConn.Table<JP_City>().ToListAsync();
            return cities;
        }

        public async Task<List<JP_City>> GetCitiesAsync(string state_name)
        {
            //List<JP_City> cities = await dbConn.QueryAsync<JP_City>("select field2 from owner_city" + " where field1 = @name", new { state_name });
            List<JP_City> cities = await dbConn.Table<JP_City>()
                                                .Where(i=> i.field1 == state_name)
                                                .ToListAsync();
            return cities;
        }
    }
}