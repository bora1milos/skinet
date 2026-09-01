using Core.Entities;

namespace Core.Specifications;

public class TypeListSpecifications : BaseSpecification<Product, string>
{
    public TypeListSpecifications()
    {
        AddSelect(x => x.Type);
        ApplyDistinct();
    }
}
