using CsvHelper;
using System.Globalization;

namespace CSVExample
{
    public class CSVService : ICSVService
    {
        public IEnumerable<T> ReadCSV<T>(Stream file)
        {
            var reader = new StreamReader(file);
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<T>();
            return records;
        }

        public IAsyncEnumerable<T> ReadCSVAsync<T>(Stream file)
        {
            var reader = new StreamReader(file);
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            return csv.GetRecordsAsync<T>();
        }
    }
}
