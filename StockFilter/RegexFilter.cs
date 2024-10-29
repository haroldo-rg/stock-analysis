using System.Text.RegularExpressions;

/// <summary>
/// Classe para aplicar um filtro por expressão regular em uma propriedade da lista de ações
/// </summary>
public class RegexFilter : IStockDataFilter
{
    private static bool SHOW_LOG = true;

    private readonly string _propertyName;
    private readonly string _pattern;
    public bool Enabled { get; }

    public RegexFilter(string propertyName, string pattern, bool enabled)
    {
        _propertyName = propertyName;
        _pattern = pattern;
        Enabled = enabled;
    }

    public List<StockData> Apply(List<StockData> stockDataList)
    {
        List<StockData> resultList = stockDataList.Where(stock =>
        {
            var propertyInfo = typeof(StockData).GetProperty(_propertyName);

            if (propertyInfo != null)
            {
                // Obtém o valor da propriedade
                var value = propertyInfo.GetValue(stock);

                // Converte o valor para string, tratando null como string.Empty
                var stringValue = value?.ToString() ?? string.Empty;

                // Aplica a expressão regular
                return Regex.IsMatch(stringValue ?? string.Empty, _pattern);
            }

            return false;
        }).ToList();

        if(SHOW_LOG)
            Console.WriteLine($"* RegexFilter : propertyName: { _propertyName } , pattern: { _pattern } , registros entrada { stockDataList.Count } , registros saída: { resultList.Count }");        

        return resultList;
    }
}
