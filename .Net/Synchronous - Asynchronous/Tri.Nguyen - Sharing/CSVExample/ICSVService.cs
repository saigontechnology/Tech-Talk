namespace CSVExample
{
    public interface ICSVService
    {
        public IEnumerable<T> ReadCSV<T>(Stream file);
        public IAsyncEnumerable<T> ReadCSVAsync<T>(Stream file);
    }
}
