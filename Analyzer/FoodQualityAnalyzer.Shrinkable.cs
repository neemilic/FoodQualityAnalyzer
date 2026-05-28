using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public partial class FoodQualityAnalyzer : IShrinkable
    {
        public void RemoveProduct(FoodProduct product)
        {
            if (product == null) return;
            _products = _products.Where(pr => pr != product).ToArray();
        }

        public void RemoveProduct(int index)
        {
            if (index < 0 || index >= _products.Length) return;

            FoodProduct[] temp = new FoodProduct[_products.Length - 1];
            Array.Copy(_products, 0, temp, 0, index);
            Array.Copy(_products, index + 1, temp, index, _products.Length - 1 - index);
            _products = temp;
        }

        public void RemoveLastProduct()
        {
            if (_products.Length == 0) return;
            Array.Resize(ref _products, _products.Length - 1);
        }
    }
}
