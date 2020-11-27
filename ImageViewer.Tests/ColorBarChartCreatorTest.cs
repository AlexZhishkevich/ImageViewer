using ImageViewer.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ImageViewer.Tests
{
    [TestClass]
    public class ColorBarChartCreatorTest
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateImageHistogram_WhenCalledWithImageDataOfIncorrectLength_ThrowsException()
        {
            // Arrange
            var colorBarChartCreator = new ColorBarChartCreator();
            var bytesPerPixel = 3;
            var random = new Random();
            var incorrectImageData = new byte[random.Next(100000, 200000) * bytesPerPixel - 1];

            //Act
            colorBarChartCreator.CalculateImageHistogram(incorrectImageData, bytesPerPixel);

            //Assert - Expects exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateImageHistogram_WhenCalledWithBytesPerPixelValueAsLessThanThree_ThrowsException()
        {
            // Arrange
            var colorBarChartCreator = new ColorBarChartCreator();
            var bytesPerPixel = 0;
            var random = new Random();
            var incorrectImageData = new byte[random.Next(100000, 200000)];

            //Act
            colorBarChartCreator.CalculateImageHistogram(incorrectImageData, bytesPerPixel);

            //Assert - Expects exception
        }
    }
}
