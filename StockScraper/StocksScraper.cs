using System.Globalization;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Classe para extrair as informações das ações do web site cuja URL está
/// configurada em stock_prices_url" no arquivo appsettings.json
/// </summary>
public class StocksScraper
{
    private static readonly HttpClient client = new HttpClient();
    private readonly IConfiguration _configuration;
    private static readonly CultureInfo cultureInfo = new CultureInfo("pt-BR");

    public StocksScraper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<List<StockData>> GetStockDataAsync()
    {
        string? url = _configuration["stock_prices_url"];
        if (string.IsNullOrEmpty(url))
        {
            throw new Exception("URL de dados de ações não está configurada.");
        }

        client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var pageContent = await response.Content.ReadAsStringAsync();

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(pageContent);

        var table = htmlDoc.DocumentNode.SelectSingleNode("//table[@id='resultado']");
        var rows = table.SelectNodes(".//tr");

        var stockDataList = new List<StockData>();

        foreach (var row in rows.Skip(1)) // Ignora o cabeçalho
        {
            var columns = row.SelectNodes(".//td");
            if (columns != null && columns.Count >= 20)
            {
                var stockData = new StockData
                {
                    Papel = columns[0].InnerText.Trim(),
                    Cotacao = ParseDecimal(columns[1].InnerText.Trim()),
                    Pl = ParseDecimal(columns[2].InnerText.Trim()),
                    Pvp = ParseDecimal(columns[3].InnerText.Trim()),
                    Psr = ParseDecimal(columns[4].InnerText.Trim()),
                    DivYield = ParsePercentage(columns[5].InnerText.Trim()),
                    PAtivo = ParseDecimal(columns[6].InnerText.Trim()),
                    PCapGiro = ParseDecimal(columns[7].InnerText.Trim()),
                    PEbit = ParseDecimal(columns[8].InnerText.Trim()),
                    PAtivCircLiq = ParseDecimal(columns[9].InnerText.Trim()),
                    EvEbit = ParseDecimal(columns[10].InnerText.Trim()),
                    EvEbitda = ParseDecimal(columns[11].InnerText.Trim()),
                    MrgEbit = ParsePercentage(columns[12].InnerText.Trim()),
                    MrgLiq = ParsePercentage(columns[13].InnerText.Trim()),
                    LiqCorr = ParseDecimal(columns[14].InnerText.Trim()),
                    Roic = ParsePercentage(columns[15].InnerText.Trim()),
                    Roe = ParsePercentage(columns[16].InnerText.Trim()),
                    Liq2Meses = ParseDecimal(columns[17].InnerText.Trim()),
                    PatrimLiq = ParseDecimal(columns[18].InnerText.Trim()),
                    DivBrutPatrim = ParseDecimal(columns[19].InnerText.Trim()),
                    CrescRec5a = ParsePercentage(columns[20].InnerText.Trim())
                };
                stockDataList.Add(stockData);
            }
        }

        return stockDataList;
    }

    private decimal? ParseDecimal(string value)
    {
        if (decimal.TryParse(value, NumberStyles.Any, cultureInfo, out decimal result))
        {
            return result;
        }
        return null; // Valor padrão caso a conversão falhe
    }

    private decimal? ParsePercentage(string value)
    {
        if (value != null)
        {
            value = value.Replace("%", string.Empty).Trim();
        }

        if (decimal.TryParse(value, NumberStyles.Any, cultureInfo, out decimal result))
        {
            return result / 100;
        }
        
        return null;
    }  
}
