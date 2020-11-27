using ImageViewer.Domain;
using ImageViewer.Mvvm;
using System;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ImageViewer
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel() 
        {
        }

        public ICommand DownloadImageFromWebCommand => new DelegateCommandd(DownloadImageByUri);

        #region PathToWebImage property
        private string _pathToWebImage;

        public string PathToWebImage
        {
            get => _pathToWebImage;
            set
            {
                SetField(ref _pathToWebImage, value);
                ImageUploadingErrorText = string.Empty;
            }
        }
        #endregion

        #region Image property
        private BitmapSource _image;

        public BitmapSource Image
        {
            get => _image;
            set => SetField(ref _image, value);
        }
        #endregion

        #region ImageUploadingErrorText property
        private string _imageUploadingErrorText;

        public string ImageUploadingErrorText
        {
            get => _imageUploadingErrorText;
            set => SetField(ref _imageUploadingErrorText, value);
        }
        #endregion


        private void DownloadImageByUri()
        {
            var imageProvider = new WebImageProvider();

            try
            {
                imageProvider.GetImage(_pathToWebImage);
            }
            catch(UriFormatException)
            {
                ImageUploadingErrorText = "Incorrect image URL";
            }
            catch (Exception)
            {
                ImageUploadingErrorText = "An error occurred while trying to download image";
            }

            Image = imageProvider.UploadedImage;
        }
    }
}
