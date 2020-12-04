using ImageViewer.Core;
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
        private IImageSaver _imageSaver;

        public MainWindowViewModel(IImageSaver imageSaver) 
        {
            _imageSaver = imageSaver ?? throw new ArgumentNullException(nameof(imageSaver));

            WebImageProvider = new WebImageProvider();
            HardDriveImageProvider = new HardDriveImageProvider();
        }

        public ICommand ProcessImageFromWebCommand => new DelegateCommandd(ProcessImageFromWeb);

        public ICommand ProcessImageFromHardDriveCommand => new DelegateCommandd(ProcessImageFromHardDrive);

        public ICommand SetFolderToSaveImageCommand => new DelegateCommandd(SetFolderToSaveImage);

        public ICommand SaveImageCommand => new DelegateCommandd(SaveImage);

        public IImageProvider WebImageProvider { get; set; }

        public IImageProvider HardDriveImageProvider { get; set; }

        #region PathToWebImage property
        private string _pathToWebImage = string.Empty;

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

        #region DirectoryToSaveImage property
        private string _directoryToSaveImage;

        public string DirectoryToSaveImage
        {
            get => _directoryToSaveImage;
            set => SetField(ref _directoryToSaveImage, value);
        }
        #endregion

        private void ProcessImageFromWeb()
        {
            CleanData();

            try
            {
                Image = WebImageProvider.GetImage(_pathToWebImage);
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

            if (Image.Height != 1)
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
            else
            {
                HistogramShouldBeShawn = false;
                HistogramCreationErrorMessage = "Can not create histogram. Incorrect image data";
            }
        }

        private void ProcessImageFromHardDrive()
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
                else
                    return;
            }

            try
            {
                Image = HardDriveImageProvider.GetImage(pathToImage);
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

            try
            {
                var barChartCreator = new ColorBarChartCreator();
                barChartCreator.CalculateImageHistogram(imageBytes, bytesPerPixel);

                RedColorBarChart = barChartCreator.RedBarChart.ToImageSource();
                GreenColorBarChart = barChartCreator.GreenBarChart.ToImageSource();
                BlueColorBarChart = barChartCreator.BlueBarChart.ToImageSource();
            }
            catch (ArgumentException)
            {
                HistogramShouldBeShawn = false;
                HistogramCreationErrorMessage = "Can not create histogram. Incorrect image data";
            }
        }

        private void SetFolderToSaveImage()
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    DirectoryToSaveImage = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void SaveImage()
        {
            _imageSaver.SaveImage(DirectoryToSaveImage, "image", Image);
        }

        private void CleanData()
        {
            Image = null;
            DirectoryToSaveImage = null;
            HardDriveImageUploadingErrorMessage = string.Empty;
            ImageUploadingErrorText = string.Empty;
            RedColorBarChart = null;
            GreenColorBarChart = null;
            BlueColorBarChart = null;
            HistogramCreationErrorMessage = string.Empty;
        }
    }
}
