using System.IO;
using System.Windows.Media.Imaging;

namespace ImageViewer.Core
{
    public class PngImageSaver : IImageSaver
    {
        public void SaveImage(string directoryToSaveImage, string imageName, BitmapSource image)
        {
            var pathToSaveImage = Path.Combine(directoryToSaveImage, $"{imageName}.png");

            using (var fileStream = new FileStream(pathToSaveImage, FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(fileStream);
            }
        }
    }
}
