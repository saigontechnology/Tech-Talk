using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace IdentityServerHost.Quickstart.UI
{
    public class FillInformationViewModel
    {
        [Required]
        [MaxLength(250)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "Given Name")]
        public string GivenName { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "Family Name")]
        public string FamilyName { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [StringLength(2)]
        [Display(Name = "Country")]
        public string Country { get; set; }

        public SelectList CountryCodes { get; } = new SelectList(
            new[] {
                new { Id = "BE", Value = "Belgium" },
                new { Id = "US", Value = "United States" },
                new { Id = "VN", Value = "Vietnam" },
                new { Id = "IN", Value = "India" }
            },
            "Id",
            "Value");

        public string ReturnUrl { get; set; }
    }
}
