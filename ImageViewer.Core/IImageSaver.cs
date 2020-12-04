using System.Windows.Media.Imaging;

namespace ImageViewer.Core
{
    public interface IImageSaver
    {
        void SaveImage(string directoryToSaveImage, string imageName, BitmapSource image);
    }
}
