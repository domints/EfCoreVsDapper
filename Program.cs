using BenchmarkDotNet.Running;

namespace EfCoreVsDapper;

internal static class Program
{
    private static async Task Main()
    {
        BenchmarkRunner.Run(new[] { /*typeof(BenchmarksNoIndex), */typeof(BenchmarksWithIndex) });
    }
}