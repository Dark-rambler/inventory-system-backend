namespace Inventory.Domain.Entities.Builders
{
    public class ProductBuilder
    {
        private readonly Product _product = new();

        public ProductBuilder WithId(Guid id)
        {
            _product.Id = id;
            return this;
        }

        public ProductBuilder WithName(string name)
        {
            _product.Name = name;
            return this;
        }

        public ProductBuilder WithDescription(string description)
        {
            _product.Description = description;
            return this;
        }

        public ProductBuilder WithCode(string code)
        {
            _product.Code = code;
            return this;
        }

        public ProductBuilder WithCategoryId(Guid categoryId)
        {
            _product.CategoryId = categoryId;
            return this;
        }

        public ProductBuilder WithCategory(Category category)
        {
            _product.Category = category;
            return this;
        }

        public ProductBuilder WithMeasureId(int? measureId)
        {
            _product.MeasureId = measureId;
            return this;
        }

        public ProductBuilder WithMeasure(Measure? measure)
        {
            _product.Measure = measure;
            return this;
        }

        public Product Build() => _product;
    }
}