using System.Windows;

using Microsoft.Extensions.Logging;

namespace WorkloadsClient.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILogger<MainWindow> logger;

        public MainWindow(ILogger<MainWindow> logger)
        {
            //We could have injected the ViewModel but the we wouldn't have the Bind ability in xaml
            //Instead we use a Locator as a DataContext in the xaml and connect DataContext.Path to Locator.MainViewModel
            InitializeComponent();
            this.logger = logger;
            logger.LogInformation("Constructing MainWindow");
        }
    }
}
