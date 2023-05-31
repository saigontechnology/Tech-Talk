using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessImage.Services
{
    public class ImageResizer : IImageResizer
    {
        public void ResizeBig(Stream input, Stream output)
        {
            using (Image image = Image.Load(input))
            {
                image.Mutate(x => x.Resize(500, 500));
                image.Save(output, new JpegEncoder());
            }
        }

        public void ResizeMedium(Stream input, Stream output)
        {
            using (Image image = Image.Load(input))
            {
                image.Mutate(x => x.Resize(300, 300));
                image.Save(output, new JpegEncoder());
            }
        }

        public void ResizeSmall(Stream input, Stream output)
        {
            using (Image image = Image.Load(input))
            {
                image.Mutate(x => x.Resize(100, 100));
                image.Save(output, new JpegEncoder());
            }
        }
    }
}
