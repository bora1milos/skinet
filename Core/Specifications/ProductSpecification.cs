using Core.Entities;

namespace Core.Specifications;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(ProductSpecParameters specParameters) : base(p =>
        (string.IsNullOrEmpty(specParameters.Search) || p.Name.ToLower().Contains(specParameters.Search)) &&
        (specParameters.Brands.Count == 0 || specParameters.Brands.Contains(p.Brand)) &&
        (specParameters.Types.Count == 0 || specParameters.Types.Contains(p.Type)))
    {
        ApplyPaging((specParameters.PageIndex - 1) * specParameters.PageSize, specParameters.PageSize);

        switch (specParameters.Sort)
        {
            case "priceAsc":
                AddOrderBy(p => p.Price);
                break;
            case "priceDesc":
                AddOrderByDescending(p => p.Price);
                break;
            default:
                AddOrderBy(p => p.Name);
                break;
        }
    }
}
