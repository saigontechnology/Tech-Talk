using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessImage.Models
{
    public class FileMetadata
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public CategoryEnum Category { get; set; }
        public Color Color { get; set; }
    }
}
