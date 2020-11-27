using System.Windows.Media.Imaging;

namespace ImageViewer.Domain
{
    public interface IImageProvider
    {
        BitmapSource UploadedImage { get; }

        void GetImage(string pathToImage);
    }
}
