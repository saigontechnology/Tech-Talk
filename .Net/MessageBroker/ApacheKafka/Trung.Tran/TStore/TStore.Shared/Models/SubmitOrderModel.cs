using System;
using System.Collections.Generic;

namespace TStore.Shared.Models
{
    public class SubmitOrderModel
    {
        public string UserName { get; set; }
        public IEnumerable<Guid> ProductIds { get; set; }
    }
}
