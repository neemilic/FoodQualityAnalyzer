using System;
using System.Collections.Generic;
using System.Text;

namespace Analyzer
{
    public class Meat : FoodProduct 
    {
        private string _animalType;
        private bool _isOrganic;
        private bool _isFresh;

        public string AnimalType;
        public bool IsOrganic;
        public bool IsFresh;

        public Meat(string name, DateTime expDate, DateTime productionDate, string animalType, bool isOrganic, bool isFresh)
           : base(name, expDate, productionDate)
        {
            _animalType = animalType;
            _isOrganic = isOrganic;
            _isFresh = isFresh;
        }

        public override double GetQuality()
        {
            double quality = (DaysBeforeExpDate / (double)MaxShelfLife) * 10.0;
            if (IsOrganic) quality += 0.5;
            if (IsFresh) quality += 0.5;
            else quality -= 0.5;

            return Math.Clamp(Math.Round(quality, 1), 0.0, 10.0);
        }
    }
}
