using System.ComponentModel;

namespace BasicLinQ.Models.Product
{
    public class ProductRequestModel
    {
        public string? SearchTerm { get; set; }

        public bool IsSortSimple { get; set; }
        public bool IsSortUseThenBy { get; set; }
       
        [DefaultValue(1)]
        public int Skip { get; set; }
        [DefaultValue(3)]
        public int Take { get; set; }
    }
}
