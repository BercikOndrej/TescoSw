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
using TescoSwTask.Helpers;
using TescoSwTask.Models;

namespace TescoSoftwareTask.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public bool HasData => Cars.Any();

        public ObservableCollection<Car> Cars { get; } = new ObservableCollection<Car>();

        public ObservableCollection<SaleInfo> Summary { get; } = new ObservableCollection<SaleInfo>();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
            if (data != null || data.Count != 0)
            {
                UpdateData(data);
            }
        }

        private ObservableCollection<SaleInfo> CreateSaleInfo()
        {
            List<string> modelNames = Cars.Select(c => c.Name).Distinct().ToList();
            List<SaleInfo> result = new List<SaleInfo>();
            var carsSaleAtWeekend = Cars.Where(c => Helper.IsSaleAtWeekend(c.DateOfSale)).ToList();

            foreach (var modelName in modelNames)
            {
                double totalPrice = carsSaleAtWeekend.Where(c => c.Name == modelName).Sum(c => c.Price);
                if (totalPrice == 0)
                {
                    continue;
                }
                double totalPriceWithDph = carsSaleAtWeekend.Where(c => c.Name == modelName).Sum(c => c.PriceWithDPH);
                SaleInfo info = new SaleInfo() { ModelName = modelName, PriceWithDph = totalPriceWithDph, PriceWithoutDph = totalPrice };
                
                result.Add(info);
            }

            return new ObservableCollection<SaleInfo>(result);
        }

        private void UpdateData(ObservableCollection<Car> data)
        {
            Cars.Clear();
            foreach (Car car in data)
            {
                Cars.Add(car);
            }

            ObservableCollection<SaleInfo> infos = CreateSaleInfo();
            Summary.Clear();
            foreach (var info in infos)
            {
                Summary.Add(info);
            }
            OnPropertyChanged(nameof(HasData));
        }
    }
}
