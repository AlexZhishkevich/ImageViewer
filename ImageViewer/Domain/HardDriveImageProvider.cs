using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;

namespace ImageViewer.Domain
{
    public class HardDriveImageProvider : IImageProvider
    {
        private IDictionary<string, Func<string, BitmapSource>> _imageDecoders => new Dictionary<string, Func<string, BitmapSource>>()
        {
            { ".png", ReadPngImage },
            { ".jpg", ReadJpgImage },
            { ".jpeg", ReadJpgImage }
        };

        public BitmapSource GetImage(string pathToImage)
        {
            if (!File.Exists(pathToImage))
                throw new FileNotFoundException(nameof(pathToImage));

            var imageExtension = Path.GetExtension(pathToImage);

            if (string.IsNullOrEmpty(imageExtension))
                throw new BadImageFormatException(nameof(imageExtension));

            if (!_imageDecoders.ContainsKey(imageExtension))
                throw new NotSupportedException(nameof(imageExtension));

            return _imageDecoders[imageExtension].Invoke(pathToImage);
        }

        private BitmapSource ReadPngImage(string pathToImage)
        {
            BitmapSource resultImage = null;

            using (Stream imageStreamSource = new FileStream(pathToImage, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                PngBitmapDecoder decoder = new PngBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                resultImage = decoder.Frames[0];
            }

            return resultImage;
        }

        private BitmapSource ReadJpgImage(string pathToImage)
        {
            BitmapSource resultImage = null;

            using (Stream imageStreamSource = new FileStream(pathToImage, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                JpegBitmapDecoder decoder = new JpegBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                resultImage = decoder.Frames[0];
            }

            return resultImage;
        }


    }
}
