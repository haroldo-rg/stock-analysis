/// <summary>
/// Classe que aplica os critérios de rankeamento em uma lista de ações
/// Os critérios a serem aplicados em cada propriedade da ação são 
/// configurados em "stock_rank" no arquivo appsettings.json
/// 
/// Regra:
///     Para cada critério
///     |    Ordenar as ações pela coluna do critério, na ordem definida (asc ou desc)
///     |    
///     |    Para as 05 cinco primeira ações da lista ordenada
///     |    |    Atribuir a posição de 1-5 para a ação/critério
///     |    |    Acrescentar 1 (um) ponto para a ação em TotalScore
///     
///     Retornar a lista de ações ordena pelo TotalScore (decrescente) e em caso de empate, DÍV.BRT/PATRIM (crescente)
/// </summary>
public class Ranker
{
    private List<RankCriteria> _criteria;

    public Ranker(List<RankCriteria> criteria)
    {
        _criteria = criteria;
    }

public List<RankedStock> RankStocks(List<StockData> stockDataList)
{
    var rankedStocks = stockDataList.Select(stock => new RankedStock { Stock = stock }).ToList();

    foreach (var criteria in _criteria.Where(c => c.Enabled))
    {
        // Ordenar com base no critério
        var sortedStocks = criteria.Order == "asc"
            ? rankedStocks.OrderBy(s => GetPropertyValue(s.Stock, criteria.Property)).ToList()
            : rankedStocks.OrderByDescending(s => GetPropertyValue(s.Stock, criteria.Property)).ToList();

        // Atribuir posição do critério na ação (aplicar somente para as cinco primeiras colocadas)
        int position = 1;
        decimal lastValue = decimal.MinValue;
        int lastPosition = 0;

        for (int i = 0; i < sortedStocks.Count && i < 5; i++)
        {
            var currentValue = GetPropertyValue(sortedStocks[i].Stock, criteria.Property);

            // Se for um empate, mantém a mesma posição
            if (currentValue != null && currentValue == lastValue)
            {
                sortedStocks[i].Positions[criteria.Property] = lastPosition;
            }
            else
            {
                sortedStocks[i].Positions[criteria.Property] = position;
                lastValue = currentValue ?? decimal.MinValue;
                lastPosition = position;
            }

            // Adiciona 01 (um) a pontuação TotalScore da ação
            sortedStocks[i].TotalScore += criteria.Weight;
            position++;
        }
    }

    // Ordena final pelo TotalScore (do maior para o menor) e DÍV.BRT/PATRIM (do menor para o maior)
    return rankedStocks
        .OrderByDescending(rs => rs.TotalScore)
        .ThenBy(rs => rs.Stock.DivBrutPatrim)
        .ToList();
}

    private decimal? GetPropertyValue(StockData stock, string propertyName)
    {
        return propertyName switch
        {
            "Pl" => stock.Pl,
            "Pvp" => stock.Pvp,
            "DivYield" => stock.DivYield,
            "Roe" => stock.Roe,
            _ => null
        };
    }
}