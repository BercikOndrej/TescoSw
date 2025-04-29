using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TescoSoftwareTask.ViewModels;

namespace TescoSoftwareTask
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel => (MainViewModel)DataContext;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            // Show dialog
            var dialog = new OpenFileDialog
            {
                Filter = "XML Files (*.xml)|*.xml|All files (*.*)|*.*",
                Title = "Choose a file"
            };

            bool? result = dialog.ShowDialog();

            // Process selected file
            if (result == true)
            {
                string selectedFilePath = dialog.FileName;
                viewModel.LoadCars(selectedFilePath);
            }
        }
    }
}