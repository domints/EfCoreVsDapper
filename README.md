Code inspired by Nick Chapsas's video: https://www.youtube.com/watch?v=OxqAUIYemMs

I've written it from scratch, using his ideas, added check for `.AsNoTracking()` (which ended up being slower o.O)

Run by executing `dotnet run -c Release`

Results on my machine: 
|                                   Method |      Mean |    Error |   StdDev | Allocated |
|----------------------------------------- |----------:|---------:|---------:|----------:|
|                 EF_SimpleSingleOrDefault | 125.39 us | 1.582 us | 1.403 us |   9.96 KB |
|              EF_Compiled_SingleOrDefault |  90.84 us | 1.216 us | 1.138 us |    5.9 KB |
|                  EF_SimpleFirstOrDefault | 124.80 us | 2.179 us | 1.932 us |   9.96 KB |
|               EF_Compiled_FirstOrDefault |  90.53 us | 1.754 us | 2.020 us |    5.9 KB |
|          EF_AsNoTracking_SingleOrDefault | 129.09 us | 2.556 us | 2.266 us |  10.66 KB |
| EF_Compiled_AsNoTracking_SingleOrDefault |  89.79 us | 1.389 us | 1.300 us |    5.9 KB |
|           EF_AsNoTracking_FirstOrDefault | 128.82 us | 0.959 us | 0.897 us |  10.66 KB |
|  EF_Compiled_AsNoTracking_FirstOrDefault |  90.08 us | 1.177 us | 1.101 us |    5.9 KB |
|                           Dapper_GetById |  70.89 us | 0.708 us | 0.662 us |   2.21 KB |