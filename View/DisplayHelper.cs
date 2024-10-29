// Classe auxiliar para exibir dados formatados
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

/// <summary>
/// Classe com os métodos de exibição dos dados na saída padrão e exportação da lista
/// de ações em formatos de arquivo ou para a área de transferência
/// </summary>
public static class DisplayHelper
{
    public static string OutputDir => Path.Combine(AppContext.BaseDirectory, "output");

    public static void WriteToFile(string fileName, string content)
    {
        if(!Directory.Exists(OutputDir))
            Directory.CreateDirectory(OutputDir);

        Directory.CreateDirectory(OutputDir);

        string filePath = Path.Combine(OutputDir, fileName);
        File.WriteAllText(filePath, content, Encoding.Default);
        Console.WriteLine($"Arquivo gravado: {filePath}");
    }

    public static void PrintStockData(List<RankedStock> rankedStocksList)
    {
        CultureInfo cultureInfo = new CultureInfo("pt-BR");

        Console.WriteLine();
        string header = $"{ "PAPEL",-10} | { "P/L",-11} | { "P/VP",-11} | { "DIV.YIELD",-11} | { "ROE",-11} | { "LIQ.2MESES",-15} | { "PATRIM. LÍQ",-20} | { "DÍV.BRT/ PAT",-9} | { "CRESC. REC.5A",-11} | { "RATING",-7}";
        Console.WriteLine(new string('-', header.Length));
        Console.WriteLine(header);
        Console.WriteLine(new string('-', header.Length));

        foreach (var item in rankedStocksList)
        {
            string line = FormatLine(item, cultureInfo);
            Console.WriteLine(line);
        }

        Console.WriteLine(new string('-', header.Length));
    }

private static string FormatLine(RankedStock item, CultureInfo cultureInfo)
{
    string papel = $"{item.Stock.Papel,-10} | ";
    string pl = FormatWithRank(item, "Pl", item.Stock.Pl?.ToString("N2", cultureInfo));
    string pvp = FormatWithRank(item, "Pvp", item.Stock.Pvp?.ToString("N2", cultureInfo));
    string divYield = FormatWithRank(item, "DivYield", item.Stock.DivYield != null ? (item.Stock.DivYield.Value * 100).ToString("N2", cultureInfo) + "%" : null);
    string roe = FormatWithRank(item, "Roe", item.Stock.Roe != null ? (item.Stock.Roe.Value * 100).ToString("N2", cultureInfo) + "%" : null);
    string liq2Meses = FormatWithRank(item, "Liq2Meses", item.Stock.Liq2Meses?.ToString("N0", cultureInfo));
    string patrimLiq = FormatWithRank(item, "PatrimLiq", item.Stock.PatrimLiq?.ToString("N0", cultureInfo));
    string divBrutPatrim = FormatWithRank(item, "DivBrutPatrim", item.Stock.DivBrutPatrim?.ToString("N2", cultureInfo));
    string crescRec5a = FormatWithRank(item, "CrescRec5a", item.Stock.CrescRec5a != null ? (item.Stock.CrescRec5a.Value * 100).ToString("N2", cultureInfo) + "%" : null);
    string rating = $"{item.TotalScore,7:N0}";

    return $"{papel}{pl} | {pvp} | {divYield} | {roe} | {liq2Meses,15} | {patrimLiq,20} | {divBrutPatrim,12} | {crescRec5a,13} | {rating}";
}


    private static string FormatWithRank(RankedStock item, string propertyName, string? value)
    {
        string rankPosition = GetRankPosition(item, propertyName);
        string formatted = $"{rankPosition,-3} {value ?? "N/D",7}"; // Usa "N/D" se 'value' for nulo

        return formatted;
    }

    private static string GetRankPosition(RankedStock item, string propertyName)
    {
        return item.Positions.ContainsKey(propertyName) && item.Positions[propertyName] > 0 ? $"({item.Positions[propertyName]})" : "   ";
    }

    public static void ShowMenu(List<StockData> stockDataList)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("O que deseja fazer?");
            Console.WriteLine("1 - Exportar a lista completa em formato JSON");
            Console.WriteLine("2 - Exportar a lista completa em formato CSV");
            Console.WriteLine("3 - Exportar a lista completa em formato XML");
            Console.WriteLine("4 - Copiar a lista completa para a área de transferência");
            Console.WriteLine("<ENTER> Finalizar o programa");

            Console.WriteLine();
            Console.Write("Opção: ");

            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Programa finalizado.");
                break;
            }

            if (int.TryParse(input, out int option))
            {
                switch (option)
                {
                    case 1:
                        ExportToJson(stockDataList);
                        break;
                    case 2:
                        ExportToCsv(stockDataList);
                        break;
                    case 3:
                        ExportToXml(stockDataList);
                        break;
                    case 4:
                        CopyToClipboard(stockDataList);
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Entrada inválida. Por favor, insira um número.");
            }
        }
    }

    private static void ExportToJson(List<StockData> stockDataList)
    {
        try
        {
            string json = JsonSerializer.Serialize(stockDataList, new JsonSerializerOptions { WriteIndented = true });
            WriteToFile("stockData.json", json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao exportar para JSON: {ex.Message}");
        }
    }

    private static void ExportToCsv(List<StockData> stockDataList)
    {
        try
        {
            var csv = new StringBuilder();
            csv.AppendLine("PAPEL;COTACAO;P/L;P/VP;PSR;DIV.YIELD;P.ATIVO;P.CAP.GIRO;P.EBIT;P.ATIV.CIRC.LIQ;EV/EBIT;EV/EBITDA;MRG EBIT;MRG LIQ;LIQ. CORR;ROIC;ROE;LIQ. 2 MESES;PATRIM. LÍQ;DÍV.BRT/PATRIM;CRESC. REC. 5A");

            foreach (var item in stockDataList)
            {
                var line = $"{item.Papel};{item.Cotacao};{item.Pl};{item.Pvp};{item.Psr};" +
                        $"{item.DivYield};{item.PAtivo};{item.PCapGiro};{item.PEbit};" +
                        $"{item.PAtivCircLiq};{item.EvEbit};{item.EvEbitda};{item.MrgEbit};" +
                        $"{item.MrgLiq};{item.LiqCorr};{item.Roic};{item.Roe};" +
                        $"{item.Liq2Meses};{item.PatrimLiq};{item.DivBrutPatrim};{item.CrescRec5a}";

                csv.AppendLine(line);
            }

            WriteToFile("stockData.csv", csv.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao exportar para CSV: {ex.Message}");
        }
    }

    private static void ExportToXml(List<StockData> stockDataList)
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<StockData>));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, stockDataList);
                WriteToFile("stockData.xml", writer.ToString());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao exportar para CSV: {ex.Message}");
        }        
    }

    private static void CopyToClipboard(List<StockData> stockDataList)
    {
        try
        {
            var csv = new StringBuilder();
            csv.AppendLine("PAPEL\tCOTACAO\tP/L\tP/VP\tPSR\tDIV.YIELD\tP.ATIVO\tP.CAP.GIRO\tP.EBIT\tP.ATIV.CIRC.LIQ\tEV/EBIT\tEV/EBITDA\tMRG EBIT\tMRG LIQ\tLIQ. CORR\tROIC\tROE\tLIQ. 2 MESES\tPATRIM. LÍQ\tDÍV.BRT/PATRIM\tCRESC. REC. 5A");

            foreach (var item in stockDataList)
            {
                // Formatação das colunas percentuais
                var divYield = item.DivYield.HasValue ? (item.DivYield.Value * 100).ToString("N2") + "%" : "N/A";
                var mrgebit = item.MrgEbit.HasValue ? (item.MrgEbit.Value * 100).ToString("N2") + "%" : "N/A";
                var mrqliq = item.MrgLiq.HasValue ? (item.MrgLiq.Value * 100).ToString("N2") + "%" : "N/A";
                var roic = item.Roic.HasValue ? (item.Roic.Value * 100).ToString("N2") + "%" : "N/A";
                var roe = item.Roe.HasValue ? (item.Roe.Value * 100).ToString("N2") + "%" : "N/A";
                var crescRec5a = item.CrescRec5a.HasValue ? (item.CrescRec5a.Value * 100).ToString("N2") + "%" : "N/A";

                var line = $"{item.Papel}\t{item.Cotacao}\t{item.Pl}\t{item.Pvp}\t{item.Psr}\t" +
                        $"{divYield}\t{item.PAtivo}\t{item.PCapGiro}\t{item.PEbit}\t" +
                        $"{item.PAtivCircLiq}\t{item.EvEbit}\t{item.EvEbitda}\t{mrgebit}\t" +
                        $"{mrqliq}\t{item.LiqCorr}\t{roic}\t{roe}\t" +
                        $"{item.Liq2Meses}\t{item.PatrimLiq}\t{item.DivBrutPatrim}\t{crescRec5a}";

                csv.AppendLine(line);
            }

            ClipboardHelper.CopyToClipboard(csv.ToString());
            Console.WriteLine("Lista copiada para a área de transferência.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao copiar para a área de transferência: {ex.Message}");
        }
    }

   
    
}