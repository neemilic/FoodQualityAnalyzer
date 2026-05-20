using System;
using System.Collections.Generic;
using System.Text;

namespace Analyzer
{
    public class Vegetable : FoodProduct
    {
        private bool _isRootVegetable;
        private string _growingSeason;

        public bool IsRootVegetable => _isRootVegetable;
        public string GrowingSeason => _growingSeason;

        public Vegetable(string name, DateTime expDate, DateTime productionDate, bool isRootVegetable, string growingSeason)
            : base(name, expDate, productionDate)
        {
            _isRootVegetable = isRootVegetable;
            _growingSeason = growingSeason;
        }

        public override double GetQuality()
        {
            double quality = (DaysBeforeExpDate / (double)MaxShelfLife) * 10.0;
            if (IsRootVegetable) quality += 0.5;
            if (GrowingSeason == GetCurrentSeason()) quality += 0.5;
            else quality -= 0.5;
            return Math.Clamp(Math.Round(quality, 1), 0.0, 10.0);
        }

        private string GetCurrentSeason()
        {
            int season = DateTime.Now.Month;
            return season switch
            {
                12 or 1 or 2 => "Winter",
                3 or 4 or 5 => "Spring",
                6 or 7 or 8 => "Summer",
                9 or 10 or 11 => "Autumn"
            };
        }
    }
}
