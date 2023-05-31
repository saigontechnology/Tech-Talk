using Microsoft.AspNetCore.Mvc;

namespace CSVExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : Controller
    {
        private readonly ICSVService _csvService;

        public EmployeeController(ICSVService csvService)
        {
            _csvService = csvService;
        }

        [HttpPost("read-employees-csv")]
        public async Task<IActionResult> GetEmployeeCSV([FromForm] IFormFileCollection file)
        {
            var employees = _csvService.ReadCSVAsync<Employee>(file[0].OpenReadStream());

            return Ok(employees);
        }
    }
}
