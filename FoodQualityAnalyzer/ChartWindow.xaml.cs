using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Analyzer;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;

namespace FoodQualityAnalyzer
{
    /// <summary>
    /// Логика взаимодействия для ChartWindow.xaml
    /// </summary>
    public partial class ChartWindow : Window, INotifyPropertyChanged
    {
        private PlotModel _plotModel;
        public PlotModel PlotModel
        {
            get => _plotModel;
            set
            {
                _plotModel= value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlotModel)));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public ChartWindow(List<FoodProduct> selectedProducts)
        {
            InitializeComponent();
            DataContext = this;
            BuildChart(selectedProducts);
        }
        
        public void BuildChart(List<FoodProduct> selectedProducts)
        {
            PlotModel = new PlotModel { Title = "Оценка продуктов." };
            var series = new BarSeries
            {
                LabelPlacement = LabelPlacement.Inside,
                LabelFormatString = "{0: .0}"
            };
            foreach (var product in selectedProducts)
            {
                series.Items.Add(new BarItem { Value = product.GetQuality() });
            }
            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            foreach (var product in selectedProducts) { categoryAxis.Labels.Add(product.Name); }

            PlotModel.Series.Add(series);
            PlotModel.Axes.Add(categoryAxis);
            PlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = 10
            });
        }
    }
}
