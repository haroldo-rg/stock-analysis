/// <summary>
/// Objeto que representa uma ação rankeada, contendo o score geral (TotalScore)
/// e a posição que a ação está rankeada em cada critério
/// </summary>
public class RankedStock
{
    public StockData Stock { get; set; } = new StockData();
    public Dictionary<string, int> Positions { get; set; } = new Dictionary<string, int>();
    public decimal? TotalScore { get; set; } = 0;
}