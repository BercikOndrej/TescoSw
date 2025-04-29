using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using TescoSwTask.Models;

namespace TescoSoftwareTask.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public bool HasData => Cars.Any();

        public ObservableCollection<Car> Cars { get; } = new ObservableCollection<Car>();

        public ObservableCollection<SummaryInfo> Summary { get; } = new ObservableCollection<SummaryInfo>();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void LoadCars(string filePath) 
        {
            FileInfo file = new FileInfo(filePath);
            if (!file.Exists)
            {
                MessageBox.Show("loading the file was unsuccessful. File not found.");
            }

            ObservableCollection<Car> data = new ObservableCollection<Car>();
            try
            {
                // The root of xml file must be "Cars"
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Car>), new XmlRootAttribute("Cars"));
                using (var sr = new StreamReader(filePath))
                {
                    data = (ObservableCollection<Car>)serializer.Deserialize(sr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error durring loading a file: {ex.Message}");
            }
            UpdateData(data);
        }

        private ObservableCollection<SummaryInfo> CreateSummaryInfo()
        {
            List<string> modelNames = Cars.Select(c => c.Name).Distinct().ToList();
            List<SummaryInfo> result = new List<SummaryInfo>();

            foreach (var modelName in modelNames)
            {
                double totalPrice = Cars.Where(c => c.Name == modelName).Sum(c => c.Price);
                double totalPriceWithDph = Cars.Where(c => c.Name == modelName).Sum(c => c.PriceWithDPH);
                SummaryInfo info = new SummaryInfo() { ModelName = modelName, PriceWithDph = totalPriceWithDph, PriceWithoutDph = totalPrice };
                result.Add(info);
            }

            return new ObservableCollection<SummaryInfo>(result);
        }

        private void UpdateData(ObservableCollection<Car> data)
        {
            Cars.Clear();
            foreach (Car car in data)
            {
                Cars.Add(car);
            }

            ObservableCollection<SummaryInfo> infos = CreateSummaryInfo();
            Summary.Clear();
            foreach (var info in infos)
            {
                Summary.Add(info);
            }
            OnPropertyChanged(nameof(HasData));
        }
    }
}
