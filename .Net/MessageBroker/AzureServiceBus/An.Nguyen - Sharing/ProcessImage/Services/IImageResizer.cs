using System.IO;

namespace ProcessImage.Services
{
    public interface IImageResizer
    {
        void ResizeSmall(Stream input, Stream output);
        void ResizeMedium(Stream input, Stream output);
        void ResizeBig(Stream input, Stream output);
    }
}
