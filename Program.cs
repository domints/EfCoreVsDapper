using BenchmarkDotNet.Running;

namespace EfCoreVsDapper;

internal static class Program
{
    private static async Task Main()
    {
        BenchmarkRunner.Run<BenchmarksWithIndex>();
        BenchmarkRunner.Run<BenchmarksNoIndex>();
    }
}