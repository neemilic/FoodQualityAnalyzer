using System;
using System.Collections.Generic;
using System.Text;

namespace Analyzer
{
    public class Backery : FoodProduct
    {
        private bool _isGlutenfree;
        private double _fatContent;

        public bool IsGlutenFree => _isGlutenfree;
        public double FatContent => _fatContent;

        public Backery(string name, DateTime expDate, DateTime productionDate, bool isGlutenFree, double fatContent)
            : base(name, expDate, productionDate)
        {
            _isGlutenfree = isGlutenFree;
            _fatContent = fatContent;
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
            if (!IsGlutenFree) quality -= 0.2;
            if (FatContent > 20.0 && FatContent < 50.0) quality -= 1.0;
            else if (FatContent >= 50.0) quality -= 3.0;

            return Math.Clamp(Math.Round(quality, 1), 0.0, 10.0);
        }
    }
}
