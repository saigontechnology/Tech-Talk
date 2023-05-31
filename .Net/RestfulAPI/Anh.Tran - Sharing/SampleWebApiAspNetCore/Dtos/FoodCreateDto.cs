using System.ComponentModel.DataAnnotations;

namespace SampleWebApiAspNetCore.Dtos
{
    public class FoodCreateDto
    {
        [Required]
        public string? Name { get; set; }
        public string? Type { get; set; }
        public int Calories { get; set; }
        public DateTime Created { get; set; }
    }

    public class Error
    {
        public string? Name { get; set; }
        public IList<ErrorItem>? ErrorItems { get; set; }
        public string? Issue { get; set; }
    }
    public class ErrorItem
    {
        public string? Field { get; set; }
        public string? Value { get; set; }
        public string? Issue { get; set; }
    }

}
