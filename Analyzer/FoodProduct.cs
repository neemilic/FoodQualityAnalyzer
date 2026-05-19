namespace Analyzer
{
    public abstract class FoodProduct
    {
        private string _name;
        private DateTime _maxExpDate;

        public string Name => _name;
        public int DaysBeforeExpDate => (int)Math.Ceiling((_maxExpDate - DateTime.Now).TotalDays);
        public DateTime MaxExpDate => _maxExpDate; 

        public FoodProduct(string name, DateTime maxExpDate)
        {
            _name = name;
            _maxExpDate = maxExpDate;
        }

        public abstract double GetQuality();
    }
}
