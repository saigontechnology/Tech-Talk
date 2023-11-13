| Method                          |      Mean |     Error |    StdDev |    Median |  Gen 0 | Allocated |
|---------------------------------|----------:|----------:|----------:|----------:|-------:|----------:|
| Log_WithoutIf_Information       | 73.827 ns | 1.0062 ns | 0.9412 ns | 74.066 ns | 0.0069 |      88 B |
| Log_WithIf_Information              |  4.856 ns | 0.0751 ns | 0.0627 ns |  4.839 ns |      - |         - |
| LogAdapter_Information              | 11.960 ns | 0.2629 ns | 0.5189 ns | 11.885 ns |      - |         - |
| LoggerMessage_SourceGen_Warning | 25.405 ns | 0.5300 ns | 1.3680 ns | 24.819 ns |      - |         - |
