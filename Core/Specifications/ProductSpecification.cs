using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(string? brand, string? type, string? sort) : base(Crit(brand, type))
    {
        switch (sort)
        {
            case "priceAsc":
                AddOrderBy(x => x.Price);
                break;
            case "priceDesc":
                AddOrderByDescending(x => x.Price);
                break;
            default:
                AddOrderBy(x => x.Name);
                break;
        }
    }

    private static Expression<Func<Product, bool>> Crit(string? brand, string? type)
    {
        return x =>
            (string.IsNullOrWhiteSpace(brand) || x.Brand == brand) &&
            (string.IsNullOrWhiteSpace(type) || x.Type == type);
    }
}
