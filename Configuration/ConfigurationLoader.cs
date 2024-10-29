using Microsoft.Extensions.Configuration;

/// <summary>
/// Carrega as configurações, critérios de filtro e rankeamento
/// </summary>
public static class ConfigurationLoader
{
    public static IConfiguration LoadConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    public static List<RankCriteria> LoadRankCriteria(IConfiguration configuration)
    {
        var rankCriteriaSection = configuration.GetSection("stock_rank");
        return rankCriteriaSection.Exists() ? rankCriteriaSection.Get<List<RankCriteria>>() ?? new List<RankCriteria>() : new List<RankCriteria>();
    }

    public static List<RegexFilterConfig> LoadRegexFilterCriteria(IConfiguration configuration)
    {
        var regexFiltersSection = configuration.GetSection("stock_filters:regex_filters");
        return regexFiltersSection.Exists() ? regexFiltersSection.Get<List<RegexFilterConfig>>() ?? new List<RegexFilterConfig>() : new List<RegexFilterConfig>();
    }

    public static List<RangeFilterConfig> LoadRangeFilterCriteria(IConfiguration configuration)
    {
        var rangeFiltersSection = configuration.GetSection("stock_filters:range_filters");
        return rangeFiltersSection.Exists() ? rangeFiltersSection.Get<List<RangeFilterConfig>>() ?? new List<RangeFilterConfig>() : new List<RangeFilterConfig>();
    }
}