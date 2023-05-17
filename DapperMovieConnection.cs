using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace EfCoreVsDapper
{
    public class DapperMovieConnection : SqliteConnection
    {
        public DapperMovieConnection(string dbPath) : base($"Data Source={dbPath}")
        {
        }
    }
}