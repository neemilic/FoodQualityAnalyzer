namespace Analyzer
{
    public abstract class FoodProduct
    {
        private string _name;
        private DateTime _expDate;
        private DateTime _productionDate;
        private int _maxShelfLife;

        public string Name => _name;
        public int DaysBeforeExpDate => (int)Math.Ceiling((_expDate - DateTime.Now).TotalDays) + 1;
        public DateTime ExpDate => _expDate;
        public DateTime ProductionDate { get; }
        public int MaxShelfLife => (int)Math.Ceiling((_expDate - _productionDate).TotalDays) + 1;

        public FoodProduct(string name, DateTime ExpDate, DateTime productionDate)
        {
            _name = name;
            _expDate = ExpDate;
            _productionDate = productionDate;
        }

        public abstract double GetQuality();
    }
}
