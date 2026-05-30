using Analyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Analyzer
{
    public abstract class ReportSerializer
    {
        public abstract void Save(List<FoodProduct> products, string filePath);
        public abstract string GetExtension();
        public abstract List<FoodProduct> Load(string filePath);

    }
}
