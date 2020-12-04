using System.IO;
using System.Windows.Media.Imaging;

namespace ImageViewer.Core
{
    public class BmpImageSaver : IImageSaver
    {
        public void SaveImage(string directoryToSaveImage, string imageName, BitmapSource image)
        {
            var pathToSaveImage = Path.Combine(directoryToSaveImage, $"{imageName}.bmp");

            using (var fileStream = new FileStream(pathToSaveImage, FileMode.Create))
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(fileStream);
            }
        }
    }
}
