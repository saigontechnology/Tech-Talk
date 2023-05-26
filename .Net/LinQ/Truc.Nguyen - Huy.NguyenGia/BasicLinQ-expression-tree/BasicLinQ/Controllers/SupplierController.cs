using BasicLinQ.Context;
using BasicLinQ.Models.Product;
using BasicLinQ.Models.Supplier;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace BasicLinQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : Controller
    {
        [HttpPost("suppliers-products-xml")]
        public IActionResult GetSuppliersProductsByIdFromXML([FromForm] SupplierProducsRequestXMLModel model)
        {
            using ApplicationDbContext context = new();

            var reader = new StreamReader(model.File.OpenReadStream());
            XElement xmlSuppliers = XElement.Load(reader);
            var suppliersProductQueryWithSyntax =
                from item in xmlSuppliers.Descendants("SupplierProductsModel")
                where model.SuppliersId.Contains((int)item.Element("Id"))
                select new SupplierProductsModel
                {
                    Id = (int)item.Element("Id"),
                    Name = (string)item.Element("Name"),
                    Products = item.Element("Products").Descendants("Product")
                      .Where(x => x != null)
                      .Select(x => new ProductBasicModel
                      {
                          Id = (int)x.Element("Id"),
                          Name = (string)x.Element("Name"),
                          CategoryId = (int)x.Element("CategoryId"),
                          SupplierId = (int)x.Element("SupplierId")
                      })
                };

            return Ok(suppliersProductQueryWithSyntax);
        }

        [HttpGet("export-xml-suppliers-products")]
        public IActionResult ExportXMLSuppliersProducts()
        {
            using ApplicationDbContext context = new();
            var suppliers = context.Suppliers
                .Include(x => x.Products)
                .Select(x => new SupplierProductsModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Products = x.Products.Select(y => new ProductBasicModel
                    {
                        Id = y.Id,
                        Name = y.Name,
                        CategoryId = y.CategoryId,
                        SupplierId = y.SupplierId
                    })
                })
                .ToList();
            string fileName = "all-suppliers-include-products.xml";
            MemoryStream memoryStream = new();
            var writer = new System.Xml.Serialization.XmlSerializer(typeof(List<SupplierProductsModel>));

            var fileXML = new StreamWriter(memoryStream);
            writer.Serialize(fileXML, suppliers);
            fileXML.Close();
            return File(memoryStream.GetBuffer().Where(x => x != 0).ToArray(), "text/xml", fileName);
        }
    }
}
