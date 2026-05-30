using Analyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Analyzer
{
    public class ReportJson : ReportSerializer
    {
        public override string GetExtension() => ".json";

        public override void Save(List<FoodProduct> products, string filePath)
        {
            var dtos = products.Select(p => new FoodProductDtocs(p)).ToList();
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            string json = JsonSerializer.Serialize(dtos, options);
            File.WriteAllText(filePath, json, Encoding.UTF8);
        }
        public override List<FoodProduct> Load(string filePath)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            string json = File.ReadAllText(filePath);
            var dtos = JsonSerializer.Deserialize<List<FoodProductDtocs>>(json, options);
            return dtos?.Select(MapToProduct).ToList() ?? new List<FoodProduct>();
        }

        private FoodProduct MapToProduct(FoodProductDtocs dto) => dto.Type switch
        {
            "Vegetable" => new Vegetable(dto.Name, dto.ExpDate, dto.ProductionDate, dto.IsRootVegetable, dto.GrowingSeason),
            "Fruit" => new Fruit(dto.Name, dto.ExpDate, dto.ProductionDate, dto.IsCitrus, dto.SugarContent),
            "Meat" => new Meat(dto.Name, dto.ExpDate, dto.ProductionDate, dto.IsOrganic, dto.IsFresh),
            "Bakery" => new Backery(dto.Name, dto.ExpDate, dto.ProductionDate, dto.IsGlutenFree, dto.FatContent)

        };
    }
}
