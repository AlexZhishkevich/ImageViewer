using System.Windows.Media.Imaging;

namespace ImageViewer.Domain
{
    internal interface IImageProvider
    {
        BitmapSource UploadedImage { get; }

        void GetImage(string pathToImage);
    }
}
