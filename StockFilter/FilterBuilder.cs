using Microsoft.Extensions.Configuration;

/// <summary>
/// Criar a lista de filtros a serem aplicados da lista de ações
/// </summary>
public static class FilterBuilder
{
    public static List<IStockDataFilter> BuildFilters(IConfiguration configuration)
    {
        var filters = new List<IStockDataFilter>();
        
        // Filtros regex
        var regexFilters = ConfigurationLoader.LoadRegexFilterCriteria(configuration);
        filters.AddRange(regexFilters.Where(f => f.Enabled).Select(f => new RegexFilter(f.Property, f.Pattern, f.Enabled)));

        // Filtros de faixa
        var rangeFilters = ConfigurationLoader.LoadRangeFilterCriteria(configuration);
        foreach (var rangeFilter in rangeFilters)
        {
            if (rangeFilter.Enabled)
            {
                decimal min = decimal.Parse(rangeFilter.Min.ToString());
                decimal max = decimal.Parse(rangeFilter.Max.ToString());
                filters.Add(new RangeFilter(rangeFilter.Property, min, max, rangeFilter.Enabled));
            }
        }

        return filters;
    }
}