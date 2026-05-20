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
            double quality = (DaysBeforeExpDate / (double)MaxShelfLife) * 10.0;
            if (IsGlutenFree) quality += 0.5;
            if (FatContent > 20.0) quality -= 0.5;
            else if (FatContent < 5.0) quality += 0.5;

            return Math.Clamp(Math.Round(quality, 1), 0.0, 10.0);
        }
    }
}
