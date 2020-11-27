using ImageViewer.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageViewer.Tests
{
    [TestClass]
    public class MainWindowViewModelTest
    {
        [TestMethod]
        public void ProcessImageFromWeb_OnLoadedImageHeightIsOne_IgnoreHistogramCreating()
        {
            // Arrange
            var mainWindowViewModel = new MainWindowViewModel();

            var emptyImage = BitmapSource.Create(1, 1, 96, 96,
                                        PixelFormats.Indexed1,
                                        new BitmapPalette(new List<Color> { Colors.Transparent }),
                                        new byte[] { 0, 0, 0, 0 }, 1);

            var mockWebImageProvider = new Mock<IImageProvider>();
            mockWebImageProvider.Setup(c => c.GetImage(""))
                                .Returns(emptyImage)
                                .Verifiable();

            mainWindowViewModel.WebImageProvider = mockWebImageProvider.Object;

            //Act
            mainWindowViewModel.ProcessImageFromWebCommand.Execute(null);

            //Assert
            Assert.IsNotNull(mainWindowViewModel.Image);
            Assert.IsFalse(mainWindowViewModel.HistogramShouldBeShawn);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ProcessImageFromWeb_OnLoadedImageHasOneBytePerPixelAndHeightGreaterThanOne_ThrowsException()
        {
            // Arrange
            var mainWindowViewModel = new MainWindowViewModel();

            var emptyImage = BitmapSource.Create(2, 2, 96, 96,
                                        PixelFormats.Indexed1,
                                        new BitmapPalette(new List<Color> { Colors.Transparent }),
                                        new byte[] { 0, 0, 0, 0 }, 1);

            var mockWebImageProvider = new Mock<IImageProvider>();
            mockWebImageProvider.Setup(c => c.GetImage(""))
                                .Returns(emptyImage)
                                .Verifiable();

            mainWindowViewModel.WebImageProvider = mockWebImageProvider.Object;

            //Act
            mainWindowViewModel.ProcessImageFromWebCommand.Execute(null);

            //Assert -Expects exception
        }

        [TestMethod]
        public void ProcessImageFromWeb_OnLoadedImageHasCorrectParameters_MethodHasPassed()
        {
            // Arrange
            var mainWindowViewModel = new MainWindowViewModel();

            PixelFormat pf = PixelFormats.Bgr32;
            int width = 200;
            int height = 200;
            int rawStride = (width * pf.BitsPerPixel + 7) / 8;
            byte[] rawImage = new byte[rawStride * height];
            Random value = new Random();
            value.NextBytes(rawImage);
            BitmapSource stubImage = BitmapSource.Create(width, height,
                96, 96, pf, null,
                rawImage, rawStride);


            var mockWebImageProvider = new Mock<IImageProvider>();
            mockWebImageProvider.Setup(c => c.GetImage(""))
                                .Returns(stubImage)
                                .Verifiable();

            mainWindowViewModel.WebImageProvider = mockWebImageProvider.Object;

            //Act
            mainWindowViewModel.ProcessImageFromWebCommand.Execute(null);

            //Assert
            Assert.IsNotNull(mainWindowViewModel.Image);
            Assert.IsNotNull(mainWindowViewModel.RedColorBarChart);
            Assert.IsNotNull(mainWindowViewModel.GreenColorBarChart);
            Assert.IsNotNull(mainWindowViewModel.BlueColorBarChart);
        }
    }
}
