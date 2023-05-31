using ProcessImage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessImage.Services
{
    public class ImageAnalysis : IImageAnalysis
    {
        public FileMetadata AnalyzeImage(string name, long size)
        {
            Random random = new Random();
            var color = new Color();
            if (name == "white")
            {
                color.Red = 0;
                color.Green = 0;
                color.Blue = 0;
            }
            else
            {
                color.Red = random.Next(0, 255);
                color.Green = random.Next(0, 255);
                color.Blue = random.Next(0, 255);

            }
            var result = new FileMetadata
            {
                Name = name,
                Size = size,
                Color = color,
                Category = (CategoryEnum)random.Next(0, 4)
            };

            return result;
        }
    }
}
