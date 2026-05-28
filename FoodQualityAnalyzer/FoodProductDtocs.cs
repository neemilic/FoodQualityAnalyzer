using Analyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodQualityAnalyzer
{
    public class FoodProductDtocs
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public DateTime ExpDate { get; set; }
        public DateTime ProductionDate { get; set; }

        // for vegetable
        public bool IsRootVegetable { get; set; }
        public string GrowingSeason { get; set; }

        // for fruit
        public bool IsCitrus {  get; set; }
        public double SugarContent { get; set; }


        public string AnimalType {  get; set; }
        public bool IsOrganic {get; set; }
        public bool IsFresh {  get; set; }

        public bool IsGlutenFree {  get; set; }
        public double FatContent { get; set; }
        public FoodProductDtocs(FoodProduct obj)
        {
            Type = obj.GetType().Name;
            Name = obj.Name;
            ProductionDate = obj.ProductionDate;
            ExpDate = obj.ExpDate;

            if (obj is Vegetable v)
            {
                IsRootVegetable = v.IsRootVegetable;
                GrowingSeason = v.GrowingSeason;
            }

            if (obj is Fruit fruit)
            {
                IsCitrus = fruit.IsCitrus;
                SugarContent = fruit.SugarContent;
            }
            if (obj is Meat meat)
            { 
                IsOrganic = meat.IsOrganic;
                IsFresh = meat.IsFresh;
            }
            if (obj is Backery b)
            {
                IsGlutenFree = b.IsGlutenFree;
                FatContent = b.FatContent;
            }
        }
        public FoodProductDtocs() { }
        
    }
}
