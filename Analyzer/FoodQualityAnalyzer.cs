using System;
using System.Collections.Generic;
using System.Text;

namespace Analyzer
{
    internal class FoodQualityAnalyzer : ISpreadable
    {
        private FoodProduct[] _products;
        public FoodProduct[] Products => _products;
        
        public FoodQualityAnalyzer()
        {
            _products = new FoodProduct[0];
        }

        public void Add(FoodProduct product)
        {
            Array.Resize(ref _products, _products.Length + 1);
            _products[^1] = product;
        }

        public void Add(FoodProduct[] products)
        {
            for (int i = 0; i < products.Length; i++)
            {
                Add(products[i]);
            }
        }
    }
}
