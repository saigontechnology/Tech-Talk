##EF Core 7

|                Method |        Mean |     Error |    StdDev |   Gen0 |   Gen1 | Allocated |
|---------------------- |------------:|----------:|----------:|-------:|-------:|----------:|
| EFCore_FirstOrDefault |    77.22 us |  2.054 us |  6.024 us | 0.8545 | 0.1221 |   8.54 KB |
|         EFCore_Filter |   180.76 us |  3.603 us |  6.019 us | 6.8359 | 0.2441 |  64.13 KB |
|     EFCore_Add_Delete | 2,268.54 us | 44.977 us | 82.243 us | 3.9063 |      - |     37 KB |

##Dapper

|                     Method |      Mean   |    Error |    StdDev |   Gen0 |   Gen1 | Allocated |
|--------------------------- |------------:|---------:|----------:|-------:|-------:|----------:|
| Dapper_QueryFirstOrDefault |    42.56 us | 0.751 us |  1.053 us | 0.4272 |      - |   3.98 KB |
|              Dapper_Filter |    90.99 us | 2.319 us |  2.277 us | 9.2773 | 1.2207 |  36.04 KB |
|          Dapper_Add_Delete | 2,027.34 us | 6.578 us | 18.768 us |      - |      - |  10.95 KB |

