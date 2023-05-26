using BasicLinQ.Models.Product;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Xml.Serialization;
using BasicLinQ.Entities;

namespace BasicLinQ.Models.Supplier
{
    public class SupplierProductsTemplateXMLModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [XmlIgnore]
        public IEnumerable<ProductBasicModel> Products { get; set; }

        [XmlArray("Products")]
        [XmlArrayItem("Product")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ProductBasicModel[] ProductBasicModelArray
        {
            get
            {
                return Products?.ToArray();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
    public class SupplierProductsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ProductBasicModel> Products { get; set; }
    }
}
