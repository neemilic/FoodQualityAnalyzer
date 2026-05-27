using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Analyzer;

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

                // Шаг 1: читаем JSON в плоские DTO
                var dtos = JsonSerializer.Deserialize<List<FoodProductDtocs>>(jsonContent, options);

                // Шаг 2: преобразуем DTO в конкретные типы
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
                "Meat" => new Meat(dto.Name, dto.ExpDate, dto.ProductionDate, dto.AnimalType, dto.IsOrganic, dto.IsFresh),
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
            // Проверяем, выбран ли хотя бы один продукт
            bool anySelected = checkboxes.Any(cb => cb.IsChecked == true);

            // Активируем/деактивируем кнопку
            ShowQualityButton.IsEnabled = anySelected;

            // Обновляем информационную метку
            if (anySelected)
            {
                int selectedCount = checkboxes.Count(cb => cb.IsChecked == true);
                InfoLabel.Text = $"Выбрано продуктов: {selectedCount}. Нажмите 'Показать качество' для анализа.";
            }
            else
            {
                InfoLabel.Text = "Выберите хотя бы один продукт для анализа качества";
            }
        }

        private void ShowQualityButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранные продукты
            var selectedProducts = new List<FoodProduct>();

            for (int i = 0; i < checkboxes.Count; i++)
            {
                if (checkboxes[i].IsChecked == true)
                {
                    selectedProducts.Add(products[i]);
                }
            }

            // Показываем сообщение с выбранными продуктами
            string message = "Выбранные продукты:\n\n";
            foreach (var product in selectedProducts)
            {
                message += $"• {product.Name} ()\n";
            }

            var wnd = new ChartWindow(selectedProducts);
            wnd.Show();

        }
    }

}
