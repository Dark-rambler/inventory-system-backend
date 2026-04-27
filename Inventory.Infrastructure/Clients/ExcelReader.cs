using ClosedXML.Excel;
using Inventory.Application.Common.Abstracts.Clients;
using Inventory.Domain.Entities;
using Inventory.Domain.Entities.Builders;

namespace Inventory.Infrastructure.Clients
{
    public class ExcelReader : IExcelReader
    {
        public async Task<List<Product>> ReadProductsAsync(Stream stream)
        {
            var products = new List<Product>();

            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.First();
            var rows = worksheet.RangeUsed()!.RowsUsed().Skip(1);

            foreach (var row in rows)
            {
                var name = row.Cell(1).GetString();
                var description = row.Cell(2).GetString();
                var code = row.Cell(3).GetString();
                var categoryIdStr = row.Cell(4).GetString();
                var measureIdStr = row.Cell(5).GetString();

                if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(code))
                    continue;

                var product = new ProductBuilder()
                    .WithName(name)
                    .WithDescription(description)
                    .WithCode(code)
                    .WithCategoryId(Guid.TryParse(categoryIdStr, out var catId) ? catId : Guid.Empty)
                    .WithMeasureId(int.TryParse(measureIdStr, out var mId) ? mId : null)
                    .Build();

                products.Add(product);
            }

            return await Task.FromResult(products);
        }

        public Stream GenerateProductsTemplate()
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Products");

            worksheet.Cell(1, 1).Value = "Name";
            worksheet.Cell(1, 2).Value = "Description";
            worksheet.Cell(1, 3).Value = "Code";
            worksheet.Cell(1, 4).Value = "CategoryId";
            worksheet.Cell(1, 5).Value = "MeasureId";

            worksheet.Row(1).Style.Font.Bold = true;

            worksheet.Columns().AdjustToContents();

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            return stream;
        }
    }
}