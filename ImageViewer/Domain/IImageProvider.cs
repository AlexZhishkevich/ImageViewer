using System.Windows.Media.Imaging;

namespace ImageViewer.Domain
{
    public interface IImageProvider
    {
        BitmapSource GetImage(string pathToImage);
    }
}
