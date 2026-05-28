using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    internal interface IShrinkable
    {
        void RemoveProduct(FoodProduct product);
        void RemoveProduct(int index);
        void RemoveLastProduct();
    }
}
