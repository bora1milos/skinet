namespace Core.Specifications;

public class ProductSpecParameters
{
    private const int kMaxPageSize = 50;

    public int PageIndex { get; set; } = 1;

    private int m_pageSize = 6;

    public int PageSize
    {
        get => m_pageSize;
        set => m_pageSize = (value > kMaxPageSize) ? kMaxPageSize : value;
    }

    private List<string> m_brands = [];

    public List<string> Brands
    {
        get => m_brands; // Return the list of brands as a list of strings
        set => m_brands = value.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries))
            .ToList();
    }

    private List<string> m_types = [];

    public List<string> Types
    {
        get => m_types; // Return the list of types as a list of strings
        set => m_types = value.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries))
            .ToList();
    }

    public string? Sort { get; set; }

    private string? m_search;

    public string? Search
    {
        get => m_search ?? string.Empty;
        set => m_search = value?.ToLower();
    }
}

