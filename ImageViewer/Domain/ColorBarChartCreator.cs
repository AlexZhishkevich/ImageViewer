using System;
using System.Drawing;

namespace ImageViewer.Domain
{
    public class ColorBarChartCreator
    {
        private static int _height = 90;

        private int[] _redBarChartData;
        private int[] _greenBarChartData;
        private int[] _blueBarChartData;

        public Bitmap RedBarChart { get; private set; }
        public Bitmap GreenBarChart { get; private set; }
        public Bitmap BlueBarChart { get; private set; }

        public void CalculateImageHistogram(byte[] image, int bytesPerPixel)
        {
            if (bytesPerPixel < 3)
                throw new ArgumentException(nameof(bytesPerPixel));

            if (image.Length % bytesPerPixel != 0)
                throw new ArgumentException(nameof(image));

            _redBarChartData = new int[256];
            _greenBarChartData = new int[256];
            _blueBarChartData = new int[256];

            for (int index = 0; index < image.Length; index += bytesPerPixel)
            {
                ++_redBarChartData[image[index]];
                ++_greenBarChartData[image[index + 1]];
                ++_blueBarChartData[image[index + 2]];
            }

            RedBarChart = CreateBarChart(_redBarChartData, Color.Red);
            GreenBarChart = CreateBarChart(_greenBarChartData, Color.Green);
            BlueBarChart = CreateBarChart(_blueBarChartData, Color.Blue);
        }

        private Bitmap CreateBarChart(int[] colorIntensities, Color colorToFillBarChart)
        {
            var resultBarChart = new Bitmap(256, _height);

            int maxValue = 0;
            for (int i = 0; i < 256; ++i)
            {
                if (colorIntensities[i] > maxValue)
                    maxValue = colorIntensities[i];
            }

            double point = (double)maxValue / _height;

            for (int i = 0; i < 256 - 1; ++i)
            {
                for (int j = _height - 1; j > _height - colorIntensities[i] / point; --j)
                {
                    resultBarChart.SetPixel(i, j, colorToFillBarChart);
                }
            }

            return resultBarChart;
        }
    }
}
