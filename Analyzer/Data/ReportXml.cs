using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace Analyzer
{
    public class ReportXml : ReportSerializer
    {
        public override string GetExtension() => ".xml";

        public override void Save(List<FoodProduct> products, string filePath)
        {
            var dtos = products.Select(p => new FoodProductDtocs(p)).ToList();
            var serializer = new XmlSerializer(typeof(List<FoodProductDtocs>));
            using var writer = new StreamWriter(filePath, false, Encoding.UTF8);
            serializer.Serialize(writer, dtos);
        }

        public override List<FoodProduct> Load(string filePath)
        {
            var serializer = new XmlSerializer(typeof(List<FoodProductDtocs>));
            using var reader = new StreamReader(filePath, Encoding.UTF8);
            var dtos = (List<FoodProductDtocs>)serializer.Deserialize(reader);
            return dtos?.Select(MapToProduct).ToList() ?? new List<FoodProduct>();
        }

        private FoodProduct MapToProduct(FoodProductDtocs dto) => dto.Type switch
        {
            "Vegetable" => new Vegetable(dto.Name, dto.ExpDate, dto.ProductionDate, dto.IsRootVegetable, dto.GrowingSeason),
            "Fruit" => new Fruit(dto.Name, dto.ExpDate, dto.ProductionDate, dto.IsCitrus, dto.SugarContent),
            "Meat" => new Meat(dto.Name, dto.ExpDate, dto.ProductionDate, dto.IsOrganic, dto.IsFresh),
            "Bakery" => new Backery(dto.Name, dto.ExpDate, dto.ProductionDate, dto.IsGlutenFree, dto.FatContent),
            _ => throw new InvalidOperationException($"Неизвестный тип: {dto.Type}")
        };
    }
}
