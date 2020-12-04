using ImageViewer.Core;
using ImageViewer.Mvvm;
using Ninject;
using System.Windows;

namespace ImageViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel _kernel;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            RegisterDependencies();

            MainWindow mainWindow = new MainWindow()
            {
                DataContext = _kernel.Get<MainWindowViewModel>()
            };
            mainWindow.Show();


        }

        private void RegisterDependencies()
        {
            _kernel = new StandardKernel();

            _kernel.Bind<IImageSaver>().To<BmpImageSaver>().InSingletonScope();
            _kernel.Bind<MainWindowViewModel>().ToSelf().InSingletonScope();
        }
    }
}
