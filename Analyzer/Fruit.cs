using System;
using System.Collections.Generic;
using System.Text;

namespace Analyzer
{
    public class Fruit : FoodProduct
    {
        private bool _isCitrus;
        private double _sugarContent;

        public bool IsCitrus => _isCitrus;
        public double SugarContent => _sugarContent;
        public Fruit(string name, DateTime expDate, DateTime productionDate, bool isCitrus, double sugarContent)
            : base(name, expDate, productionDate)
        {
            _isCitrus = isCitrus;
            _sugarContent = sugarContent;
        }

        public override double GetQuality()
        {
            double quality = 10.0;
            if (DaysBeforeExpDate <= Math.Ceiling(MaxShelfLife / 2.0) && DaysBeforeExpDate > 3.0)
            {
                quality -= 0.5;
            }
            if (DaysBeforeExpDate == 3.0)
            {
                quality -= 2.0;
            }
            if (DaysBeforeExpDate == 1.0)
            {
                quality -= 3.0;
            }
            if (DaysBeforeExpDate <= 0.0)
            {
                quality = 0.0;
                return quality;
            }
            if (!IsCitrus) quality += 0.2;
            if (SugarContent <= 10.0) quality -= 1.0;

            return Math.Clamp(Math.Round(quality, 1), 0.0, 10.0);
        }
    }
}
