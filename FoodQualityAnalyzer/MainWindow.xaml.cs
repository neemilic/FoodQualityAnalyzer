using Analyzer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FoodQualityAnalyzer
{

    public partial class MainWindow : Window
    {
        private List<FoodProduct> products;
        private List<CheckBox> checkboxes;

        public MainWindow()
        {
            InitializeComponent();
            LoadProducts();
            CreateProductCheckboxes();
        }

        private void LoadProducts()
        {
            try
            {
                var jsonPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json");
                string jsonContent = File.ReadAllText(jsonPath);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var dtos = JsonSerializer.Deserialize<List<FoodProductDtocs>>(jsonContent, options);

                products = dtos?.Select(MapToProduct).ToList() ?? new List<FoodProduct>();

            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Файл data.json не найден!", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки файла: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }

        }

        private FoodProduct MapToProduct(FoodProductDtocs dto)
        {
            return dto.Type switch
            {
                "Vegetable" => new Vegetable(
            dto.Name,
            dto.ExpDate,
            dto.ProductionDate,
            dto.IsRootVegetable,
            dto.GrowingSeason),
            "Fruit" => new Fruit( dto.Name, dto.ExpDate, dto.ProductionDate, dto.IsCitrus, dto.SugarContent),
                "Meat" => new Meat(dto.Name, dto.ExpDate, dto.ProductionDate, dto.IsOrganic, dto.IsFresh),
                "Bakery" => new Backery(dto.Name, dto.ExpDate, dto.ProductionDate, dto.IsGlutenFree, dto.FatContent),


                _ => throw new InvalidOperationException($"Неизвестный тип продукта: {dto.Type}"),

            };
        }

        private void CreateProductCheckboxes()
        {
            checkboxes = new List<CheckBox>();
            ProductsPanel.Children.Clear();

            foreach (var product in products)
            {
                var checkbox = new CheckBox
                {
                    Content = product.Name,
                    Margin = new Thickness(0, 5, 0, 5),
                    FontSize = 14
                };
                checkbox.Checked += Checkbox_CheckedChanged;
                checkbox.Unchecked += Checkbox_CheckedChanged;

                ProductsPanel.Children.Add(checkbox);
                checkboxes.Add(checkbox);
            }
        }

        private void Checkbox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            bool anySelected = checkboxes.Any(cb => cb.IsChecked == true);

            ShowQualityButton.IsEnabled = anySelected;

            if (anySelected)
            {
                int selectedCount = checkboxes.Count(cb => cb.IsChecked == true);
                //InfoLabel.Text = $"Выбрано продуктов: {selectedCount}. Нажмите 'Показать качество' для анализа.";
            }

        }

        private void ShowQualityButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedProducts = new List<FoodProduct>();

            for (int i = 0; i < checkboxes.Count; i++)
            {
                if (checkboxes[i].IsChecked == true)
                    selectedProducts.Add(products[i]);
            }

            try
            {
                var wnd = new ChartWindow(selectedProducts);
                wnd.Show();
                SaveReport(selectedProducts);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}\n\n{ex.StackTrace}", "Ошибка");
            }
        }
        private ReportSerializer _currentSerializer = new ReportJson();

        private string GetReportsFolder() => System.IO.Path.GetFullPath(
            System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Отчеты"));

        private void FormatComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_currentSerializer == null) return; // защита от срабатывания при инициализации

            var selected = (FormatComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            ReportSerializer newSerializer = selected == "XML"
                ? new ReportXml()
                : new ReportJson();

            ConvertReports(_currentSerializer, newSerializer);
            _currentSerializer = newSerializer;
        }

        private void ConvertReports(ReportSerializer oldSerializer, ReportSerializer newSerializer)
        {
            string folder = GetReportsFolder();
            if (!Directory.Exists(folder)) return;

            var oldFiles = Directory.GetFiles(folder, $"Отчет_*{oldSerializer.GetExtension()}");
            foreach (var oldFile in oldFiles)
            {
                var products = oldSerializer.Load(oldFile);
                if (products == null) continue;

                // Сохраняем в новый формат с тем же номером
                string newFile = oldFile.Replace(oldSerializer.GetExtension(), newSerializer.GetExtension());
                newSerializer.Save(products, newFile);
            }
        }

        private void SaveReport(List<FoodProduct> selected)
        {
            string folder = GetReportsFolder();
            Directory.CreateDirectory(folder);

            int number = Directory.GetFiles(folder, $"Отчет_*{_currentSerializer.GetExtension()}").Length + 1;
            string date = DateTime.Now.ToString("dd-MM-yyyy");
            string fileName = $"Отчет_{number}_от_{date}{_currentSerializer.GetExtension()}";
            string fullPath = System.IO.Path.Combine(folder, fileName);

            _currentSerializer.Save(selected, fullPath);
        }
    }

}
