/// <summary>
/// Processar um conjunto de filtros em uma lista de ações
/// </summary>
public static class StockDataFilterProcessor
{
    public static List<StockData> ApplyFilters(List<StockData> stockDataList, List<IStockDataFilter> filters)
    {
        // Inicialmente, a lista filtrada é igual à lista original
        var filteredList = new List<StockData>(stockDataList);

        // aplica na lista todos os filtros
        foreach (var filter in filters)
            if (filter is IStockDataFilter stockFilter)
                if (stockFilter.Enabled) // Somente aplica o filtro se estiver habilitado
                    filteredList = stockFilter.Apply(filteredList);

        // retornando a lista resultante dos filtros
        return filteredList;
    }
}
