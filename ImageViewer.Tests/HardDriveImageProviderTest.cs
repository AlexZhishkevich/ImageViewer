using ImageViewer.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace ImageViewer.Tests
{
    [TestClass]
    public class HardDriveImageProviderTest
    {
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void GetImage_WhenCalledWithPathToNotExistingFile_ThrowsException()
        {
            // Arrange
            var hardDriveImageProvider = new HardDriveImageProvider();
            var pathToFile = "foo";

            //Act
            hardDriveImageProvider.GetImage(pathToFile);

            //Assert - Expects exception
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetImage_WhenFileHasNotSupportedFormat_ThrowsException()
        {
            // Arrange
            var hardDriveImageProvider = new HardDriveImageProvider();
            var pathToFile = "ImageViewer.Tests.dll";

            //Act
            hardDriveImageProvider.GetImage(pathToFile);

            //Assert - Expects exception
        }
    }
}
