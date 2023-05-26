using System.Collections.Generic;

namespace TStore.Shared.Models
{
    public class PagingListResponse<T>
    {
        public int Total { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
