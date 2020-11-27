using ImageViewer.Domain;
using ImageViewer.Mvvm;
using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageViewer
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel() 
        {
            WebImageProvider = new WebImageProvider();
            HardDriveImageProvider = new HardDriveImageProvider();
        }

        public ICommand DownloadImageFromWebCommand => new DelegateCommandd(DownloadImageByUri);

        public ICommand DownloadImageFromHardDriveCommand => new DelegateCommandd(DownloadImageFromHardDrive);

        public IImageProvider WebImageProvider { get; internal set; }

        public IImageProvider HardDriveImageProvider { get; internal set; }

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
            set 
            { 
                SetField(ref _image, value);
                if (_image == null)
                {
                    HistogramShouldBeShawn = false;
                    HistogramCreationErrorMessage = string.Empty;
                }
                else
                {
                    HistogramShouldBeShawn = true;
                    HistogramCreationErrorMessage = string.Empty;
                }
            }
        }
        #endregion

        #region HistogramShouldBeShawn property
        private bool _histogramShouldBeShawn;

        public bool HistogramShouldBeShawn
        {
            get => _histogramShouldBeShawn;
            set => SetField(ref _histogramShouldBeShawn, value);
        }
        #endregion

        #region RedColorBarChart property
        private ImageSource _redColorBarChart;

        public ImageSource RedColorBarChart
        {
            get => _redColorBarChart;
            set => SetField(ref _redColorBarChart, value);
        }
        #endregion

        #region GreenColorBarChart property
        private ImageSource _greenColorBarChart;

        public ImageSource GreenColorBarChart
        {
            get => _greenColorBarChart;
            set => SetField(ref _greenColorBarChart, value);
        }
        #endregion

        #region BlueColorBarChart property
        private ImageSource _bluenColorBarChart;

        public ImageSource BlueColorBarChart
        {
            get => _bluenColorBarChart;
            set => SetField(ref _bluenColorBarChart, value);
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

        #region HardDriveImageUploadingErrorMessage property
        private string _hardDriveImageUploadingErrorMessage;

        public string HardDriveImageUploadingErrorMessage
        {
            get => _hardDriveImageUploadingErrorMessage;
            set => SetField(ref _hardDriveImageUploadingErrorMessage, value);
        }
        #endregion

        #region HistogramCreationErrorMessage property
        private string _histogramCreationErrorMessage;

        public string HistogramCreationErrorMessage
        {
            get => _histogramCreationErrorMessage;
            set => SetField(ref _histogramCreationErrorMessage, value);
        }
        #endregion

        private void DownloadImageByUri()
        {
            CleanData();

            try
            {
                WebImageProvider.GetImage(_pathToWebImage);
            }
            catch(UriFormatException)
            {
                ImageUploadingErrorText = "Incorrect image URL";
                return;

            }
            catch (Exception)
            {
                ImageUploadingErrorText = "An error occurred while trying to download image";
                return;
            }

            Image = WebImageProvider.UploadedImage;

            if (Image.Height == 1)
            {
                var bytesPerPixel = (Image.Format.BitsPerPixel + 7) / 8;
                var imageBytes = new byte[bytesPerPixel * Image.PixelWidth * Image.PixelHeight];
                Image.CopyPixels(imageBytes, bytesPerPixel * Image.PixelWidth, 0);

                var barChartCreator = new ColorBarChartCreator();
                barChartCreator.CalculateImageHistogram(imageBytes, bytesPerPixel);

                RedColorBarChart = barChartCreator.RedBarChart.ToImageSource();
                GreenColorBarChart = barChartCreator.GreenBarChart.ToImageSource();
                BlueColorBarChart = barChartCreator.BlueBarChart.ToImageSource();
            }
        }


        private void DownloadImageFromHardDrive()
        {
            CleanData();

            string pathToImage = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pathToImage = openFileDialog.FileName;
                }
            }

            try
            {
                HardDriveImageProvider.GetImage(pathToImage);
                Image = HardDriveImageProvider.UploadedImage;
            }
            catch (FileNotFoundException)
            {
                HardDriveImageUploadingErrorMessage = "Selected image file was not found";
                return;
            }
            catch (BadImageFormatException)
            {
                HardDriveImageUploadingErrorMessage = "Selected file has incorrect extension";
                return;
            }
            catch (NotSupportedException)
            {
                HardDriveImageUploadingErrorMessage = "Current image format is not supported";
                return;
            }

            var bytesPerPixel = (Image.Format.BitsPerPixel + 7) / 8;
            var imageBytes = new byte[bytesPerPixel * Image.PixelWidth * Image.PixelHeight];
            Image.CopyPixels(imageBytes, bytesPerPixel * Image.PixelWidth, 0);

            var barChartCreator = new ColorBarChartCreator();
            barChartCreator.CalculateImageHistogram(imageBytes, bytesPerPixel);

            RedColorBarChart = barChartCreator.RedBarChart.ToImageSource();
            GreenColorBarChart = barChartCreator.GreenBarChart.ToImageSource();
            BlueColorBarChart = barChartCreator.BlueBarChart.ToImageSource();
        }

        private void CleanData()
        {
            Image = null;
            HardDriveImageUploadingErrorMessage = string.Empty;
            ImageUploadingErrorText = string.Empty;
            RedColorBarChart = null;
            GreenColorBarChart = null;
            BlueColorBarChart = null;
        }
    }
}
