using System;
using System.Windows.Media.Imaging;

namespace ImageViewer.Domain
{
    public class WebImageProvider : IImageProvider
    {
        public BitmapSource UploadedImage { get; private set; }

        public void GetImage(string pathToImage)
        {
            bool isPathValid = Uri.TryCreate(pathToImage, UriKind.Absolute, out Uri uriResult) && 
                               (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!isPathValid)
                throw new UriFormatException(nameof(pathToImage));
            else
            {
                UploadedImage = new BitmapImage(uriResult);
            }
        }
    }
}
