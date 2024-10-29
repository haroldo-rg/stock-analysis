using System.Reflection;

/// <summary>
/// Classe para aplicar um filtro por range de valores (valor_propriedade >= min E <= max) 
/// em uma propriedade da lista de ações
/// </summary>
public class RangeFilter : IStockDataFilter
{
    private static bool SHOW_LOG = true;
    private readonly string _propertyName;
    private readonly decimal _min;
    private readonly decimal _max;
    
    public bool Enabled { get; }

    public RangeFilter(string propertyName, decimal min, decimal max, bool enabled)
    {
        _propertyName = propertyName;
        _min = min;
        _max = max;
        Enabled = enabled;
    }

    public List<StockData> Apply(List<StockData> stockDataList)
    {
        List<StockData> resultList = stockDataList.Where(stock =>
        {
            var propertyInfo = typeof(StockData).GetProperty(_propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo != null)
            {
                // Obtém o valor da propriedade e verifica se não é null
                var value = (decimal?)propertyInfo.GetValue(stock);
                return value.HasValue && value.Value >= _min && value.Value <= _max; // Verifica se o valor está na faixa
            }

            return false;
        }).ToList();

        if(SHOW_LOG)
            Console.WriteLine($"* RangeFilter : propertyName: { _propertyName } , min: { _min } , max: { _max } , registros entrada { stockDataList.Count } , registros saída: { resultList.Count }");

        return resultList;
    }
}
