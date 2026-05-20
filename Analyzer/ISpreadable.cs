using System;
using System.Collections.Generic;
using System.Text;

namespace Analyzer
{
    public interface ISpreadable
    {
        FoodProduct[] Products { get; }

        void Add(FoodProduct product);
        void Add(FoodProduct[] products);
    }
}
