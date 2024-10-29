/// <summary>
/// Interface que representa um filtro genérico
/// </summary>
public interface IStockDataFilter
{
    bool Enabled { get; }
    List<StockData> Apply(List<StockData> stockDataList);
}
