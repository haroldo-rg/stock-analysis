class Program
{
    static async Task Main(string[] args)
    {
        // Carregar todas as configurações do arquivo appsettings.json
        var configuration = ConfigurationLoader.LoadConfiguration();

        // Extrair dados (web scapring)
        var scraper = new StocksScraper(configuration);
        List<StockData> stockDataList = await scraper.GetStockDataAsync();
        Console.WriteLine($"* Registros obtidos: { stockDataList.Count }");

        // Filtrar dados de acordo com os filtros definidos em appsettings.json
        var filters = FilterBuilder.BuildFilters(configuration);
        var filteredStockDataList = StockDataFilterProcessor.ApplyFilters(stockDataList, filters);

        // Ranquear dados de acordo com os critérios definidos em appsettings.json
        var rankCriteria = ConfigurationLoader.LoadRankCriteria(configuration);
        var rankedStocks = new Ranker(rankCriteria).RankStocks(filteredStockDataList);

        // Exibir dados filtrados e rankeados
        DisplayHelper.PrintStockData(rankedStocks);

        // Exibir menu que permite exportar os dados "as is" (sem filtros e rankeamento) para formatos diversos
        DisplayHelper.ShowMenu(stockDataList);
    }
}