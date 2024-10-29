/// <summary>
/// Configurações de um critério de rankeamento para um atributo da ação
/// </summary>
public class RankCriteria
{
    public string Property { get; set; } = string.Empty;
    public string Order { get; set; } = string.Empty;
    public decimal Weight { get; set; }
    public bool Enabled { get; set; }
}