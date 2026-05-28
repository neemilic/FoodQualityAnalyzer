using System;
using System.Collections.Generic;
using System.Text;

namespace Analyzer
{
    public class Meat : FoodProduct 
    {
        private bool _isOrganic;
        private bool _isFresh;

        public bool IsOrganic => _isOrganic;
        public bool IsFresh => _isFresh;

        public Meat(string name, DateTime expDate, DateTime productionDate, bool isOrganic, bool isFresh)
           : base(name, expDate, productionDate)
        {
            _isOrganic = isOrganic;
            _isFresh = isFresh;
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
            if (!IsOrganic) quality -= 1.0;
            if (!IsFresh) quality -= 1.0;

            return Math.Clamp(Math.Round(quality, 1), 0.0, 10.0);
        }
    }
}
