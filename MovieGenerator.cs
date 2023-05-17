using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace EfCoreVsDapper
{
    public class MovieGenerator
    {
        private readonly Random _random;
        private readonly DapperMovieConnection _connection;

        public MovieGenerator(DapperMovieConnection connection, Random random)
        {
            _random = random;
            _connection = connection;
        }

        public async Task EnsureDatabaseExists(bool addIndex)
        {
            SqlMapper.AddTypeHandler(new GuidHandler());
            var table = await _connection.QueryAsync<string>("SELECT name FROM sqlite_master WHERE type='table' AND name = 'Movies';");
            var tableName = table.FirstOrDefault();
            if (!string.IsNullOrEmpty(tableName) && tableName == "Movies")
            {
                await _connection.ExecuteAsync("drop table Movies");
            }
 
            await _connection.ExecuteAsync("create table Movies (" +
                "Id NVARCHAR(36) NOT NULL," +
                "Name VARCHAR(100) NOT NULL," +
                "ReleaseYear INTEGER NOT NULL);");
            if (addIndex)
                await _connection.ExecuteAsync("create unique index Movies_Id_ix on Movies(Id)");
        }

        public async Task<Movie> GenerateMovies(int numberOfMovies)
        {
            var testMovieIx = _random.Next(numberOfMovies);
            var movies = Enumerable.Range(0, numberOfMovies).Select(_ => GetRandomMovie()).ToList();
            await _connection.ExecuteAsync("insert into Movies (Id, Name, ReleaseYear) values (@Id, @Name, @ReleaseYear)", movies);
            var testMovie = movies[testMovieIx];
            return testMovie;
        }

        private Movie GetRandomMovie()
        {
            var releaseYear = _random.Next(1925, 2025);
            var nameLength = _random.Next(1, 100);
            var name = new string(Enumerable.Range(0, nameLength).Select(_ => (char)_random.Next(0x20, 0x7F)).ToArray());
            return new Movie
            {
                Id = Guid.NewGuid(),
                ReleaseYear = releaseYear,
                Name = name
            };
        }
    }
}