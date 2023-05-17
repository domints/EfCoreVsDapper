using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace EfCoreVsDapper
{
    [MemoryDiagnoser(false)]
    public class BenchmarksNoIndex
    {
        const string DatabaseFile = "./movies.db";
        private MovieContext _movieContext = null!;
        private MovieGenerator _movieGenerator = null!;
        private Random _random = null!;
        private DapperMovieConnection _movieConnection = null!;
        private Movie _testMovie = null!;

        [GlobalSetup]
        public async Task Setup()
        {
            if (File.Exists(DatabaseFile))
                File.Delete(DatabaseFile);

            _random = new Random(42069);
            _movieConnection = new(DatabaseFile);
            _movieContext = new MovieContext(DatabaseFile);
            _movieGenerator = new MovieGenerator(_movieConnection, _random);
            await _movieGenerator.EnsureDatabaseExists(false);

            _testMovie = await _movieGenerator.GenerateMovies(100);
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            if (File.Exists(DatabaseFile))
                File.Delete(DatabaseFile);
        }

        [Benchmark]
        public async Task<Movie?> EF_SimpleSingleOrDefault()
        {
            return await _movieContext.Movies.SingleOrDefaultAsync(m => m.Id == _testMovie.Id);
        }

        private static readonly Func<MovieContext, Guid, Task<Movie?>> Compiled_SingleOrDefault =
            EF.CompileAsyncQuery((MovieContext context, Guid id) => context.Movies.SingleOrDefault(m => m.Id == id));

        [Benchmark]
        public async Task<Movie?> EF_Compiled_SingleOrDefault()
        {
            return await Compiled_SingleOrDefault(_movieContext, _testMovie.Id);
        }

        [Benchmark]
        public async Task<Movie?> EF_SimpleFirstOrDefault()
        {
            return await _movieContext.Movies.FirstOrDefaultAsync(m => m.Id == _testMovie.Id);
        }

        private static readonly Func<MovieContext, Guid, Task<Movie?>> Compiled_FirstOrDefault =
            EF.CompileAsyncQuery((MovieContext context, Guid id) => context.Movies.FirstOrDefault(m => m.Id == id));

        [Benchmark]
        public async Task<Movie?> EF_Compiled_FirstOrDefault()
        {
            return await Compiled_FirstOrDefault(_movieContext, _testMovie.Id);
        }

        [Benchmark]
        public async Task<Movie?> EF_AsNoTracking_SingleOrDefault()
        {
            return await _movieContext.Movies.AsNoTracking().SingleOrDefaultAsync(m => m.Id == _testMovie.Id);
        }

        private static readonly Func<MovieContext, Guid, Task<Movie?>> Compiled_AsNoTracking_SingleOrDefault =
            EF.CompileAsyncQuery((MovieContext context, Guid id) => context.Movies.SingleOrDefault(m => m.Id == id));

        [Benchmark]
        public async Task<Movie?> EF_Compiled_AsNoTracking_SingleOrDefault()
        {
            return await Compiled_AsNoTracking_SingleOrDefault(_movieContext, _testMovie.Id);
        }

        [Benchmark]
        public async Task<Movie?> EF_AsNoTracking_FirstOrDefault()
        {
            return await _movieContext.Movies.AsNoTracking().FirstOrDefaultAsync(m => m.Id == _testMovie.Id);
        }

        private static readonly Func<MovieContext, Guid, Task<Movie?>> Compiled_AsNoTracking_FirstOrDefault =
            EF.CompileAsyncQuery((MovieContext context, Guid id) => context.Movies.FirstOrDefault(m => m.Id == id));

        [Benchmark]
        public async Task<Movie?> EF_Compiled_AsNoTracking_FirstOrDefault()
        {
            return await Compiled_AsNoTracking_FirstOrDefault(_movieContext, _testMovie.Id);
        }

        [Benchmark]
        public async Task<Movie?> Dapper_GetById()
        {
            return await _movieConnection.QuerySingleOrDefaultAsync<Movie>("select * from Movies where Id=@Id limit 1", new { Id = _testMovie.Id });
        }
    }
}